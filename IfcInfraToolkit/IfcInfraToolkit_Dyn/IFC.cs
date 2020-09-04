/* Author: Stefan Huber 
   Creation: 12.06.2020
   File: IFC.cs
   Description: library for C3D dynamo to export corridors to IFC models

 */


using System;
using GeometryGym.Ifc;
using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using NUnit.Framework;
using System.Linq;
using NUnit.Framework.Constraints;

namespace IfcInfraToolkit_Dyn
{
    public class IFC_root
    {
        private DatabaseIfc _vb;

        /// <summary>
        /// Initialize IFC model container
        /// </summary>
        /// <param name="familyName">Author's given name</param>
        /// <param name="firstName">Author's first name</param>
        /// <param name="organization">optional: organization</param>
        /// <param name="project">optional project name for IFC model</param>
        public IFC_root(string familyName, string firstName, string organization="none",string project="project1")
        {
            // init new Ifc model database
            _vb = new DatabaseIfc(ModelView.Ifc4Reference);
            _vb.Release = ReleaseVersion.IFC4X3;
            
            // set model author and organization
            IfcPerson author = new IfcPerson(_vb);
            author.FamilyName = familyName;
            author.GivenName = firstName;
            IfcOrganization org = new IfcOrganization(_vb, organization);
            IfcProject projectname = new IfcProject(_vb, project);
            IfcSite site = new IfcSite(_vb, "local_site");
        }

        //Finalizing 
        public void IFC_export(string modelName, string filepath)
        {
            // build path
            var finalPath = filepath + "/" + modelName + ".ifc";

            // write the database to the STEP file
            _vb.WriteFile(finalPath);
        }

        
        //TODO: testing
        /// <summary>
        /// Adds an IfcRoad with IfcOpenProfileDefs to the IFC model
        /// </summary>
        /// <param name="stations"></param>
        /// <param name="width_left"></param>
        /// <param name="width_right"></param>
        /// <param name="slope_left"></param>
        /// <param name="slope_right"></param>
        /// <returns></returns>
        public IFC_root IFC_road_add(List<double> stations,List<double> width_left, List<double> width_right, List<double> slope_left,List<double> slope_right)
        {
            //Error Detection
            //Check for Inputs for null
            if (width_right == null || width_left == null || slope_right == null || slope_left == null
                || stations == null)
            {
                throw new ArgumentNullException("All Inputs have to be different from NULL");
            }

            //Check if all Inputs have the same length 
            if (width_right.Count!=width_left.Count || slope_right.Count != slope_left.Count
                || width_right.Count != slope_right.Count || stations.Count != width_right.Count)
            {
                throw new ArgumentException("All Inputs need to have the same length");
            }
            
          
            //Create lists in which the widths/slopes are sorted in tuple
            //ordered by station
            List<List<double>> widths = new List<List<double>>();
            List<List<double>> slopes = new List<List<double>>();

            //List for all Crossprofiles and DistanceExpression
            List<IfcOpenCrossProfileDef> ocpd = new List<IfcOpenCrossProfileDef>();
            List<IfcDistanceExpression> de = new List<IfcDistanceExpression>();

            //var definition
            IfcSite site = _vb.OfType<IfcSite>().First();
            IfcAlignmentCurve curve = _vb.OfType<IfcAlignmentCurve>().First();
           
            //Convert data in suitable order/format
            for (int i = 0; i < width_right.Count; i++)
            {
                //Add the widths at each Station to the final list
                List<double> tmp_w = new List<double>();
                tmp_w.Add(width_left[i]);
                tmp_w.Add(width_right[i]);
                widths.Add(tmp_w);

                //Add the slopes at each Station to the final list
                List<double> tmp_s = new List<double>();
                tmp_s.Add(slope_left[i]);
                tmp_s.Add(slope_right[i]);
                slopes.Add(tmp_s);

            }
            
            //Add at each station the Crossprofiledef
            for (int i = 0; i< widths.Count; i++) {

                //Create OpenCrossProfileDef and collect them
                IfcOpenCrossProfileDef crsec = new IfcOpenCrossProfileDef(_vb, "Road_part" + (i + 1),true, widths[i], slopes[i]);
                ocpd.Add(crsec);
                //Create DistanceExpression and collect them
                IfcDistanceExpression distex = new IfcDistanceExpression(_vb, stations[i]);
                de.Add(distex);

            }

            
            //link OpenCrossProfileDef to DistanceExpression
            IfcSectionedSurface secsurf = new IfcSectionedSurface(curve, de, ocpd, true);
            IfcShapeRepresentation shaperep = new IfcShapeRepresentation(secsurf);
            IfcProductDefinitionShape produktdef = new IfcProductDefinitionShape(shaperep);


            //Placement TODO: Change 0 to real starting Point
            IfcDistanceExpression dist = new IfcDistanceExpression(_vb, 0);
            IfcLinearPlacement origin = new IfcLinearPlacement(curve, dist);  //-> Placemtent


            //final assembly
            IfcPavement pavement = new IfcPavement(site, origin, produktdef);

            //create Road -> TODO: Link Pavement to Road
            IfcRoad road = new IfcRoad(site, "street1", origin, produktdef);
            

            return this;
        }


        //TODO: Implement
        /// <summary>
        /// Adds an alignment curve to the project and links it with the IfcSite entity
        /// </summary>
        /// <returns></returns>
        public IFC_root IFC_Alignment_add()
        { 
            IfcSite site = _vb.OfType<IfcSite>().First();

            //dummy curve
            IfcAlignmentCurve curve = new IfcAlignmentCurve(_vb);
            IfcAlignment alignment = new IfcAlignment(site, curve);

            return this;
        }
        
        /// <summary>
        /// Watch node for Ifc content in the database
        /// </summary>
        /// <returns></returns>
        public DatabaseIfc Watch_IFC()
        {
           return _vb;
        }
    }
}


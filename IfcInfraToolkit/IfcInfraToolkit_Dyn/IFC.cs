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

namespace IfcInfraToolkit_Dyn
{
    public class IFC_root
    {
        private DatabaseIfc _vb;

        //Constructor
        public IFC_root(string familyName, string firstName, string organization)
        {
            // init new Ifc model database
            _vb = new DatabaseIfc(ModelView.Ifc4Reference);
            _vb.Release = ReleaseVersion.IFC4X3;

            // set model author and organization
            IfcPerson author = new IfcPerson(_vb);
            author.FamilyName = familyName;
            author.GivenName = firstName;
            IfcOrganization org = new IfcOrganization(_vb, organization);

        }

        //Finalizing 
        public void IFC_export(string modelName, string filepath)
        {
            // build path
            var finalPath = filepath + "/" + modelName + ".ifc";

            // write the database to the STEP file
            _vb.WriteFile(finalPath);
        }

        //add Roadcrosssection 
        //TODO: testing
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
            if (width_right.Count==width_left.Count && slope_right.Count==slope_left.Count 
                && width_right.Count==slope_right.Count && stations.Count==width_right.Count)
            {
                throw new ArgumentException("All Inputs need to have the same length");
            }

            //Create dummy curve ONLY FOR TESTING need to be changed later 
            var curve = new IfcAlignmentCurve(_vb);

            //Create lists in which the widths/slopes are sorted in tuple
            //ordered by station
            List<List<double>> widths = new List<List<double>>();
            List<List<double>> slopes = new List<List<double>>();

            //List for all Crossprofiles and DistanceExpression
            List<IfcOpenCrossProfileDef> ocpd = new List<IfcOpenCrossProfileDef>();
            List<IfcDistanceExpression> de = new List<IfcDistanceExpression>();

            //Create site for export
            IfcSite site = new IfcSite(_vb, "locel_site");


            for (int i = 0; i < width_right.Count; i++)
            {
                //Add the widths at each Station to the final list
                widths[i].Add(width_left[i]);
                widths[i].Add(width_right[i]);

                //Add the slopes at each Station to the final list
                slopes[i].Add(width_left[i]);
                slopes[i].Add(width_right[i]);

            }

            //Add at each station the Crossprofiledef
            for (int i = 0; i< widths.Count; i++) {

                //Create OpenCrossProfileDef and collect them
                IfcOpenCrossProfileDef crsec = new IfcOpenCrossProfileDef(_vb, "Road_part " + (i + 1),true, widths[i], slopes[i]);
                ocpd.Add(crsec);
                //Create DistanceExpression and collect them
                IfcDistanceExpression distex = new IfcDistanceExpression(_vb, stations[i]);
                de.Add(distex);

            }

            
            //link OpenCrossProfileDef to DistanceExpression
            IfcSectionedSurface test = new IfcSectionedSurface(curve, de, ocpd, true);

            

            return this;
        }


        //add Alignemnt
        //TODO: Impliment
        public IFC_root IFC_Alignment_add(List<double> dummy)
        {




            return this;
        }

    }
}


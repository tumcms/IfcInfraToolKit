/* Author: Stefan Huber 
   Creation: 18.09.2020
   File: Road.cs
   Description: library for C3D dynamo to export Roads to IFC models

 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;
using IfcInfraToolKit_DynamoCore;

namespace IfcInfraToolkit_Dyn
{
    public class Road
    {
        /*
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
        [MultiReturn(new[] { "DatabaseContainer", "RoadPartGUID" })]
        public static Dictionary<string, object> IfcRoadAddGeometry(DatabaseContainer databaseContainer,
            object RoadGuid,
            object AlignmentGuid,
            List<double> stations,
            List<double> width_left,
            List<double> width_right,
            List<double> slope_left,
            List<double> slope_right,
            double roadstart = 0.0)
        {
            var db = databaseContainer.Database;
            //Error Detection
            //Check for Inputs for null
            if (width_right == null || width_left == null || slope_right == null || slope_left == null
                || stations == null)
            {
                throw new ArgumentNullException("All Inputs have to be different from NULL");
            }

            //Check if all Inputs have the same length 
            if (width_right.Count != width_left.Count || slope_right.Count != slope_left.Count
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
            IfcSite site = db.OfType<IfcSite>().First();


            //Select Alignment via Name TODO: Select Axis out of the Alignment
            IfcAlignment alignment = null;
            IEnumerable<IfcAlignment> query2 = db.OfType<IfcAlignment>();
            foreach (IfcAlignment i in query2)
            {
                if (i.Guid.Equals(AlignmentGuid))
                {
                    alignment = i;
                }
            }
            //Check if there is a Alignment
            if (alignment == null)
            {
                throw new ArgumentNullException("No Alignment found!\n");
            }

            IfcCurve curve = (IfcCurve)alignment.Axis;//Not sure if it is a proper solution


            //Select the road for adding the crosssection
            IfcRoad road = null;
            IEnumerable<IfcRoad> query1 = db.OfType<IfcRoad>();
            string debug = null;
            foreach (IfcRoad i in query1)
            {
                if (i.Guid.ToString().Equals(RoadGuid.ToString()))
                {
                    road = i;
                }
            }

            //Check if there is a road
            if (road == null)
            {
                throw new ArgumentNullException(debug+"No road found!\n");
            }


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
            for (int i = 0; i < widths.Count; i++)
            {

                //Create OpenCrossProfileDef and collect them
                IfcOpenCrossProfileDef crsec = new IfcOpenCrossProfileDef(db, "Road_part" + (i + 1), true, widths[i], slopes[i]);
                ocpd.Add(crsec);
                //Create DistanceExpression and collect them
                IfcDistanceExpression distex = new IfcDistanceExpression(db, stations[i]);
                de.Add(distex);

            }


            //link OpenCrossProfileDef to DistanceExpression
            IfcSectionedSurface secsurf = new IfcSectionedSurface(curve, de, ocpd, true);
            IfcShapeRepresentation shaperep = new IfcShapeRepresentation(secsurf);
            IfcProductDefinitionShape produktdef = new IfcProductDefinitionShape(shaperep);


            //Placement
            IfcDistanceExpression dist = new IfcDistanceExpression(db, roadstart);
            IfcLinearPlacement start = new IfcLinearPlacement(curve, dist);  //-> Placemtent

            //final assembly
            IfcPavement pavement = new IfcPavement(site, start, produktdef);

            //Link Pavement to Road
            //road.AddElement(pavement);
            road.AddAggregated(pavement);

            var re = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"RoadPartGUID", pavement.Guid}
            };

            return re;
        }
        */



        //TODO: Testing, Placement fixing & linking to side
        /// <summary>
        /// Adds an road to the project
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "RoadGUID" })]
        public static Dictionary<string, object> IFCAddRoad(
            DatabaseContainer databaseContainer,
            object hostGuid = null,
            string roadname = "DefaultRoad")
        {
            //var definition
            IfcSite site = databaseContainer.Database.OfType<IfcSite>().First();
            IfcRoad road = new IfcRoad(databaseContainer.Database);
            road.Name = roadname;
            site.AddAggregated(road);
            // Placement needs to be changed when alingment is implemented
            //IfcLinearPlacement origin = new IfcLinearPlacement();
            //road.ObjectPlacement(origin); 

            var re = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"RoadGUID", road.Guid.ToString()}
            };

            return re;
        }



    }
}

/* Author: Stefan Huber 
   Creation: 12.06.2020
   File: IFC.cs
   Description: library for C3D dynamo to export corridors to IFC models

 */


using System;
using GeometryGym.Ifc;
using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;

namespace IfcInfraToolkit_Dyn
{
    public class IfcBaseService
    {
        private DatabaseIfc _vb;

        /// <summary>
        /// Initialize IFC model, set Author
        /// </summary>
        /// <param name="familyName"></param>
        /// <param name="firstName"></param>
        /// <param name="organization"></param>
        public IfcBaseService(string familyName, string firstName, string organization)
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

        /// <summary>
        /// Finalize the IFC model and export it to the given path
        /// </summary>
        /// <param name="modelName">Desired filename</param>
        /// <param name="filepath">Desired folder</param>
        public void IFC_export(string modelName, string filepath)
        {
            // build path
            var finalPath = filepath + "/" + modelName + ".ifc";

            // write the database to the STEP file
            _vb.WriteFile(finalPath);
        }

        //add Roadcrosssection 
        //still requires testing
        public IfcBaseService IFC_road_add(List<double> stations,List<double> width_left, List<double> width_right, List<double> slope_left,List<double> slope_right)
        {
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

        /// <summary>
        /// Adds the selected surface to the IFC database container
        /// </summary>
        /// <param name="db"></param>
        /// <param name="civil3DSurface"></param>
        /// <returns></returns>
        [MultiReturn(new[] {"PointList", "DatabaseIfc"})]
        public Dictionary<string, object> ExportSurfaceToIfc(Surface civil3DSurface)
        {
            // data extraction from the civil project
            var surf = civil3DSurface._surface as AeccTinSurface;

            var triangles = surf.OutputTriangles; // only available in a TIN surface instance. Not in pure surfaces!
            var numTriangles = triangles.Length / 3;
            List<Point> pointList = new List<Point>();
            for (int i = 0; i < triangles.Length; i += 3)
            {
                var pt = Point.ByCoordinates(triangles[i], triangles[i + 1], triangles[i + 2]);
                pointList.Add(pt);
            }
            // ifc preparation
            List<Coord3d> points = new List<Coord3d>();
            List<CoordIndex> coordIndex = new List<CoordIndex>();

            // mapping Civil data -> IFC
            for (int i = 0; i < pointList.Count; i +=3)
            {
                var p1 = new Coord3d(pointList[i    ].X, pointList[i    ].Y, pointList[i    ].Z) ;
                var p2 = new Coord3d(pointList[i + 1].X, pointList[i + 1].Y, pointList[i + 1].Z) ;
                var p3 = new Coord3d(pointList[i + 2].X, pointList[i + 2].Y, pointList[i + 2].Z) ;
               

                points.Add(p1);
                points.Add(p2);
                points.Add(p3);

                coordIndex.Add(new CoordIndex(i, i+1, i+2));
            }

            var flags = new List<int>(coordIndex.Count);
            for (int i = 0; i < flags.Count; i++)
            {
                flags[i] = 0;
            }

            ;

            
            // assign pts to ptList
            IfcCartesianPointList3D cartesianPointList3D = new IfcCartesianPointList3D(_vb, points);
            var triangulatedFaceSet = new IfcTriangulatedIrregularNetwork(cartesianPointList3D, coordIndex, flags);

            // assign output vals
            return new Dictionary<string, object>
            {
                { "PointList", pointList },
                { "DatabaseIfc", _vb }
            };
        }

    }
}


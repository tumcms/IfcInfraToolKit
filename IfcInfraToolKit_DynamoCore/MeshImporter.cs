using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using IfcInfraToolkit_Common;
using Off_GeomLibrary;


namespace IfcInfraToolKit_DynamoCore
{
    public class MeshImporter
    {
        /// <summary> Imports the off geometry from a given file </summary>
        /// <search> off, pointcloud </search>
        /// <returns> OffGeometry instance </returns>
        [MultiReturn(new[] { "OffGeometry" })]
        public static Dictionary<string, object> LoadOffGeometry(string [] offFilePath)
        {
            var geometries = new List<OffGeometry>();

            foreach (var path in offFilePath)
            {
                var geometry = new OffGeometry(path);
                geometries.Add(geometry);
            }
            
            
            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"OffGeometry", geometries}
            };

            return d;
        }

        /// <summary> visualizing Off Geometry </summary>
        /// <search> off, pointcloud </search>
        /// <returns>  </returns>
        [MultiReturn(new[] { "DynamoPointList" })]
        public static Dictionary<string, object> ToDynamoPoints(OffGeometry[] geometry)
        {
            var outerList = new List<List<Autodesk.DesignScript.Geometry.Point>>();

            foreach (var offGeometry in geometry)
            { 
                var dynPtList = new List<Autodesk.DesignScript.Geometry.Point>();

                foreach (var vertex in offGeometry.Vertices)
                {
                    Autodesk.DesignScript.Geometry.Point p = Point.ByCoordinates(vertex.X, vertex.Y, vertex.Z);
                    dynPtList.Add(p);
                }

                outerList.Add(dynPtList);
            }

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DynamoPointList", outerList}
            };

            return d;

        }


    }
}
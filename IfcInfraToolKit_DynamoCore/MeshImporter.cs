using System;
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

        /// <summary> visualizing Off Geometry </summary>
        /// <search> off, pointcloud </search>
        /// <returns>  </returns>
        [MultiReturn(new[] { "DynamoPatchList" })]
        public static Dictionary<string, object> ToDynamoPatches(OffGeometry[] geometry)
        {
            var patchList = new List<Surface>();

            // loop over all incoming off geometries
            foreach (var offGeometry in geometry)
            {
                // loop over all faces of the incoming off geometry and build a dynamo-based patch
                foreach (var face in offGeometry.Faces)
                {
                    var vertexIds = face.VertexIds;

                    var dynPts = new List<Point>();

                    foreach (var vertexId in vertexIds)
                    {
                        var pt = offGeometry.Vertices[vertexId];
                        var dynPoint = Point.ByCoordinates(pt.X, pt.Y, pt.Z);
                        dynPts.Add(dynPoint);
                    }

                    var curve = PolyCurve.ByPoints(dynPts, true);
                    var patch = Autodesk.DesignScript.Geometry.Surface.ByPatch(curve);
                    patchList.Add(patch);
                }

                
            }

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DynamoPatchList", patchList}
            };

            return d;

        }

        /// <summary> group single Off meshes </summary>
        /// <search> off, pointcloud </search>
        /// <returns>  </returns>
        /*[MultiReturn(new[] {"OffGeometry"})]
        public static Dictionary<string, object> JoinOffGeometries(OffGeometry [] geometries)
        {
            var util = new OffGeometryUtilities();
            var jointGeometry = util.JoinOffGeometries(geometries);

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"OffGeometry", jointGeometry}
            };

            return d;
        }

    */
    }
}
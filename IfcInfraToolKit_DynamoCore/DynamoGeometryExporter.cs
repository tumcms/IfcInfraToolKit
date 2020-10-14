using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace IfcInfraToolKit_DynamoCore
{
    public class DynamoGeometryExporter
    {
        [IsVisibleInDynamoLibrary(false)]   // Don't show this class in Dynamo but make it accessible among all dlls
        public DynamoGeometryExporter()
        {
        }

        /// <summary> calculates some geometric values of a given Dynamo solid geometry </summary>
        /// <search> pointcloud, bb </search>
        /// <returns>  </returns>
        [MultiReturn(new[] { "DatabaseContainer", "solidVertices", "solidEdges", "solidFaces", "centerOfGravity" })]
        public static Dictionary<string, object> GetGeometryInformation(Solid solidGeometry, DatabaseContainer databaseContainer, string productGuid)
        {
            var solidVertices = solidGeometry.Vertices;
            var solidEdges = solidGeometry.Edges;
            var solidFaces = solidGeometry.Faces;
            var centerOfGravity = solidGeometry.Centroid();
            
            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"solidVertices", solidVertices},
                {"solidEdges", solidEdges},
                {"solidFaces", solidFaces},
                {"centerOfGravity", centerOfGravity}
            };

            return d;

        }

    }
}

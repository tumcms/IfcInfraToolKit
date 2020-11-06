using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;

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
        public static Dictionary<string, object> ExportSolidGeometryAsBRep(Solid solidGeometry, DatabaseContainer databaseContainer, string productGuid)
        {
            var solidVertices = solidGeometry.Vertices;
            var solidEdges = solidGeometry.Edges;
            var solidFaces = solidGeometry.Faces;
            
            var centerOfGravity = solidGeometry.Centroid();

            // get current db
            var database = databaseContainer.Database;

            //// get host
            //var hostProduct = database.OfType<IfcProduct>()
            //    .FirstOrDefault(a => a.Guid.ToString() == productGuid);   

            //calculate half of the height of the bounding box in z-direction
            var middleBoundingBoxZ = (centerOfGravity.BoundingBox.MaxPoint.Z - centerOfGravity.BoundingBox.MinPoint.Z) / 2;
            //Set the z-coordinate to be at the bottom of the solid (or at the bottom of the bounding box)
            Point newCenterOfGravity= Point.ByCoordinates(centerOfGravity.X,centerOfGravity.Y,centerOfGravity.Z-middleBoundingBoxZ);

            var hostProduct = new IfcBuildingElementProxy(
                database.Project,
                new IfcLocalPlacement(
                    new IfcAxis2Placement3D(
                        new IfcCartesianPoint(database, centerOfGravity.X, centerOfGravity.Y, centerOfGravity.Z-middleBoundingBoxZ))),//set to centroid??
                null);

            var coordList = new List<Tuple<double, double, double>>();


            var faces = new List<IfcIndexedPolygonalFace>();

            foreach (var solidFace in solidFaces)
            {
                // map coordinates
                var indexMap = new List<int>();
                foreach (var faceVertex in solidFace.Vertices)
                {
                    var x = faceVertex.PointGeometry.X - centerOfGravity.X;
                    var y = faceVertex.PointGeometry.Y - centerOfGravity.Y;
                    var z = faceVertex.PointGeometry.Z - newCenterOfGravity.Z;
                    coordList.Add(new Tuple<double, double, double>(x,y,z));
                    indexMap.Add(coordList.Count());
                }

                // map face
                faces.Add(new IfcIndexedPolygonalFace(database, indexMap));
            }

            var coordinates = new IfcCartesianPointList3D(database, coordList);
            

            hostProduct.Representation = new IfcProductDefinitionShape(
                new IfcShapeRepresentation(
                    new IfcPolygonalFaceSet(
                        coordinates, 
                        faces
                        )
                    )
                );

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"solidVertices", solidVertices},
                {"solidEdges", solidEdges},
                {"solidFaces", solidFaces},
                {"centerOfGravity", newCenterOfGravity}
            };

            return d;

        }

    }
}

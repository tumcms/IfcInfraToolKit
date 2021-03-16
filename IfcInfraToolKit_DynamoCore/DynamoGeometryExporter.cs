using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

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
        public static Dictionary<string, object> ExportSolidGeometryAsBRep(Solid solidGeometry, DatabaseContainer databaseContainer, string parentGuid, string buildingElementType)
        {
            //ToDo: Convert the input parameter buildingElementType into a dropdown menu in Dynamo
            var solidVertices = solidGeometry.Vertices;
            var solidEdges = solidGeometry.Edges;
            var solidFaces = solidGeometry.Faces;
            
            var centerOfGravity = solidGeometry.Centroid();

            // get current db
            var database = databaseContainer.Database;

            //Get direct access to element for adding placement and representation
            //var buildingElement = database.OfType<IfcElement>().First(a => a.Guid.ToString() == hostProduct["ElementGUID"].ToString());

            //calculate half of the height of the bounding box in z-direction
            var middleBoundingBoxZ = (centerOfGravity.BoundingBox.MaxPoint.Z - centerOfGravity.BoundingBox.MinPoint.Z) / 2;

            //Set the z-coordinate to be at the bottom of the solid (or at the bottom of the bounding box)
            Point newCenterOfGravity= Point.ByCoordinates(centerOfGravity.X,centerOfGravity.Y,centerOfGravity.Z-middleBoundingBoxZ);

            //set the local placement
            IfcLocalPlacement placement = new IfcLocalPlacement(
                new IfcAxis2Placement3D(
                    new IfcCartesianPoint(database, centerOfGravity.X, centerOfGravity.Y,
                        centerOfGravity.Z - middleBoundingBoxZ)));


            // process geometry
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
            

            IfcProductDefinitionShape representation = new IfcProductDefinitionShape(
                new IfcShapeRepresentation(
                    new IfcPolygonalFaceSet(
                        coordinates, 
                        faces
                        )
                    )
                );
            //create correct instance of the BuildingElement

            var hostProduct = ElementDesigner.AddElement(databaseContainer, buildingElementType, representation,placement,"DefaultName", parentGuid);

            // writing the db back to the container
            databaseContainer.Database = database;

            // beautify return values
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
        /// <summary> calculates some geometric values of a given Dynamo solid geometry </summary>
        /// <search> pointcloud, bb </search>
        /// <returns>  </returns>
        [MultiReturn(new[] { "DatabaseContainer", "meshFaceIndices", "meshVertexNormals", "meshVertexPositions", "centerOfGravity" })]
        public static Dictionary<string, object> ExportMeshGeometryAsBRep(Mesh meshGeometry, DatabaseContainer databaseContainer, string parentGuid, string buildingElementType)
        {
            //ToDo: Convert the input parameter buildingElementType into a dropdown menu in Dynamo
            var meshFaceIndices = meshGeometry.FaceIndices;
            var meshVertexNormals = meshGeometry.VertexNormals;
            var meshVertexPositions = meshGeometry.VertexPositions;

            // get current db
            var database = databaseContainer.Database;

            //ToDo: get the correct Center Of Gravity and shift all coordinates accordingly
            //define Center of Gravity
            var newCenterOfGravity = new IfcCartesianPoint(database, 0, 0, 0);

            //Get all Coordinates and match them to an IfcCoordinateList
            var coordList = new List<Tuple<double, double, double>>();
            foreach (var meshPoint in meshVertexPositions)
            {
                coordList.Add(new Tuple<double, double, double>(meshPoint.X, meshPoint.Y, meshPoint.Z));
            }
            var coordinates = new IfcCartesianPointList3D(database, coordList);

            //Get all Indices for all faces and define them as IfcFaceIndexList
            var faceIndexList = new List<Tuple<int, int, int>>();
            foreach (var meshFace in meshFaceIndices)
            {
                faceIndexList.Add(new Tuple<int, int, int>((int)meshFace.A,(int)meshFace.B,(int)meshFace.C));
            }

            IfcProductDefinitionShape representation = new IfcProductDefinitionShape(
                new IfcShapeRepresentation(
                    new IfcTriangulatedFaceSet(
                        coordinates,
                        faceIndexList)
                    )
                );
            //create correct instance of the BuildingElement
            var hostProduct = ElementDesigner.AddElement(databaseContainer, buildingElementType, representation, null, "DefaultName", parentGuid);

            // beautify return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"meshFaceIndices", meshFaceIndices},
                {"meshVertexNormals", meshVertexNormals},
                {"meshVertexPositions", meshVertexPositions},
                {"centerOfGravity", newCenterOfGravity}
            };

            return d;

        }
    }
}

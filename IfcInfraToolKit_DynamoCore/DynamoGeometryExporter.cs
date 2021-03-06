using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

namespace IfcInfraToolKit_DynamoCore
{
    /// <summary>
    /// Management of all geometry-related export methods
    /// </summary>
    public class DynamoGeometryExporter
    {
        [IsVisibleInDynamoLibrary(false)]   // Don't show this class in Dynamo but make it accessible among all dlls
        internal DynamoGeometryExporter()
        {
        }

        /// <summary> calculates some geometric values of a given Dynamo solid geometry, converts it to a BRep and adds the geometry to a predefined IfcElement in the database container </summary>
        /// <param name="solidGeometry">Solid geometry data</param>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <param name="elementGuid">Guid of IfcElement subtype, to which the geometry should be added</param>
        /// <search> pointcloud, bb, solid, geometry, add geometry, add, BRep, convert geometry </search>
        /// <returns> Updated database container with added geometry, Lists of solid data</returns>
        [NodeCategory("Actions")]
        [MultiReturn(new[] { "DatabaseContainer", "solidVertices", "solidEdges", "solidFaces", "centerOfGravity" })]
        public static Dictionary<string, object> AddSolidGeometryAsBRep(Solid solidGeometry, DatabaseContainer databaseContainer, string elementGuid)
        {
            //ToDo: Convert the input parameter buildingElementType into a dropdown menu in Dynamo
            var solidVertices = solidGeometry.Vertices;
            var solidEdges = solidGeometry.Edges;
            var solidFaces = solidGeometry.Faces;
            
            var centerOfGravity = solidGeometry.Centroid();

            // get current db
            var database = databaseContainer.Database;

            //calculate half of the height of the bounding box in z-direction
            var middleBoundingBoxZ = (centerOfGravity.BoundingBox.MaxPoint.Z - centerOfGravity.BoundingBox.MinPoint.Z) / 2;

            //Set the z-coordinate to be at the bottom of the solid (or at the bottom of the bounding box)
            Point newCenterOfGravity= Point.ByCoordinates(centerOfGravity.X,centerOfGravity.Y,centerOfGravity.Z-middleBoundingBoxZ);

            //set the local placement
            var site = database.Project.UppermostSite();
            IfcAxis2Placement3D placement = new IfcAxis2Placement3D(new IfcCartesianPoint(database, centerOfGravity.X, centerOfGravity.Y, centerOfGravity.Z - middleBoundingBoxZ));
            IfcLocalPlacement localPlacement = new IfcLocalPlacement(site.ObjectPlacement, placement);

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

                    //var x = faceVertex.PointGeometry.X;
                    //var y = faceVertex.PointGeometry.Y;
                    //var z = faceVertex.PointGeometry.Z;

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

            // find host element by GUID
            var element = database.OfType<IfcElement>().FirstOrDefault(a => a.GlobalId == elementGuid);

            if (element == null)
            {
                var e = new Exception("Could not find IfcElement specified by given GUID. ");
                throw e; 
            }

            // add shape representation to product
            element.Representation = representation;
            element.ObjectPlacement = localPlacement;

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
        /// <summary> calculates some geometric values of a given Dynamo mesh geometry, converts it to a BRep and adds the geometry to a predefined IfcElement in the database container </summary>
        /// <param name="meshGeometry">mesh geometry data in dynamo format</param>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <param name="elementGuid">Guid of IfcElement subtype, to which the geometry should be added</param>
        /// <search> bb, mesh, geometry, add, BRep </search>
        /// <returns> Updated database container with added geometry, Lists of mesh data</returns>
        [NodeCategory("Actions")]
        [MultiReturn(new[] { "DatabaseContainer" })]
        public static Dictionary<string, object> AddMeshGeometryAsBRep(Mesh meshGeometry, DatabaseContainer databaseContainer, string elementGuid)
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
                faceIndexList.Add(new Tuple<int, int, int>((int)meshFace.A +1,(int)meshFace.B +1,(int)meshFace.C +1));
            }

            IfcProductDefinitionShape representation = new IfcProductDefinitionShape(
                new IfcShapeRepresentation(
                    new IfcTriangulatedFaceSet(
                        coordinates,
                        faceIndexList)
                    )
                );

            // find host element by GUID
            var element = database.OfType<IfcElement>().FirstOrDefault(a => a.GlobalId == elementGuid);

            if (element == null)
            {
                var e = new Exception("Could not find IfcElement specified by given GUID. ");
                throw e;
            }


            // append representation to element
            element.Representation = representation;


            // beautify return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer}
            };

            return d;

        }
    }
}

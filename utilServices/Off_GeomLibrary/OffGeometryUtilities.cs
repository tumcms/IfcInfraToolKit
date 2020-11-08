using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Off_GeomLibrary
{
    public class OffGeometryUtilities
    {
        /// <summary>
        /// combines several given OffGeometries into one offGeom
        /// </summary>
        /// <param name="geometries"></param>
        /// <returns></returns>
        public OffGeometry JoinOffGeometries(OffGeometry[] geometries)
        {
            var jointGeometry = new OffGeometry();

            int currentLastIndexVertex = 0;
            int currentLastIndexFace = 0;

            foreach (var geometry in geometries)
            {
                // add vertices at the end
                var numNewVertices = geometry.Vertices.Count; 
                jointGeometry.Vertices.AddRange(geometry.Vertices);

                // add faces and shift the vertexIndex
                foreach (var face in geometry.Faces)
                {
                    var updatedFace = new Face(face.NumVertices);
                    foreach (var faceVertexId in face.VertexIds)
                    {
                        updatedFace.VertexIds.Add(faceVertexId + currentLastIndexVertex);
                    }
                    jointGeometry.Faces.Add(updatedFace);
                    currentLastIndexFace++;
                }
            }

            return jointGeometry;
        }
    }
}
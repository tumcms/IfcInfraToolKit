using System.Collections.Generic;

namespace Off_GeomLibrary
{
    public class Face
    {
        public int NumVertices { get; set; }
        public List<int> VertexIds { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numVertices"></param>
        /// <param name="vertexIds"></param>
        public Face(int numVertices, List<int> vertexIds)
        {
            this.NumVertices = numVertices;
            this.VertexIds = vertexIds;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numVertices"></param>
        public Face(int numVertices)
        {
            this.NumVertices = numVertices;
        }
    }
}
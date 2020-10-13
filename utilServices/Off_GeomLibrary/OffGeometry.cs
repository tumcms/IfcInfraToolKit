using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Off_GeomLibrary
{
    public class OffGeometry
    {
        public List<Point3D> Vertices { get; set; }
        public List<Face> Faces { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OffGeometry()
        { 
            Vertices = new List<Point3D>();
            Faces = new List<Face>(); 
        }

        /// <summary>
        /// Constructor with load 
        /// </summary>
        public OffGeometry(string offFile)
        {
            // Create a NumberFormatInfo object and set some of its properties.
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            // read off file
            string[] rawTxt = File.ReadAllLines(offFile);

            // load nums
            var nums = rawTxt[1].Split(' ');
            var numVertices = int.Parse(nums[0]);
            int numFaces = int.Parse(nums[1]);
            int numEdges = int.Parse(nums[2]);


            // allocate storage
            Vertices = new List<Point3D>(numVertices);
            Faces = new List<Face>(numFaces);

            // load vertices
            for (int i = 2; i < numVertices +2; i++)
            {
                // load
                var rawPt = rawTxt[i].Split(' ');

                // cast to double
                var x = Convert.ToDouble(rawPt[0], provider);
                var y = Convert.ToDouble(rawPt[1], provider);
                var z = Convert.ToDouble(rawPt[2], provider);

                // add to list
                Vertices.Add(new Point3D(x, y, z));                
            }

            // load faces
            for (int i = numVertices + 2 ; i < (numVertices + numFaces +2); i++)
            {
                // load
                var rawFace = rawTxt[i].Split(' ');

                var numPts = int.Parse(rawFace[0]);

                var vertexIndices = new List<int>(); 
                for (int j = 1; j < numPts +1; j++)
                {
                    vertexIndices.Add(int.Parse(rawFace[j]));
                }

                // Add face
                Faces.Add(new Face(numPts, vertexIndices));
            }
        }
    }
}

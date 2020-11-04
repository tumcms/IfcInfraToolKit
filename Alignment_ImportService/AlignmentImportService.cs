using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alignment_ImportService
{
    public class AlignmentImportService
    {
        public List<Point3D> points { get; set; }

        public AlignmentImportService()
        {
            points = new List<Point3D>();
        }

        public List<Point3D> ImportFromCSV(string csvFilePath)
        {
            // Create a NumberFormatInfo object and set some of its properties.
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";
            provider.NumberGroupSizes = new int[] { 3 };

            // read off file
            string[] rawTxt = File.ReadAllLines(csvFilePath);

            // load nums
            var header = rawTxt[0].Split(',');

            var fileLength = rawTxt.Length;
            for (int i = 1; i < fileLength; i++)
            {
                var rawPt = rawTxt[i].Split(',');

                var pt = new Point3D()
                    {
                        X = Convert.ToDouble(rawPt[0], provider), 
                        Y = Convert.ToDouble(rawPt[1], provider) , 
                        Z = Convert.ToDouble(rawPt[2], provider)
                    };
                points.Add(pt);
            }

            return points;
        }
    }

    public class Point3D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}

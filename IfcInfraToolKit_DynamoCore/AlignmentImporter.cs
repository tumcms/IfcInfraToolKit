using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.Runtime.InteropServices;
using Alignment_ImportService;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Off_GeomLibrary;

namespace IfcInfraToolKit_DynamoCore
{
    public class AlignmentImporter
    {
        public AlignmentImporter()
        {

        }

        /// <summary> visualizing Off Geometry </summary>
        /// <search> off, pointcloud </search>
        /// <returns>  </returns>
        [MultiReturn(new[] { "DynamoPointList" })]
        public static Dictionary<string, object> ImportAlignmentAsPoints(string csvPath)
        {
            var importService = new AlignmentImportService();
            importService.ImportFromCSV(csvPath);

            var ptList = new List<Autodesk.DesignScript.Geometry.Point>();

            foreach (var pt in importService.points)
            {
                Autodesk.DesignScript.Geometry.Point p = Point.ByCoordinates(pt.X, pt.Y, pt.Z);
                ptList.Add(p);
            }

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DynamoPointList", ptList}
            };

            return d;

        }
    }
}
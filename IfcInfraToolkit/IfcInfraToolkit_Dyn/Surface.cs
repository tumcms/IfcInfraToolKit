// Sebastian Esser20200608

using System.Collections.Generic;
using System.Linq;
using Autodesk.AECC.Interop.Land;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace IfcInfraToolkit_Dyn
{
    public class Surface
    {
        private AeccSurface _surface;
        public string Name { get; set; }

        /// Constructor for a Civil 3D surface
        internal Surface(AeccSurface surface)
        {
            this._surface = surface;
            this.Name = surface.DisplayName;
        }
        
        /// Static Method to extract surface data for analysis
        [MultiReturn(new[] { "NumPoints", "NumTris", "Area2D", "Area3D", "MaxElev", "MinElev", "MaxGrade", "MinGrade" })] // DesignScript.Runtime
        public static Dictionary<string, object> GetTerrainStatistics(Surface Civil3DSurface)
        {
            AeccSurface surf = Civil3DSurface._surface;
            AeccTinSurface tinSurf = null;
            AeccGridSurface gridSurf = null;
            AeccTinVolumeSurface tinVolSurf = null;
            AeccGridVolumeSurface gridVolSurf = null;
            switch (surf.Type)
            {
                case AeccSurfaceType.aecckGridSurface:
                    //surf1 = surf as AeccGridSurface;
                    break;
                case AeccSurfaceType.aecckTinSurface:
                    tinSurf = surf as AeccTinSurface;
                    break;
                case AeccSurfaceType.aecckGridVolumeSurface:
                    //surf1 = surf as AeccGridVolumeSurface;
                    break;
                case AeccSurfaceType.aecckTinVolumeSurface:
                    //surf1 = surf as AeccTinVolumeSurface;
                    break;
                default:
                    break;
            }
            AeccTinSurfaceStatistics tinStats = tinSurf.Statistics;
            int numPoints = tinStats.NumberOfPoints;
            int numTri = tinStats.NumberOfTriangles;
            double area2D = tinStats.Area2d;
            double area3D = tinStats.Area3d;
            double maxElev = tinStats.MaxElevation;
            double minElev = tinStats.MinElevation;
            double maxGrade = tinStats.MaxGrade;
            double minGrade = tinStats.MinGrade;
            return new Dictionary<string, object>
            {
                { "NumPoints", numPoints },
                { "NumTris", numTri },
                { "Area2D", area2D },
                { "Area3D", area3D },
                { "MaxElev", maxElev },
                { "MinElev", minElev },
                { "MaxGrade", maxGrade },
                { "MinGrade", minGrade }
            };
        }

        /// Static method to drape a list of Dynamo points onto a surface
        public static IList<Point> DrapePoints(Surface Civil3DSurface, IList<Point> Points)
        {
            IList<Point> ptsList = new List<Point>();
            foreach (Point pt in Points)
            {
                double z = Civil3DSurface._surface.FindElevationAtXY(pt.X, pt.Y);
                ptsList.Add(Point.ByCoordinates(pt.X, pt.Y, z));
            }
            var ptsListPruned = Point.PruneDuplicates(ptsList, 0.001).ToList();
            return ptsListPruned;
        }
        /// Override the string output for the Civil 3D Surface object
        public override string ToString()
        {
            return string.Format($"Surface (\"{this.Name}\")");
        }

    }
}
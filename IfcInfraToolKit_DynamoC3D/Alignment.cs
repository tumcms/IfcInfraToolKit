// Sebastian Esser20200608

using System.Collections.Generic;
using System.Linq;
using Autodesk.AECC.Interop.Land;
using Autodesk.DesignScript.Runtime;

namespace IfcInfraToolkit_Dyn
{
    public class Alignment
    {
        private AeccAlignment _alignment;
        private AeccAlignmentEntities _entities;
        public double EndStation { get; set; }

        public double StartStation { get; set; }

        public double Length { get; set; }

        public string Name { get; set; }

        /// Constructor for a Civil 3D Alignment
        internal Alignment(AeccAlignment alignment)
        {
            this._alignment = alignment;
            this._entities = alignment.Entities;
            this.Name = alignment.DisplayName;
            this.Length = alignment.Length;
            this.StartStation = alignment.StartingStation;
            this.EndStation = alignment.EndingStation;
        }

       
        /// Extract Civil 3D Alignment Station, Offset and Elevation data
        [MultiReturn(new[] { "Station", "Easting", "Northing" })]
        public static Dictionary<string, object> GetAlignmentStations(Alignment Alignment, double Interval, bool Geometry)
        {
            AeccAlignmentStations stations = Alignment._alignment.GetStations(AeccStationType.aeccMajor, Interval, 1.0);
            IList<AeccAlignmentStation> stationsList = new List<AeccAlignmentStation>();
            IList<double> s = new List<double>();
            IList<double> e = new List<double>();
            IList<double> n = new List<double>();
            foreach (AeccAlignmentStation station in stations)
            {
                stationsList.Add(station);
            }
            // if geometry points are included
            if (Geometry == true)
            {
                AeccAlignmentStations stationsGeom = Alignment._alignment.GetStations(AeccStationType.aeccGeometryPoint, Interval, 1.0);
                foreach (AeccAlignmentStation station in stationsGeom)
                {
                    stationsList.Add(station);
                }
            }
            // Sort and remove duplicates (if geometry selected)
            var uniqueList = stationsList.GroupBy(i => i.Station).Select(j => j.FirstOrDefault());
            var sortedList = uniqueList.OrderBy(i => i.Station);
            foreach (AeccAlignmentStation stat in sortedList)
            {
                s.Add(stat.Station);
                e.Add(stat.Easting);
                n.Add(stat.Northing);
            }
            return new Dictionary<string, object>
            {
                {"Station", s},
                {"Easting", e},
                {"Northing", n}
            };
        }

        /// Get the Civil 3D Profiles associated with an Alignment
        public IList<Profile> GetProfiles()
        {
            IList<Profile> profiles = new List<Profile>();
            foreach (AeccProfile profile in this._alignment.Profiles)
            {
                profiles.Add(new Profile(profile));
            }
            return profiles;
        }

        /// Override the string output for the Civil 3D Alignment object
        public override string ToString()
        {
            return string.Format($"Alignment (Name = {this.Name}, Length = {this.Length.ToString("#.###")} )");
        }
    }
}
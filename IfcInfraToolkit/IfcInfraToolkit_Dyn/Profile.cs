// Sebastian Esser20200608

using System;
using System.Collections.Generic;
using Autodesk.AECC.Interop.Land;

namespace IfcInfraToolkit_Dyn
{
    public class Profile
    {
        private AeccProfile _profile;
        public string Name { get; set; }

        /// Constructor for a Civil 3D Profile
        internal Profile(AeccProfile profile)
        {
            this._profile = profile;
            this.Name = profile.DisplayName;
        }

       public IList<double> GetProfileElevationByStation(Alignment Alignment, IList<double> Stations)
        {
            IList<double> elevations = new List<double>();
            foreach (double station in Stations)
            {
                try
                {
                    elevations.Add(this._profile.ElevationAt(station));
                }
                catch (Exception)
                {
                    elevations.Add(-999.000);
                }
            }
            return elevations;
        }

        /// <summary>
        /// Override the string output for the Civil 3D Profile object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format($"Profile (Name = {this.Name} )");
        }


    }
}
// Sebastian Esser20200608

using System;
using static System.Math;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AECC.Interop.Land;
using Autodesk.AutoCAD.Interop.Common;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;
using IfcInfraToolKit_DynamoCore;
using Autodesk.AutoCAD.Interop;
using NUnit.Framework.Internal;

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


        //Direction is measured diffrently in C3D compared to IFC
        //-> Corretion C3D measure Clockwise start at Y axis
        static double angleconv(double direction)
        {

            direction = direction - (PI / 2);
            if (direction <= 0)
            {
                direction = direction * (-1);
            }
            else
            {
                direction = (2 * PI) - direction;
            }

            return (direction);
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

        //quick solution for Alignment export
        /// <summary>
        /// Adds an alignment curve by points to the project and links it with the IfcSite entity
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "AlignmentGUID" })]
        public static Dictionary<string, object> IfcAddAlignmentByPoints(DatabaseContainer databaseContainer,
            string Alignmentname,
            List<double> x,
            List<double> y,
            List<double> z) { 
        
            var db = databaseContainer.Database;
            IfcSite site = db.OfType<IfcSite>().First();
            //Error Handling
            if (x.Count != y.Count || x.Count != z.Count)
            {
                throw new Exception("Pointlist must have the same lenght!\n");
            }

            List<IfcCartesianPoint> points = new List<IfcCartesianPoint>();

            //Create Points for Polyline
            for (int i = 0; i < x.Count; i++)
            {
                points.Add(new IfcCartesianPoint(db, x[i], y[i], z[i]));
            }

            //Create Alingment with Polyline
            IfcPolyline polyline = new IfcPolyline(points);
            IfcAlignment alignment = new IfcAlignment(site, polyline);
            alignment.Name = Alignmentname;

            var re = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"AlignmentGUID", alignment.Guid}
            };

            return re;
            }

        //TODO: Implement Vertical Segemnts / Testing
        /// <summary>
        /// Adds an alignment curve to the project and links it with the IfcSite entity
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "AlignmentGUID" })]
        public static Dictionary<string, object> IfcAddAlignmentByCurve(
            DatabaseContainer databaseContainer,
            Alignment alignment,
            bool twoDim = true,
            string Alignmentname = "DefaultAlignment")
        {
            var db = databaseContainer.Database;
            IfcSite site = db.OfType<IfcSite>().First();

            //Errorhandling
            if (alignment == null)
            {
                throw new ArgumentNullException("No Alignment found!");
            }

            //standard informations
            IfcAlignmentCurve curve = new IfcAlignmentCurve(db);
            IfcAlignment ifcalignment = new IfcAlignment(site, curve);
            ifcalignment.Name = Alignmentname;

            var segmentshoz = new List<IfcAlignment2DHorizontalSegment>();


            //Horizontal Export of alignment
            var entities = alignment._entities;
            foreach (AeccAlignmentCurve ae in entities)
            {
                //Circular handling
                if (ae.Type == AeccAlignmentEntityType.aeccArc)
                {
                    //Get Values of the Circular element
                    AeccAlignmentArc allvalues = ae as AeccAlignmentArc;
                    var startx = allvalues.StartEasting;
                    var starty = allvalues.StartNorthing;
                    var lenght = allvalues.Length;
                    var direction = allvalues.StartDirection;
                    var clockwise = allvalues.Clockwise;
                    var radius = allvalues.Radius;
                    direction = angleconv(direction);


                    //Convert data into IFC
                    var start = new IfcCartesianPoint(db, startx, starty);
                    var seg = new IfcCircularArcSegment2D(start, direction, radius, lenght, !clockwise);
                    var tmp = new IfcAlignment2DHorizontalSegment(seg);
                    segmentshoz.Add(tmp);
                    continue;

                }

                // strait lines handling
                if (ae.Type == AeccAlignmentEntityType.aeccTangent)
                {
                    AeccAlignmentTangent allvalues = ae as AeccAlignmentTangent;
                    //Get Values of the line
                    var startx = allvalues.StartEasting;
                    var starty = allvalues.StartNorthing;
                    var direction = allvalues.Direction;
                    var length = allvalues.Length;
                    direction = angleconv(direction);


                    //Convert Values into IFC
                    var start = new IfcCartesianPoint(db, startx, starty);
                    var seg = new IfcLineSegment2D(start, direction, length);
                    var tmp = new IfcAlignment2DHorizontalSegment(seg);
                    segmentshoz.Add(tmp);

                }


            }

            //Errorhandling no Segments
            if (segmentshoz.Count == 0)
            {
                throw new Exception("No Horizontale Segemente found!");
            }


            var segmentsvert = new List<IfcAlignment2DVerticalSegment>();
            //Vertikal Export of alignemt
            if (twoDim == false)
            {
                var aeccAlignment = alignment._alignment;


                //Errorhandling
                if (aeccAlignment.Profiles == null)
                {
                    throw new Exception("No Profil found -> maybe only 2D?");
                }


                
                //check every profil -> needs to be changed now its exporting every profil
                foreach (AeccProfile ap in aeccAlignment.Profiles)
                {

                    //Iterate through all segments
                    AeccProfileEntities ape = ap.Entities;
                    var iter = ape.GetEnumerator();
                    while(iter.MoveNext())
                    {

                        //Check which Entity are used
                        IAeccProfileEntity enti = iter.Current as IAeccProfileEntity;
                        AeccProfileEntityType entitype = enti.Type;

                        //Vertical Line
                        if (entitype.ToString().Equals(AeccProfileEntityType.aeccProfileEntityTangent.ToString()))
                        {
                            //Gather all infos for the segments
                            aeccProfileTangent expo= enti as aeccProfileTangent;
                            var startst = expo.StartStation;
                            var starthi = expo.StartElevation;
                            var grad = expo.Grade;
                            var length = expo.Length;


                            //add Segments to exportlist
                            var verseg = new IfcAlignment2DVerSegLine(db, startst, length, starthi, grad);
                            segmentsvert.Add(verseg);
                            

                        }

                        //Vertical Circular
                        if (entitype.ToString().Equals(AeccProfileEntityType.aeccProfileEntityCurveCircular.ToString()))
                        {
                            //Gather all infos for the segments
                            aeccProfileCurveCircular expo = enti as aeccProfileCurveCircular;
                            var startst = expo.StartStation;
                            var starthi = expo.StartElevation;
                            var endhi = expo.EndElevation;
                            var length = expo.Length;   //not sure
                            var radius = expo.Radius;
                            var grad = expo.GradeIn;
                            var curvetype = expo.CurveType;
                            bool convex = true;
                            //Crest == Convex and Sag== Concave
                            //default -> Convex change if the curve is Concave
                            if (curvetype.ToString().Equals(AeccProfileVerticalCurveType.aeccSag.ToString()))
                            {
                                convex = false;
                            }


                            //add Segments to exportlist
                            var verseg = new IfcAlignment2DVerSegCircularArc(db, startst, length, starthi, grad, radius, convex); 
                            segmentsvert.Add(verseg);
                        }


                        //parabola -> needs testing
                        if (entitype.ToString().Equals(AeccProfileEntityType.aeccProfileEntityCurveSymmetricParabola.ToString()))
                        {
                            //Gather all infos for the segments
                            AeccProfileCurveParabolic expo = enti as AeccProfileCurveParabolic;
                            var startsi = expo.StartStation;
                            var starthi = expo.StartElevation;
                            var grad = expo.GradeIn;
                            var lenght = expo.Length;
                            var paracon = expo.Radius; //Not sure if right maybe change of sign
                            var curvetype = expo.CurveType;
                            bool convex = true;
                            //Crest == Convex and Sag== Concave
                            //default -> Convex change if the curve is Concave
                            if (curvetype.ToString().Equals(AeccProfileVerticalCurveType.aeccSag.ToString()))
                            {
                                convex = false;
                            }

                            //add Segments to exportlist
                            var verseg = new IfcAlignment2DVerSegParabolicArc(db, startsi, lenght, starthi, grad, paracon, convex);
                            segmentsvert.Add(verseg);

                        }

                    }

                }

            }



            //Save Data into Curve
            IfcAlignment2DHorizontal horizontal = new IfcAlignment2DHorizontal(segmentshoz);
            curve.Horizontal = horizontal;

            //Placement in 2D or 3D
            //not sure if necessary
            if (twoDim == true)
            {
                var placement = new IfcAxis2Placement2D(db);
                ifcalignment.ObjectPlacement = new IfcLocalPlacement(placement);
            }

            //TODO: Impliment
            if (twoDim == false)
            {
                IfcAlignment2DVertical vertical = new IfcAlignment2DVertical(segmentsvert);
                curve.Vertical = vertical;
            }

            //end testing

            //return values
            var re = new Dictionary<string, object>
                {
                    {"DatabaseContainer", databaseContainer},
                    {"AlignmentGUID", ifcalignment.Guid}
                };

            return re;
        }


    }





}


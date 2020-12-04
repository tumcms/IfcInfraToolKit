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
        //Change Horizontal Segemts for RC2 + Add Spiral for Horizontal
        /// <summary>
        /// Adds an alignment curve to the project and links it with the IfcSite entity
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "AlignmentGUID" })]
        public static Dictionary<string, object> IfcAddAlignmentByCurve(
            DatabaseContainer databaseContainer,
            Alignment alignment,
            bool twoDim = true,
            int profilnum = 2,
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
            IfcAlignment ifcalignment = new IfcAlignment(site);
            ifcalignment.Name = Alignmentname;
            if (twoDim == true)
            {
                var placement = new IfcAxis2Placement2D(new IfcCartesianPoint(db,0,0));
                ifcalignment.ObjectPlacement = new IfcLocalPlacement(placement);
            }
            else
            {
                var placement = new IfcAxis2Placement3D(new IfcCartesianPoint(db, 0, 0, 0));
                ifcalignment.ObjectPlacement = new IfcLocalPlacement(placement);

            }

            //Create Containers for IFCCurve
            var segmentshoz = new List<IfcAlignmentHorizontalSegment>();
            var segmentsvert = new List<IfcAlignmentVerticalSegment>();
            var compsegshoz = new List<IfcCurveSegment>();
            var compsegsver = new List<IfcCurveSegment>();

            //Horizontal Export of alignment
            //ToDo:Circular update to RC2 + Clothoid implemnent
            var entities = alignment._entities;
            var last = entities.Count;
            var count = 1;
            foreach (AeccAlignmentCurve ae in entities)
            {
                // line handling
                if (ae.Type == AeccAlignmentEntityType.aeccTangent)
                {
                    AeccAlignmentTangent allvalues = ae as AeccAlignmentTangent;
                    //Get Values of the C3D line 
                    var startx = allvalues.StartEasting;
                    var starty = allvalues.StartNorthing;
                    var direction = allvalues.Direction;
                    var length = allvalues.Length;
                    direction = angleconv(direction);


                    //Convert Values into IFC Sematic
                    var start = new IfcCartesianPoint(db, startx, starty);
                    var tmp = new IfcAlignmentHorizontalSegment(start, direction, 0, 0, length, IfcAlignmentHorizontalSegmentTypeEnum.LINE);
                    segmentshoz.Add(tmp);


                    //Convert data into IFC Gemometric
                    var dir = new IfcDirection(db, 1, Tan(direction));
                    var vector = new IfcVector(dir, length);
                    var line = new IfcLine(start, vector);
                    var place = new IfcAxis2Placement2D(start);
                    var contin = IfcTransitionCode.CONTINUOUS;

                    if (count == last)
                    {
                        contin = IfcTransitionCode.DISCONTINUOUS;
                    }

                    var temp_comp = new IfcCurveSegment(contin, place, length, line);

                    // Connection of Semantic and Geometric on 1st order
                    //var connect_geo = new IfcShapeRepresentation(temp_comp);
                    //var connect_sem = new IfcProductDefinitionShape(connect_geo);

                    compsegshoz.Add(temp_comp);
                    count++;
                    continue;

                }


                //Circular handling
                //TODO: Add Geometic Representation
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



                    var start = new IfcCartesianPoint(db, startx, starty);
                    var tmp = new IfcAlignmentHorizontalSegment(start, direction, radius, radius, lenght, IfcAlignmentHorizontalSegmentTypeEnum.CIRCULARARC);
                    segmentshoz.Add(tmp);
                    count++;
                    continue;


                }
               

                //Spiral / Clothoid handling
                //TODO: Testing
                if (ae.Type == AeccAlignmentEntityType.aeccSpiral)
                {
                    AeccAlignmentSpiral allvalues = ae as AeccAlignmentSpiral;

                    var startx = allvalues.StartEasting;
                    var starty = allvalues.StartNorthing;
                    var length = allvalues.Length;
                    var direction = allvalues.StartDirection;
                    direction = angleconv(direction);
                    var startradius = allvalues.RadiusIn;
                    var endradius = allvalues.RadiusOut;
                    var clothoidconstant = allvalues.A;
 

                    //Convert Values into IFC Sematic
                    var start = new IfcCartesianPoint(db, startx, starty);
                    var tmp = new IfcAlignmentHorizontalSegment(start, direction, startradius, 
                        endradius, length, IfcAlignmentHorizontalSegmentTypeEnum.CLOTHOID);
                    segmentshoz.Add(tmp);



                    //Convert data into IFC Gemometric
                    var place = new IfcAxis2Placement2D(start);
                    var clothoid = new IfcClothoid(place, clothoidconstant);
                    var contin = IfcTransitionCode.CONTINUOUS;
                    if (count == last)
                    {
                        contin = IfcTransitionCode.DISCONTINUOUS;
                    }

                    var temp_comp = new IfcCurveSegment(contin, place, length, clothoid);


                    compsegshoz.Add(temp_comp);
                    count++;
                    continue;
                }

            }



            //Errorhandling no Segments
            if (segmentshoz.Count == 0)
            {
                throw new Exception("No Horizontale Segemente found!");
            }


            //put together horizontal segments
            var basecurve = new IfcCompositeCurve(compsegshoz, IfcLogicalEnum.FALSE);

            //Save Data into Curve horizontal
            IfcAlignmentHorizontal horizontal = new IfcAlignmentHorizontal(ifcalignment, segmentshoz);

            //Connect Sematic and Geometric Horizontal 2nd order
            var share = new IfcShapeRepresentation(basecurve);
            var pds = new IfcProductDefinitionShape(share);
            horizontal.Representation = pds;


            //Vertikal Export of alignment
            //need to be adjusted for IFC4.3RC2 
            if (twoDim == false)
            {
                var aeccAlignment = alignment._alignment;



                //Errorhandling
                if (aeccAlignment.Profiles == null)
                {
                    throw new Exception("No Profil found -> maybe only 2D?");
                }


                AeccProfile ap=new AeccProfile();
                var count_prof = 1;
                //Select Profil
                foreach (AeccProfile ap_tmp in aeccAlignment.Profiles)
                { 
                    if (count_prof == profilnum)
                    {
                        ap = ap_tmp;
                        break;
                    }
                    count_prof++;
                }


                double current_length = 0;

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
                        var starthi = expo.StartElevation;
                        var grad = expo.Grade;
                        var lengthhoz = expo.Length*Cos(grad); //should be right


                        //add Segments to exportlist for vertical export
                        IfcAlignmentVerticalSegment verseg = new IfcAlignmentVerticalSegment(db, current_length, lengthhoz,
                            starthi, grad, grad, IfcAlignmentVerticalSegmentTypeEnum.CONSTANTGRADIENT);
                        segmentsvert.Add(verseg);



                        //testing for IFC Curve Date into line
                        var dir = new IfcDirection(db, 1, Tan(grad));
                        var vector = new IfcVector(dir, expo.Length);
                        var start = new IfcCartesianPoint(db, 0, 0);
                        var line = new IfcLine(start, vector);
                        var pointdist = new IfcPointByDistanceExpression(current_length, basecurve);
                        var place = new IfcAxis2PlacementLinear(pointdist);
                        
                        //update horizontal length
                        current_length += lengthhoz;



                        var contin = IfcTransitionCode.CONTINUOUS;
                        //Last Segment needs to be Discontinuous
                        if (!(iter.MoveNext()))
                        {
                            contin = IfcTransitionCode.DISCONTINUOUS;
                        }
                        var temp_comp = new IfcCurveSegment(contin, place, expo.Length, line);
                        compsegsver.Add(temp_comp);

                        continue;
                    }

                    //Vertical Circular
                    //TODO: Add Geometric representation
                    if (entitype.ToString().Equals(AeccProfileEntityType.aeccProfileEntityCurveCircular.ToString()))
                    {
                        //Gather all infos for the segments
                        aeccProfileCurveCircular expo = enti as aeccProfileCurveCircular;
                        var startst = expo.StartStation;
                        var starthi = expo.StartElevation;
                        var endhi = expo.EndElevation;
                        var endst = expo.EndStation;
                        var radius = expo.Radius;
                        var gradin = expo.GradeIn;
                        var gradout = expo.GradeOut;
                        var pathlength = expo.Length;
                        var curvetype = expo.CurveType;
                        bool convex = true;
                        //Crest == Convex and Sag== Concave
                        //default -> Convex change if the curve is Concave
                        if (curvetype.ToString().Equals(AeccProfileVerticalCurveType.aeccSag.ToString()))
                        {
                            convex = false;
                        }


                        //horizonal lenght calc
                        //calc chord length
                        var angle = pathlength / radius;
                        var chordl = 2 * radius * Sin(angle / 2); //always hypotenuse
                        var elevationdelta = expo.EndElevation - expo.StartElevation; 
                        var lengthhoz = Sqrt(chordl*chordl - elevationdelta *elevationdelta);


                        //add Segments to exportlist
                        var verseg = new IfcAlignmentVerticalSegment(db, current_length, lengthhoz, starthi,
                            gradin,gradout, IfcAlignmentVerticalSegmentTypeEnum.CIRCULARARC);
                        segmentsvert.Add(verseg);
                        current_length += lengthhoz;
                        continue;
                    }


                    //parabola -> currently not working
                    //TODO: Add Geometric representation
                    if (entitype.ToString().Equals(AeccProfileEntityType.aeccProfileEntityCurveSymmetricParabola.ToString()))
                    {
                        //Gather all infos for the segments
                        AeccProfileCurveParabolic expo = enti as AeccProfileCurveParabolic;
                        var starthi = expo.StartElevation;
                        var endhi = expo.EndElevation;
                        var endst = expo.EndStation;
                        var length = expo.Length; //leng
                        var grad = expo.GradeIn;
                        var paracon = expo.Radius; //Not sure if right maybe change of sign
                        var curvetype = expo.CurveType;
                        bool convex = true;
                        //Crest == Convex and Sag== Concave
                        //default -> Convex change if the curve is Concave
                        if (curvetype.ToString().Equals(AeccProfileVerticalCurveType.aeccSag.ToString()))
                        {
                            convex = false;
                        }
                        var min = expo.HighLowPointElevation;



                       // var verseg = new IfcAlignmentVerticalSegment(db, current_length, lengthhoz, starthi,
                       //     grad, IfcAlignmentVerticalSegmentTypeEnum.PARABOLICARC);
                       //segmentsvert.Add(verseg);
                       //current_length += length;

                    }

                }

                

            }


            //Save Data into Curve vertical
            if (twoDim == false)
            {
                IfcAlignmentVertical vertical = new IfcAlignmentVertical(ifcalignment,segmentsvert);
            }


            //Further testing put together all segments for IFCCurve
            var curve = new IfcGradientCurve(basecurve,compsegsver);
            ifcalignment.Axis = basecurve;

            //Connect Semantic and Geo on 3rd order
            var shaperep = new IfcShapeRepresentation(curve);
            var alignment2gradient = new IfcProductDefinitionShape(shaperep);
            ifcalignment.Representation = alignment2gradient;



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


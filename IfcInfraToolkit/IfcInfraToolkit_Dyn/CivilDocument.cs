﻿// Sebastian Esser20200608
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime;
using System.Runtime.InteropServices;

using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Interop.Common;
using Autodesk.AECC.Interop.UiRoadway;
using Autodesk.AECC.Interop.Roadway;
using Autodesk.AECC.Interop.Land;
using System.Reflection;

using Autodesk.DesignScript.Runtime;

using Autodesk.DesignScript.Geometry;

using System.Xml;
using System.Globalization;

namespace IfcInfraToolkit_Dyn
{
    public class CivilDocument
    {
        #region PRIVATE PROPERTIES
        /// <summary>
        /// The document
        /// </summary>
        internal AeccRoadwayDocument _document;
        /// <summary>
        /// The document name.
        /// </summary>
        public string Name { get { return _document.Name; } }
        /// <summary>
        /// The corridors
        /// </summary>
        private AeccCorridors _corridors;
        /// <summary>
        /// The alignments
        /// </summary>
        private AeccAlignmentsSiteless _alignments;
        /// <summary>
        /// The Surfaces
        /// </summary>
        private AeccSurfaces _surfaces;
        /// <summary>
        /// Gets the internal element.
        /// </summary>
        /// <value>
        /// The internal element.
        /// </value>
        internal object InternalElement { get { return this._document; } }
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of the <see cref="CivilDocument"/> class.
        /// </summary>
        /// <param name="_doc">The document.</param>
        internal CivilDocument(AeccRoadwayDocument _doc)
        {
            this._document = _doc;
            _corridors = _doc.Corridors;
            _alignments = _doc.AlignmentsSiteless;
            _surfaces = _doc.Surfaces;
        }
        #endregion

        #region PUBLIC METHODS
        /// <summary>
        /// Gets the corridors.
        /// </summary>
        /// <returns></returns>
        //public IList<Corridor> GetCorridors()
        //{
        //    Utils.Log(string.Format("CivilDocument.GetCorridors started...", ""));

        //    IList<Corridor> output = new List<Corridor>();

        //    foreach (AeccCorridor corridor in this._document.Corridors)
        //    {
        //        output.Add(new Corridor(corridor, this._document));
        //    }

        //    Utils.Log(string.Format("CivilDocument.GetCorridors completed.", ""));

        //    return output;
        //}

        /// <summary>
        /// Get the corridor by name.
        /// </summary>
        /// <param name="name">The corridor name.</param>
        /// <returns></returns>
        //public Corridor GetCorridorByName(string name)
        //{
        //    return this.GetCorridors().First(x => x.Name == name);
        //}

        /// <summary>
        /// Gets the alignments.
        /// </summary>
        /// <returns></returns>
        public IList<Alignment> GetAlignments()
        {
           // Utils.Log(string.Format("CivilDocument.GetAlignments started...", ""));

            IList<Alignment> output = new List<Alignment>();

            foreach (AeccAlignment a in this._alignments)
            {
                output.Add(new Alignment(a));
            }

          //  Utils.Log(string.Format("CivilDocument.GetAlignments completed.", ""));

            return output;
        }

        /// <summary>
        /// Gets alignment by name.
        /// </summary>
        /// <param name="name">The alignment name.</param>
        /// <returns></returns>
        public Alignment GetAlignmentByName(string name)
        {
            return this.GetAlignments().First(x => x.Name == name);
        }

        /// <summary>
        /// Gets all surfaces in the document
        /// </summary>
        /// <returns>
        /// List of surfaces
        /// </returns>
        //public IList<CivilSurface> GetSurfaces()
        //{
        //   // Utils.Log(string.Format("CivilDocument.GetSurfaces started...", ""));

        //    IList<CivilSurface> output = new List<CivilSurface>();

        //    foreach (AeccSurface s in this._surfaces)
        //    {
        //        output.Add(new CivilSurface(s));
        //    }

        //    Utils.Log(string.Format("CivilDocument.GetSurfaces completed.", ""));

        //    return output;
        //}

        /// <summary>
        /// Gets surface by name.
        /// </summary>
        /// <param name="name">The name of the surface</param>
        /// <returns>
        /// Civil Surface
        /// </returns>
        //public CivilSurface GetSurfaceByName(string name)
        //{
        //    return this.GetSurfaces().First(x => x.Name == name);
        //}

        #region AUTOCAD METHODS
        /// <summary>
        /// Adds the arc to the document.
        /// </summary>
        /// <param name="arc">The arc.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string AddArc(Arc arc, string layer)
        //{
        //    return Utils.AddArcByArc(this._document, arc, layer);
        //}

        /// <summary>
        /// Adds the circle to the document.
        /// </summary>
        /// <param name="circle">The circle.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string AddCircle(Circle circle, string layer)
        //{
        //    return Utils.AddCircleByCircle(this._document, circle, layer);
        //}

        /// <summary>
        /// Adds the 2D polyline by points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string AddLWPolylineByPoints(IList<Point> points, string layer)
        //{
        //    return Utils.AddLWPolylineByPoints(this._document, points, layer);
        //}

        /// <summary>
        /// Adds the 3D polyline by curve.
        /// </summary>
        /// <param name="curve">The curve.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string AddPolylineByCurve(Curve curve, string layer)
        //{
        //    return Utils.AddPolylineByCurve(this._document, curve, layer);
        //}

        /// <summary>
        /// Adds the 3D polylines by curve.
        /// </summary>
        /// <param name="curves">The curves.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string AddPolylineByCurve(IList<Curve> curves, string layer)
        //{
        //    return Utils.AddPolylineByCurves(this._document, curves, layer);
        //}

        /// <summary>
        /// Adds the region by closed profile.
        /// </summary>
        /// <param name="closedCurve">The closed curve.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string AddRegionByPatch(Curve closedCurve, string layer)
        //{
        //    return Utils.AddRegionByPatch(this._document, closedCurve, layer);
        //}

        /// <summary>
        /// Creates a closed profile form the points and adds the extruded solid.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="height">The height. By Default is equal to 1.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string AddExtrudedSolidByPoints(IList<Point> points, double height = 1, string layer = "_CivilConnectionSolid")
        //{
        //    return Utils.AddExtrudedSolidByPoints(this._document, points, height, layer);
        //}

        /// <summary>
        /// Adds the extruded solid by closed profile.
        /// </summary>
        /// <param name="closedCurve">The closed curve.</param>
        /// <param name="height">The height. By Default is equal to 1.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string AddExtrudedSolidByPatch(Curve closedCurve, double height = 1, string layer = "_CivilConnectionSolid")
        //{
        //    return Utils.AddExtrudedSolidByPatch(this._document, closedCurve, height, layer);
        //}

        /// <summary>
        /// Adds multiple extruded solid by closed curves.
        /// </summary>
        /// <param name="curves">The curves.</param>
        /// <param name="height">The height. By Default is equal to 1.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string AddExtrudedSolidByCurves(IList<Curve> curves, double height = 1, string layer = "_CivilConnectionSolid")
        //{
        //    return Utils.AddExtrudedSolidByCurves(this._document, curves, height, layer);
        //}

        /// <summary>
        /// Adds a new layer to the Civil Document by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        //public string AddLayer(string name)
        //{
        //    Utils.AddLayer(this._document, name);
        //    return name;
        //}

        /// <summary>
        /// Creates a text in the CivilDocument and rotates it to match the plane.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="point">The point.</param>
        /// <param name="textHeight">Height of the text.</param>
        /// <param name="layer">The layer.</param>
        /// <param name="cs">The cs.</param>
        /// <returns></returns>
        //public string AddText(string text, Point point, double textHeight, string layer, CoordinateSystem cs)
        //{
        //    return Utils.AddText(this._document, text, point, textHeight, layer, cs);
        //}

        /// <summary>
        /// Creates an extrusion based on a closed curve (Polycurve, Polygon or Rectangle) profile to be used to cut the solids in the Civil Document.
        /// </summary>
        /// <param name="closedCurve">The closed curve.</param>
        /// <param name="height">The height. By Default is equal to 1.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public bool CutSolidsByPatch(Curve closedCurve, double height = 1, string layer = "_CivilConnectionSolid")
        //{
        //    return Utils.CutSolidsByPatch(this._document, closedCurve, height, layer);
        //}

        /// <summary>
        /// Creates an extrusion based on a profile defined by a set of curves profile to be used to cut the solids in the Civil Document.
        /// </summary>
        /// <param name="closedCurves">The closed curves.</param>
        /// <param name="height">The height. By Default is equal to 1.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public bool CutSolidsByCurves(IList<Curve> closedCurves, double height = 1, string layer = "_CivilConnectionSolid")
        //{
        //    return Utils.CutSolidsByCurves(this._document, closedCurves, height, layer);
        //}

        /// <summary>
        /// Creates a solid to be used to cut the solids in the Civil 3D Document.
        /// </summary>
        /// <param name="geometry">The solid geometry.</param>
        /// <param name="layer">The layer where to crerate the cutting solid.</param>
        /// <returns></returns>
        //public bool CutSolidsByGeometry(Geometry[] geometry, string layer)
        //{
        //    return Utils.CutSolidsByGeometry(this._document, geometry, layer);
        //}

        /// <summary>
        /// Import the geometry from Dynamo into the Civil 3D Document.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="layer">The layer where to crerate the solid.</param>
        /// <returns></returns>
        //public IList<string> ImportGeometry(Geometry[] geometry, string layer)
        //{
        //    return Utils.ImportGeometry(this._document, geometry, layer);
        //}

        /// <summary>
        /// Links the geometry associated to a Revit object into Civil 3D.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //public string LinkElement(Revit.Elements.Element element, string parameter, string layer)
        //{
        //    return Utils.ImportElement(this._document, element, parameter, layer);
        //}

        /// <summary>
        /// Send Command to the Civil Document.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        //public bool SendCommand(string command)
        //{
        //    Utils.Log(string.Format("CivilDocument.SendCommand started...", ""));

        //    bool output = true;

        //    try
        //    {
        //        AcadDocument doc = this.InternalElement as AcadDocument;
        //        doc.SendCommand(command);
        //    }
        //    catch (Exception ex)
        //    {
        //        output = false;

        //        Utils.Log(string.Format("ERROR: {0}", ex.Message));

        //        throw ex;
        //    }

        //    Utils.Log(string.Format("CivilDocument.SendCommand completed.", ""));

        //    return output;
        //}

        /// <summary>
        /// Slices the solids in Civil 3D using a Dynamo Plane.
        /// </summary>
        /// <param name="plane">The plane.</param>
        /// <returns></returns>
        //public bool SliceSolidsByPlane(Plane plane)
        //{
        //    return Utils.SliceSolidsByPlane(this._document, plane);
        //}

        #endregion

        #region CIVIL 3D METHODS

        /// <summary>
        /// Adds the civil point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        //public string AddCivilPoint(Point point)
        //{
        //    return Utils.AddCivilPointByPoint(this._document, point);
        //}

        /// <summary>
        /// Adds the civil point group.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        //public string AddCivilPointGroup(Point[] points, string name)
        //{
        //    return Utils.AddPointGroupByPoint(this._document, points, name);
        //}

        /// <summary>
        /// Gets the Civil point groups.
        /// </summary>
        ///// <returns></returns>
        //[MultiReturn(new string[] { "PointGroupNames", "Points" })]
        //public Dictionary<string, object> GetPointGroups()
        //{
        //    var dict = Utils.GetPointGroups(this._document);

        //    return new Dictionary<string, object>() { { "PointGroupNames", dict.Keys }, { "Points", dict.Values } };
        //}


        /// <summary>
        /// Adds the tin surface by points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="name">The name.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        //[IsVisibleInDynamoLibrary(false)]
        //public string AddTINSurfaceByPoints(Point[] points, string name, string layer)
        //{
        //    return Utils.AddTINSurfaceByPoints(this._document, points, name, layer);
        //}

        #endregion

        /// <summary>
        /// Public textual representation of the Dynamo node preview
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("CivilDocument(Name = {0})", this.Name);
        }
        #endregion   
    }
}
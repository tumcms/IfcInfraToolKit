//<<<<<<< Updated upstream
//﻿
//namespace IfcInfraToolkit_Dyn
//{
//    public static class IfcInfraToolkitDyn
//    {
//        public static string getDocumentName(Autodesk.AECC.Interop.UiLand.AeccDocument document)
//        {
//            return document.Name;
//        }

//        public static string getAlignmentName(Autodesk.AECC.Interop.UiLand.AeccDocument document)
//        {
//            var alignments = document.AlignmentsSiteless;
//            return "alingmentNames";
//=======
﻿using System;
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

namespace IfcInfraToolkit_Dyn
{
    public static class IfcInfraToolkitDynInteraction
    {

        //public static string getDocument()
        //{

        //    return AeccRoadwayDocument; 
        //}
      
        ///// Return a list of all the Civil 3D Alignments
        //public static string GetAlignments(CivilDocument civil3DDocument)
        //{
        //    var civilDocument = civil3DDocument;
        //    var alignments = civilDocument.GetAlignments();

        //    return civilDocument.Name;

            //var _alignments = CivilDocument.AlignmentsSiteless;




            //IList<Alignment> alignmentList = new List<Alignment>();
            //foreach (AeccAlignment align in Civil3DDocument._alignments)
            //{
            //    alignmentList.Add(new Alignment(align as AeccAlignment));
            //}
            //return alignmentList;
            //Document doc = Application.DocumentManager.MdiActiveDocument;
            //using (doc.LockDocument())
            //{
            //    using (Database db = doc.Database)
            //    {
            //        using (Transaction t = db.TransactionManager.StartTransaction())
            //        {
            //        }
            //    }
            //}

        }

    }



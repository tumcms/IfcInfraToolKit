// Sebastian Esser20200608

using System.Collections.Generic;
using Autodesk.AECC.Interop.Land;
using Autodesk.AECC.Interop.Roadway;
using Autodesk.AECC.Interop.UiRoadway;

namespace IfcInfraToolkit_Dyn
{
    public class Civil3DDocument
    {
        private string Name; 
        private AeccRoadwayDocument CivilDocument;
        private AeccSurfaces _surfaces;
        private AeccAlignmentsSiteless _alignments;
        private AeccCorridors _corridors;

        /// Constructor
        internal Civil3DDocument(AeccRoadwayDocument civDoc)
        {
            Name = civDoc.Name;
            this.CivilDocument = civDoc;
            this._surfaces = civDoc.Surfaces;
            this._alignments = civDoc.AlignmentsSiteless;
            this._corridors = civDoc.Corridors;
        }

        /// Return a list of all the Civil 3D Surfaces
        public static IList<Surface> GetSurfaces(Civil3DDocument Civil3DDocument)
        {
            IList<Surface> surfacesList = new List<Surface>();
            foreach (AeccSurface surf in Civil3DDocument._surfaces)
            {
                surfacesList.Add(new Surface(surf as AeccSurface));
            }
            return surfacesList;
        }

        /// Return a list of all the Civil 3D Alignments
        public static IList<Alignment> GetAlignments(Civil3DDocument Civil3DDocument)
        {
            IList<Alignment> alignmentList = new List<Alignment>();
            foreach (AeccAlignment align in Civil3DDocument._alignments)
            {
                alignmentList.Add(new Alignment(align as AeccAlignment));
            }
            return alignmentList;
        }

        /// Return a list of all the Civil 3D Corridors
        //public static IList<Corridor> GetCorridors(Civil3DDocument Civil3DDocument)
        //{
        //    IList<Corridor> corridorList = new List<Corridor>();
        //    foreach (AeccCorridor corr in Civil3DDocument._corridors)
        //    {
        //        corridorList.Add(new Corridor(corr as AeccCorridor));
        //    }
        //    return corridorList;
        //}


    }
}
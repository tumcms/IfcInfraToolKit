// Sebastian Esser20200608

using System.Collections.Generic;
using System.Runtime.InteropServices;
using Autodesk.AECC.Interop.UiRoadway;
using Autodesk.AutoCAD.Interop;

namespace IfcInfraToolkit_Dyn
{
    public class Civil3DApplication
    {
        private AeccRoadwayApplication _civApp;
        private AcadDocument _activeDocument;

        // AutoCAD Application
        const string progID = "AutoCAD.Application";
        // Get a Civil 3D 2019 Roadway Application (preferred - as this includes corridors)
        const string civAppName = "AeccXUiRoadway.AeccRoadwayApplication.13.3";

        /// Constructor
        public Civil3DApplication()
        {
            this._civApp = this.GetApplication();
            this._activeDocument = this._civApp.ActiveDocument;
        }
        /// Get the active Civil 3D Application
        internal AeccRoadwayApplication GetApplication()
        {
            AcadApplication acadApplication = null;
            try
            {
                acadApplication = (AcadApplication)Marshal.GetActiveObject(progID);
            }
            catch
            {
            }
            if (acadApplication != null)
            {
                return acadApplication.GetInterfaceObject(civAppName);
            }
            return null;
        }
        /// Return a list of all the open Documents
        public static IList<Civil3DDocument> GetDocuments(Civil3DApplication Civil3DApplication)
        {
            IList<Civil3DDocument> docsList = new List<Civil3DDocument>();
            foreach (AeccRoadwayDocument doc in Civil3DApplication._civApp.Documents)
            {
                docsList.Add(new Civil3DDocument(doc as AeccRoadwayDocument));
            }
            return docsList;
        }

        /// Override the string output for the CivilApplication object
        public static IList<Civil3DDocument> Civil3DDocuments;

        public override string ToString()
        {
            return string.Format($"Civil3DApplication (ActiveDocument = {this._activeDocument.Name})");
        }


    }
}
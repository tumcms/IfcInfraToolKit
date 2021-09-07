using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

namespace IfcInfraToolkit_RevitAddin
{
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        public class SpacialStructureCreatorHardcoded : IExternalCommand
        {

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                //var app = commandData.Application.Application;
                //var doc = commandData.Application.ActiveUIDocument?.Document;
                //DeleteAllWalls(app, doc);
                CreateSpacialStructurePDT();
                return Result.Succeeded;
            }
        /// <summary>
        /// Creates the spacial structure semantics of a hardcoded facility item and stores the data into an Ifc file
        /// </summary>
            public static void CreateSpacialStructurePDT() //Application rvtApp, Document doc)
            {
            // Init IFC model by creating a database container to store all Ifc data inside
            ProjectSetupService psservice = new ProjectSetupService();
            var database = psservice.CreateDatabase();
            database = psservice.AddBaseProjectSetup(database, "Sample Ifc Project for semantics", "semantics site sample");

            // Add facility to database
            SpatialStructureService ssservice = new SpatialStructureService();
            Guid facilityGuid = ssservice.AddFacility(ref database, "sample semantic facility", null);

            // Add FacilityPartType
            IfcSpatialStructureElement host = database.Project.Extract<IfcFacility>()
                .FirstOrDefault(a => a.Guid == facilityGuid);
            Guid facilityPartGuid = ssservice.AddFacilityPart(ref database, "IfcRoadPartTypeEnum",
                "SampleSemanticFacilityPart", "ROUNDABOUT", "LATERAL", host);

            //4. Save Ifc model
            //ToDo: Take relative path!!!
            database.WriteFile("C:/Users/janin/OneDrive/Dokumente/HiwiCMS/IfcInfraToolkit/RevitTests/AppBundleTests" + "/" + "IfcResult" + ".ifc");
            //if (rvtApp == null) throw new InvalidDataException(nameof(rvtApp));
                //
                //if (doc == null) throw new InvalidOperationException("Could not open document.");
                //
                //using (Transaction transaction = new Transaction(doc))
                //{
                //    FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(Wall));
                //    transaction.Start("Delete All Walls");
                //    doc.Delete(col.ToElementIds());
                //    transaction.Commit();
                //}
            }
        }
}

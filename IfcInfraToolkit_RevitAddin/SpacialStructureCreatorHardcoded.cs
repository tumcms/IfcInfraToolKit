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
using IfcInfraToolKit_DynamoCore;

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
               Dictionary<string,object> databaseContainer =
                    IfcModelSetup.CreateIfcModel("SemanticCheck", "SemanticSampleSite");
                
               // Add facility to database
                databaseContainer = SpatialStructureDesigner.AddFacility(databaseContainer["DatabaseContainer"] as DatabaseContainer, null,
                    "SampleFacility");

                // Add FacilityPartType
                databaseContainer = SpatialStructureDesigner.AddFacilityPart(databaseContainer["DatabaseContainer"] as DatabaseContainer,
                    databaseContainer["FacilityGUID"] as string, "Section1", "IfcRoadPartTypeEnum", "ROUNDABOUT",
                    "LATERAL");

                //4. Save Ifc model
                IfcModelSetup.SaveIfcModel(databaseContainer["DatabaseContainer"] as DatabaseContainer, "C:/Users/janin/OneDrive/Dokumente/HiwiCMS/IfcInfraToolkit/RevitTests/AppBundleTests", "IfcResult");

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

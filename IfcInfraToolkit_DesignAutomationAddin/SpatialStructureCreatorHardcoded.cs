using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

namespace IfcInfraToolkit_DesignAutomationAddin
{
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SpacialStructureCreatorHardcoded : IExternalDBApplication
    {
        public ExternalDBApplicationResult OnStartup(Autodesk.Revit.ApplicationServices.ControlledApplication app)
        {
            DesignAutomationBridge.DesignAutomationReadyEvent += HandleDesignAutomationReadyEvent;
            return ExternalDBApplicationResult.Succeeded;
        }
        public void HandleDesignAutomationReadyEvent(object sender, DesignAutomationReadyEventArgs e)
        {
            e.Succeeded = true;
            CreateSpacialStructurePDT();
        }

        public ExternalDBApplicationResult OnShutdown(Autodesk.Revit.ApplicationServices.ControlledApplication app)
        {
            return ExternalDBApplicationResult.Succeeded;
        }

        /// <summary>
        /// Creates the spacial structure semantics of a hardcoded facility item and stores the data into an Ifc file
        /// </summary>
        public static void CreateSpacialStructurePDT()
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
            //ToDo: Take a relative path!!!
            //relative path
            //string imagesDirectory = Path.Combine(Environment.CurrentDirectory, "IfcResult" + ".ifc");
            //absolute path
            //database.WriteFile("C:/Users/janin/OneDrive/Dokumente/HiwiCMS/IfcInfraToolkit/RevitTests/AppBundleTests" + "/" + "resultSpacialStructurePDT" + ".ifc");
            //Path inside DesignAutomation
            //When setting the output parameter inside Postman/Node.js .... this name must match the result name inside Design Automation completely!
            database.WriteFile("resultSpacialStructurePDT.ifc");
        }
    }
}

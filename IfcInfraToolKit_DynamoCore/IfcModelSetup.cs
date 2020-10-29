using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;
using Microsoft.SqlServer.Server;


namespace IfcInfraToolKit_DynamoCore
{
    /// <summary>
    /// Provides basic methods to create an IFC model and add some spatial structure stuff
    /// </summary>
    public static class IfcModelSetup 
    {
        #region CreateAndSave

        
        /// <summary> Creates a new DatabaseIfc instance that acts as a central container for the IFC content. </summary>
        /// <search> init, create, IFC </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc </returns>
        [MultiReturn(new[] {"DatabaseContainer"})]
        public static Dictionary<string, object> CreateIfcModel(string projectName = "sampleProject", string siteName = "sampleSite")
        {
            // init container
            var container = new DatabaseContainer();

            // add content to the database
            ProjectSetupService service = new ProjectSetupService();
            container.Database = service.CreateDatabase();
            container.Database = service.AddBaseProjectSetup(container.Database, projectName , siteName );

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", container}
            };

            return d;
        }
        
        /// <summary> Stores an DatabaseIfc instance into an *.ifc Model </summary>
        /// <param name="container">IFC container</param>
        /// <param name="path">folder to store the resulting model</param>
        /// <param name="modelName">file name without *.ifc label</param>
        /// <search> store, save, IFC </search>
        public static void SaveIfcModel(DatabaseContainer container, string path, string modelName)
        {
            container.Database.WriteFile(path + "/" + modelName + ".ifc");
        }

        #endregion

        #region SpatialStructure

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="facilityName"></param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "FacilityGUID" })]
        public static Dictionary<string, object> AddFacility(DatabaseContainer databaseContainer, string hostGuid = "null", string facilityName = "DefaultFacility")
        {
            // get current db
            var database = databaseContainer.Database;

            // get host
            var host = database.OfType<IfcSpatialStructureElement>().FirstOrDefault(a => a.Guid.ToString() == hostGuid);

            ProjectSetupService service = new ProjectSetupService();
            var guid = service.AddFacility(ref database, facilityName, host);

            // assign updated db to container
            databaseContainer.Database = database;

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"FacilityGUID", guid.ToString()}
            };

            return d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="facilityPartName"></param>
        /// <param name="hostGuid"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "FacilityPartGUID" })]
        public static Dictionary<string, object> AddFacilityPart(DatabaseContainer databaseContainer, string hostGuid, string facilityPartName = "DefaultFacilityPart")
        {
            // get current db
            var database = databaseContainer.Database;

            // get host
            var hostFacility = database.OfType<IfcFacility>()
                .FirstOrDefault(a => a.Guid.ToString() == hostGuid);
            var hostFacilityPart = database.OfType<IfcFacilityPart>()
                .FirstOrDefault(a => a.Guid.ToString() == hostGuid);

            ProjectSetupService service = new ProjectSetupService();

            Guid guid; 

            if (hostFacility != null)
            {
                var host = hostFacility;
                guid = service.AddFacilityPart(ref database, facilityPartName, "type", host);
            }
            else if (hostFacilityPart != null)
            {
                var host = hostFacilityPart;
                guid = service.AddFacilityPart(ref database, facilityPartName, "type", host);
            }
            else
            {
                guid = Guid.Empty;
            }
           

            // assign updated db to container
            databaseContainer.Database = database;
            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"FacilityPartGUID", guid.ToString()}
            };

            return d;
        }




        #endregion


        /// <summary>
        /// Watch node for Ifc content in the database
        /// </summary>
        /// <returns></returns>
        public static DatabaseIfc WatchIFC(DatabaseContainer databaseContainer)
        {
            return databaseContainer.Database;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;


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
        public static Dictionary<string, object> CreateIfcModel()
        {
            // init container
            var container = new DatabaseContainer();

            // add content to the database
            ProjectSetupService service = new ProjectSetupService();
            container.Database = service.CreateDatabase();
            container.Database = service.AddBaseProjectSetup(container.Database);

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
        public static Dictionary<string, object> AddFacility(DatabaseContainer databaseContainer, string facilityName, string hostGuid = "null")
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
        /// <param name="hostGUID"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "FacilityPartGUID" })]
        public static Dictionary<string, object> AddFacilityPart(DatabaseContainer databaseContainer, string facilityPartName, string hostGUID)
        {
            // get current db
            var database = databaseContainer.Database;

            // get host
            var hostFacility = database.OfType<IfcFacility>().First(a => a.Guid.ToString() == hostGUID);
            var hostFacilityPart = database.OfType<IfcFacilityPart>().First(a => a.Guid.ToString() == hostGUID);

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

    }
}

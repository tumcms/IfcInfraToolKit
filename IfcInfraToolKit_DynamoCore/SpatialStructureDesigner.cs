using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

namespace IfcInfraToolKit_DynamoCore
{
    public static class SpatialStructureDesigner
    {
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="facilityName"></param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <returns></returns>
        [MultiReturn(new[] {"DatabaseContainer", "FacilityGUID"})]
        public static Dictionary<string, object> AddFacility(DatabaseContainer databaseContainer,
            string hostGuid = "null", string facilityName = "DefaultFacility")
        {
            // get current db
            var database = databaseContainer.Database;

            // get host
            var host = database.OfType<IfcSpatialStructureElement>().FirstOrDefault(a => a.Guid.ToString() == hostGuid);

            var service = new SpatialStructureService();
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
        /// <param name="facilityType"></param>This is the type of the facility (e.g. IfcBridgePartTypeEnum), case sensitive
        /// <param name="facilityPartType"></param>The part of the facilityPartTypeEnum, e.g. PIER
        /// <param name="hostGuid"></param>
        /// <returns></returns>
        [MultiReturn(new[] {"DatabaseContainer", "FacilityPartGUID"})]
        public static Dictionary<string, object> AddFacilityPart(DatabaseContainer databaseContainer, string hostGuid, string facilityType = "IfcFacilityPartCommonTypeEnum", string facilityPartType = "NOTDEFINED", string facilityPartName = "DefaultFacilityPart")
        {
            // get current db
            var database = databaseContainer.Database;

            // get host
            var hostFacility = database.OfType<IfcObjectDefinition>()
                .FirstOrDefault(a => a.Guid.ToString() == hostGuid);


            var service = new SpatialStructureService();

            Guid guid;

            if (hostFacility != null)
            {
                var host = hostFacility;
                guid = service.AddFacilityPart(ref database, facilityType, facilityPartName, facilityPartType, host);
            }

            else
            {
                var e = new Exception("Couldn't find the host item.");
                throw e;
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




    }
}
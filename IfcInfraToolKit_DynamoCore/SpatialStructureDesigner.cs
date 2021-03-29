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
        /// <param name="name">Name of IfcFacilityPart</param>
        /// <param name="facilityType">This is the type of the facility (e.g. IfcBridgePartTypeEnum), case sensitive</param>
        /// <param name="selectType">The part of the facilityPartTypeEnum, e.g. PIER</param>
        /// <param name="hostGuid"></param>
        /// <returns></returns>
        [MultiReturn(new[] {"DatabaseContainer", "FacilityPartGUID"})]
        public static Dictionary<string, object> AddFacilityPart(DatabaseContainer databaseContainer, string name, string facilityType, string selectType,  string hostGuid)
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
                guid = service.AddFacilityPart(ref database, facilityType, name, selectType, host);
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

        [MultiReturn(new[] { "DatabaseContainer", "FacilityPartGUID" })]
        public static Dictionary<string, object> AddBridge(DatabaseContainer databaseContainer, string hostGuid, string bridgeName)
        {
            // get current db
            var database = databaseContainer.Database;

            // get host
            var hostFacility = database.OfType<IfcSpatialStructureElement>()
                .FirstOrDefault(a => a.Guid.ToString() == hostGuid);


            var service = new SpatialStructureService();

            Guid guid;

            if (hostFacility != null)
            {
                var host = hostFacility;
                guid = service.AddBridge(ref database, bridgeName, host);
            }

            else
            {
                var e = new Exception("Couldn't find host item with specified GUID.");
                throw e;
            }


            // assign updated db to container
            databaseContainer.Database = database;
            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"BridgeGUID", guid.ToString()}
            };

            return d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailableFacilityPartTypes()
        {
            return new List<string>
            {
                "IfcRailwayPartTypeEnum",
                "IfcBridgePartTypeEnum",
                "IfcMarinePartTypeEnum",
                "IfcRoadPartTypeEnum",
                "IfcFacilityPartCommonTypeEnum"
            };
        }

        public static List<string> GetPredefinedType(string FacilityPartEnum)
        {
            var pdt = new Dictionary<string, List<string>>
            {
                {
                    "IfcRailwayPartTypeEnum", new List<string>
                    {
                        "CREST",
                        "MANUFACTURING",
                        "LOWWATERLINE",
                        "CORE",
                        "WATERFIELD",
                        "CILL_LEVEL",
                        "BERTHINGSTRUCTURE",
                        "COPELEVE",
                        "CHAMBER",
                        "STORAGE",
                        "APPROACHCHANNEL",
                        "VEHICLESERVICING",
                        "SHIPTRANSFER",
                        "GATEHEAD",
                        "GUDINGSTRUCTURE",
                        "BELOWWATERLINE",
                        "WEATHERSIDE",
                        "LANDFIELD",
                        "PROTECTION",
                        "LEEWARDSIDE",
                        "ABOVEWATERLINE",
                        "ANCHORAGE",
                        "NAVIGATIONALAREA",
                        "HIGHWATERLINE",
                        "USERDEFINED",
                        "NOTDEFINED"
                    }
                }
            };

            return pdt[FacilityPartEnum];

        }

    }
}
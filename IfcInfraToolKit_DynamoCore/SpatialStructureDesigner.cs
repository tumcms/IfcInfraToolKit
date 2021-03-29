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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="hostGuid"></param>
        /// <param name="bridgeName"></param>
        /// <returns></returns>
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
        /// Returns a list of possible Facility types. Choose between Railway, Bridge, Marine, Road and CommonType
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "FacilityPartTypeList"})]
        public static Dictionary<string, object> GetAvailableFacilityPartTypes()
        {
            var lst =  new List<string>
            {
                "IfcRailwayPartTypeEnum",
                "IfcBridgePartTypeEnum",
                "IfcMarinePartTypeEnum",
                "IfcRoadPartTypeEnum",
                "IfcFacilityPartCommonTypeEnum"
            };

            var d = new Dictionary<string, object>
            {
                {"FacilityPartTypeList", lst}
            };

            return d;
        }

        /// <summary>
        /// Returns a list of the matching predefined FacilityPartTypes depending on the input of the facility type.
        /// </summary>
        /// <param name="facilityPartEnum">Choose between IfcRailwayPartTypeEnum, IfcBridgePartTypeEnum, IfcMarinePartTypeEnum, IfcRoadPartTypeEnum and IfcFacilityPartCommonTypeEnum (=Default) </param>
        /// <returns></returns>
        [MultiReturn(new[] { "PredefinedType" })]
        public static Dictionary<string, object> GetPredefinedType(string facilityPartEnum="IfcFacilityPartCommonTypeEnum")
        {
            var pdt = new Dictionary<string, List<string>>
            {
                {
                    "IfcMarinePartTypeEnum", new List<string>
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
                },
                {
                    "IfcRailwayPartTypeEnum", new List<string> {
                    "TRACKSTRUCTURE",
                    "TRACKSTRUCTUREPART",
                    "LINESIDESTRUCTUREPART",
                    "DILATATIONSUPERSTRUCTURE",
                    "PLAINTRACKSUPESTRUCTURE",
                    "LINESIDESTRUCTURE",
                    "SUPERSTRUCTURE",
                    "TURNOUTSUPERSTRUCTURE",
                    "USERDEFINED",
                    "NOTDEFINED"
                    }
                },
                {
                    "IfcBridgePartTypeEnum", new List<string>
                    {
                        "ABUTMENT",
                        "DECK",
                        "DECK_SEGMENT",
                        "FOUNDATION",
                        "PIER",
                        "PIER_SEGMENT",
                        "PYLON",
                        "SUBSTRUCTURE",
                        "SUPERSTRUCTURE",
                        "SURFACESTRUCTURE",
                        "USERDEFINED",
                        "NOTDEFINED"
                    }
                },
                {
                    "IfcRoadPartTypeEnum", new List<string>
                    {
                        "ROADSIDEPART",
                        "BUS_STOP",
                        "HARDSHOULDER",
                        "INTERSECTION",
                        "PASSINGBAY",
                        "ROADWAYPLATEAU",
                        "ROADSIDE",
                        "REFUGEISLAND",
                        "TOLLPLAZA",
                        "CENTRALRESERVE",
                        "SIDEWALK",
                        "PARKINGBAY",
                        "RAILWAYCROSSING",
                        "PEDESTRIAN_CROSSING",
                        "SOFTSHOULDER",
                        "BICYCLECROSSING",
                        "CENTRALISLAND",
                        "SHOULDER",
                        "TRAFFICLANE",
                        "ROADSEGMENT",
                        "ROUNDABOUT",
                        "LAYBY",
                        "CARRIAGEWAY",
                        "TRAFFICISLAND",
                        "USERDEFINED",
                        "NOTDEFINED"
                    }
                },
                {
                    "IfcFacilityPartCommonTypeEnum", new List<string>
                    {
                        "SEGMENT",
                        "ABOVEGROUND",
                        "JUNCTION",
                        "LEVELCROSSING",
                        "BELOWGROUND",
                        "SUBSTRUCTURE",
                        "TERMINAL",
                        "SUPERSTRUCTURE",
                        "USERDEFINED",
                        "NOTDEFINED"
                    }
                }
            };
            //Check, if facilityPartEnum (input) exists in dictionary, otherwise set default value: "IfcFacilityPartCommonTypeEnum"
            var d = new Dictionary<string, object>
            {
                {"PredefinedType", pdt.ContainsKey(facilityPartEnum)
                    ? pdt[facilityPartEnum]
                    : pdt["IfcFacilityPartCommonTypeEnum"]}
            };
            return d;
        }

    }
}
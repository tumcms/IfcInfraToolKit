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
        /// Adds a facility to the Ifc database
        /// </summary>
        /// <search> Facility, IfcFacility, Add </search>
        /// <param name="databaseContainer"></param>
        /// <param name="facilityName">Enter any name of the Facility</param>
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
        /// Creates a facility Part, which either belongs to an IfcFacility or to another IfcFacilityPart depending on hostGuid
        /// </summary>
        /// <search> FacilityPart, IfcFacilityPart, Add </search>
        /// <param name="databaseContainer"></param>
        /// <param name="hostGuid">GUID of the parent item in the spatial structure. If "null", the program stops</param>
        /// <param name="name">user defined name of the IfcFacilityPart</param>
        /// <param name="facilityType">Type of the facility (e.g. IfcBridgePartTypeEnum), case sensitive</param>
        /// <param name="facilityPartType">Predefined FacilityPartType according to the Facility Type, e.g. PIER, case sensitive</param>
        /// <param name="usageType">Predefined usage type of the facility part</param>
        /// <returns>The updated databaseContainer including the added facility Part and also returns the guid of the new created facility Part</returns>
        [MultiReturn(new[] {"DatabaseContainer", "FacilityPartGUID"})]
        public static Dictionary<string, object> AddFacilityPart(DatabaseContainer databaseContainer, string hostGuid,
            string name = "DefaultFacilityPart", string facilityType = "IfcFacilityPartCommonTypeEnum",
            string facilityPartType = "NOTDEFINED", string usageType = "NOTDEFINED")
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
                guid = service.AddFacilityPart(ref database, facilityType, name, facilityPartType, usageType, host);
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
        /// Adds an IfcBridge entity to the spatial structure
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="hostGuid">GUID of the parent item in the spatial structure. If "null", the uppermost IfcSite item is taken. </param>
        /// <param name="bridgeName">The bridge's name</param>
        /// <returns></returns>
        [MultiReturn(new[] {"DatabaseContainer", "BridgeGUID" })]
        public static Dictionary<string, object> AddBridge(DatabaseContainer databaseContainer, string hostGuid,
            string bridgeName)
        {
            // get current db
            var database = databaseContainer.Database;

            // get host
            var host = database.OfType<IfcSpatialStructureElement>().FirstOrDefault(a => a.GlobalId == hostGuid);

            // run transaction on database
            var service = new SpatialStructureService();
            var bridgeGuid = service.AddBridge(ref database, bridgeName, host);
            
            // assign updated db to container
            databaseContainer.Database = database;
            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"BridgeGUID", bridgeGuid.ToString()}
            };

            return d;
        }

        /// <summary>
        /// Adds an IfcRoad entity to the spatial structure
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="hostGuid">GUID of the parent item in the spatial structure. If "null", the uppermost IfcSite item is taken. </param>
        /// <param name="roadName">The road's name</param>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "RoadGUID" })]
        public static Dictionary<string, object> AddRoad(DatabaseContainer databaseContainer, string hostGuid,
            string roadName)
        {
            // get current db
            var database = databaseContainer.Database;

            // get host
            var host = database.OfType<IfcSpatialStructureElement>().FirstOrDefault(a => a.GlobalId == hostGuid);

            // run transaction on database
            var service = new SpatialStructureService();
            var bridgeGuid = service.AddRoad(ref database, roadName, host);

            // assign updated db to container
            databaseContainer.Database = database;
            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"RoadGUID", bridgeGuid.ToString()}
            };

            return d;
        }


        /// <summary>
        /// Returns a list of possible Facility types. Choose between Railway, Bridge, Marine, Road and CommonType
        /// </summary>
        /// <returns>List with all IfcFacility Types</returns>
        [MultiReturn(new[] {"FacilityPartTypeList"})]
        public static Dictionary<string, object> GetAvailableFacilityPartTypes()
        {
            var lst = new List<string>
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
        /// <returns>List with all Predefined Types for a facility type</returns>
        [MultiReturn(new[] {"PredefinedType"})]
        public static Dictionary<string, object> GetPredefinedType(
            string facilityPartEnum = "IfcFacilityPartCommonTypeEnum")
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
                    "IfcRailwayPartTypeEnum", new List<string>
                    {
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
                {
                    "PredefinedType", pdt.ContainsKey(facilityPartEnum)
                        ? pdt[facilityPartEnum]
                        : pdt["IfcFacilityPartCommonTypeEnum"]
                }
            };
            return d;
        }

        /// <summary>
        /// Returns a list of all predefined usage types of IfcFacilityParts (IfcFacilityPartUsageEnum).
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] {"PredefinedFacilityPartUsage"})]
        public static Dictionary<string, object> GetPredefinedFacilityPartUsage()
        {
            var usageList = new List<string>
            {
                "LATERAL",
                "REGION",
                "VERTICAL",
                "LONGITUDINAL",
                "USERDEFINED",
                "NOTDEFINED"
            };
            var d = new Dictionary<string, object>
            {
                {
                    "PredefinedFacilityPartUsage", usageList
                }
            };
            return d;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

namespace IfcInfraToolKit_DynamoCore
{
   
    public static class ElementDesigner
    {
        #region IfcBuiltElement

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="IfcClass">Desired IfcClass name</param>
        /// <param name="elementName"></param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "ElementGUID" })]
        public static Dictionary<string, object> AddBuiltElement(DatabaseContainer databaseContainer,
            string IfcClass="IfcBuildingElementProxy", string elementName = "DefaultElement",
            string hostGuid = "null")
        {
            // get current db
            var database = databaseContainer.Database;
            
            var guid = ElementService.AddBuiltElement(ref database, elementName, IfcClass, hostGuid);

            // assign updated db to container
            databaseContainer.Database = database;

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"ElementGUID", guid}
            };

            return d;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "PredefinedBuiltElement" })]
        public static Dictionary<string, object> GetAvailableBuiltElementClassNames()
        {
            var builtElements= new List<string>
            {
                "IfcBeam",
                "IfcBearing",
                "IfcBuildingElementProxy",
                "IfcChimney",
                "IfcColumn",
                "IfcCourse",
                "IfcCovering",
                "IfcCurtainWall",
                "IfcDeepFoundation",
                "IfcDoor",
                "IfcEarthworksElement",
                "IfcFooting",
                "IfcKerb",
                "IfcMember",
                "IfcMooringDevice",
                "IfcNavigationElement",
                "IfcPavement",
                "IfcPlate",
                "IfcRail",
                "IfcRailing",
                "IfcRamp",
                "IfcRampFlight",
                "IfcRoof",
                "IfcShadingDevice",
                "IfcSlab",
                "IfcStair",
                "IfcStairFlight",
                "IfcTrackElement",
                "IfcWall",
                "IfcWindow"
            };
            var d = new Dictionary<string, object>
            {
                {
                    "PredefinedBuiltElement", builtElements
                }
            };
            return d;
        }
        #endregion

        #region IfcDistributionElement

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="IfcClass"></param>
        /// <param name="elementName"></param>
        /// <param name="hostGuid"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AddDistributionElement(DatabaseContainer databaseContainer,
            string IfcClass = "IfcDistributionElement", string elementName = "DefaultElement",
            string hostGuid = "null")
        {
            // get current db
            var database = databaseContainer.Database;

            var guid = ElementService.AddDistributionElement(ref database, elementName, IfcClass, hostGuid);

            // assign updated db to container
            databaseContainer.Database = database;

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"ElementGUID", guid}
            };

            return d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "PredefinedDistributionElement" })]
        public static Dictionary<string, object> GetAvailableDistributionElementClassNames()
        {
            var distributionElement = new List<string>
            {
                "IfcDistributionElement",
                "IfcDistributionControlElement",
                "IfcActuator",
                "IfcAlarm",
                "IfcController",
                "IfcFlowInstrument",
                "IfcProtectiveDeviceTrippingUnit",
                "IfcSensor",
                "IfcUnitaryControlElement",
                "IfcDistributionFlowElement",
                "IfcDistributionChamberElement",
                "IfcEnergyConversionDevice",
                "IfcFlowController",
                "IfcFlowFitting",
                "IfcFlowMovingDevice",
                "IfcFlowSegment",
                "IfcFlowStorageDevice",
                "IfcFlowTerminal",
                "IfcFlowTreatmentDevice"
            };
            var d = new Dictionary<string, object>
            {
                {
                    "PredefinedDistributionElement", distributionElement
                }
            };
            return d;
        }

        #endregion

        #region IfcElementComponent

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="IfcClass"></param>
        /// <param name="elementName"></param>
        /// <param name="hostGuid"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AddElementComponent(DatabaseContainer databaseContainer,
            string IfcClass = "IfcBuildingElementPart", string elementName = "DefaultElement",
            string hostGuid = "null")
        {
            // get current db
            var database = databaseContainer.Database;

            var guid = ElementService.AddElementComponent(ref database, elementName, IfcClass, hostGuid);

            // assign updated db to container
            databaseContainer.Database = database;

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"ElementGUID", guid}
            };

            return d;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "PredefinedElementComponents" })]
        public static Dictionary<string, object> GetAvailableElementComponentClassNames()
        {
            var elementComponents= new List<string>
            {
                "IfcBuildingElementPart",
                "IfcDiscreteAccessory",
                "IfcFastener",
                "IfcImpactProtectionDevice",
                "IfcMechanicalFastener",
                "IfcReinforcingBar",
                "IfcReinforcingMesh",
                "IfcTendon",
                "IfcTendonAnchor",
                "IfcTendonConduit",
                "IfcSign",
                "IfcVibrationDamper",
                "IfcVibrationIsolator"
            };
            var d = new Dictionary<string, object>
            {
                {
                    "PredefinedElementComponents", elementComponents
                }
            };
            return d;
        }

        #endregion

        #region OtherIfcElementSubtypes

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="IfcClass">Desired IfcClass name</param>
        /// <param name="elementName"></param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "ElementGUID" })]
        public static Dictionary<string, object> AddOtherElement(DatabaseContainer databaseContainer,
            string IfcClass = "IfcTransportElement", string elementName = "DefaultElement",
            string hostGuid = "null")
        {
            // get current db
            var database = databaseContainer.Database;

            var guid = ElementService.AddOtherElement(ref database, elementName, IfcClass, hostGuid);

            // assign updated db to container
            databaseContainer.Database = database;

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"ElementGUID", guid}
            };

            return d;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "PredefinedOtherElements" })]
        public static Dictionary<string, object> GetAvailableOtherElementClassNames()
        {
            var otherElements= new List<string>
            {
                //IfcFeatureElement
                "IfcSurfaceFeature",
                //IfcFurnishingElement
                "IfcFurnishingElement",
                "IfcFurniture",
                "IfcSystemFurnitureElement",
                //IfcGeographicElement
                "IfcGeographicElement",
                "IfcPlant",
                //IfcGeotechnicalElement
                "IfcBorehole",
                "IfcGeomodel",
                "IfcGeoslice",
                "IfcSolidStratum",
                "IfcVoidStratum",
                "IfcWaterStratum",
                //IfcTransportElement
                "IfcTransportElement",
                //IfcVirtualElement
                "IfcVirtualElement"
            };
            var d = new Dictionary<string, object>
            {
                {
                    "PredefinedOtherElements", otherElements
                }
            };
            return d;
        }
        #endregion


        //IfcElementAssembly is missing as it is an abstract class
        //IfcFeatureElementAddition and IfcFeatureElementSubtraction are missing, as they need different input attributes


    }
}
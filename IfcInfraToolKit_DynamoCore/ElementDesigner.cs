using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

namespace IfcInfraToolKit_DynamoCore
{
   
    public static class ElementDesigner
    {
        #region IfcBuiltElement

        /// <summary>
        /// Adds an IfcElement of the subclass IfcBuiltElement to the database
        /// </summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <param name="IfcClass">Desired IfcClass, that should be created</param>
        /// <param name="elementName">provide an arbitrary name of the BuiltElement</param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <search> IfcBuiltElement, Ifc, IfcElement, add, add element, builtElement</search>
        /// <returns> Updated database container with added builtElement semantics and the guid of the created IfcElement instance</returns>
        [NodeCategory("Actions")]
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
        /// Provides a list with all possible predefined subclasses of IfcBuiltElement
        /// </summary>
        /// <search> predefined IfcBuiltElement, Ifc, predefined IfcElement, add, add element, builtElement, predefined, list IfcElement </search>
        /// <returns> List of possible IfcBuiltElement predefined subclasses</returns>
        [NodeCategory("Query")]
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
        /// Adds an IfcElement of the subclass IfcDistributionElement to the database
        /// </summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <param name="IfcClass">Desired IfcClass, that should be created (A subclass of IfcDistributionElement)</param>
        /// <param name="elementName">provide an arbitrary name of the Distribution Element</param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <search> IfcDistributionElement, Ifc, IfcElement, add, add element, distributionElement</search>
        /// <returns> Updated database container with added distributionElement semantics and the guid of the created IfcElement instance</returns>
        [NodeCategory("Actions")]
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
        /// Provides a list with all possible predefined subclasses of IfcDistributionElement
        /// </summary>
        /// <search> predefined IfcDistributionElement, Ifc, predefined IfcElement, add, add element, distributionElement, predefined, list IfcElement </search>
        /// <returns> List of possible IfcDistributionElement predefined subclasses as string</returns>
        [NodeCategory("Query")]
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
        /// Adds an IfcElement of the subclass IfcElementComponent to the database
        /// </summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <param name="IfcClass">Desired IfcClass, that should be created (A subclass of IfcElementComponent)</param>
        /// <param name="elementName">provide an arbitrary name of the Element component</param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <search> IfcElementComponent, Ifc, IfcElementComponent, add, add element, elementComponent</search>
        /// <returns> Updated database container with added ElementComponent semantics and the guid of the created IfcElement instance</returns>
        [NodeCategory("Actions")]
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
        /// Provides a list with all possible predefined subclasses of IfcElementComponent as input for IfcClass in function AddElementComponent()
        /// </summary>
        /// <search> predefined IfcElementComponent, Ifc, predefined IfcElement, add, add element, elementComponent, predefined, list IfcElement </search>
        /// <returns> List of possible IfcElementComponent predefined subclasses as string</returns>
        [NodeCategory("Query")]
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
        /// Adds an IfcElement of a subclass other then IfcBuiltElement, IfcDistributionElement or IfcElementComponent to the database
        /// </summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <param name="IfcClass">Desired IfcClass, that should be created, choose from GetAvailableOtherElementClassNames() </param>
        /// <param name="elementName">provide an arbitrary name of the Element component</param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <search> IfcElement, Ifc, other IfcElement, add, add element, other element</search>
        /// <returns> Updated database container with added subclass of IfcElement semantics and the guid of the created IfcElement instance</returns>
        [NodeCategory("Actions")]
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
        /// Provides a list with all possible predefined subclasses other then
        /// IfcBuiltElement, IfcDistributionElement or IfcElementComponent as input for IfcClass in function AddOtherElement()
        /// </summary>
        /// <search> predefined other IfcElement, Ifc, predefined IfcElement, add, add element, other element, predefined, list IfcElement </search>
        /// <returns> List of possible other IfcElement predefined subclasses as string</returns>
        [NodeCategory("Query")]
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
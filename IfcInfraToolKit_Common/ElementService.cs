

using System;
using System.Collections.Generic;
using System.Linq;
using GeometryGym.Ifc;

namespace IfcInfraToolkit_Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ElementService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="elementName"></param>
        /// <param name="IfcClass"></param>
        /// <param name="hostGuid"></param>
        /// <param name="systemGuid"></param>
        /// <returns></returns>
        public static string AddDistributionElement(ref DatabaseIfc database, string elementName, string IfcClass, string hostGuid, string systemGuid = null)
        {
            // get host
            var host = database.OfType<IfcObjectDefinition>().FirstOrDefault(a => a.Guid.ToString() == hostGuid);

            // catch issues of no element with specified host guid is available
            if (host == null)
            {
                host = database.Project.UppermostSite();
            }

            // ToDo: catch exceptions of specific IfcClass doesnt exist
            //For all subclasses of IfcDistributionElements another input parameter is necessary (IfcDistributionSystem)
            //Not yet completed!!!
            Dictionary<string, Func<IfcElement>> dict = new Dictionary<string, Func<IfcElement>>
            {
                //IfcDistributionElement
                {"IfcDistributionElement", () => new IfcDistributionElement(host, null, null)},
                {"IfcDistributionControlElement", () => new IfcDistributionControlElement(host, null, null, null)},
                {"IfcActuator", () => new IfcActuator(host, null, null, null)},
                {"IfcAlarm", () => new IfcAlarm(host, null, null, null)},
                {"IfcController", () => new IfcController(host, null, null, null)},
                {"IfcFlowInstrument", () => new IfcFlowInstrument(host, null, null, null)},
                {"IfcProtectiveDeviceTrippingUnit", () => new IfcProtectiveDeviceTrippingUnit(host, null, null, null)},
                {"IfcSensor", () => new IfcSensor(host, null, null, null)},
                {"IfcUnitaryControlElement", () => new IfcUnitaryControlElement(host, null, null, null)},
                {"IfcDistributionFlowElement", () => new IfcDistributionFlowElement(host, null, null, null)},
                {"IfcDistributionChamberElement", () => new IfcDistributionChamberElement(host, null, null, null)},
                {"IfcEnergyConversionDevice", () => new IfcEnergyConversionDevice(host, null, null, null)},
                {"IfcFlowController", () => new IfcFlowController(host, null, null, null)},
                {"IfcFlowFitting", () => new IfcFlowFitting(host, null, null, null)},
                {"IfcFlowMovingDevice", () => new IfcFlowMovingDevice(host, null, null, null)},
                {"IfcFlowSegment", () => new IfcFlowSegment(host, null, null, null)},
                {"IfcFlowStorageDevice", () => new IfcFlowStorageDevice(host, null, null, null)},
                {"IfcFlowTerminal", () => new IfcFlowTerminal(host, null, null, null)},
                {"IfcFlowTreatmentDevice", () => new IfcFlowTreatmentDevice(host, null, null, null)},
            };
            //ToDo: Catch Exception if Element does not exist?
            //Choose correct IfcElement class from dictionary
            var buildingElement = dict.ContainsKey(IfcClass) ? dict[IfcClass]() : dict["IfcDistributionElement"]();

            //set attributes of the building element
            buildingElement.Name = elementName;

            // return GUID of recently created element
            return buildingElement.GlobalId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="elementName"></param>
        /// <param name="IfcClass"></param>
        /// <param name="hostGuid"></param>
        /// <returns></returns>
        public static string AddBuiltElement(ref DatabaseIfc database, string elementName, string IfcClass, string hostGuid)
        {
            // get host
            var host = database.OfType<IfcObjectDefinition>().FirstOrDefault(a => a.Guid.ToString() == hostGuid);

            // catch issues of no element with specified host guid is available
            if (host == null)
            {
                host = database.Project.UppermostSite();
            }

            // ToDo: extend the dict to create any kind of IfcElement specialization
            // ToDo: catch exceptions of specific IfcClass doesnt exist
            //For all subclasses of IfcDistributionElements another input parameter is necessary (IfcDistributionSystem)
            //Not yet completed!!!
            Dictionary<string, Func<IfcElement>> dict = new Dictionary<string, Func<IfcElement>>
            {
                //Contains all subclasses of IfcBuiltElement
                {"IfcBeam", () => new IfcBeam(host, null, null)},
                {"IfcBearing", () => new IfcBearing(host, null, null)},
                {"IfcBuildingElementProxy", () => new IfcBuildingElementProxy(host, null, null) },
                {"IfcBuiltElement", () => new IfcBuiltElement(host, null, null)},
                {"IfcChimney", () => new IfcChimney(host,null,null) },
                {"IfcColumn", () => new IfcColumn(host,null,null) },
                {"IfcCourse", () => new IfcCourse(host,null,null) },
                {"IfcCovering", () => new IfcCovering(host,null,null) },
                {"IfcCurtainWall", () => new IfcCurtainWall(host,null,null) },
                {"IfcDeepFoundation", () => new IfcDeepFoundation(host,null,null) },
                {"IfcDoor", () => new IfcDoor(host,null,null) },
                {"IfcEarthworksElement", () => new IfcEarthworksElement(host,null,null) },
                {"IfcFooting", () => new IfcFooting(host,null,null) },
                {"IfcKerb", () => new IfcKerb(host,null,null) },
                {"IfcMember", () => new IfcMember(host,null,null) },
                {"IfcMooringDevice", () => new IfcMooringDevice(host,null,null) },
                {"IfcNavigationElement", () => new IfcNavigationElement(host,null,null) },
                {"IfcPavement", () => new IfcPavement(host,null,null) },
                {"IfcPlate", () => new IfcPlate(host,null,null) },
                {"IfcRail", () => new IfcRail(host,null,null) },
                {"IfcRailing", () => new IfcRailing(host,null,null) },
                {"IfcRamp", () => new IfcRamp(host,null,null) },
                {"IfcRampFlight", () => new IfcRampFlight(host,null,null) },
                {"IfcRoof", () => new IfcRoof(host,null,null) },
                {"IfcShadingDevice", () => new IfcShadingDevice(host,null,null) },
                {"IfcSlab", () => new IfcSlab(host,null,null) },
                {"IfcStair", () => new IfcStair(host,null,null) },
                {"IfcStairFlight", () => new IfcStairFlight(host,null,null) },
                {"IfcTrackElement", () => new IfcTrackElement(host,null,null) },
                {"IfcWall", () => new IfcWall(host, null, null)},
                {"IfcWindow", () => new IfcWindow(host,null,null) },
                {"Default", () => new IfcBuildingElementProxy(host,null,null) },

            };

            //ToDo: Catch Exception if Element does not exist?
            //Choose correct IfcElement class from dictionary
            var buildingElement = dict.ContainsKey(IfcClass) ? dict[IfcClass]() : dict["Default"]();

            //set attributes of the building element
            buildingElement.Name = elementName;

            // return GUID of recently created element
            return buildingElement.GlobalId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="elementName"></param>
        /// <param name="IfcClass"></param>
        /// <param name="hostGuid"></param>
        /// <returns></returns>
        public static string AddElementComponent(ref DatabaseIfc database, string elementName, string IfcClass,
            string hostGuid)
        {
            // get host
            var host = database.OfType<IfcObjectDefinition>().FirstOrDefault(a => a.Guid.ToString() == hostGuid);

            // catch issues of no element with specified host guid is available
            if (host == null)
            {
                host = database.Project.UppermostSite();
            }

            // ToDo: catch exceptions of specific IfcClass doesnt exist

            Dictionary<string, Func<IfcElement>> dict = new Dictionary<string, Func<IfcElement>>
            {
                //IfcElementComponent
                {"IfcBuildingElementPart", () => new IfcBuildingElementPart(host, null, null)},
                {"IfcDiscreteAccessory", () => new IfcDiscreteAccessory(host, null, null)},
                {"IfcFastener", () => new IfcFastener(host, null, null)},
                {"IfcImpactProtectionDevice", () => new IfcImpactProtectionDevice(host, null, null)},
                {"IfcMechanicalFastener", () => new IfcMechanicalFastener(host, null, null)},
                //IfcReinforcingElement is abstract
                {"IfcReinforcingBar", () => new IfcReinforcingBar(host, null, null)},
                {"IfcReinforcingMesh", () => new IfcReinforcingMesh(host, null, null)},
                {"IfcTendon", () => new IfcTendon(host, null, null)},
                {"IfcTendonAnchor", () => new IfcTendonAnchor(host, null, null)},
                {"IfcTendonConduit", () => new IfcTendonConduit(host, null, null)},

                {"IfcSign", () => new IfcSign(host, null, null)},
                {"IfcVibrationDamper", () => new IfcVibrationDamper(host, null, null)},
                {"IfcVibrationIsolator", () => new IfcVibrationIsolator(host, null, null)}
            };
            //Choose correct IfcElement class from dictionary
            var buildingElement = dict.ContainsKey(IfcClass) ? dict[IfcClass]() : dict["IfcBuildingElementPart"]();

            //set attributes of the building element
            buildingElement.Name = elementName;

            // return GUID of recently created element
            return buildingElement.GlobalId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="elementName"></param>
        /// <param name="IfcClass"></param>
        /// <param name="hostGuid"></param>
        /// <returns></returns>
        public static string AddOtherElement(ref DatabaseIfc database, string elementName, string IfcClass,
            string hostGuid)
        {
            // get host
            var host = database.OfType<IfcObjectDefinition>().FirstOrDefault(a => a.Guid.ToString() == hostGuid);

            // catch issues of no element with specified host guid is available
            if (host == null)
            {
                host = database.Project.UppermostSite();
            }

            // ToDo: catch exceptions of specific IfcClass doesnt exist

            Dictionary<string, Func<IfcElement>> dict = new Dictionary<string, Func<IfcElement>>
            {
                //IfcFeatureElement
                {"IfcSurfaceFeature", () => new IfcSurfaceFeature(host,null,null)},
                //IfcFurnishingElement
                {"IfcFurnishingElement", () => new IfcFurnishingElement(host,null,null)},
                {"IfcFurniture", () => new IfcFurniture(host,null,null)},
                {"IfcSystemFurnitureElement", () => new IfcSystemFurnitureElement(host,null,null)},
                //IfcGeographicElement
                {"IfcGeographicElement", () => new IfcGeographicElement(host,null,null)},
                {"IfcPlant", () => new IfcPlant(host,null,null)},
                //IfcGeotechnicalElement
                { "IfcBorehole", () => new IfcBorehole(host,null,null)},
                { "IfcGeomodel", () => new IfcGeomodel(host,null,null)},
                { "IfcGeoslice", () => new IfcGeoslice(host,null,null)},
                { "IfcSolidStratum", () => new IfcSolidStratum(host,null,null)},
                { "IfcVoidStratum", () => new IfcVoidStratum(host,null,null)},
                { "IfcWaterStratum", () => new IfcWaterStratum(host,null,null)},
                //IfcTransportElement
                { "IfcTransportElement", () => new IfcTransportElement(host,null,null)},
                //IfcVirtualElement
                { "IfcVirtualElement", () => new IfcVirtualElement(host,null,null)}
            };
            //Choose correct IfcElement class from dictionary
            var buildingElement = dict.ContainsKey(IfcClass) ? dict[IfcClass]() : dict["IfcTransportElement"]();

            //set attributes of the building element
            buildingElement.Name = elementName;

            // return GUID of recently created element
            return buildingElement.GlobalId;
        }
    }
}
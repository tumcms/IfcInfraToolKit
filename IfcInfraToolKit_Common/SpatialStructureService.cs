using System;
using System.Collections.Generic;
using System.Linq;
using GeometryGym.Ifc;

namespace IfcInfraToolkit_Common
{
    public class SpatialStructureService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="facilityName"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public Guid AddFacility(ref DatabaseIfc database, string facilityName, IfcSpatialStructureElement host)
        {
            // if no host was found in the model, add the facility to the IfcSite entity
            if (host == null)
            {
                var project = database.Project;
                host = project.Extract<IfcSite>().First();
            }

            // create an IfcFacility element
            var trafficFacility = new IfcFacility(host, facilityName)
            {
                //Should we leave this hardcoded?
                CompositionType = IfcElementCompositionEnum.COMPLEX
            };
            return trafficFacility.Guid;
        }

        /// <summary>
        /// This function creates a facility part. FacilityType differs between Railway, Bridge, Marine, Road or Common Facility Type 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="facilityType">Differs between the different Facility Types, e.g. IfcRailwayPartTypeEnum (case Sensitive)</param>
        /// <param name="facilityPartName">The individual name of the facility Part</param>
        /// <param name="facilityPartType">Enter a valid facility Part Type for the according Facility Type, e.g. for IfcRailwayPartTypeEnum: TRACKSTRUCTURE</param>
        /// <param name="host"></param>
        /// <returns></returns>
        public Guid AddFacilityPart(ref DatabaseIfc database, string facilityType, string facilityPartName,
            string facilityPartType,
            IfcObjectDefinition host)
        {
            Dictionary<string, Func<IfcFacilityPartTypeSelect>> dict =
                new Dictionary<string, Func<IfcFacilityPartTypeSelect>>
                {
                    {
                        "IfcRailwayPartTypeEnum", () => new IfcFacilityPartTypeSelect(
                            (IfcRailwayPartTypeEnum) Enum.Parse(typeof(IfcRailwayPartTypeEnum), facilityPartType,
                                false))
                    },
                    {
                        "IfcBridgePartTypeEnum",
                        () => new IfcFacilityPartTypeSelect(
                            (IfcBridgePartTypeEnum) Enum.Parse(typeof(IfcBridgePartTypeEnum), facilityPartType, false))
                    },
                    {
                        "IfcMarinePartTypeEnum",
                        () => new IfcFacilityPartTypeSelect(
                            (IfcMarinePartTypeEnum) Enum.Parse(typeof(IfcMarinePartTypeEnum), facilityPartType, false))
                    },
                    {
                        "IfcRoadPartTypeEnum",
                        () => new IfcFacilityPartTypeSelect(
                            (IfcRoadPartTypeEnum) Enum.Parse(typeof(IfcRoadPartTypeEnum), facilityPartType, false))
                    },
                    {
                        "IfcFacilityPartCommonTypeEnum",
                        () => new IfcFacilityPartTypeSelect(
                            (IfcFacilityPartCommonTypeEnum) Enum.Parse(typeof(IfcFacilityPartCommonTypeEnum),
                                facilityPartType, false))
                    },
                    {"Default", () => new IfcFacilityPartTypeSelect(IfcFacilityPartCommonTypeEnum.NOTDEFINED)}
                };
            IfcFacilityPartTypeSelect selectedPartType =new IfcFacilityPartTypeSelect();
            if (dict.ContainsKey(facilityType))
            {
                selectedPartType = dict[facilityType]();
            }
            else
            {
                selectedPartType = dict["Default"]();
            }

            //Should this be left hardcoded?
            var usage = IfcFacilityUsageEnum.LATERAL;

            //ToDo: Check if host is valid???
            var facilityPart = host.StepClassName == "IfcFacility" ? new IfcFacilityPart(host as IfcFacility, facilityPartName, selectedPartType, usage) : new IfcFacilityPart(host as IfcFacilityPart, facilityPartName, selectedPartType, usage);

            return facilityPart.Guid;
        }
    }
}
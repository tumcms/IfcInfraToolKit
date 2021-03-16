using System;
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
        /// This function creates a facility part. FacilityType differs between Railway, Bridge, Marine, Road or Common Type 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="facilityType"></param>
        /// <param name="facilityPartName"></param>
        /// <param name="facilityPartType"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public Guid AddFacilityPart(ref DatabaseIfc database, string facilityType, string facilityPartName,
            string facilityPartType,
            IfcObjectDefinition host)
        {
            //Generic Attempt:
            //Type enumType = Type.GetType("GeometryGym.Ifc." + facilityType);
            IfcFacilityPartTypeSelect selectedPartType = new IfcFacilityPartTypeSelect();
            if (facilityType == "IfcRailwayPartTypeEnum")
            {
                IfcRailwayPartTypeEnum partType =
                    (IfcRailwayPartTypeEnum)Enum.Parse(typeof(IfcRailwayPartTypeEnum), facilityPartType,
                        false);
                selectedPartType = new IfcFacilityPartTypeSelect(partType);
            }
            else if (facilityType == "IfcBridgePartTypeEnum")
            {
                IfcBridgePartTypeEnum partType =
                    (IfcBridgePartTypeEnum)Enum.Parse(typeof(IfcBridgePartTypeEnum), facilityPartType, false);
                selectedPartType = new IfcFacilityPartTypeSelect(partType);
            }
            else if (facilityType == "IfcMarinePartTypeEnum")
            {
                IfcMarinePartTypeEnum partType = (IfcMarinePartTypeEnum)Enum.Parse(typeof(IfcMarinePartTypeEnum), facilityPartType, false);
                selectedPartType = new IfcFacilityPartTypeSelect(partType);
            }
            else if (facilityType == "IfcRoadPartTypeEnum")
            {
                IfcRoadPartTypeEnum partType = (IfcRoadPartTypeEnum)Enum.Parse(typeof(IfcRoadPartTypeEnum), facilityPartType, false);
                selectedPartType = new IfcFacilityPartTypeSelect(partType);
            }
            else if (facilityType == "IfcFacilityPartCommonTypeEnum")
            {
                IfcFacilityPartCommonTypeEnum partType =
                    (IfcFacilityPartCommonTypeEnum)Enum.Parse(typeof(IfcFacilityPartCommonTypeEnum), facilityPartType,
                        false);
                selectedPartType = new IfcFacilityPartTypeSelect(partType);
            }
            else
            {
                //Default value, does this one make sense?
                selectedPartType = new IfcFacilityPartTypeSelect(IfcFacilityPartCommonTypeEnum.NOTDEFINED);
            }
            // ToDo: parse user input and select corresponding IFC content

            //Should this be left hardcoded?
            var usage = IfcFacilityUsageEnum.LATERAL;

            var facilityPart = host.StepClassName == "IfcFacility" ? new IfcFacilityPart(host as IfcFacility, facilityPartName, selectedPartType, usage) : new IfcFacilityPart(host as IfcFacilityPart, facilityPartName, selectedPartType, usage);

            return facilityPart.Guid;
        }
    }
}
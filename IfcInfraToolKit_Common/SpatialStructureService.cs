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
                CompositionType = IfcElementCompositionEnum.COMPLEX
            };
            return trafficFacility.Guid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="facilityPartName"></param>
        /// <param name="facilityType"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public Guid AddFacilityPart(ref DatabaseIfc database, string facilityPartName, string facilityType,
            IfcObjectDefinition host)
        {
            // ToDo: parse user input and select corresponding IFC content
            var partType = new IfcFacilityPartTypeSelect(IfcBridgePartTypeEnum.ABUTMENT);
            var usage = IfcFacilityUsageEnum.LATERAL;

            var facilityPart = host.StepClassName == "IfcFacility" ? new IfcFacilityPart(host as IfcFacility, facilityPartName, partType, usage) : new IfcFacilityPart(host as IfcFacilityPart, facilityPartName, partType, usage);

            return facilityPart.Guid;
        }
    }
}
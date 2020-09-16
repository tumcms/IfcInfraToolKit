using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryGym.Ifc;

namespace IfcInfraToolkit_Common
{
    public class ProjectSetupService
    {
        /// <summary>
        /// DFLT Constructor
        /// </summary>
        public ProjectSetupService()
        {

        }

        /// <summary>
        /// creates a new DatabaseIfc instance using IFC4X3 and returns it
        /// </summary>
        /// <returns></returns>
        public DatabaseIfc CreateDatabase()
        {
            var database = new DatabaseIfc(ModelView.Ifc4X3NotAssigned);
            return database;
        }

        /// <summary>
        /// Set basic things that every ifc model needs -> InfraDEPL UT1
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public DatabaseIfc AddBaseProjectSetup(DatabaseIfc database)
        {
            // create IfcSite instance
            var site = new IfcSite(database, "sampleSite");

            // create top-most spatial structure element IfcProject, set units and assign facility to project
            var project = new IfcProject(site, "myProject", IfcUnitAssignment.Length.Metre);
            return database;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="FacilityName"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public Guid AddFacility(ref DatabaseIfc database, string FacilityName, IfcSpatialStructureElement host)
        {
            if (host == null)
            {
                var project = database.Project;
                host = project.Extract<IfcSite>().First();
            }


            // create an IfcFacility element
            var trafficFacility = new IfcFacility(host, FacilityName)
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
            IfcSpatialStructureElement host)
        {
            // ToDo: parse user input and select corresponding IFC content
            var partType = new IfcFacilityPartTypeSelect(IfcBridgePartTypeEnum.ABUTMENT);
            var usage = IfcFacilityUsageEnum.LATERAL;

            var facilityPart = host.StepClassName == "IfcFacility" ? new IfcFacilityPart(host as IfcFacility, facilityPartName, partType, usage) : new IfcFacilityPart(host as IfcFacilityPart, facilityPartName, partType, usage);

            return facilityPart.Guid;
        }
    }
}

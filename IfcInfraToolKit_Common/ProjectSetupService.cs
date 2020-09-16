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
        /// <param name="FacilityType"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public DatabaseIfc AddFacility(DatabaseIfc database, string FacilityName, string FacilityType,
            IfcSpatialStructureElement host)
        {
            // create an IfcFacility element
            var trafficFacility = new IfcFacility(host, FacilityName)
            {
                CompositionType = IfcElementCompositionEnum.COMPLEX
            };
            return database;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="facilityPartName"></param>
        /// <param name="facilityType"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public DatabaseIfc AddFacilityPart(DatabaseIfc database, string facilityPartName, string facilityType,
            IfcFacility host)
        {
            // ToDo: parse user input and select corresponding IFC content
            var partType = new IfcFacilityPartTypeSelect(IfcBridgePartTypeEnum.ABUTMENT);
            var usage = IfcFacilityUsageEnum.LATERAL;

            var facilityPart = new IfcFacilityPart(host, facilityPartName, partType, usage);

            return database;
        }
    }
}

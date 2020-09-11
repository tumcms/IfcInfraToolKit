using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryGym.Ifc;

namespace IfcInfraToolkit
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

            // create an IfcFacility element
            var facilityA = new IfcFacility(site, "FacilityA")
            {
                CompositionType = IfcElementCompositionEnum.NOTDEFINED
            };
            var facilityB = new IfcFacility(site, "myFacilityB")
            {
                CompositionType = IfcElementCompositionEnum.NOTDEFINED
            };

            var facilityPart1 = new IfcFacilityPart(
                facilityA,
                "Part1",
                new IfcFacilityPartTypeSelect(
                    IfcBridgePartTypeEnum.DECK), 
                IfcFacilityUsageEnum.LATERAL);

            return database;

        }
    }
}

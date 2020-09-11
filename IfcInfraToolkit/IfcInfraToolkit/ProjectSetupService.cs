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
            var trafficFacility = new IfcFacility(site, "TrafficWayA")
            {
                CompositionType = IfcElementCompositionEnum.COMPLEX
            };
            var River = new IfcFacility(site, "River")
            {
                CompositionType = IfcElementCompositionEnum.NOTDEFINED
            };

            var facilityPart1 = new IfcFacilityPart(
                trafficFacility,
                "myRoadPart01",
                new IfcFacilityPartTypeSelect(
                    IfcRoadPartTypeEnum.ROADSEGMENT), 
                IfcFacilityUsageEnum.LONGITUDINAL);
            facilityPart1.Description = "TrafficWayA -> Segment 1";

            var facilityPart2 = new IfcFacility(trafficFacility, "myBridge");
            facilityPart2.Description = "TrafficWayA -> Segment 2";

            var facilityPart3 = new IfcFacilityPart(
                trafficFacility,
                "myRoadPart02",
                new IfcFacilityPartTypeSelect(
                    IfcRoadPartTypeEnum.ROADSEGMENT),
                IfcFacilityUsageEnum.LONGITUDINAL);
            facilityPart3.Description = "TrafficWayA -> Segment 3";
            
            // River facility
            var RiverPart = new IfcFacilityPart(
                River,
                "myRiver", 
                new IfcFacilityPartTypeSelect(
                    IfcMarinePartTypeEnum.WATERFIELD), 
                IfcFacilityUsageEnum.LONGITUDINAL );
            RiverPart.Description = "River that passes under the bridge";


            return database;

        }
    }
}

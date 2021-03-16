using System;
using System.Linq;
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
        /// Set basic things that every ifc model needs -> InfraDEPL UT1
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public DatabaseIfc AddBaseProjectSetup(DatabaseIfc database, string projectName, string siteName)
        {
            // create IfcSite instance
            var site = new IfcSite(database, siteName);

            // create top-most spatial structure element IfcProject, set units and assign facility to project
            var project = new IfcProject(site, projectName, IfcUnitAssignment.Length.Metre);
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
            //IfcProject:   Direct assignment to project, if the facility is the outermost spatial container, and no site information is provided for the building projects
            //IfcSite:      Assignment to site, if the facility is the spatial container for the building project with site information
            //IfcFacility:  Assignment to another facility as spatial container, e.g. if this facility represents a facility section.
            
            // if no host was found in the model, add the facility to the IfcSite entity
            if (host == null)
            {
                var project = database.Project;
                host = project.Extract<IfcSite>().First();

                // if no IfcSite information exists for the building project, direct assignment to project
                if (host == null)
                {
                    var spatialElement= database.Project.RootElement();
                    //ToDo: change IFCSpatialElement into IfcSpatialStructureElement
                }
            }

            // create an IfcFacility element
            var trafficFacility = new IfcFacility(host, FacilityName)
            {
                //ToDo: should it be left hardcoded?
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
                    (IfcRailwayPartTypeEnum) Enum.Parse(typeof(IfcRailwayPartTypeEnum), facilityPartType,
                        false);
                selectedPartType = new IfcFacilityPartTypeSelect(partType);
            }
            else if (facilityType == "IfcBridgePartTypeEnum") { 
                IfcBridgePartTypeEnum partType =
                    (IfcBridgePartTypeEnum) Enum.Parse(typeof(IfcBridgePartTypeEnum), facilityPartType, false);
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
            else if (facilityType== "IfcFacilityPartCommonTypeEnum")
            {
                IfcFacilityPartCommonTypeEnum partType =
                    (IfcFacilityPartCommonTypeEnum) Enum.Parse(typeof(IfcFacilityPartCommonTypeEnum), facilityPartType,
                        false);
                selectedPartType = new IfcFacilityPartTypeSelect(partType);
            }
            else
            {
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

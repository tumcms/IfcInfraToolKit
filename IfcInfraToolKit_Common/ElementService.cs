

using System;
using System.Collections.Generic;
using System.Linq;
using GeometryGym.Ifc;

namespace IfcInfraToolkit_Common
{
    public class ElementService
    {
       
        // ToDo: Create method overloading to accept also geometry and placement

        public string AddElement(ref DatabaseIfc database, string elementName, string IfcClass, string hostGuid, IfcProductDefinitionShape representation=null, IfcLocalPlacement placement=null)
        {
            // get host
            var host = database.OfType<IfcObjectDefinition>().FirstOrDefault(a => a.Guid.ToString() == hostGuid);

            // catch issues of no element with specified host guid is available
            if (host== null)
            {
                host = database.Project.UppermostSite();
            }

            // ToDo: extend the dict to create any kind of IfcElement specialization
            // ToDo: catch exceptions of specific IfcClass doesnt exist
            Dictionary<string, Func<IfcElement>> dict = new Dictionary<string, Func<IfcElement>>
            {
                {"IfcBeam", () => new IfcBeam(host, null, null)},
                {"IfcBearing", () => new IfcBearing(host, null, null)},
                {"IfcBuiltElement", () => new IfcBuiltElement(host, null, null)},
                {"IfcWall", () => new IfcWall(host, null, null)}
            };

            //set attributes of the building element
            var buildingElement = dict[IfcClass]();
            buildingElement.Name = elementName;
            buildingElement.Representation = representation;
            buildingElement.ObjectPlacement = placement;

            // return GUID of recently created element
            return buildingElement.GlobalId;
        }
    }
}
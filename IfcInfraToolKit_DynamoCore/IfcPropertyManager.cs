using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

namespace IfcInfraToolKit_DynamoCore
{
    public class IfcPropertyManager
    {
        [IsVisibleInDynamoLibrary(false)]   // Don't show the constructor in Dynamo 
        internal IfcPropertyManager()
        {
        }

        /// <summary>
        /// Adds a list of IfcProperties to an Ifc Instance with a specific hostGuid
        /// </summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <param name="hostGuid">GlobalId of host item</param>
        /// <param name="pSetName">arbitrary name of the property set</param>
        /// <param name="ifcProperties">List of IfcProperty with the properties that should be added</param>
        /// <returns>Returns the updated database container including the added property set</returns>
        /// <search> IFC, add, append, IfcProperty, list IfcProperty, append property, property, set of properties, property set </search>
        [NodeCategory("Actions")]
        [MultiReturn(new[] { "DatabaseContainer"})]
        public static Dictionary<string, object> AppendPropertySet(DatabaseContainer databaseContainer, string hostGuid, string pSetName, List<IfcProperty> ifcProperties)
        {
            // get current db
            var database = databaseContainer.Database;

            var propertyService = new PropertyService();
            database = propertyService.AddPSet(database, hostGuid, pSetName, ifcProperties);
            
            // assign updated db to container
            databaseContainer.Database = database;

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer}
            };

            return d;
        }

        /// <summary>
        /// Adds a list of IfcProperties to an Ifc Instance having the specified hostElementName
        /// </summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <param name="hostElementName">arbitrarily defined name of host item</param>
        /// <param name="pSetName">arbitrary name of the property set</param>
        /// <param name="ifcProperties">List of IfcProperty with the properties that should be added</param>
        /// <returns>Returns the updated database container including the added property set</returns>
        /// <search> IFC, add, append, IfcProperty, list IfcProperty, append property, property, set of properties, property set, append property by elementName  </search>
        [NodeCategory("Actions")]
        [MultiReturn(new[] { "DatabaseContainer" })]
        public static Dictionary<string, object> AppendPropertySetByElementName(DatabaseContainer databaseContainer, string hostElementName, string pSetName, List<IfcProperty> ifcProperties)
        {
            // get current db
            var database = databaseContainer.Database;

            var propertyService = new PropertyService();
            database = propertyService.AddPSetByElementName(database, hostElementName, pSetName, ifcProperties);

            // assign updated db to container
            databaseContainer.Database = database;

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer}
            };

            return d;
        }


        /// <summary>
        /// Adds an IfcPropertySingleValue as text property to the database
        /// </summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <param name="pName">arbitrary name of the property set</param>
        /// <param name="pValue">IfcValue as string, that should be added</param>
        /// <returns>Returns the updated database container including the added Property</returns>
        /// <search> IFC, add, IfcValue, append property, property, value, append value, append property as text </search>
        [NodeCategory("Actions")]
        [MultiReturn(new[] { "DatabaseContainer", "IfcTextProperty" })]
        public static Dictionary<string, object> CreateIfcTextProperty(DatabaseContainer databaseContainer, string pName, string pValue)
        {
            // get current db
            var database = databaseContainer.Database;

            var propertyService = new PropertyService();
            var ifcProperty = propertyService.CreateIfcTextProperty(database, pName, pValue);

            // assign updated db to container
            databaseContainer.Database = database;

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"IfcTextProperty", ifcProperty}

            };

            return d;
        }
    }
}
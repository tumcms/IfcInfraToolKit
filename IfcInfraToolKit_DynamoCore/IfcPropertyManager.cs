using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

namespace IfcInfraToolKit_DynamoCore
{
    public class IfcPropertyManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <param name="pSetName"></param>
        /// <param name="ifcProperties"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <param name="pSetName"></param>
        /// <param name="ifcProperties"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer" })]
        public static Dictionary<string, object> AppendPropertySetByElementName(DatabaseContainer databaseContainer, string HostElementName, string pSetName, List<IfcProperty> ifcProperties)
        {
            // get current db
            var database = databaseContainer.Database;

            var propertyService = new PropertyService();
            database = propertyService.AddPSetByElementName(database, HostElementName, pSetName, ifcProperties);

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
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="pName"></param>
        /// <param name="pValue"></param>
        /// <returns></returns>
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
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;

namespace IfcInfraToolKit_DynamoCore
{
    public class ElementDesigner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContainer"></param>
        /// <param name="IfcClass">Desired IfcClass name</param>
        /// <param name="elementName"></param>
        /// <param name="hostGuid">GlobalId of parent element</param>
        /// <returns></returns>
        [MultiReturn(new[] { "DatabaseContainer", "ElementGUID" })]
        public static Dictionary<string, object> AddElement(DatabaseContainer databaseContainer,
            string IfcClass, string elementName = "DefaultElement",
            string hostGuid = "null")
        {
            // get current db
            var database = databaseContainer.Database;
            
            var service = new ElementService();
            var guid = service.AddElement(ref database, elementName, IfcClass, hostGuid);

            // assign updated db to container
            databaseContainer.Database = database;

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"ElementGUID", guid}
            };

            return d;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using GeometryGym.Ifc;

namespace IfcInfraToolkit_Common
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyService
    {
        /// <summary>
        /// 
        /// </summary>
        public PropertyService()
        {
        }

        /// <summary>
        /// generates a new string-based IfcProperty 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="pName"></param>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public IfcProperty CreateIfcTextProperty(DatabaseIfc database, string pName, string pValue)
        {
            var property = new IfcPropertySingleValue(database, pName, new IfcIdentifier(pValue));

            return property;
        }

        /// <summary>
        ///  takes a list of IfcProperties and adds them as a property set to a given host element
        /// </summary>
        /// <param name="database"></param>
        /// <param name="hostGuid"></param>
        /// <param name="pSetName"></param>
        /// <param name="ifcProperties"></param>
        /// <returns></returns>
        public DatabaseIfc AddPSet(DatabaseIfc database, string hostGuid, string pSetName, List<IfcProperty> ifcProperties)
        {
            //var value = hostGuid.Replace("_", "/").Replace("-", "+");
            //byte[] buffer = Convert.FromBase64String(value + "==");
            //var guid =  new Guid(buffer);


            var host = database.OfType<IfcObjectDefinition>().FirstOrDefault(a => a.Guid.ToString() == hostGuid);
            if (host == null)
            {
                var e = new Exception("did not find host element by given GUID.");
                throw e;
            }

            new IfcPropertySet(host, pSetName, ifcProperties);

            return database;
        }

        /// <summary>
        ///  takes a list of IfcProperties and adds them as a property set to a given host element
        /// </summary>
        /// <param name="database"></param>
        /// <param name="hostGuid"></param>
        /// <param name="pSetName"></param>
        /// <param name="ifcProperties"></param>
        /// <returns></returns>
        public DatabaseIfc AddPSetByElementName(DatabaseIfc database, string ElementName, string pSetName, List<IfcProperty> ifcProperties)
        {
            //var value = hostGuid.Replace("_", "/").Replace("-", "+");
            //byte[] buffer = Convert.FromBase64String(value + "==");
            //var guid =  new Guid(buffer);


            var host = database.OfType<IfcObjectDefinition>().FirstOrDefault(a => a.Name == ElementName);
            if (host == null)
            {
                var e = new Exception("did not find host element by given GUID.");
                throw e;
            }

            new IfcPropertySet(host, pSetName, ifcProperties);

            return database;
        }


    }
}
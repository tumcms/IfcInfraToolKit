using System.Collections.Generic;
using Autodesk.DesignScript.Runtime;
using IfcInfraToolkit_Common;


namespace IfcInfraToolKit_DynamoCore
{
    /// <summary>
    /// Provides basic methods to create an IFC model and add some spatial structure stuff
    /// </summary>
    public static class IfcModelSetup 
    {
        /// <summary> Creates a new DatabaseIfc instance that acts as a central container for the IFC content. </summary>
        /// <search> init, create, IFC </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc </returns>
        [MultiReturn(new[] {"DatabaseContainer"})]
        public static Dictionary<string, object> CreateIfcModel()
        {
            // init container
            var container = new DatabaseContainer();

            // add content to the database
            ProjectSetupService service = new ProjectSetupService();
            container.Database = service.CreateDatabase();
            
            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", container}
            };

            return d;
        }

        /// <summary> Stores an DatabaseIfc instance into an *.ifc Model </summary>
        /// <param name="container">IFC container</param>
        /// <param name="path">folder to store the resulting model</param>
        /// <param name="modelName">file name without *.ifc label</param>
        /// <search> store, save, IFC </search>
        public static void SaveIfcModel(DatabaseContainer container, string path, string modelName)
        {
            container.Database.WriteFile(path + "/" + modelName + ".ifc");
        }

    }
}

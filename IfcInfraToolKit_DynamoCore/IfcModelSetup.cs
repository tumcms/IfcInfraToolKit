using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using GeometryGym.Ifc;
using IfcInfraToolkit_Common;
using Microsoft.SqlServer.Server;


namespace IfcInfraToolKit_DynamoCore
{
    /// <summary>
    /// Provides basic methods to create an IFC model and add some spatial structure stuff
    /// </summary>
    public static class IfcModelSetup 
    {
        #region CreateAndSave


        /// <summary> Creates a new DatabaseIfc instance that acts as a central container for the IFC content. </summary>
        /// <param name="projectName"></param>
        /// <param name="siteName"></param>
        /// <search> init, create, IFC </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc </returns>
        [MultiReturn(new[] {"DatabaseContainer"})]
        public static Dictionary<string, object> CreateIfcModel(string projectName = "sampleProject", string siteName = "sampleSite")
        {
            // init container
            var container = new DatabaseContainer();

            // add content to the database
            ProjectSetupService service = new ProjectSetupService();
            container.Database = service.CreateDatabase();
            container.Database = service.AddBaseProjectSetup(container.Database, projectName , siteName );

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

        #endregion

        #region OpenExistingModel
        /// <summary> Creates a new DatabaseIfc instance that acts as a central container for the IFC content. </summary>
        /// <search> init, create, IFC </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc </returns>
        [MultiReturn(new[] { "DatabaseContainer" })]
        public static Dictionary<string, object> OpenIfcModel(string path)
        {
            // init container
            var container = new DatabaseContainer();

            container.Database = new DatabaseIfc(path);

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", container}
            };

            return d;
        }


        #endregion

       

        /// <summary>
        /// Watch node for Ifc content in the database
        /// </summary>
        /// <returns></returns>
        public static DatabaseIfc WatchIFC(DatabaseContainer databaseContainer)
        {
            return databaseContainer.Database;
        }

        /// <summary> Creates a new DatabaseIfc instance that acts as a central container for the IFC content. </summary>
        /// <search> init, create, IFC </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc </returns>
        [NodeCategory("Query")]
        [MultiReturn(new[] {"DatabaseContainer", "GUIDs", "Names", "IfcClasses"})]
        public static Dictionary<string, object> GetObjectDefinitionItems(DatabaseContainer databaseContainer)
        {
            var db = databaseContainer.Database;

            // get all IfcObjectDefinition items
            var items = db.OfType<IfcObjectDefinition>().ToList();

            var guids = items.Select(a => a.GlobalId).ToList();
            var names = items.Select(a => a.Name).ToList();
            var clsNames = items.Select(a => a.StepClassName).ToList();

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"GUIDs", guids}, 
                {"Names", names}, 
                {"IfcClasses", clsNames}, 

            };
            return d;
        }

        /// <summary> Creates a new DatabaseIfc instance that acts as a central container for the IFC content. </summary>
        /// <search> init, create, IFC </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc </returns>
        [NodeCategory("Query")]
        [MultiReturn(new[] { "DatabaseContainer", "GUIDs", "Names", "IfcClasses", "PredefinedTypes" })]
        public static Dictionary<string, object> GetSpatialStructureItems(DatabaseContainer databaseContainer)
        {
            var db = databaseContainer.Database;

            // get all IfcObjectDefinition items
            var items = db.OfType<IfcSpatialElement>().ToList();

            var guids = items.Select(a => a.GlobalId).ToList();
            var names = items.Select(a => a.Name).ToList();
            var clsNames = items.Select(a => a.StepClassName).ToList();
            var pdts = items.Select(a => a.GetPredefinedType()).ToList();

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"GUIDs", guids},
                {"Names", names},
                {"IfcClasses", clsNames},
                {"PredefinedTypes", pdts},

            };
            return d;
        }

        /// <summary> Creates a new DatabaseIfc instance that acts as a central container for the IFC content. </summary>
        /// <search> init, create, IFC </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc </returns>
        [NodeCategory("Query")]
        [MultiReturn(new[] { "DatabaseContainer", "GUIDs", "Names", "IfcClasses", "PredefinedTypes" })]
        public static Dictionary<string, object> GetElementItems(DatabaseContainer databaseContainer)
        {
            var db = databaseContainer.Database;

            // get all IfcObjectDefinition items
            var items = db.OfType<IfcElement>().ToList();

            var guids = items.Select(a => a.GlobalId).ToList();
            var names = items.Select(a => a.Name).ToList();
            var clsNames = items.Select(a => a.StepClassName).ToList();
            var pdts = items.Select(a => a.GetPredefinedType()).ToList();

            // beautiful return values
            var d = new Dictionary<string, object>
            {
                {"DatabaseContainer", databaseContainer},
                {"GUIDs", guids},
                {"Names", names},
                {"IfcClasses", clsNames},
                {"PredefinedTypes", pdts},

            };
            return d;
        }

    }
}

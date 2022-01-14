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
        /// <search> init, create, IFC, create model, ifc model </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc </returns>
        [NodeCategory("Actions")]
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

        /// <summary> Stores a DatabaseIfc instance into a *.ifc Model </summary>
        /// <param name="databaseContainer">IFC container</param>
        /// <param name="path">folder to store the resulting model</param>
        /// <param name="modelName">file name without *.ifc label</param>
        /// <search> store, save, IFC </search>
        [NodeCategory("Actions")]
        public static void SaveIfcModel(DatabaseContainer databaseContainer, string path, string modelName)
        {
            databaseContainer.Database.WriteFile(path + "/" + modelName + ".ifc");
        }

        #endregion

        #region OpenExistingModel
        /// <summary> Creates a new DatabaseIfc instance that acts as a central container for the IFC content. </summary>
        /// <param name="path">folder and model name for opening the ifc model</param>
        /// <search> init, create, IFC, open model, existing model, open </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc </returns>
        [NodeCategory("Actions")]
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
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <search> watch, IFC, database content, model content, content </search>
        /// <returns></returns>
        [NodeCategory("Actions")]
        public static DatabaseIfc WatchIFC(DatabaseContainer databaseContainer)
        {
            return databaseContainer.Database;
        }

        /// <summary> Lists attributes of all IfcObjectDefinition items in the database</summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <search> IfcObjectDefinition, IFC, get, object definition, get object </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc, List of specific attributes of all IfcObjectDefinition items in this database </returns>
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

        /// <summary> Lists attributes of all IfcSpatialElements from the database. </summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <search> IFC, spatial structure, get, spatial, IfcSpatialElement </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc, List of specific attributes of all IfcSpatialElements in this database </returns>
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

        /// <summary> Lists attributes of all IfcElements in the database. </summary>
        /// <param name="databaseContainer">IFC container including all Ifc content</param>
        /// <search> IFC, find, get, IfcElement, elements, get elements </search>
        /// <returns> DatabaseContainer that owns the DatabaseIfc object of GeometryGymIfc, List of specific attributes of all IfcElements in this database </returns>
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

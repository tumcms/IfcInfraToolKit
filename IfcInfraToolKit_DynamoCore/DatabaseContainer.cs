using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;

namespace IfcInfraToolKit_DynamoCore
{
    /// <summary>
    /// Central storage item to interact with IFC content
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]   // Don't show this class in Dynamo but make it accessible among all dlls
    public class DatabaseContainer
    {
        /// <summary>
        /// the IFC model object
        /// </summary>
        public DatabaseIfc Database { get; set; }
        /// <summary>
        /// The directory where the model should be stored to
        /// </summary>
        public string ExportPath { get; set; }
        /// <summary>
        /// name of the IFC model
        /// </summary>
        public string ModelName { get; set; }
    }
}
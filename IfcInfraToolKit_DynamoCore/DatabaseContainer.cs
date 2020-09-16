using Autodesk.DesignScript.Runtime;
using GeometryGym.Ifc;

namespace IfcInfraToolKit_DynamoCore
{
    [IsVisibleInDynamoLibrary(false)]   // Don't show this class in Dynamo but make it accessible among all dlls
    public class DatabaseContainer
    {
        public DatabaseIfc Database { get; set; }
        public string ExportPath { get; set; }
        public string ModelName { get; set; }
    }
}
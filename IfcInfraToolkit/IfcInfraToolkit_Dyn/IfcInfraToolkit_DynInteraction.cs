
namespace IfcInfraToolkit_Dyn
{
    public static class IfcInfraToolkitDyn
    {
        public static string getDocumentName(Autodesk.AECC.Interop.UiLand.AeccDocument document)
        {
            return document.Name;
        }

        public static string getAlignmentName(Autodesk.AECC.Interop.UiLand.AeccDocument document)
        {
            var alignments = document.AlignmentsSiteless;
            return "alingmentNames";
        }

    }
}

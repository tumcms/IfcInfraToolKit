// Sebastian Esser20200608

namespace IfcInfraToolkit_Dyn
{
    public class SessionVariables
    {
        #region PRIVATE PROPERTIES


        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// Gets or sets the land XML path.
        /// </summary>
        /// <value>
        /// The land XML path.
        /// </value>
        public static string LandXMLPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is land XML exported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is land XML exported; otherwise, <c>false</c>.
        /// </value>
        public static bool IsLandXMLExported { get; set; }

        /// <summary>
        /// Returns the CivilApplication object
        /// </summary>
        public static CivilApplication CivilApplication { get; set; }

        /// <summary>
        /// Returns true if the shared parameters have been created for this session.
        /// </summary>
        public static bool ParametersCreated { get; set; }

        /// <summary>
        /// Returns a Dynamo CoordinateSystem that represents the Revti Document Total Transform for the session.
        /// </summary>
        public static Autodesk.DesignScript.Geometry.CoordinateSystem DocumentTotalTransform { get; set; }

        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionVariables"/> class.
        /// </summary>
        internal SessionVariables()
        {
            ParametersCreated = false;
            DocumentTotalTransform = null;
        }

        #endregion

        #region PRIVATE METHODS


        #endregion

        #region PUBLIC METHODS


        #endregion
    }
}
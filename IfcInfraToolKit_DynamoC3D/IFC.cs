/* Author: Stefan Huber 
   Creation: 12.06.2020
   File: IFC.cs
   Description: library for C3D dynamo to export corridors to IFC models

 */
  //Not needed anymore

using System;
using GeometryGym.Ifc;
using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using NUnit.Framework;
using System.Linq;
using NUnit.Framework.Constraints;
using Autodesk.AECC.Interop.Land;
using IfcInfraToolKit_DynamoCore;
using Autodesk.DesignScript.Runtime;


namespace IfcInfraToolkit_Dyn
{
    public class IFC_root
    {
        private DatabaseIfc _vb;

        /// <summary>
        /// Initialize IFC model container
        /// </summary>
        /// <param name="familyName">Author's given name</param>
        /// <param name="firstName">Author's first name</param>
        /// <param name="organization">optional: organization</param>
        /// <param name="project">optional project name for IFC model</param>
        public IFC_root(string familyName, string firstName, string organization="none",string project="project1")
        {
            // init new Ifc model database
            _vb = new DatabaseIfc(ModelView.Ifc4Reference);
            _vb.Release = ReleaseVersion.IFC4X3;
            
            // set model author and organization
            IfcPerson author = new IfcPerson(_vb);
            author.FamilyName = familyName;
            author.GivenName = firstName;
            IfcOrganization org = new IfcOrganization(_vb, organization);
            IfcProject projectname = new IfcProject(_vb, project);
            IfcSite site = new IfcSite(_vb, "local_site");
            projectname.AddAggregated(site);

        }

        //Finalizing 
        public void IFC_export(string modelName, string filepath)
        {
            // build path
            var finalPath = filepath + "/" + modelName + ".ifc";

            // write the database to the STEP file
            _vb.WriteFile(finalPath);
        }






    }
}


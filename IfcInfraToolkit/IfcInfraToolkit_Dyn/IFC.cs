/* Author: Stefan Huber 
   Creation: 12.06.2020
   File: IFC.cs
   Description: library for C3D dynamo to export corridors to IFC models

 */


using System.Collections.Generic;
using Autodesk.AECC.Interop.Land;
using Autodesk.AECC.Interop.Roadway;
using Autodesk.AECC.Interop.UiRoadway;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryGym.Ifc;

namespace IfcInfraToolkit_Dyn
{
    public class IFC_root
    {
        private DatabaseIfc _vb;

        //Construktor
        public IFC_root(String FamilyName, String first_name,String organzization)
        {
            _vb = new DatabaseIfc(ModelView.Ifc4Reference);
            _vb.Release = ReleaseVersion.IFC4X3;
            IfcPerson author = new IfcPerson(_vb);
            author.FamilyName = FamilyName;
            author.GivenName = first_name;
            IfcOrganization org = new IfcOrganization(_vb,organzization);

        }

        //Finalising 
        public void IFC_export(String filepath)
        {
            _vb.WriteFile(filepath);
        }

    }
}


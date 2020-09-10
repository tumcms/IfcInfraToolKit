using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometryGym.Ifc;

namespace IfcInfraToolkit
{
    public class ProjectSetupService
    {
        public ProjectSetupService()
        {

        }

        public GeometryGym.Ifc.DatabaseIfc AddBaseProjectSetup(DatabaseIfc database)
        {
            var project = new IfcProject(database, null);

            return database;
        }


    }
}

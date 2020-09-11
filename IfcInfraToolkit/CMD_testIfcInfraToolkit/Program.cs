using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IfcInfraToolkit;
using GeometryGym.Ifc;

namespace CMD_testIfcInfraToolkit
{
    class Program
    {
        static void Main(string[] args)
        {

            var myProjectSetupService = new IfcInfraToolkit.ProjectSetupService();
            var myIfcDatabase = myProjectSetupService.CreateDatabase();

            myIfcDatabase = myProjectSetupService.AddBaseProjectSetup(myIfcDatabase);


            myIfcDatabase.WriteFile("myIfcModel.ifc");
        }
    }
}

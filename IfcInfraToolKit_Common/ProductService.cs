

using System;
using System.Collections.Generic;
using GeometryGym.Ifc;

namespace IfcInfraToolkit_Common
{
    public class ProductService
    {
        /// <summary>
        /// This functions creates a subtype of IfcBuildingElement using the specified type in "buildingType"
        /// </summary>
        /// <param name="buildingType">Case sensitive, specify the type of the IfcBuildingElement</param>
        /// <param name="database"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static IfcBuiltElement createBuildingElement(string buildingType, ref DatabaseIfc database, double x,
            double y,
            double z)
        {
            switch (buildingType)
            {
                case "IfcBeam":
                    return new IfcBeam(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcBearing":
                    return new IfcBearing(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcBuildingElementProxy":
                    return new IfcBuildingElementProxy(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcChimney":
                    return new IfcChimney(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcColumn":
                    return new IfcColumn(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcCourse":
                    return new IfcCourse(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcCovering":
                    return new IfcCovering(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcCurtainWall":
                    return new IfcCurtainWall(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcDeepFoundation":
                    return new IfcDeepFoundation(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcDoor":
                    return new IfcDoor(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcEarthworksElement":
                    return new IfcEarthworksElement(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcFooting":
                    return new IfcFooting(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcMember":
                    return new IfcMember(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcKerb":
                    return new IfcBuildingElementProxy(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                    ; //somehow not possible using IfcKerb
                case "IfcMooringDevice":
                    return new IfcMooringDevice(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcNavigationElement":
                    return new IfcNavigationElement(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcPavement":
                    return new IfcPavement(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcPlate":
                    return new IfcPlate(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcRail":
                    return new IfcRail(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcRailing":
                    return new IfcRailing(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcRamp":
                    return new IfcRamp(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcRampFlight":
                    return new IfcRampFlight(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcRoof":
                    return new IfcRoof(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcShadingDevice":
                    return new IfcShadingDevice(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcSlab":
                    return new IfcSlab(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcStair":
                    return new IfcStair(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcStairFlight":
                    return new IfcStairFlight(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcTrackElement":
                    return new IfcTrackElement(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcWall":
                    return new IfcWall(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                case "IfcWindow":
                    return new IfcWindow(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
                default:
                    return new IfcBuildingElementProxy(database.Project,
                        new IfcLocalPlacement(
                            new IfcAxis2Placement3D(
                                new IfcCartesianPoint(database, x, y, z))),
                        null);
            }
        }

    }
}
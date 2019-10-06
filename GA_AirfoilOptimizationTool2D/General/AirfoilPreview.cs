using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General
{
    /// <summary>
    /// This Class provides some functions for previewing airfoil.
    /// </summary>
    public static class AirfoilPreview
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="airfoil">Previewing Airfoil</param>
        /// <param name="height">Preview Window Height</param>
        /// <param name="width">Preview Window Width</param>
        /// <returns></returns>
        public static System.Collections.ObjectModel.ObservableCollection<System.Windows.Point> GetPreviewPointList(Airfoil.AirfoilManager airfoil, double height, double width)
        {
            System.Collections.ObjectModel.ObservableCollection<System.Windows.Point> pointList = new System.Collections.ObjectModel.ObservableCollection<System.Windows.Point>();
            var airfoilCoordinates = airfoil.InterpolatedCoordinate;

            double airfoilLength = Airfoil.AirfoilCoordinate.GetMaximumValue(airfoilCoordinates, 0) - Airfoil.AirfoilCoordinate.GetMinimumValue(airfoilCoordinates, 0);
            double airfoilHeight = Airfoil.AirfoilCoordinate.GetMaximumValue(airfoilCoordinates, 1) - Airfoil.AirfoilCoordinate.GetMinimumValue(airfoilCoordinates, 1);

            double magnification = 0.8 * (double)width / (double)airfoilLength;

            // Adjustment variable
            double adjustX = (width - airfoilLength * magnification) / 2 - Airfoil.AirfoilCoordinate.GetMinimumValue(airfoilCoordinates, 0);
            double adjustZ = (height - airfoilHeight * magnification) / 2 + Airfoil.AirfoilCoordinate.GetMaximumValue(airfoilCoordinates, 1) * magnification;

            // Adjust Airfoil Coordinates to Fit Preview Window
            var adjustedCoordinate = Airfoil.AirfoilCoordinate.Scaling(airfoilCoordinates, magnification);
            var temp = new Double[adjustedCoordinate.Length, 2];
            for (int i = 0; i < adjustedCoordinate.Length; i++)
            {
                temp[i, 0] = adjustX + adjustedCoordinate[i].X;
                temp[i, 1] = adjustZ - adjustedCoordinate[i].Z;
            }
            adjustedCoordinate.Import(temp);

            // Convert adjustCoordinate to ObservableCollection 
            for (int i = 0; i < adjustedCoordinate.Length; i++)
            {
                pointList.Add(new System.Windows.Point() { X = adjustedCoordinate[i].X, Y = adjustedCoordinate[i].Z });
            }

            return pointList;
        }
    }
}

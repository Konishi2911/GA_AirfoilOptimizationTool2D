﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FOptConfig.Models
{
    public class AirfoilCoordinate
    {
        public List<Double[]> CoordinateList = new List<double[]>();

        public AirfoilCoordinate() { }

        public void Import(Double[,] coordinate)
        {
            try
            {
                // Format Check
                var rowNo = coordinate.GetLength(0);
                var columnNo = coordinate.GetLength(1);
                if (columnNo != 2)
                {
                    throw new FormatException();
                }

                // Add coordinate
                for (int i = 0; i < rowNo; i++)
                {
                    var dataRpw = new Double[2];
                    for (int j = 0; j < columnNo; j++)
                    {
                        dataRpw[j] = coordinate[i, j];
                    }
                    CoordinateList.Add(dataRpw);
                }

            }
            catch (FormatException)
            {

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.FMainWindow.Models
{
    class AirfoilSynthesizer
    {
        private Airfoil.AirfoilManager _synthesizedAirfoil;

        public Airfoil.AirfoilManager SynthesizedAirfoil
        {
            get
            {
                return _synthesizedAirfoil;
            }
            private set
            {
                _synthesizedAirfoil = value;
            }
        }

        public void SynthesizeAirfoil(Airfoil.AirfoilManager[] airfoils, Double[] coefficients)
        {
            // Format Check
            var length = airfoils.Length;
            if(length != coefficients.Length)
            {
                throw new FormatException("Number of basis airfoils and coefficients did not match.");
            }


            Double[,] newCoordinate = new Double[length, 2];
            for (int i = 0; i < length; i++)
            {
                var basis = airfoils[i].ResizedCoordinate;

                newCoordinate[i, 0] += coefficients[i] * basis[i].X;
                newCoordinate[i, 1] += coefficients[i] * basis[i].Z;
            }
            var sAirfoil = new Airfoil.AirfoilCoordinate();
            sAirfoil.Import(newCoordinate);

            SynthesizedAirfoil = new Airfoil.AirfoilManager(sAirfoil);
        }
    }
}

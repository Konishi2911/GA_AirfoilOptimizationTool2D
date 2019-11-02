﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.Airfoil
{
    /// <summary>
    /// This class will be removed. Not be recomended to use.
    /// </summary>
    public class CombinedAirfoilsGroupManager
    {
        private Airfoil.Representation.AirfoilCombiner[] _combinedAirfoils;
        /// <summary>
        /// Number of combined airfoils
        /// </summary>
        private int _numberOfAirfoils;

        public event EventHandler CombinedAirfoilsUpdated;
        public class CombinedAirfoilsUpdatedEventArgs : EventArgs
        {
            public Airfoil.Representation.AirfoilCombiner[] combinedAirfoils;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numOfAirfoils">Number of combined airfoils</param>
        public CombinedAirfoilsGroupManager(int numOfAirfoils)
        {
            this._numberOfAirfoils = numOfAirfoils;
            this._combinedAirfoils = new Airfoil.Representation.AirfoilCombiner[_numberOfAirfoils];
        }

        public CombinedAirfoilsGroupManager(Representation.AirfoilCombiner[] elements)
        {
            this._numberOfAirfoils = elements.Length;
            this._combinedAirfoils = elements;
        }

        /// <summary>
        /// This method combines the passed base airfoil with the passed coefficients as aparallel processing. 
        /// If finish the processing, it fires the event combinatedAirfoilUpdated.
        /// </summary>
        /// <param name="basisAirfoils"></param>
        /// <param name="coefficients"></param>
        public void CombineAirfoils(General.BasisAirfoils basisAirfoils, Double[,] coefficients)
        {
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < _numberOfAirfoils; i++)
            {
                // Null Check
                if (_combinedAirfoils[i] == null)
                {
                    _combinedAirfoils[i] = new Representation.AirfoilCombiner();
                }

                var x = i;
                var basis = basisAirfoils.AirfoilGroup.ToArray();
                var coef = GetRowArray(coefficients, i);

                // Update Source and Re-Combinate Airfoil.
                tasks.Add(Task.Run(() => _combinedAirfoils[x].UpdateBaseSource(coef, basis)));
            }
            Task.WaitAll(tasks.ToArray());

            // Fire the event combinedAirfoilsUpdated
            CombinedAirfoilsUpdated?.Invoke(this, new CombinedAirfoilsUpdatedEventArgs() { combinedAirfoils = this._combinedAirfoils });
        }

        public void CombineAirfoils(General.BasisAirfoils basisAirfoils, Double[,] coefficients, bool isAppend)
        {
            List<Task> tasks = new List<Task>();
            Representation.AirfoilCombiner[] tempAirfoils = new Representation.AirfoilCombiner[coefficients.GetLength(1)];

            for (int i = 0; i < _numberOfAirfoils; i++)
            {
                // Null Check
                if (tempAirfoils[i] == null)
                {
                    tempAirfoils[i] = new Representation.AirfoilCombiner();
                }

                var x = i;
                var basis = basisAirfoils.AirfoilGroup.ToArray();
                var coef = GetRowArray(coefficients, i);

                // Update Source and Re-Combinate Airfoil.
                tasks.Add(Task.Run(() => tempAirfoils[x].UpdateBaseSource(coef, basis)));
            }
            Task.WaitAll(tasks.ToArray());

            var tempCurrentAirfoils = new List<Representation.AirfoilCombiner>(_combinedAirfoils);
            tempCurrentAirfoils.AddRange(tempAirfoils);
            _combinedAirfoils = tempCurrentAirfoils.ToArray();

            // Fire the event combinedAirfoilsUpdated
            CombinedAirfoilsUpdated?.Invoke(this, new CombinedAirfoilsUpdatedEventArgs() { combinedAirfoils = this._combinedAirfoils });
        }

        /// <summary>
        /// This method returns the combinedAirfoils as a class type AirfoilCombiner array.
        /// </summary>
        /// <returns></returns>
        public Airfoil.Representation.AirfoilCombiner[] GetCombinedAirfoilsArray()
        {
            return _combinedAirfoils.Clone() as Representation.AirfoilCombiner[];
        }

        public void AddElement(Representation.AirfoilCombiner element)
        {
            List<Representation.AirfoilCombiner> tempAirfoils = new List<Representation.AirfoilCombiner>(_combinedAirfoils);
            tempAirfoils.Add(element);
            _combinedAirfoils = tempAirfoils.ToArray();

            // Increment number of airfoils
            ++this._numberOfAirfoils;

            // Fire the event combinedAirfoilsUpdated
            CombinedAirfoilsUpdated?.Invoke(this, new CombinedAirfoilsUpdatedEventArgs() { combinedAirfoils = this._combinedAirfoils });
        }

        private T[] GetRowArray<T>(T[,] array, int columnNumber)
        {
            var length = array.GetLength(0);
            var width = array.GetLength(1);

            var rArray = new T[width][];

            for (int i = 0; i < width; i++)
            {
                rArray[i] = new T[length];
                for (int j = 0; j < length; j++)
                {
                    rArray[i][j] = array[j, i];
                }
            }
            return rArray[columnNumber];
        }
        private T[] GetColumnArray<T>(T[,] array, int rowNumber)
        {
            var length = array.GetLength(0);
            var width = array.GetLength(1);

            var rArray = new T[width][];

            for (int i = 0; i < length; i++)
            {
                rArray[i] = new T[length];
                for (int j = 0; j < width; j++)
                {
                    rArray[i][j] = array[i, j];
                }
            }
            return rArray[rowNumber];
        }
    }
}

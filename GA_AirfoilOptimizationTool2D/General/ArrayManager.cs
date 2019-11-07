using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General
{
    public static class ArrayManager
    {
        public static T[][] ConvertArrayToJuggedArray<T>(T[,] array)
        {
            var length = array.GetLength(0);
            var width = array.GetLength(1);
            var jArray = new T[length][];

            for (int i = 0; i < length; i++)
            {
                jArray[i] = new T[width];
                for (int j = 0; j < width; j++)
                {
                    jArray[i][j] = array[i, j];
                }
            }

            return jArray;
        }
        /// <summary>
        /// If invalid format jArray is passed, null is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jArray"></param>
        /// <returns></returns>
        public static T[,] ConvertJuggedArrayToArray<T>(T[][] jArray)
        {
            bool isSameSize = true;
            // FormatCheck
            for (int i = 0; i < jArray.Length - 1; i++)
            {
                isSameSize &= jArray[i].Length == jArray[i + 1].Length;
            }

            if (isSameSize == false)
            {
                return null;
            }

            var length = jArray.Length;
            var width = jArray[0].Length;
            var array = new T[length, width];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array[i, j] = jArray[i][j];
                }
            }

            return array;
        }

        public static List<List<T>> ConvertArrayToList<T>(T[,] array)
        {
            var height = array.GetLength(0);
            var width = array.GetLength(1);
            List<List<T>> list = new List<List<T>>();

            for (int i = 0; i < height; i++)
            {
                list.Add(new List<T>());
                for (int j = 0; j < width; j++)
                {
                    list[i].Add(array[i, j]);
                }
            }

            return list;
        }
        public static List<List<T>> ConvertJuggedArrayToList<T>(T[][] jArray)
        {
            List<List<T>> list = new List<List<T>>();

            for (int i = 0; i < jArray.Length; i++)
            {
                list.Add(new List<T>());
                for (int j = 0; j < jArray[i].Length; j++)
                {
                    list[i].Add(jArray[i][j]);
                }
            }
            return list;
        }

        /// <summary>
        /// If invalid format list is passed, null is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T[,] ConvertListToArray<T>(List<List<T>> list)
        {
            bool isSameSize = true;
            // FormatCheck
            for (int i = 0; i < list.Count - 1; i++)
            {
                isSameSize &= list[i].Count == list[i + 1].Count;
            }

            if (isSameSize == false)
            {
                return null;
            }

            var length = list.Count;
            var width = list[0].Count;
            var array = new T[length, width];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    array[i, j] = list[i][j];
                }
            }

            return array;
        }
    }
}

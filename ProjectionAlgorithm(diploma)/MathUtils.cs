using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    public static class MathUtils
    {
        private static Random rnd = new Random();
        public static double GetUniformDistribution(double a, double b)
        {
            double rand = rnd.NextDouble();
            double res = a + (b - a) * rand;
            return res;
        }

        /// <summary>
        /// Дискретная равномерно распределённая случайная величина
        /// </summary>
        /// <param name="left">Левая граница включается</param>
        /// <param name="right">Правая граница включается</param>
        /// <returns></returns>
        public static int GetDicreteUniformDistribution(int left, int right)
        {
            double betweenZeroOne = rnd.NextDouble();
            double result = Math.Floor(betweenZeroOne * right);
            return (int)result;
        }

        public static double Ln(double x)
        {
            double res = Math.Log(x);
            return res;
        }

        public static int ChooseBtwTwoRndVars()
        {
            int res = rnd.Next(0, 2);
            return res;
        }

        public static double GetDistBtwPoints((double, double) point1, (double, double) point2)
        {
            double result = Math.Pow(point1.Item1 - point2.Item1, 2) + Math.Pow(point1.Item2 - point2.Item2, 2);
            return Math.Sqrt(result);
        }

        /// <summary>
        /// Фундаментальное решение (в теории обозначается за Eps).
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static double E((double, double) p1, (double, double) p2)
        {
            double result = ((double)1 / (2 * Math.PI)) * Ln((double)1 / GetDistBtwPoints(p1, p2));
            return result;
        }

    }
}

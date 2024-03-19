using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    public class Vector : IEnumerable<double>
    {
        private double[] data;
        public const int GetColCount = 1;
        private int dimension;

        public Vector(IEnumerable<double> data)
        {
            this.data = data.ToArray();
            this.dimension = this.data.Length;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            double[] data1 = v1.data;
            double[] data2 = v2.data;
            int n = data1.Length;

            double[] resData = new double[n];
            for (int i = 0; i < n; i++)
            {
                resData[i] = data1[i] + data2[i];
            }
            var res = new Vector(resData);
            return res;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            double[] data1 = v1.data;
            double[] data2 = v2.data;
            int n = data1.Length;

            double[] resData = new double[n];
            for (int i = 0; i < n; i++)
            {
                resData[i] = data1[i] - data2[i];
            }

            var res = new Vector(resData);
            return res;
        }

        /// <summary>
        /// Скалярное произведение двух векторов
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(Vector v1, Vector v2)
        {
            if (v1.data.Length != v2.data.Length)
            {
                throw new Exception("Вектора должны иметь одинаковые размерности!");
            }
            double[] data1 = v1.data;
            double[] data2 = v2.data;
            int n = data1.Length;

            double res = 0;
            for (int i = 0; i < n; i++)
            {
                res += data1[i] * data2[i];
            }
            return res;
        }

        public static Vector operator *(Vector v1, double factor)
        {
            double[] data = v1.data;
            int n = data.Length;

            double[] resData = new double[n];
            for (int i = 0; i < n; i++)
            {
                resData[i] = factor * data[i];
            }
            Vector res = new Vector(resData);
            return res;
        }

        public static Vector operator *(double factor, Vector v1)
        {
            double[] data = v1.data;
            int n = data.Length;

            double[] resData = new double[n];
            for (int i = 0; i < n; i++)
            {
                resData[i] = factor * data[i];
            }
            Vector res = new Vector(resData);
            return res;
        }

        public double this[int index]
        {
            get => this.data[index];
            set => this.data[index] = value;
        }

        public int GetDimension()
        {
            return this.dimension;
        }

        public double GetEuclideanNorm()
        {
            double squaredNorm = 0;
            for (int i = 0; i < this.dimension; i++)
            {
                squaredNorm += this.data[i] * this.data[i];
            }

            return Math.Sqrt(squaredNorm);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        public IEnumerator<double> GetEnumerator()
        {
            return ((IEnumerable<double>)data).GetEnumerator();
        }
    }
}

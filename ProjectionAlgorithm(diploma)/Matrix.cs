using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    public class Matrix
    {
        private double[,] data;

        public Matrix(double[,] data)
        {
            this.data = data;
        }

        public double this[int i, int j]
        {
            get => this.data[i, j];
            set => this.data[i, j] = value;
        }

        public int GetRowCount()
        {
            int res = this.data.GetLength(0);
            return res;
        }

        public int GetColCount()
        {
            int res = this.data.GetLength(1);
            return res;
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            //int n = 
            //double[] resData = new double[]
            return null;
        }
    }
}

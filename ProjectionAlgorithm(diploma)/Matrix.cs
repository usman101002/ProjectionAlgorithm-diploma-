using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    public class Matrix
    {
        private double[,] data;
        private int rowCount;
        private int colCount; 

        public Matrix(double[,] data)
        {
            this.data = data;
            rowCount = data.GetLength(0);
            colCount = data.GetLength(1);
        }

        public double this[int i, int j]
        {
            get => this.data[i, j];
            set => this.data[i, j] = value;
        }

        public int GetRowCount()
        {
            return this.rowCount;
        }

        public int GetColCount()
        {
            return this.colCount;
        }

        public Vector GetRowByIndex(int index)
        {
            var resData = new double[this.colCount];
            for (int j = 0; j < this.colCount; j++)
            {
                resData[j] = this.data[index, j];
            }

            var result = new Vector(resData);
            return result;
        }

        public Vector GetColByIndex(int index)
        {
            var resData = new double[this.rowCount];
            for (int i = 0; i < this.rowCount; i++)
            {
                resData[i] = this.data[i, index];
            }

            var result = new Vector(resData);
            return result;
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            if (matrix.colCount != vector.GetDimension())
            {
                throw new Exception("Число столбцов в матрице должно быть равно размерности вектора!");
            }

            int rowCount = matrix.rowCount;
            int colCount = matrix.colCount;
            int resDimension = rowCount;
            double[] resData = new double[resDimension];

            for (int i = 0; i < resDimension; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    resData[i] += matrix[i, j] * vector[j];
                }
            }

            var result = new Vector(resData);
            return result;
        }

        public static Vector operator *(Vector vector, Matrix matrix)
        {
            if (vector.GetDimension() != matrix.rowCount)
            {
                throw new Exception("Число стобцов в векторе должно быть равно числу строк в матрице");
            }
            int rowCount = matrix.rowCount;
            int colCount = matrix.colCount;
            int resDimension = colCount;
            double[] resData = new double[resDimension];

            for (int i = 0; i < resDimension; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    resData[i] += vector[j] * matrix[j, i];
                }
            }

            var result = new Vector(resData);
            return result;
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.colCount != matrix2.rowCount)
            {
                throw new Exception("Число столбцов первой матрицы должно быть равно числу строк второй матрицы!");
            }

            double[,] resData = new double[matrix1.rowCount, matrix2.colCount];
            int m1RowCount = matrix1.rowCount;
            int m2ColCount = matrix2.colCount;
            int m2RowCount = matrix2.rowCount;
            

            return null;
        }
    }
}

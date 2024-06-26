﻿using System;
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

        //public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        //{
        //    if (matrix1.colCount != matrix2.rowCount)
        //    {
        //        throw new Exception("Число столбцов первой матрицы должно быть равно числу строк второй матрицы!");
        //    }

        //    double[,] resData = new double[matrix1.rowCount, matrix2.colCount];
        //    int m1RowCount = matrix1.rowCount;
        //    int m1ColCount = matrix1.colCount;
        //    int m2ColCount = matrix2.colCount;
        //    int m2RowCount = matrix2.rowCount;

        //    if (matrix1.IsDiagonal = true)
        //    {
        //        for (int i = 0; i < m2RowCount; i++)
        //        {
        //            for (int j = 0; j < m2ColCount; j++)
        //            {
        //                resData[i, j] = matrix1[i, i] * matrix2[i, j];
        //            }
        //        }
        //        Matrix resLeftDiagCase = new Matrix(resData);
        //        return resLeftDiagCase;
        //    }

        //    else if (matrix2.IsDiagonal = true)
        //    {
        //        for (int i = 0; i < m1RowCount; i++)
        //        {
        //            for (int j = 0; j < m1ColCount; j++)
        //            {
        //                resData[i, j] = matrix1[i, j] * matrix2[j, j];
        //            }
        //        }

        //        Matrix resRightDiagCase = new Matrix(resData);
        //        return resRightDiagCase;
        //    }

        //    for (int i = 0; i < m1RowCount; i++)
        //    {
        //        for (int j = 0; j < m2ColCount; j++)
        //        {
        //            for (int k = 0; k < m2RowCount; k++)
        //            {
        //                resData[i, j] += matrix1[i, k] * matrix2[k, j];
        //            }
        //        }
        //    }

        //    Matrix res = new Matrix(resData);
        //    return res;
        //}

        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.rowCount != matrix2.rowCount || matrix1.colCount != matrix2.colCount)
            {
                throw new Exception("Матрицы должны иметь одинаковые размерности!");
            }

            int m1RowCount = matrix1.rowCount;
            int m1ColCount = matrix1.colCount;
            double[,] resData = new double[m1RowCount, m1ColCount];

            for (int i = 0; i < m1RowCount; i++)
            {
                for (int j = 0; j < m1ColCount; j++)
                {
                    resData[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
            Matrix res = new Matrix(resData);
            return res;
        }

        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.rowCount != matrix2.rowCount || matrix1.colCount != matrix2.colCount)
            {
                throw new Exception("Матрицы должны иметь одинаковые размерности!");
            }

            int m1RowCount = matrix1.rowCount;
            int m1ColCount = matrix1.colCount;
            double[,] resData = new double[m1RowCount, m1ColCount];

            for (int i = 0; i < m1RowCount; i++)
            {
                for (int j = 0; j < m1ColCount; j++)
                {
                    resData[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
            Matrix res = new Matrix(resData);
            return res;
        }

        public static Matrix operator *(Matrix matrix1, double scalar)
        {
            int m1RowCount = matrix1.rowCount;
            int m1ColCount = matrix1.colCount;
            double[,] resData = new double[m1RowCount, m1ColCount];

            for (int i = 0; i < m1RowCount; i++)
            {
                for (int j = 0; j < m1ColCount; j++)
                {
                    resData[i, j] = matrix1[i, j] * scalar;
                }
            }
            Matrix res = new Matrix(resData);
            return res;
        }

        public static Matrix operator *(double scalar, Matrix matrix1)
        {
            int m1RowCount = matrix1.rowCount;
            int m1ColCount = matrix1.colCount;
            double[,] resData = new double[m1RowCount, m1ColCount];

            for (int i = 0; i < m1RowCount; i++)
            {
                for (int j = 0; j < m1ColCount; j++)
                {
                    resData[i, j] = matrix1[i, j] * scalar;
                }
            }
            Matrix res = new Matrix(resData);
            return res;
        }


        /// <summary>
        /// Данный метод актуален только для диагональных квадратных матриц!!!
        /// </summary>
        /// <returns></returns>
        public Matrix Inverse()
        {
            double[,] inverseData = new double[this.rowCount, this.colCount];
            for (int i = 0; i < this.rowCount; i++)
            {
                inverseData[i, i] = (double)1 / this.data[i, i];
            }
            Matrix inverse = new Matrix(inverseData);
            return inverse;
        }

        /// <summary>
        /// Костыльный метод --- работает только для проекционных методов, когда слева или справа
        /// диагональная матрица.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="DiagFromLeft"></param>
        /// <returns></returns>
        public Matrix Multiply(Matrix matrix, bool DiagFromLeft)
        {
            int rowCount = matrix.rowCount;
            int colCount = matrix.colCount;
            double[,] resData = new double[matrix.rowCount, matrix.colCount];
            // подразумевается, что либо слева, либо левый, либо правый множитель будет диагональной матрицей
            if (DiagFromLeft == true)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        resData[i, j] = this[i, i] * matrix[i, j];
                    }
                }
                Matrix resLeftDiagCase = new Matrix(resData);
                return resLeftDiagCase;
            }
            else
            {
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        resData[i, j] = this[i, j] * matrix[j, j];
                    }
                }

                Matrix resRightDiagCase = new Matrix(resData);
                return resRightDiagCase;
            }
        }

        public Matrix DiagonalMultiply(Matrix matrix)
        {
            int rowCount = matrix.rowCount;
            int colCount = matrix.colCount;
            double[,] resData = new double[matrix.rowCount, matrix.colCount];

            for (int i = 0; i < rowCount; i++)
            {
                resData[i, i] = this[i, i] * matrix[i, i];
            }

            Matrix result = new Matrix(resData);
            return result;
        }

    }
}

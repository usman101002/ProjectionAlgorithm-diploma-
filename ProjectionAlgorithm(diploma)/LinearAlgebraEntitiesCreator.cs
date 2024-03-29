﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonteKarloMatrixVectorProduct;

namespace ProjectionAlgorithm_diploma_
{
    /// <summary>
    /// В качестве тестового примера вектором x должен быть вектор, состоящий из единиц.
    /// В качестве матрицы AMatrix будет матрица c элементами вида i * size + (j + 1). В соответствии с матрицей будет
    /// уже заполняться вектор b. А потом уже эта сущность будет передаваться в класс-решателя СЛАУ
    /// проекционным методом. 
    /// </summary>
    internal class AlgebraEntitiesCreator
    {
        public Matrix AMatrix { get; set; } = null;
        public Vector BVector { get; set; } = null;

        public AlgebraEntitiesCreator(Matrix aMatrix, Vector bVector)
        {
            this.AMatrix = aMatrix;
            this.BVector = bVector;
        }

        /// <summary>
        /// Левая и правая часть СЛАУ составляются таким образом, чтобы решением был вектор (1,...1).
        /// </summary>
        /// <param name="n"></param>
        public AlgebraEntitiesCreator(int n)
        {
            this.AMatrix = this.CreateMatrix(n);
            this.BVector = this.CreateVector(n);
        }

        
        private Matrix CreateMatrix(int n)
        {
            var aArr = new double[n, n];
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < n; j++)
            //    {
            //        aArr[i, j] = (double)(n * i + j + 1) / n * n * n;
            //    }
            //}
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    aArr[i, j] = (i + 1);
                }
            }

            var result = new Matrix(aArr);
            return result;
        }

        /// <summary>
        /// Предполагается, что 
        /// </summary>
        /// <param name="n">Размерность вектора</param>
        /// <returns></returns>
        private Vector CreateVector(int n)
        {
            if (this.AMatrix == null)
            {
                throw new Exception("Матрица A должна уже быть заполнена");
            }

            var bArr = new double[n];
            for (int i = 0; i < n; i++)
            {
                bArr[i] = this.AMatrix.GetRowByIndex(i).Sum();
            }

            var result = new Vector(bArr);
            return result;
        }

        #region MyRegion
        //private Matrix<double> CreateStochasticMatrix(int n)
        //{
        //    var aArr = new double[n, n];
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            aArr[i, j] = rnd.NextDouble();
        //        }
        //    }

        //    for (int i = 0; i < n; i++)
        //    {
        //        double s = 0;
        //        for (int j = 0; j < n; j++)
        //        {
        //            s += aArr[i, j];
        //        }

        //        for (int j = 0; j < n; j++)
        //        {
        //            aArr[i, j] /= s;
        //        }
        //    }

        //    var builder = Matrix<double>.Build;
        //    var result = builder.DenseOfArray(aArr);
        //    return result;
        //}

        //private Vector<double> CreateStochasticBVector(int n)
        //{
        //    var b = new double[n];

        //    for (int i = 0; i < n; i++)
        //    {
        //        b[i] = rnd.NextDouble();
        //    }

        //    double s = b.Sum();
        //    for (int i = 0; i < n; i++)
        //    {
        //        b[i] /= s;
        //    }

        //    var builder = Vector<double>.Build;
        //    var result = builder.DenseOfArray(b);
        //    return result;
        //}

        #endregion

    }
}

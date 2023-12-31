﻿using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MonteKarloMatrixVectorProduct;

namespace ProjectionAlgorithm_diploma_
{
    internal class LinearSystemSolver : ILinearSystemSolver
    {
        public Matrix<double> A { get; set; }
        public Vector<double> BVector { get; set; }
        public Walker Walker { get; set; }

        public LinearSystemSolver(Matrix<double> a, Vector<double> bVector)
        {
            A = a;
            BVector = bVector;
            Walker = new Walker(GetDistributionOfMatrixRows(A));
        }



        /// <summary>
        /// Метод позволяет решать СЛАУ Ax = BVector, где BVector -- вектор правой части с
        /// помощью проекционного метода.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Vector<double> Solve()
        {
            var xPrev = Vector<double>.Build.Dense(this.BVector.Count);
            //var xPrev = BVector;
            for (int i = 1; i < 2000000; i++)
            {
                int index = this.Walker.GetSelection();
                var numerator = this.BVector[index] - (xPrev * A.Row(index));
                var denominator = this.A.Row(index).Select(x => x * x).Sum();
                var xCur = xPrev + ((numerator * A.Row(index)) / (denominator));
                xPrev = xCur;
                if (i % 10000 == 0)
                    Console.WriteLine(i);
                
            }
            return xPrev;
        }

        private List<double> GetDistributionOfMatrixRows(Matrix<double> matrix)
        {
            var probabilities = new List<double>();
            var matrixRows = new List<List<double>>();

            //матрица 
            for (int i = 0; i < matrix.RowCount; i++)
            {
                var matrixRow = matrix.Row(i).ToList();
                matrixRows.Add(matrixRow);
            }

            double normSquared = 0;
            for (int i = 0; i < matrixRows.Count; i++)
            {
                for (int j = 0; j < matrixRows.Count; j++)
                {
                    normSquared += matrixRows[i][j] * matrixRows[i][j];
                }
            }

            for (int i = 0; i < matrixRows.Count; i++)
            {
                probabilities.Add(matrixRows[i].Select(x => x * x).Sum() / normSquared);
            }

            var sum = probabilities.Sum();

            return probabilities;
        }
    }
}

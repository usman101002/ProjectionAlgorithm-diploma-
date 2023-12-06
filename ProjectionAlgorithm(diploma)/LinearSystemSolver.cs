using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
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

            for (int i = 0; i < matrixRows.Count; i++)
            {
                var norm = matrix.FrobeniusNorm();
                probabilities.Add((Math.Sqrt(matrixRows[i].Select(x => x * x).Sum())) / norm );
            }

            return probabilities;
        }

        

    }
}

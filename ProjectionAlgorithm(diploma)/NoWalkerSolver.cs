using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    /// <summary>
    /// Потом сюда можно будет добавить итерационное уточнение.
    /// </summary>
    public class NoWalkerSolver : IProjectionSolver
    {
        private Random rnd;

        public NoWalkerSolver()
        {
            this.rnd = new Random();
        }

        public Vector<double> Solve(Matrix<double> aMatrix, Vector<double> bVector)
        {
            // Переход к равномерно распределённой матрице путём домножения специальной диагональной матрицы на исходную.
            // Вектор правой части соответственно тоже домножается на эту диагональную матрицу. 
            var diagProbMatrix = this.GetDiagProbMatrix(aMatrix);
            var uniformMatrix = diagProbMatrix * aMatrix;
            var newB = diagProbMatrix * bVector;

            int rowCount = uniformMatrix.RowCount;
            var xPrev = newB;
            int numberOfIterations = 1000 * 1000;
            for (int i = 0; i < numberOfIterations; i++)
            {
                int index = this.GetRandomIndex(rowCount);
                var uniformMatrixRow = uniformMatrix.Row(index);
                var numerator = newB[index] - (xPrev * uniformMatrixRow);
                var denominator = this.GetRowNorm(uniformMatrixRow) * this.GetRowNorm(uniformMatrixRow);
                var xCur = xPrev + ((numerator * uniformMatrixRow) / denominator);
                xPrev = xCur;
            }

            return xPrev;
        }

        
        private Matrix<double> GetDiagProbMatrix(Matrix<double> matrix)
        {
            int rowCount = matrix.RowCount;
            int colCount = matrix.ColumnCount;
            double[,] matrixArr = new double[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                var row = matrix.Row(i);
                var norm = GetRowNorm(row);
                matrixArr[i, i] = 1 / norm;
            }

            var result = DenseMatrix.OfArray(matrixArr);
            return result;
        }

        private List<double> GetDistributionOfMatrixRows(Matrix<double> matrix)
        {
            var probabilities = new List<double>();
            var matrixRows = new List<List<double>>();

            for (int i = 0; i < matrix.RowCount; i++)
            {
                var matrixRow = matrix.Row(i).ToList();
                matrixRows.Add(matrixRow);
            }

            double frobeniusNormSquared = 0;
            for (int i = 0; i < matrixRows.Count; i++)
            {
                for (int j = 0; j < matrixRows.Count; j++)
                {
                    frobeniusNormSquared += matrixRows[i][j] * matrixRows[i][j];
                }
            }

            for (int i = 0; i < matrixRows.Count; i++)
            {
                var rowNorm = GetRowNorm(matrixRows[i]);
                probabilities.Add(rowNorm * rowNorm / frobeniusNormSquared);
            }
            //var sum = probabilities.Sum(); нужно для проверки того, что получившийся набор действительно является распределением.
            return probabilities;
        }

        private double GetRowNorm(IEnumerable<double> row)
        {
            var result = row.Select(x => x * x).Sum();
            result = Math.Sqrt(result);
            return result;
        }

        /// <summary>
        /// В NoWalkerSolver будет обычная генерация случайного числа от 0 до n (n не включается)
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int GetRandomIndex(int n)
        {
            int result = rnd.Next(n);
            return result;
        }
    }
}

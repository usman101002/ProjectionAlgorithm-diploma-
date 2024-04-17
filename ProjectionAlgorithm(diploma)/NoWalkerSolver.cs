using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Vector Solve(Matrix aMatrix, Vector bVector)
        {
            // Переход к равномерно распределённой матрице путём домножения специальной диагональной матрицы на исходную.
            // Вектор правой части соответственно тоже домножается на эту диагональную матрицу. 

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var diagProbMatrix = this.GetDiagProbMatrix(aMatrix);
            var uniformMatrix = diagProbMatrix * aMatrix;
            var newB = diagProbMatrix * bVector;

            int rowCount = uniformMatrix.GetRowCount();
            var xPrev = newB;
            int numberOfIterations = 100000 ;
            for (int i = 0; i < numberOfIterations; i++)
            {
                int index = this.GetRandomIndex(rowCount);
                var uniformMatrixRow = uniformMatrix.GetRowByIndex(index);
                var numerator = newB[index] - (xPrev * uniformMatrixRow);
                var denominator = this.GetRowNorm(uniformMatrixRow) * this.GetRowNorm(uniformMatrixRow);
                var xCur = xPrev + (numerator / denominator) * uniformMatrixRow;
                xPrev = xCur;
            }
            stopwatch.Stop();
            var timeInSeconds = stopwatch.ElapsedMilliseconds / (double)1000;
            Console.WriteLine(timeInSeconds);

            return xPrev;
        }

        public Vector SolveByIterativeRefinement(Matrix aMatrix, Vector bVector, int numOfIterations = 1)
        {
            if (numOfIterations <= 0)
                throw new Exception("numOfIterations должен быть не меньше 1");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Vector res = this.Solve(aMatrix, bVector);
            if (numOfIterations == 1)
                return res;

            for (int i = 0; i < numOfIterations - 1; i++)
            {
                var pseudoB = aMatrix * res;
                var d = bVector - pseudoB;
                NoWalkerSolver solver = new NoWalkerSolver();
                var refinement = solver.Solve(aMatrix, d);
                res += refinement;
                Console.WriteLine(i);
            }
            stopwatch.Stop();
            var timeInSeconds = stopwatch.ElapsedMilliseconds / (double)1000;
            Console.WriteLine(timeInSeconds);

            return res;
        }

        private Matrix GetDiagProbMatrix(Matrix matrix)
        {
            int rowCount = matrix.GetRowCount();
            int colCount = matrix.GetColCount();
            double[,] matrixArr = new double[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                var row = matrix.GetRowByIndex(i);
                var norm = GetRowNorm(row);
                matrixArr[i, i] = 1 / norm;
            }

            var result = new Matrix(matrixArr);
            return result;
        }

        private List<double> GetDistributionOfMatrixRows(Matrix matrix)
        {
            var probabilities = new List<double>();
            var matrixRows = new List<List<double>>();

            for (int i = 0; i < matrix.GetRowCount(); i++)
            {
                var matrixRow = matrix.GetRowByIndex(i).ToList();
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
            double betweenZeroOne = this.rnd.NextDouble();
            double result = Math.Floor(betweenZeroOne * n);
            return (int)result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonteKarloMatrixVectorProduct;

namespace ProjectionAlgorithm_diploma_
{
    public class WalkerSolver
    {
        private Walker walker;

        public WalkerSolver()
        {
            /// Свойству walker значение будет присваиваться только во время рандомизированной
            /// работы. Это не очень круто, но что поделать.
            /// 
        }

        public Vector Solve(Matrix aMatrix, Vector bVector)
        {
            // получение распределения строк матрицы и создания Волкера.

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var distribution = this.GetDistributionOfMatrixRows(aMatrix);
            this.walker = new Walker(distribution);

            int rowCount = aMatrix.GetRowCount();
            Vector xPrev = bVector;
            int numberOfIterations = 100000 ;

            for (int i = 0; i < numberOfIterations; i++)
            {
                int index = this.GetRandomIndex();
                Vector row = aMatrix.GetRowByIndex(index);
                double numerator = bVector[index] - (row * xPrev);
                double denominator = this.GetRowNorm(row) * this.GetRowNorm(row);
                double factor = numerator / denominator;
                Vector xCur = xPrev + factor * row;
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
                WalkerSolver solver = new WalkerSolver();
                var refinement = solver.Solve(aMatrix, d);
                res += refinement;
                Console.WriteLine(i);
            }
            stopwatch.Stop();
            var timeInSeconds = stopwatch.ElapsedMilliseconds / (double)1000;
            Console.WriteLine(timeInSeconds);
            return res;
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
                probabilities.Add((rowNorm * rowNorm) / frobeniusNormSquared);
            }
            return probabilities;
        }

        private double GetRowNorm(IEnumerable<double> row)
        {
            var result = row.Select(x => x * x).Sum();
            result = Math.Sqrt(result);
            return result;
        }

        private int GetRandomIndex()
        {
            return this.walker.GetSelection();
        }

    }
}

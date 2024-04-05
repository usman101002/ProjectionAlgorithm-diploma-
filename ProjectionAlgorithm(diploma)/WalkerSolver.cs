using System;
using System.Collections.Generic;
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
            var distribution = this.GetDistributionOfMatrixRows(aMatrix);
            this.walker = new Walker(distribution);

            int rowCount = aMatrix.GetRowCount();
            Vector xPrev = bVector;
            int numberOfIterations = 100 * 1000;

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

            return xPrev;
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

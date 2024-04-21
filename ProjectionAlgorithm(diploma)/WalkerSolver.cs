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

            var xPrevData = new double[rowCount];
            Vector xPrev = new Vector(xPrevData);
            int numberOfProjections = 7000;

            Console.WriteLine(numberOfProjections + " проекций" + " метод Solve у Уолкера");

            for (int i = 0; i < numberOfProjections; i++)
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
            Console.WriteLine(timeInSeconds + " --- время для Solve() у WalkerSolver");

            return xPrev;
        }

        public Vector SolveByMedians(Matrix aMatrix, Vector bVector)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var distribution = this.GetDistributionOfMatrixRows(aMatrix);
            this.walker = new Walker(distribution);
            int rowCount = aMatrix.GetRowCount();
            var xPrevData = new double[rowCount];
            Vector xPrev = new Vector(xPrevData);
            int numberOfProjections = 7000;
            Console.WriteLine(numberOfProjections + " проекций" + " метод SolveByMedians у Уолкерного решателя");

            for (int i = 0; i < numberOfProjections; i++)
            {
                List<Vector> trianglePoints = new List<Vector>();
                // в этом цикле идёт получение точек с разных гиперплоскостей для треугольника
                while (trianglePoints.Count < 3)
                {
                    int index = this.GetRandomIndex();
                    Vector row = aMatrix.GetRowByIndex(index);
                    if (trianglePoints.Contains(row))
                    {
                        continue;
                    }
                    else
                    {
                        double numerator = bVector[index] - (row * xPrev);
                        double denominator = this.GetRowNorm(row) * this.GetRowNorm(row);
                        double factor = numerator / denominator;
                        Vector trianglePoint = xPrev + factor * row;
                        trianglePoints.Add(trianglePoint);
                    }
                }

                Vector intersectionPoint =
                    this.GetIntersectionMediansPoint(trianglePoints[0], trianglePoints[1], trianglePoints[2]);
                xPrev = intersectionPoint;
            }
            stopwatch.Stop();
            var timeInSeconds = stopwatch.ElapsedMilliseconds / (double)1000;
            Console.WriteLine(timeInSeconds + " --- время для SolveByMedians() у WalkerSolver");

            return xPrev;
        }

        public Vector SolveByIterativeRefinement(Matrix aMatrix, Vector bVector, int numOfIterations = 1)
        {
            if (numOfIterations <= 0)
                throw new Exception("numOfIterations должен быть не меньше 1");
            Console.WriteLine($"{numOfIterations} итераций итерационного уточнения");
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

        private Vector GetIntersectionMediansPoint(Vector p1, Vector p2, Vector p3)
        {
            double factor = (double)1 / 3;
            var res = factor * (p1 + p2 + p3);
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

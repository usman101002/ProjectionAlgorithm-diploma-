using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
            int numberOfProjections = 5000;

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
            int numberOfProjections = 5000;
            Console.WriteLine(numberOfProjections + " проекций" + " метод SolveByMedians у Уолкерного решателя");

            for (int i = 0; i < numberOfProjections; i++)
            {
                //List<Vector> trianglePoints = new List<Vector>();
                //// в этом цикле идёт получение точек с разных гиперплоскостей для треугольника
                //while (trianglePoints.Count < 3)
                //{
                //    int index = this.GetRandomIndex();
                //    Vector row = aMatrix.GetRowByIndex(index);
                //    if (trianglePoints.Contains(row))
                //    {
                //        continue;
                //    }
                //    else
                //    {
                //        double numerator = bVector[index] - (row * xPrev);
                //        double denominator = this.GetRowNorm(row) * this.GetRowNorm(row);
                //        double factor = numerator / denominator;
                //        Vector trianglePoint = xPrev + factor * row;
                //        trianglePoints.Add(trianglePoint);
                //    }
                //}

                int index0 = this.GetRandomIndex();
                int index1 = this.GetRandomIndex();
                // точки треугольника должны различаться
                while (index1 == index0)
                {
                    index1 = this.GetRandomIndex();
                }
                int index2 = this.GetRandomIndex();
                while (index2 == index1 || index2 == index0)
                {
                    index2 = this.GetRandomIndex();
                }

                Vector row0 = aMatrix.GetRowByIndex(index0);
                double numerator0 = bVector[index0] - (row0 * xPrev);
                double denominator0 = this.GetRowNorm(row0) * this.GetRowNorm(row0);
                double factor0 = numerator0 / denominator0;
                Vector p0 = xPrev + factor0 * row0;

                Vector row1 = aMatrix.GetRowByIndex(index1);
                double numerator1 = bVector[index1] - (row1 * xPrev);
                double denominator1 = this.GetRowNorm(row1) * this.GetRowNorm(row1);
                double factor1 = numerator1 / denominator1;
                Vector p1 = xPrev + factor1 * row1;

                Vector row2 = aMatrix.GetRowByIndex(index2);
                double numerator2 = bVector[index2] - (row2 * xPrev);
                double denominator2 = this.GetRowNorm(row2) * this.GetRowNorm(row2);
                double factor2 = numerator2 / denominator2;
                Vector p2 = xPrev + factor2 * row2;

                Vector intersectionPoint =
                    this.GetIntersectionMediansPoint(p0, p1, p2);
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

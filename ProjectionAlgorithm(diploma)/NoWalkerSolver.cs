using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.CodeDom;
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
            var diagProbMatrix = this.GetLeftDiagProbMatrix(aMatrix);
            diagProbMatrix.IsDiagonal = true;

            var uniformMatrix = diagProbMatrix * aMatrix;
            var newB = diagProbMatrix * bVector;

            int rowCount = uniformMatrix.GetRowCount();
            var xPrevData = new double[rowCount];
            Vector xPrev = new Vector(xPrevData);

            int numberOfProjections = 9200;
            Console.WriteLine("БезУолкерный метод");
            Console.WriteLine(numberOfProjections + " итераций у метода Solve у NoWalker ");
            for (int i = 0; i < numberOfProjections; i++)
            {
                int index = this.GetRandomIndex(rowCount);
                var uniformMatrixRow = uniformMatrix.GetRowByIndex(index);
                var numerator = newB[index] - (xPrev * uniformMatrixRow);
                var factor = numerator;
                var xCur = xPrev + factor * uniformMatrixRow;
                xPrev = xCur;
            }
            stopwatch.Stop();
            var timeInSeconds = stopwatch.ElapsedMilliseconds / (double)1000;
            Console.WriteLine(timeInSeconds + " --- время для Solve() у NoWalkerSolver");

            return xPrev;
        }

        public Vector SolveByMedians(Matrix aMatrix, Vector bVector)
        {
            var diagProbMatrix = this.GetLeftDiagProbMatrix(aMatrix);
            diagProbMatrix.IsDiagonal = true;
            var uniformMatrix = diagProbMatrix * aMatrix;
            var newB = diagProbMatrix * bVector;

            int rowCount = uniformMatrix.GetRowCount();
            var xPrevData = new double[rowCount];
            Vector xPrev = new Vector(xPrevData);
            int numberOfProjections = 7500;
            Console.WriteLine(numberOfProjections + " проекций" + " метод SolveByMedians у НЕУолкерного решателя");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < numberOfProjections; i++)
            {
                //List<Vector> trianglePoints = new List<Vector>();
                //// в этом цикле идёт получение точек с разных гиперплоскостей для треугольника
                //while (trianglePoints.Count < 3)
                //{
                //    int index = this.GetRandomIndex(rowCount);
                //    Vector row = uniformMatrix.GetRowByIndex(index);
                //    if (trianglePoints.Contains(row))
                //    {
                //        continue;
                //    }
                //    else
                //    {
                //        double numerator = newB[index] - (row * xPrev);
                //        double factor = numerator;
                //        Vector trianglePoint = xPrev + factor * row;
                //        trianglePoints.Add(trianglePoint);
                //    }
                //}

                int index0 = this.GetRandomIndex(rowCount);
                int index1 = this.GetRandomIndex(rowCount);
                // точки треугольника должны различаться
                int index2 = this.GetRandomIndex(rowCount);

                if (index0 == index1 || index1 == index2 || index0 == index2)
                {
                    continue;
                }
                else
                {
                    Vector row0 = uniformMatrix.GetRowByIndex(index0);
                    double numerator0 = newB[index0] - (row0 * xPrev);
                    Vector p0 = xPrev + numerator0 * row0;

                    Vector row1 = uniformMatrix.GetRowByIndex(index1);
                    double numerator1 = newB[index1] - (row1 * xPrev);
                    Vector p1 = xPrev + numerator1 * row1;

                    Vector row2 = uniformMatrix.GetRowByIndex(index2);
                    double numerator2 = newB[index2] - (row2 * xPrev);
                    Vector p2 = xPrev + numerator2 * row2;

                    Vector intersectionPoint =
                        this.GetIntersectionMediansPoint(p0, p1, p2);
                    xPrev = intersectionPoint;
                }
            }
            stopwatch.Stop();
            var timeInSeconds = stopwatch.ElapsedMilliseconds / (double)1000;
            Console.WriteLine(timeInSeconds + " --- время для SolveByMedians() у NoWalkerSolver");
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

        // Dx=y, D --- диагональная матрица
        private Vector SolveDiagonalSLAE(Matrix d, Vector y)
        {
            int n = y.GetDimension();
            double[] xData = new double[n];
            for (int i = 0; i < n; i++)
            {
                xData[i] = (double)y[i] / d[i, i];
            }

            Vector x = new Vector(xData);
            return x;
        }

        private Vector GetIntersectionMediansPoint(Vector p1, Vector p2, Vector p3)
        {
            double factor = (double)1 / 3;
            var res = factor * (p1 + p2 + p3);
            return res;
        }

        private Matrix GetLeftDiagProbMatrix(Matrix matrix)
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

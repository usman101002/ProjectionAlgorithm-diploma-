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

        public Vector Solve(Matrix aMatrix, Vector bVector, int numProjections)
        {
            // Переход к равномерно распределённой матрице путём домножения специальной диагональной матрицы на исходную.
            // Вектор правой части соответственно тоже домножается на эту диагональную матрицу. 
            this.rnd = new Random(0);
            var diagProbMatrix = this.GetLeftDiag(aMatrix);
            
            var uniformMatrix = diagProbMatrix.Multiply(aMatrix, true);
            var newB = diagProbMatrix * bVector;

            int rowCount = uniformMatrix.GetRowCount();
            var xPrevData = new double[rowCount];
            Vector xPrev = new Vector(xPrevData);

            
            Console.WriteLine(numProjections + " проекций у метода Solve у NoWalkerSolver ");

            for (int i = 0; i < numProjections; i++)
            {
                int index = this.GetRandomIndex(rowCount);
                var uniformMatrixRow = uniformMatrix.GetRowByIndex(index);
                var numerator = newB[index] - (xPrev * uniformMatrixRow);
                var factor = numerator;
                var xCur = xPrev + factor * uniformMatrixRow;
                xPrev = xCur;
            }
            return xPrev;
        }

        public Vector SolveByMedians(Matrix aMatrix, Vector bVector, int numProjections)
        {
            var diagProbMatrix = this.GetLeftDiag(aMatrix);
            
            var uniformMatrix = diagProbMatrix.Multiply(aMatrix, true);
            var newB = diagProbMatrix * bVector;

            int rowCount = uniformMatrix.GetRowCount();
            var xPrevData = new double[rowCount];
            Vector xPrev = new Vector(xPrevData);
            
            Console.WriteLine(numProjections + " проекций" + " метод SolveByMedians у NoWalkerSolver");
            
            for (int i = 0; i < numProjections; i++)
            {
                #region
                //List<Vector> trianglePoints = new List<Vector>();
                //// в этом цикле идёт получение точек с разных гиперплоскостей для треугольника
                //while (trianglePoints.Count < 3)
                //{
                //    int index = this.GetRandomIndex(rowCount);
                //    Vector vector = uniformMatrix.GetRowByIndex(index);
                //    if (trianglePoints.Contains(vector))
                //    {
                //        continue;
                //    }
                //    else
                //    {
                //        double numerator = newB[index] - (vector * xPrev);
                //        double factor = numerator;
                //        Vector trianglePoint = xPrev + factor * vector;
                //        trianglePoints.Add(trianglePoint);
                //    }
                //}
                #endregion

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
                    //double numerator0 = newB[index0] - (row0 * xPrev);
                    Vector p0 = xPrev + (newB[index0] - (row0 * xPrev)) * row0;

                    Vector row1 = uniformMatrix.GetRowByIndex(index1);
                    //double numerator1 = newB[index1] - (row1 * xPrev);
                    Vector p1 = xPrev + (newB[index1] - (row1 * xPrev)) * row1;

                    Vector row2 = uniformMatrix.GetRowByIndex(index2);
                    //double numerator2 = newB[index2] - (row2 * xPrev);
                    Vector p2 = xPrev + (newB[index2] - (row2 * xPrev)) * row2;

                    Vector intersectionPoint =
                        this.GetIntersectionMediansPoint(p0, p1, p2);
                    xPrev = intersectionPoint;
                }
            }
            return xPrev;
        }

        public Vector SolveByMediansIterativeRefinement(Matrix aMatrix, Vector bVector, int numProjections, int numOfIterations)
        {
            if (numOfIterations <= 0)
                throw new Exception("numOfIterations должен быть не меньше 1");

            Vector res = this.SolveByMedians(aMatrix, bVector, numProjections);

            if (numOfIterations == 1)
                return res;

            Console.WriteLine($"{numOfIterations} итераций МЕДИАННОГО итерационного уточнения");

            for (int i = 0; i < numOfIterations - 1; i++)
            {
                var pseudoB = aMatrix * res;
                var d = bVector - pseudoB;
                NoWalkerSolver solver = new NoWalkerSolver();
                var refinement = solver.SolveByMedians(aMatrix, d, numProjections);
                res += refinement;
            }

            Console.WriteLine("КОНЕЦ МЕТОДА");
            Console.WriteLine();
            return res;
        }

        public Vector StupidProjectionsSolve(Matrix aMatrix, Vector bVector, int numProjections)
        {
            int dim = bVector.GetDimension();
            double[] xPrevData = new double[dim];
            Vector xPrev = new Vector(xPrevData);

            for (int i = 0; i < numProjections; i++)
            {
                int index = this.GetRandomIndex(dim);
                Vector row = aMatrix.GetRowByIndex(index);
                double numerator = bVector[index] - (row * xPrev);
                double factor = numerator;
                Vector xCur = xPrev + factor * row;
                xPrev = xCur;
            }

            return xPrev;
        }

        public Vector SolveByBalancing(Matrix aMatrix, Vector bVector, int numProjections, int numBalances)
        {
            this.rnd = new Random(1);
            Matrix newA = aMatrix;
            Vector newB = bVector;
            
            double[,] diagonalRightData = new double[bVector.GetDimension(), bVector.GetDimension()];
            Matrix diagonalRight = new Matrix(diagonalRightData);

            double[,] diagonalLeftData = new double[bVector.GetDimension(), bVector.GetDimension()];
            Matrix diagonalLeft = new Matrix(diagonalLeftData);

            double[,] restData = new double[bVector.GetDimension(), bVector.GetDimension()];
            Matrix rest = new Matrix(restData);

            Console.WriteLine(numProjections + " проекций" + " метод SolveByBalancing" + $", {numBalances} --- число балансирований");
            for (int i = 0; i < numBalances; i++)
            {
                diagonalLeft = this.GetLeftDiag(newA);
                newA = diagonalLeft.Multiply(newA, true);
                newB = diagonalLeft * newB;
                diagonalRight = this.GetRightDiag(newA);
                newA = newA.Multiply(diagonalRight, false);
                if (i == 0)
                {
                    rest = diagonalRight.Inverse();
                }
                else
                {
                    rest = diagonalRight.Inverse().DiagonalMultiply(rest);
                }
            }
            Vector y = this.Solve(newA, newB, numProjections);
            
            Vector x = this.SolveDiagonalSLAE(rest, y);
            
            return x;
        }

        public Vector SolveBySimpleIterativeRefinement(Matrix aMatrix, Vector bVector, int numProjections, int numOfIterations)
        {
            if (numOfIterations <= 0)
                throw new Exception("numOfIterations должен быть не меньше 1");

            Vector res = this.Solve(aMatrix, bVector, numProjections);
            if (numOfIterations == 1)
                return res;

            Console.WriteLine($"{numOfIterations} итераций ПРОСТОГО итерационного уточнения");
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            for (int i = 0; i < numOfIterations - 1; i++)
            {
                var pseudoB = aMatrix * res;
                var d = bVector - pseudoB;
                NoWalkerSolver solver = new NoWalkerSolver();
                var refinement = solver.Solve(aMatrix, d, numProjections);
                res += refinement;
                //var timeInSeconds = stopwatch.ElapsedMilliseconds / (double)1000;

                //var relError = this.GetRelError(trueX, res);
                //Console.WriteLine($"{relError} --- относительная погрешность: {i + 1} итерация Простого итер. уточнения");
                //Console.WriteLine($"{timeInSeconds} --- время");
                //Console.WriteLine();
            }

            Console.WriteLine("КОНЕЦ МЕТОДА");
            Console.WriteLine();
            return res;
        }

        public Vector SolveByBalancingIterativeRefinement(Matrix aMatrix, Vector bVector, int numProjections, int numOfIterations, int numbBalances)
        {
            if (numOfIterations <= 0)
                throw new Exception("numOfIterations должен быть не меньше 1");

            Vector res = this.SolveByBalancing(aMatrix, bVector, numProjections, numbBalances);
            if (numOfIterations == 1)
                return res;

            Console.WriteLine($"{numOfIterations} итераций БАЛАНСИРОВАННОГО итерационного уточнения");
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            for (int i = 0; i < numOfIterations - 1; i++)
            {
                var pseudoB = aMatrix * res;
                var d = bVector - pseudoB;
                NoWalkerSolver solver = new NoWalkerSolver();
                var refinement = solver.SolveByBalancing(aMatrix, d,numProjections, numbBalances);
                res += refinement;
                //var timeInSeconds = stopwatch.ElapsedMilliseconds / (double)1000;

                //var relError = this.GetRelError(trueX, res);
                //Console.WriteLine($"{relError} --- относительная погрешность: {i + 1} итерация Баланс. итер. уточнения");
                //Console.WriteLine($"{timeInSeconds} --- время");
                //Console.WriteLine();
            }

            Console.WriteLine("КОНЕЦ МЕТОДА");
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

        private Matrix GetLeftDiag(Matrix matrix)
        {
            int rowCount = matrix.GetRowCount();
            int colCount = matrix.GetColCount();
            double[,] matrixArr = new double[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                var row = matrix.GetRowByIndex(i);
                var norm = GetNorm(row);
                matrixArr[i, i] = (double)1 / norm;
            }

            var result = new Matrix(matrixArr);
            return result;
        }

        private Matrix GetRightDiag(Matrix matrix)
        {
            int rowCount = matrix.GetRowCount();
            int colCount = matrix.GetColCount();
            double[,] matrixArr = new double[rowCount, colCount];
            for (int i = 0; i < colCount; i++)
            {
                var col = matrix.GetColByIndex(i);
                var norm = GetNorm(col);
                matrixArr[i, i] = (double)1 / norm;
            }

            var result = new Matrix(matrixArr);
            return result;
        }

        #region GetDistributionOfMatrixRows --- не используется
        //private List<double> GetDistributionOfMatrixRows(Matrix matrix)
        //{
        //    var probabilities = new List<double>();
        //    var matrixRows = new List<List<double>>();

        //    for (int i = 0; i < matrix.GetRowCount(); i++)
        //    {
        //        var matrixRow = matrix.GetRowByIndex(i).ToList();
        //        matrixRows.Add(matrixRow);
        //    }

        //    double frobeniusNormSquared = 0;
        //    for (int i = 0; i < matrixRows.Count; i++)
        //    {
        //        for (int j = 0; j < matrixRows.Count; j++)
        //        {
        //            frobeniusNormSquared += matrixRows[i][j] * matrixRows[i][j];
        //        }
        //    }

        //    for (int i = 0; i < matrixRows.Count; i++)
        //    {
        //        var rowNorm = GetNorm(matrixRows[i]);
        //        probabilities.Add(rowNorm * rowNorm / frobeniusNormSquared);
        //    }
        //    //var sum = probabilities.Sum(); нужно для проверки того, что получившийся набор действительно является распределением.
        //    return probabilities;
        //}


        #endregion


        private double GetNorm(IEnumerable<double> vector)
        {
            var result = vector.Select(x => x * x).Sum();
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

        private double GetRelError(Vector trueX, Vector approximateX)
        {
            double res = ((trueX - approximateX).GetEuclideanNorm() / (trueX).GetEuclideanNorm()) * 100;
            return res;
        }
    }
}

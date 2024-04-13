using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    /// <summary>
    /// Задача на двумерной плоскости. Уравнение: Laplas(u) = 0, u на границе равно функции.
    /// Граница есть прямоугольник шириной width и высотой,
    /// у которого левый нижний угол находится в точке (0, 0)
    /// </summary>
    public class LaplasEquationSolver
    {
        // A*c=phi --- SLAE
        private Random rnd;
        public List<double> boundUValues;
        public List<(double, double)> boundPoints;
        public List<(double, double)> randomPointsFromArea;
        public double width;
        public double height;
        public int numWidthNodes;
        public int numHeightNodes;
        public Matrix AMatrix;
        public Vector PhiVector;
        public Vector SLAESolution;

        private double h1 = 0.5;
        private double h2 = 0.2;

        public LaplasEquationSolver()
        {
            this.rnd = new Random();
            this.width = 1;
            this.height = 1;
            this.numWidthNodes = 3;
            this.numHeightNodes = 3;
            this.boundPoints = this.GetPointsFromRectangleBoundary();
            this.boundUValues = this.GetBoundUValues(this.boundPoints);
            this.randomPointsFromArea = this.GetRandomPointsFromArea(this.boundUValues.Count);
            this.AMatrix = this.GetAMatrix();
            this.PhiVector = this.GetPhiVector();
        }

        public LaplasEquationSolver(double width, double height, int numWidthNodes, int numHeightNodes)
        {
            this.rnd = new Random();
            this.width = width;
            this.height = height;
            this.numWidthNodes = numWidthNodes;
            this.numHeightNodes = numHeightNodes;
            this.boundPoints = this.GetPointsFromRectangleBoundary();
            this.boundUValues = this.GetBoundUValues(this.boundPoints);
            this.randomPointsFromArea = this.GetRandomPointsFromArea(this.boundUValues.Count);
            this.AMatrix = this.GetAMatrix();
            this.PhiVector = this.GetPhiVector();
        }
        public double TrueUFunc(double x, double y)
        {
            var res = x * x - y * y;
            return res;
        }

        private (double, double) GetRandomPointInArea(double h1 = 0.3, double h2 = 0.2)
        {
            double xCoord = this.GetRandomXInArea(h1, h2);
            double yCoord = this.GetRandomYInArea(h1, h2);
            var resultPoint = (xCoord, yCoord);
            return resultPoint;
        }

        private double GetRandomXInArea(double h1, double h2)
        {
            double valueFromLeftSpace = MathUtils.GetUniformDistribution(0 - h1 - h2, 0 - h1);
            double valueFromRightSpace = MathUtils.GetUniformDistribution(this.width + h1, this.width + h1 + h2);
            int choice = MathUtils.ChooseBtwTwoRndVars();
            double result = choice == 0 ? valueFromLeftSpace : valueFromRightSpace;
            return result;
        }

        private double GetRandomYInArea(double h1, double h2)
        {
            double valueFromTopSpace = MathUtils.GetUniformDistribution(this.height + h1, this.height + h1 + h2);
            double valueFromBotSpace = MathUtils.GetUniformDistribution(0 - h1, 0 - h1 - h2);
            int choice = MathUtils.ChooseBtwTwoRndVars();
            double result = choice == 0 ? valueFromTopSpace : valueFromBotSpace;
            return result;
        }

        /// <summary>
        /// Прямоугольнк будет разбит равномерно как по горизонтали, так и по вертикали, но число разбиений может быть разным
        /// по горизонтали и по вертикали.
        /// </summary>
        /// <returns></returns>
        public List<(double, double)> GetPointsFromRectangleBoundary()
        {
            width = this.width;
            height = this.height;
            var boundaryPoints = new List<(double, double)>();
            double widthStep = width / (numWidthNodes - 1);
            double heightStep = height / (numHeightNodes - 1);

            for (int i = 0; i < numHeightNodes; i++)
            {
                if (i == 0 || i == numHeightNodes - 1)
                {
                    for (int j = 0; j < numWidthNodes; j++)
                    {
                        var xCoord = j * widthStep;
                        var yCoord = i * heightStep;
                        boundaryPoints.Add((xCoord, yCoord));
                    }
                }
                else
                {
                    var xCoord = 0 * widthStep; // точки с левой стороны прямоугольника
                    var yCoord = i * heightStep;
                    boundaryPoints.Add((xCoord, yCoord));
                    xCoord = (numWidthNodes - 1) * widthStep;
                    yCoord = i * heightStep;
                    boundaryPoints.Add((xCoord, yCoord));
                }
            }
            return boundaryPoints;
        }

        public List<double> GetBoundUValues(IEnumerable<(double, double)> boundaryPoints)
        {
            var result = this.GetFunctionValues(boundaryPoints, this.TrueUFunc);
            return result;
        }

        private List<double> GetFunctionValues(IEnumerable<(double, double)> points, Func<double, double, double> function)
        {
            var res = new List<double>();
            foreach (var point in points)
            {
                var functionValue = function(point.Item1, point.Item2);
                res.Add(functionValue);
            }
            return res;
        }

        public List<(double, double)> GetRandomPointsFromArea(int pointsNumber)
        {
            var result = new List<(double, double)>();
            for (int i = 0; i < pointsNumber; i++)
            {
                var randomPoint = this.GetRandomPointInArea(this.h1, this.h2);
                result.Add(randomPoint);
            }
            return result;
        }

        public Matrix GetAMatrix()
        {
            int n = this.boundPoints.Count;
            double[,] matrixAArr = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrixAArr[i, j] = MathUtils.E(this.boundPoints[i], this.randomPointsFromArea[j]);
                }
            }
            var aMatrix = new Matrix(matrixAArr);
            return aMatrix;
        }

        public Vector GetPhiVector()
        {
            var phiVector = new Vector(this.boundUValues);
            this.PhiVector = phiVector;
            return phiVector;
        }

        public double GetApproximateU((double, double) point)
        {
            this.SLAESolution = this.SolveSLAE();
            double result = 0;
            for (int i = 0; i < this.boundPoints.Count; i++)
            {
                result += this.SLAESolution[i] * MathUtils.E(point, this.randomPointsFromArea[i]);
            }
            return result;
        }
        // Решение СЛАУ A*c=phi
        public Vector SolveSLAE()
        {
            //NoWalkerSolver solver = new NoWalkerSolver();
            //var solution = solver.Solve(this.AMatrix, this.PhiVector);

            //WalkerSolver solver = new WalkerSolver();
            //var solution = solver.Solve(this.AMatrix, this.PhiVector);

            //WalkerSolver solver = new WalkerSolver();
            //var solution = solver.SolveByIterativeRefinement(this.AMatrix, this.PhiVector, 10);

            NoWalkerSolver solver = new NoWalkerSolver();
            var solution = solver.SolveByIterativeRefinement(this.AMatrix, this.PhiVector, 20);

            // отладочные действия
            
            //var residual = this.AMatrix * solution - this.PhiVector;
            //var residualNorm = residual.GetEuclideanNorm();
            
            //string path = "residual100kIterations.txt";
            //using (StreamWriter sw = new StreamWriter(path, false))
            //{
            //    for (int i = 0; i < residual.Count(); i++)
            //    {
            //        sw.WriteLine(residual[i].ToString());
            //    }
            //}
            
            return solution;
        }

    }
}

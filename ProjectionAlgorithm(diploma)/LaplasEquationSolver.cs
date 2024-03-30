using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    /// <summary>
    /// Уравнение: Laplas(u) = 0, u на границе равно функции.
    /// Граница есть прямоугольник, у которого левый нижний угол находится в точке (0, 0)
    /// </summary>
    public class LaplasEquationSolver
    {
        private Random rnd;
        private List<double> boundUValues;
        private double width;
        private double height;
        
        public LaplasEquationSolver()
        {
            this.rnd = new Random();
            this.width = 1;
            this.height = 1;
        }

        public LaplasEquationSolver(double width, double height)
        {
            this.width = width;
            this.height = height;
            this.rnd = new Random();
        }
        public double TrueUFunc(double x, double y)
        {
            var res = x * x - y * y;
            return res;
        }

        public double GetDistBtwPoints((double, double) point1, (double, double) point2)
        {
            double result = Math.Pow(point1.Item1 - point2.Item1, 2) + Math.Pow(point1.Item2 - point2.Item2, 2);
            return Math.Sqrt(result);
        }

        public (double, double) GetRandomPointInArea(double h1, double h2)
        {
            double xCoord = this.GetRandomCoordInArea(h1, h2);
            double yCoord = this.GetRandomCoordInArea(h1, h2);
            var resultPoint = (xCoord, yCoord);
            return resultPoint;
        }
        private double GetRandomCoordInArea(double h1, double h2)
        {
            double valueFromLeftSpace = this.GetUniformDistribution(0 - h1 - h2, 0 - h1);
            double valueFromRightSpace = this.GetUniformDistribution(1 + h1, 1 + h1 + h2);
            int choice = this.ChooseFromTwoRndVars();
            double result = choice == 0 ? valueFromLeftSpace : valueFromRightSpace;
            return result;
        }

        private double GetUniformDistribution(double a, double b)
        {
            double rand = rnd.NextDouble();
            double res = a + (b - a) * rand;
            return res;
        }

        private int ChooseFromTwoRndVars()
        {
            int res = this.rnd.Next(0, 2);
            return res;
        }

        /// <summary>
        /// Прямоугольнк будет разбит равномерно как по горизонтали, так и по вертикали, но число разбиений может быть разным
        /// по горизонтали и по вертикали.
        /// </summary>
        /// <returns></returns>
        public List<(double, double)> GetPointsFromRectangleBoundary(int numWidthNodes, int numHeightNodes)
        {
            width = this.width;
            height = this.height;
            var boundaryPoints = new List<(double, double)>();
            double widthStep = width / (numWidthNodes - 1);
            double heightStep = height / (numHeightNodes - 1);

            for (int i = 0; i < numHeightNodes; i++)
            {
                for (int j = 0; j < numWidthNodes; j++)
                {
                    var xCoord = j * widthStep;
                    var yCoord = i * heightStep;
                    boundaryPoints.Add((xCoord, yCoord));
                }
            }
            
            return boundaryPoints;
        }

        public List<double> GetBoundUValues(IEnumerable<(double, double)> boundaryPoints)
        {
            var result = this.GetFunctionValues(boundaryPoints, this.TrueUFunc);
            this.boundUValues = result;
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


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    /// <summary>
    /// Уравнение: Laplas(u) = 0, u на границе равно функции.
    /// Граница есть прямоугольник шириной width и высотой,
    /// у которого левый нижний угол находится в точке (0, 0)
    /// </summary>
    public class LaplasEquationSolver
    {
        private Random rnd;
        private List<double> boundUValues;
        private List<(double, double)> boundPoints;
        private List<(double, double)> randomPointsFromArea;
        private double width;
        private double height;
        private int numWidthNodes;
        private int numHeightNodes;

        public LaplasEquationSolver()
        {
            this.rnd = new Random();
            this.width = 1;
            this.height = 1;
            this.numWidthNodes = 3;
            this.numHeightNodes = 3;
        }

        public LaplasEquationSolver(double width, double height, int numWidthNodes, int numHeightNodes)
        {
            this.rnd = new Random();
            this.width = width;
            this.height = height;
            this.numWidthNodes = numWidthNodes;
            this.numHeightNodes = numHeightNodes;
        }
        public double TrueUFunc(double x, double y)
        {
            var res = x * x - y * y;
            return res;
        }

        public (double, double) GetRandomPointInArea(double h1, double h2)
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

            this.boundPoints = boundaryPoints;
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

        public Matrix CreateAMatrix()
        {
            // сначала вызываем метод для генерации точек на границе прямоугольника
            // получаем сколько-то точек (в зависимости от того, сколько мы хотим узлов на границе)
            // дальше генерируем точки в ограничивающей области в том же количестве, что и
            // точек на границе.
            // Затем уже составляем матрицу и вектор правой части для решения слау

            return null;

        }

        public Vector CreatePhiVector()
        {

        }
    }
}

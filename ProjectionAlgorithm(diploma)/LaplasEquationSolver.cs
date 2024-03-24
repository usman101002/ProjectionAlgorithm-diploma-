using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    /// <summary>
    /// Уравнение: Laplas(u) = 0, u на границе равно функции.
    /// Граница есть квадрат: x принадлежит [0, 1], y принадлежит [0, 1].
    /// </summary>
    public class LaplasEquationSolver
    {
        public double TrueUFunc(double x, double y)
        {
            var res = x * x - y * y;
            return res;
        }

        /// <summary>
        /// Квадрат будет разбит равномерно как по горизонтали, так и по вертикали
        /// </summary>
        /// <param name="numSideNodes">Число нод на одной стороне квадрата</param>
        /// <returns></returns>
        public List<(double, double)> GetPointFromBoundary(int numSideNodes)
        {
            var boundaryPoints = new List<(double, double)>();
            double step = (double)1 / (numSideNodes - 1);
            for (int i = 0; i < numSideNodes; i++)
            {
                for (int j = 0; j < numSideNodes; j++)
                {
                    var xCoord = j * step;
                    var yCoord = i * step;
                    if (i == 0 || i == numSideNodes - 1)
                    {
                        boundaryPoints.Add((xCoord, yCoord));
                    }
                    else
                    {
                        if (j == 0 || j == numSideNodes - 1)
                        {
                            boundaryPoints.Add((xCoord, yCoord));
                        }
                    }
                }
            }
            if (boundaryPoints.Count == numSideNodes * 4 - 4)
                Console.WriteLine("Количество такое, как надо");
            return boundaryPoints;
        }


    }
}

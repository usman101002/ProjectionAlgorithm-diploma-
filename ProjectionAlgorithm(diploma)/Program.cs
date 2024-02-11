using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace ProjectionAlgorithm_diploma_
{
    internal class Program
    {
        // В методе Main будет происходить финальное тестирование приближённого и метода и библиотечного точного 
        // метода (иначе не знаю как нормально протестировать). 
        static void Main(string[] args)
        {
            int n = 1000;
            AlgebraEntitiesCreator creator = new AlgebraEntitiesCreator(n);

           

            var solver = new LinearSystemSolver(creator.A, creator.BVector);
            var res = solver.Solve();
            var s = solver.BVector.Sum();
            Console.WriteLine();

            // посчитаем точное решение СЛАУ с помощью библиотеки Math.Net.Numerics
            var trueX = GetTrueSolution(solver.A, solver.BVector);
            var dif = trueX - res;

            Console.WriteLine();



            var numerator = (trueX - res).Norm(2);
            var denominator = trueX.Norm(2);

            Console.WriteLine("Относительная погрешность:");
            Console.WriteLine(numerator / denominator);

            Console.WriteLine("2-норма разности:");
            Console.WriteLine(numerator);

        }

        /// <summary>
        /// Для решения используется метод Solve библиотеки Math.Net Numerics. Он решает СЛАУ с помощью QR-разложения.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static Vector<double> GetTrueSolution(Matrix<double> a, Vector<double> b)
        {
            var res = a.Solve(b);
            return res;
        }
    }
}

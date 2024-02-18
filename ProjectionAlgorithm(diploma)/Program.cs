using System;
using System.Collections.Generic;
using System.IO;
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
            int n = 10000;
            AlgebraEntitiesCreator creator = new AlgebraEntitiesCreator(n);
            //var solver = new LinearSystemSolver(creator.A, creator.BVector);
            var trueX = GetTrueSolution(creator.A, creator.BVector);
            //var simpleSol = solver.Solve();
            //var refinementSolve = solver.SolveWithIterativeRefinement(8);

            string path = "TrueSol10000.txt";
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.WriteLine("QR-factorization, n = 10000");
                for (int i = 0; i < n; i++)
                {
                    sw.WriteLine(trueX[i].ToString());
                }
            }




            //var numIterations = 4;
            //var refinementSol = solver.SolveWithIterativeRefinement(numIterations);
            //var simpleSol = solver.Solve();

            Console.WriteLine();

            // посчитаем точное решение СЛАУ с помощью библиотеки Math.Net.Numerics
            //var trueX = GetTrueSolution(solver.A, solver.BVector);
            

            Console.WriteLine();



            //var numeratorRef = (trueX - refinementSol).Norm(2);
            //var denominator = trueX.Norm(2);

            // С итерационным уточнением
            //Console.WriteLine($"Относительная погрешность в случае итерационного уточнения ({numIterations} итераций):" );
            //Console.WriteLine(numeratorRef / denominator);

            //Console.WriteLine("2-норма разности в случае итерационного уточнения:");
            //Console.WriteLine(numeratorRef);

            //Console.WriteLine();

            // Без итерационного уточнения
            //var numeratorSimple = (trueX - simpleSol).Norm(2);
            //Console.WriteLine("Относительная погрешность без итерационного уточнения:");
            //Console.WriteLine(numeratorSimple / denominator);

            //Console.WriteLine("2-норма разности без итерационного уточнения:");
            //Console.WriteLine(numeratorSimple);


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

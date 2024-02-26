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
            //int n = 10000;
            //AlgebraEntitiesCreator creator = new AlgebraEntitiesCreator(n);

            // Сейчас будет тестирование в случае простой матрицы 3х3, чтобы было видно, что на ней всё ок 
            var A = Matrix<double>.Build.DenseOfArray(new double[,] {
                { 3, 2, -1 },
                { 2, -2, 4 },
                { -1, 0.5, -1 }
            });
            var b = Vector<double>.Build.Dense(new double[] { 1, -2, 0 });

            var solver = new LinearSystemSolver(A, b);
            //var solver = new LinearSystemSolver(creator.A, creator.BVector);
            //var trueX = GetTrueSolution(A, b);
            //var simpleSol = solver.SolveByWalker();
            var refinementSolve = solver.SolveWithIterativeRefinement(2);

            string path = "IterativeRef2Iterations.txt";
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                for (int i = 0; i < refinementSolve.Count; i++)
                {
                    sw.WriteLine(refinementSolve[i].ToString());
                }
            }




            //var numIterations = 4;
            //var refinementSol = solver.SolveWithIterativeRefinement(numIterations);
            //var simpleSol = solver.SolveByWalker();

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
        /// Для решения используется метод SolveByWalker библиотеки Math.Net Numerics. Он решает СЛАУ с помощью QR-разложения.
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

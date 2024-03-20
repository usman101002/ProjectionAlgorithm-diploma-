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

        static void Main(string[] args)
        {
            var creator = new AlgebraEntitiesCreator(5);
            NoWalkerSolver solver = new NoWalkerSolver();
            var solution = solver.Solve(creator.AMatrix, creator.BVector);



            #region Старьё, потом вернусь к нему

            //int n = 3;
            //AlgebraEntitiesCreator creator = new AlgebraEntitiesCreator(n);

            //#region Получение точного решения
            //var trueSolutionArr = new double[n];
            //for (int i = 0; i < n; i++)
            //{
            //    trueSolutionArr[i] = 1;
            //}

            //var builder = Vector<double>.Build;
            //var trueSolution = builder.DenseOfArray(trueSolutionArr);
            //#endregion

            //var noWalkerSolver = new NoWalkerSolver();
            //var solution = noWalkerSolver.Solve(creator.AMatrix, creator.BVector);

            //string path = "uniformSolving1000AfterNormirovka.txt";
            //using (StreamWriter sw = new StreamWriter(path, false))
            //{
            //    for (int i = 0; i < n; i++)
            //    {
            //        sw.WriteLine(solution[i].ToString());
            //    }
            //}

            //Console.WriteLine();

            #endregion



            #region Старьё

            // Сейчас будет тестирование в случае простой матрицы 3х3, чтобы было видно, что на ней всё ок 
            //var AMatrix = Matrix<double>.Build.DenseOfArray(new double[,] {
            //    { 3, 2, -1 },
            //    { 2, -2, 4 },
            //    { -1, 0.5, -1 }
            //});
            //var b = Vector<double>.Build.Dense(new double[] { 1, -2, 0 });

            //var solver = new LinearSystemSolver(AMatrix, b);
            //var solver = new LinearSystemSolver(creator.AMatrix, creator.BVector);
            //var trueX = GetTrueSolution(creator.AMatrix, creator.BVector);
            //var simpleSol = solver.SolveByWalker();
            //var noWalkerSolution = solver.SolveUniformly();
            //var refinementSolve = solver.SolveWithIterativeRefinement(2);

            //string path = "uniformSolving";
            //using (StreamWriter sw = new StreamWriter(path, false))
            //{
            //    for (int i = 0; i < noWalkerSolution.Count; i++)
            //    {
            //        sw.WriteLine(noWalkerSolution[i].ToString());
            //    }
            //}

            //var numIterations = 4;
            //var refinementSol = solver.SolveWithIterativeRefinement(numIterations);
            //var simpleSol = solver.SolveByWalker();

            // посчитаем точное решение СЛАУ с помощью библиотеки Math.Net.Numerics
            //var trueX = GetTrueSolution(solver.AMatrix, solver.BVector);

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

            #endregion

        }


    }
}

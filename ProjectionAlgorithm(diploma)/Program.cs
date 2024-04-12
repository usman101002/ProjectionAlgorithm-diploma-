using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using MonteKarloMatrixVectorProduct;

namespace ProjectionAlgorithm_diploma_
{
    internal class Program
    {
        static double U(double x, double y)
        {
            double result = x * x - y * y;
            return result;
        }
        static void Main(string[] args)
        {
            double[,] aArr = 
            {
                { 2, 1, 4 },
                { 1, 1, 3 },
                {4, 3, 14}
            };
            double[] bArr = { 16, 12, 52 };

            Matrix a = new Matrix(aArr);
            Vector b = new Vector(bArr);

            NoWalkerSolver noWalkerSolver = new NoWalkerSolver();
            var result = noWalkerSolver.Solve(a, b);


            LaplasEquationSolver solver = new LaplasEquationSolver(1, 1, 10, 5);
            (double, double) point = (0.5, 0.25);
            var trueU = U(point.Item1, point.Item2);
            var approximateU = solver.GetApproximateU(point);

            //var uResults = new List<double>();
            //for (int i = 0; i < 5; i++)
            //{
            //    var approximateU = solver.GetApproximateU(point);
            //    uResults.Add(approximateU);
            //    Console.WriteLine(i);
            //}
            //var averageU = uResults.Sum() / uResults.Count();
            

            // Проверка работоспособности моего Волкера (что распределение генерится верно)
            List<double> probabilities = new List<double>() { 0.5, 0.1, 0.1, 0.1, 0.1, 0.1};
            Walker walker = new Walker(probabilities);
            Dictionary<int, double> distribution = new Dictionary<int, double>();

            int N = 1000000;
            for (int i = 0; i < N; i++)
            {
                int randomValue = walker.GetSelection();
                if (!distribution.ContainsKey(randomValue))
                {
                    distribution.Add(randomValue, (double)1 / N);
                }
                else
                {
                    distribution[randomValue] += (double)1 / N;
                }
            }

            // Конец проверки

            int x = 1;
            //var pointsAtArea = new List<(double, double)>();
            //for (int i = 0; i < 10; i++)
            //{
            //    var pointAtArea = solver.GetRandomPointInArea(0.3, 0.2);
            //    pointsAtArea.Add(pointAtArea);
            //}


            //var creator = new AlgebraEntitiesCreator(10);
            //NoWalkerSolver solver = new NoWalkerSolver();
            //var solution = solver.Solve(creator.AMatrix, creator.BVector);
            //string path = "uniformSolving1000Dim1000IterationsAfterNormirovka.txt";
            //using (StreamWriter sw = new StreamWriter(path, false))
            //{
            //    for (int i = 0; i < solution.Count(); i++)
            //    {
            //        sw.WriteLine(solution[i].ToString());
            //    }
            //}

            //int x = 1;


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

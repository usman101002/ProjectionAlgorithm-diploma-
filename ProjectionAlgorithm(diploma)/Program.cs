﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MonteKarloMatrixVectorProduct;
using Accord.Math.Decompositions;

namespace ProjectionAlgorithm_diploma_
{
    internal class Program
    {
        static double U(double x, double y)
        {
            double result = x * x - y * y;
            return result;
        }

        static double GetRelError(Vector trueX, Vector approximateX)
        {
            double res = ((trueX - approximateX).GetEuclideanNorm() / (trueX).GetEuclideanNorm()) * 100;
            return res;
        }

        static double Delta(int i, int j)
        {
            int res = (i == j) ? 1 : 0;
            return res;
        }

        static void Main(string[] args)
        {
            #region Микроматрица
            //double[,] aArr =
            //{
            //    { 2, 1, 4 },
            //    { 1, 1, 3 },
            //    {4, 3, 14}
            //};
            //double[] bArr = { 16, 12, 52 };

            //Matrix a = new Matrix(aArr);
            //Vector b = new Vector(bArr);
            //WalkerSolver solver = new WalkerSolver();
            //var res = solver.SolveByMedians(a, b);
            //int x = 1;
            #endregion


            #region Решение квадратной СЛАУ Aij = 1/2 на i,i и 1 / (2*N + (i + j) / 10) либо с Кронекером
            Random rnd = new Random(1000);
            int size = 1000;
            Console.WriteLine(size + " --- размерность матрицы");
            double[,] dataA = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    dataA[i, j] = Delta(i, j) + (double)1 / (i + j * j + 1);
                    //dataA[i, j] = rnd.NextDouble() / size;

                    //if (i == j)
                    //{
                    //    dataA[i, i] = 1;
                    //}
                    //else
                    //{
                    //    dataA[i, j] = (double)1 / (size + 0.1 * (i + j) );
                    //}

                    //Тестирование на ортогональной матрице(для простоты, она единичная)
                    //if (i == j)
                    //{
                    //    dataA[i, j] = 1;
                    //}
                }
            }
            Matrix aMatrix = new Matrix(dataA);
            //GramSchmidtOrthogonalization gramSchmidt = new GramSchmidtOrthogonalization(dataA);
            //aMatrix = new Matrix(gramSchmidt.OrthogonalFactor);

            double[] trueX = new double[size];
            for (int i = 0; i < size; i++)
            {
                trueX[i] = 0.4;
            }

            Vector trueXVector = new Vector(trueX);
            Vector bVector = aMatrix * trueXVector;

            //WalkerSolver solver = new WalkerSolver();
            //var simpleSolution = solver.Solve(aMatrix, bVector);
            //var mediansSolution = solver.SolveByMedians(aMatrix, bVector);

            // Простое решение (детерминироанное, последовательное проектирование)
            //NoWalkerSolver deterministicSolver = new NoWalkerSolver();
            //Stopwatch stopwatchDeterministic = new Stopwatch();
            //stopwatchDeterministic.Start();
            //var simpleDeterministicSolution = deterministicSolver.SolveConsistently(aMatrix, bVector, 1000);
            //stopwatchDeterministic.Stop();
            //var time = stopwatchDeterministic.ElapsedMilliseconds / (double)1000;
            //Console.WriteLine(time + " --- время для детерминированного метода");
            //Console.WriteLine();

            // Простое решение WalkerSolver
            //WalkerSolver walkerSolver = new WalkerSolver();
            //Stopwatch stopwatchSimpleWalker = new Stopwatch();
            //stopwatchSimpleWalker.Start();
            //var simpleWalkerSolverSolution = walkerSolver.Solve(aMatrix, bVector, 2800);
            //stopwatchSimpleWalker.Stop();
            //var timeInSeconds = stopwatchSimpleWalker.ElapsedMilliseconds / (double)1000;
            //Console.WriteLine(timeInSeconds + " --- время для Solve() у WalkerSolver");
            //Console.WriteLine();

            // Простое решение NoWalkerSolver
            NoWalkerSolver simpleSolver = new NoWalkerSolver(new Random(123));
            Stopwatch stopwatchSimple = new Stopwatch();
            stopwatchSimple.Start();
            var simpleNoWalkerSolution = simpleSolver.Solve(aMatrix, bVector, 7500);
            stopwatchSimple.Stop();
            var timeInSeconds = stopwatchSimple.ElapsedMilliseconds / (double)1000;
            Console.WriteLine(timeInSeconds + " --- время для Solve() у NoWalkerSolver");
            Console.WriteLine();

            // Решение простой псевдоортогонализацией
            NoWalkerSolver pseudoOrthogonalizationSolver = new NoWalkerSolver(new Random(129));
            Stopwatch stopwatchPseudoOrthogonalization = new Stopwatch();
            stopwatchPseudoOrthogonalization.Start();
            var pseudoOrthogonalizationSolution =
                pseudoOrthogonalizationSolver.SolveByPseudoOrthogonalization(aMatrix, bVector, 2700);
            stopwatchPseudoOrthogonalization.Stop();
            timeInSeconds = stopwatchPseudoOrthogonalization.ElapsedMilliseconds / (double)1000;
            Console.WriteLine(timeInSeconds + " --- время для SolveByPseudoOrthogonalization() у NoWalkerSolver");
            Console.WriteLine();

            // Решение простым балансированием
            //NoWalkerSolver balansedSolver = new NoWalkerSolver(new Random(14424582));
            //Stopwatch stopwatchBalancedSimple = new Stopwatch();
            //stopwatchBalancedSimple.Start();
            //var balancedSimpleSolution = balansedSolver.SolveByBalancing(aMatrix, bVector, 900, 5);
            //stopwatchBalancedSimple.Stop();
            //timeInSeconds = stopwatchBalancedSimple.ElapsedMilliseconds / (double)1000;
            //Console.WriteLine(timeInSeconds + " --- время для SolveByBalancing() (простого балансирования) у NoWalkerSolver");
            //Console.WriteLine();

            //// Решение простым медианным способом
            //NoWalkerSolver mediansSimpleSolver = new NoWalkerSolver(new Random(650680));
            //Stopwatch stopwatchMediansSimple = new Stopwatch();
            //stopwatchMediansSimple.Start();
            //var mediansSimpleSolution = mediansSimpleSolver.SolveByMedians(aMatrix, bVector, 7000);
            //stopwatchMediansSimple.Stop();
            //timeInSeconds = stopwatchMediansSimple.ElapsedMilliseconds / (double)1000;
            //Console.WriteLine(timeInSeconds + " --- время для SolveByMedians()  у NoWalkerSolver");
            //Console.WriteLine();

            //// Решение простым итерационным уточнением
            //NoWalkerSolver simpleIterRefSolver = new NoWalkerSolver(new Random(10));
            //Stopwatch stopwatchIterRefSimple = new Stopwatch();
            //stopwatchIterRefSimple.Start();
            //var iterRefSimpleSolution = simpleIterRefSolver.SolveBySimpleIterativeRefinement(aMatrix, bVector, 3100, 4);
            //stopwatchIterRefSimple.Stop();
            //var timeInSeconds = stopwatchIterRefSimple.ElapsedMilliseconds / (double)1000;
            //Console.WriteLine(timeInSeconds + " --- время для SolveBySimpleIterativeRefinement  у NoWalkerSolver");
            //Console.WriteLine();

            //// Решение балансированным итерационным уточнением
            //NoWalkerSolver balancedIterRefSolver = new NoWalkerSolver(new Random(2));
            //Stopwatch stopwatchIterRefBalanced = new Stopwatch();
            //stopwatchIterRefBalanced.Start();
            //var iterRefBalancedSolution = balancedIterRefSolver.SolveByBalancingIterativeRefinement(aMatrix, bVector, 2500, 4, 1);
            //stopwatchIterRefBalanced.Stop();
            //timeInSeconds = stopwatchIterRefBalanced.ElapsedMilliseconds / (double)1000;
            //Console.WriteLine(timeInSeconds + " --- время для SolveByBalancingIterativeRefinement у NoWalkerSolver");
            //Console.WriteLine();

            // Решение медианным итерационным уточнением
            //NoWalkerSolver mediansIterRefSolver = new NoWalkerSolver(new Random(40));
            //Stopwatch stopwatchIterRefMedians = new Stopwatch();
            //stopwatchIterRefMedians.Start();
            //var iterRefMediansSolution = mediansIterRefSolver.SolveByMediansIterativeRefinement(aMatrix, bVector, 1000, 3);
            //stopwatchIterRefMedians.Stop();
            //timeInSeconds = stopwatchIterRefMedians.ElapsedMilliseconds / (double)1000;
            //Console.WriteLine(timeInSeconds + " --- время для SolveByMediansIterativeRefinement у NoWalkerSolver");
            //Console.WriteLine();

            //// Подсчёт погрешностей
            //var deterministicError = GetRelError(trueXVector, simpleDeterministicSolution);
            //var simpleWalkerError = GetRelError(trueXVector, simpleWalkerSolverSolution);
            var simpleError = GetRelError(trueXVector, simpleNoWalkerSolution);
            var pseudoOrthogonalizationError = GetRelError(trueXVector, pseudoOrthogonalizationSolution);

            //var balancedError = GetRelError(trueXVector, balancedSimpleSolution);
            //var mediansError = GetRelError(trueXVector, mediansSimpleSolution);
            //var iterRefSimpleError = GetRelError(trueXVector, iterRefSimpleSolution);
            //var iterRefBalancedError = GetRelError(trueXVector, iterRefBalancedSolution);
            //var iterRefMediansError = GetRelError(trueXVector, iterRefMediansSolution);

            // Вывод погрешностей
            //Console.WriteLine(deterministicError + " % -- ошибка детерминированного решения");
            //Console.WriteLine();

            //Console.WriteLine(simpleWalkerError + " % -- ошибка простого решения с Уолкером");
            //Console.WriteLine();

            Console.WriteLine(simpleError + " % -- ошибка простого решения без Уолкера");
            Console.WriteLine();

            Console.WriteLine(pseudoOrthogonalizationError + " % -- ошибка псевдоортогонального решения");
            Console.WriteLine();

            //Console.WriteLine(balancedError + " % -- ошибка сбалансированного решения");
            //Console.WriteLine();

            //Console.WriteLine(mediansError + " % -- ошибка медианного решения");
            //Console.WriteLine();

            //Console.WriteLine(iterRefSimpleError + " % -- ошибка решения ПРОСТЫМ итерационным уточнением");
            //Console.WriteLine();

            //Console.WriteLine(iterRefBalancedError + " % -- ошибка решения БАЛАНСИРОВАННЫМ итерационным уточнением");
            //Console.WriteLine();

            //Console.WriteLine(iterRefMediansError + " % -- ошибка решения МЕДИАННЫМ итерационным уточнением");
            //Console.WriteLine();

            #endregion


            #region Решение задачи Дирихле

            //LaplasEquationSolver solver = new LaplasEquationSolver(1, 1, 300, 300);
            //(double, double) point = (0.5, 0.25);
            //var trueU = U(point.Item1, point.Item2);

            //var approximateU = solver.GetApproximateU(point);
            //var relationalError = (Math.Abs(approximateU - trueU) / trueU) * 100;
            //Console.WriteLine("Приближённо: " + approximateU);
            //Console.WriteLine("Точно: " + trueU);
            //Console.WriteLine("Относительная Погрешность: " + relationalError + "%");

            #endregion


            //var uResults = new List<double>();
            //for (int i = 0; i < 5; i++)
            //{
            //    var approximateU = solver.GetApproximateU(point);
            //    uResults.Add(approximateU);
            //    Console.WriteLine(i);
            //}
            //var averageU = uResults.Sum() / uResults.Count();


            #region Проверка работоспособности Уолкера

            //Проверка работоспособности моего Волкера(что распределение генерится верно)
            //List<double> probabilities = new List<double>() {0.2, 0.5, 0.3};
            //Walker walker = new Walker(probabilities);
            //Dictionary<int, double> distribution = new Dictionary<int, double>();

            //int N = 1000000;
            //for (int i = 0; i < N; i++)
            //{
            //    int randomValue = walker.GetSelection();
            //    if (!distribution.ContainsKey(randomValue))
            //    {
            //        distribution.Add(randomValue, (double)1 / N);
            //    }
            //    else
            //    {
            //        distribution[randomValue] += (double)1 / N;
            //    }
            //}

            //// Конец проверки

            //int x = 1;

            #endregion

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

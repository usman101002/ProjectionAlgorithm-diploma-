﻿using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
using MonteKarloMatrixVectorProduct;

namespace ProjectionAlgorithm_diploma_
{
    internal class LinearSystemSolver
    {
        public Matrix<double> A { get; set; }
        public Vector<double> BVector { get; set; }
        public Walker Walker { get; set; }

        public LinearSystemSolver(Matrix<double> a, Vector<double> bVector)
        {
            A = a;
            BVector = bVector;
            Walker = new Walker(GetDistributionOfMatrixRows(A));
        }
        
        /// <summary>
        /// Метод позволяет решать СЛАУ Ax = BVector, где BVector -- вектор правой части с
        /// помощью проекционного метода.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Vector<double> SolveByWalker()
        {
            var xPrev = Vector<double>.Build.Dense(this.BVector.Count);
            //var xPrev = BVector;
            for (int i = 1; i < 100000; i++)
            {
                int index = this.Walker.GetSelection();
                var aRow = this.A.Row(index);
                var numerator = this.BVector[index] - (xPrev * aRow);
                var denominator = aRow.Select(x => x * x).Sum();
                var xCur = xPrev + ((numerator * aRow) / (denominator));
                xPrev = xCur;
                if (i % 1000 == 0)
                    Console.WriteLine(i);

            }
            return xPrev;
        }

        /// <summary>
        /// Стандартый проекционный алгритм, но индекс выбирает без Волкера, а с помощью обычного rand, но предполагается, что
        /// распределение строк матрицы будет равномерным.
        /// </summary>
        /// <returns></returns>
        private Vector<double> SolveWithoutWalker()
        {
            // делаем сто тысяч итераций...
            var xPrev = Vector<double>.Build.Dense(this.BVector.Count);
            for (int i = 0; i < 1000000; i++)
            {
                Random rnd = new Random();
                int index = rnd.Next(0, this.A.RowCount);
                var aRow = this.A.Row(index);
                var numerator = this.BVector[index] - (xPrev * aRow);
                var denominator = aRow.Select(x => x * x).Sum();
                var xCur = xPrev + ((numerator * aRow) / (denominator));
                xPrev = xCur;
            }

            return xPrev;
        }

        /// <summary>
        /// Решение с помощью итерационного уточнения
        /// </summary>
        /// <param name="numOfIterations">1 итерация это просто решение без уточнения, 2 итерации -- это уже одно уточнение,...</param>
        /// <returns></returns>
        public Vector<double> SolveWithIterativeRefinement(int numOfIterations)
        {
            if (numOfIterations <= 0)
                throw new Exception("numOfIterations должен быть не меньше 1");

            var res = this.SolveByWalker();
            if (numOfIterations == 1)
                return res;

            for (int i = 0; i < numOfIterations - 1; i++)
            {
                var pseudoB = this.A * res;
                var d = this.BVector - pseudoB;
                var solver = new LinearSystemSolver(this.A, d);
                var refinement = solver.SolveByWalker();
                res += refinement;
            }
            return res;
        }

        /// <summary>
        /// Данный метод решает СЛАУ Ax=b путём умножения всей СЛАУ на диагональную матрицу В (т.е. DAx=Db), где D такая,
        /// что матрица DA имеет равномерное распределение по строкам. Дальше используется обычный проекционный алгоритм, но плюс в том,
        /// что не надо использовать метод Уолкера для генерации случайной величины.
        /// </summary>
        /// <returns></returns>
        public Vector<double> SolveUniformly()
        {
            Matrix<double> diagMatrix = GetDiagProbMatrix(this.A);
            Matrix<double> aNew = diagMatrix * this.A;
            var norms = new List<double>();
            for (int i = 0; i < aNew.RowCount; i++)
            {
                norms.Add(this.GetRowNorm(aNew.Row(i)));
            }

            Vector<double> bNew = diagMatrix * BVector;
            var newSolver = new LinearSystemSolver(aNew, bNew);
            var result = newSolver.SolveWithoutWalker();
            return result;
        }

        private Matrix<double> GetDiagProbMatrix(Matrix<double> matrix)
        {
            int rowCount = matrix.RowCount;
            int colCount = matrix.ColumnCount;
            double[,] matrixArr = new double[rowCount, colCount];
            for (int i = 0; i < rowCount; i++)
            {
                var row = matrix.Row(i);
                var norm = GetRowNorm(row);
                matrixArr[i, i] = 1 / norm;
            }

            var result = DenseMatrix.OfArray(matrixArr);
            return result;
        }

        private double GetRowNorm(IEnumerable<double> row)
        {
            var result = row.Select(x => x * x).Sum();
            result = Math.Sqrt(result);
            return result;
        }

        private List<double> GetDistributionOfMatrixRows(Matrix<double> matrix)
        {
            var probabilities = new List<double>();
            var matrixRows = new List<List<double>>();

            //матрица 
            for (int i = 0; i < matrix.RowCount; i++)
            {
                var matrixRow = matrix.Row(i).ToList();
                matrixRows.Add(matrixRow);
            }

            double frobeniusNormSquared = 0;
            for (int i = 0; i < matrixRows.Count; i++)
            {
                for (int j = 0; j < matrixRows.Count; j++)
                {
                    frobeniusNormSquared += matrixRows[i][j] * matrixRows[i][j];
                }
            }

            for (int i = 0; i < matrixRows.Count; i++)
            {
                probabilities.Add(matrixRows[i].Select(x => x * x).Sum() / frobeniusNormSquared);
            }

            var sum = probabilities.Sum();
            return probabilities;
        }
    }
}

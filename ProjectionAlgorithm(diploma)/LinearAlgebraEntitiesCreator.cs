using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using MonteKarloMatrixVectorProduct;

namespace ProjectionAlgorithm_diploma_
{
    /// <summary>
    /// В качестве тестового примера вектором x должен быть вектор, состоящий из единиц.
    /// В качестве матрицы A будет матрица c элементами вида i * size + (j + 1). В соответствии с матрицей будет
    /// уже заполняться вектор b. А потом уже эта сущность будет передаваться в класс-решателя СЛАУ
    /// проекционным методом. 
    /// </summary>
    internal class AlgebraEntitiesCreator
    {
        
        public Matrix<double> A { get; set; }
        public Vector<double> BVector { get; set; }

        private Random rnd = new Random();

        public AlgebraEntitiesCreator()
        {
        }

        /// <summary>
        /// Лучше делать стохастическую матрицу по строкам, используя нормировку. 
        /// </summary>
        /// <param name="n"></param>
        public AlgebraEntitiesCreator(int n)
        {
            this.A = CreateStochasticMatrix(n);
            this.BVector = CreateStochasticBVector(n);
        }

        // этот метод обязательно надо переписать (чтобы стохастической по строкам была матрица) 
        private Matrix<double> CreateStochasticMatrix(int n)
        {
            var aArr = new double[n, n];
            

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    aArr[i, j] = rnd.NextDouble();
                }
            }

            for (int i = 0; i < n; i++)
            {
                double s = 0;
                for (int j = 0; j < n; j++)
                {
                    s += aArr[i, j];
                }

                for (int j = 0; j < n; j++)
                {
                    aArr[i, j] /= s;
                }
            }

            var builder = Matrix<double>.Build;
            var result = builder.DenseOfArray(aArr);
            return result;
        }

        private Vector<double> CreateStochasticBVector(int n)
        {
            var b = new double[n];

            for (int i = 0; i < n; i++)
            {
                b[i] = rnd.NextDouble();
            }

            var builder = Vector<double>.Build;
            var result = builder.DenseOfArray(b);
            return result;
        }
    }
}

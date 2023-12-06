using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace ProjectionAlgorithm_diploma_
{
    /// <summary>
    /// В качестве тестового примера вектором x должен быть вектор, состоящий из единиц.
    /// В качестве матрицы A будет матрица c элементами вида i * size + (j + 1). В соответствии с матрицей будет
    /// уже заполняться вектор b. А потом уже эта сущность будет передаваться в класс-решателя СЛАУ
    /// проекционным методом. 
    /// </summary>
    internal class LinearAlgebraEntitiesCreator
    {
        public Matrix<double> A { get; set; }
        public Vector<double> BVector { get; set; }
        public LinearAlgebraEntitiesCreator()
        {
        }

        public LinearAlgebraEntitiesCreator(int n)
        {
            this.A = CreateSquareMatrix(n);
            this.BVector = CreateBVector(this.A);
        }

        private Matrix<double> CreateSquareMatrix(int size)
        {
            var aArr = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    aArr[i, j] = i * size + (j + 1);
                }
            }

            var builder = Matrix<double>.Build;
            var result = builder.DenseOfArray(aArr);
            return result;
        }

        private Vector<double> CreateBVector(Matrix<double> mtx)
        {
            var b = new double[mtx.ColumnCount];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = mtx.Row(i).Sum();
            }

            var builder = Vector<double>.Build;
            var result = builder.DenseOfArray(b);
            return result;
        }
    }
}

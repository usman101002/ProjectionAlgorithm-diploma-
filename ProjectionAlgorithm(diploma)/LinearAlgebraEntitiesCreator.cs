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
                    if (i == j)
                    {
                        if (!((i == 0) || (i == size - 1)))
                        {
                            aArr[i, j] = i * size + (j + 1);
                            aArr[i, j - 1] = 1;
                            aArr[i, j + 1] = 1;
                        }
                        else aArr[i, j] = i * size + (j + 1);
                    }
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
                if (i == 0)
                {
                    b[i] = 1;
                }
                else if (i == mtx.RowCount - 1)
                {
                    b[i] = mtx[i, i];
                }
                else
                {
                    b[i] = mtx[i, i] + 1 + 1;
                }
            }
            var builder = Vector<double>.Build;
            var result = builder.DenseOfArray(b);
            return result;
        }
    }
}

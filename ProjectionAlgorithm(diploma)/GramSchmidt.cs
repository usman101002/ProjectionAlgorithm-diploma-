using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    /// <summary>
    /// Превращение произвольной матрицы 
    /// </summary>
    public static class GramSchmidt
    {
        /// <summary>
        /// Прокатывает только для процесса ортогонализации 3 векторов (потому что надо так). 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public static (Vector, Vector, Vector) Orthogonalize(Vector a0, Vector a1, Vector a2)
        {
            Vector b0 = a0;

            Vector b1 = a1 - ((a1 * b0) / (b0 * b0)) * b0;

            Vector b2 = a2 - ((a2 * b0) / (b0 * b0)) * b0 - ((a2 * b1) / (b1 * b1)) * b1;

            return (b0, b1, b2);

        }
    }
}

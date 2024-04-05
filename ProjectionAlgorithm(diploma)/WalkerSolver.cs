using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonteKarloMatrixVectorProduct;

namespace ProjectionAlgorithm_diploma_
{
    public class WalkerSolver
    {
        private Walker walker;

        public WalkerSolver()
        {
            /// Свойству walker значение будет присваиваться только во время рандомизированной
            /// работы. Это не очень круто, но что поделать.
            /// 
        }

        public Vector Solve(Matrix aMatrix, Vector bVector)
        {
            return null;
        }

        private List<double> GetDistributionOfMatrixRows(Matrix matrix)
        {
            return null;
        }

        private double GetRowNorm(IEnumerable<double> row)
        {
            var result = row.Select(x => x * x).Sum();
            result = Math.Sqrt(result);
            return result;
        }

        private int GetRandomIndex(int n)
        {
            return int.MaxValue;
        }


    }
}

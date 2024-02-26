using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace ProjectionAlgorithm_diploma_
{
    internal interface ILinearSystemSolver
    {
        Matrix<double> A { get; set; }
        Vector<double> BVector { get; set; }
        Vector<double> SolveByWalker();

    }
}

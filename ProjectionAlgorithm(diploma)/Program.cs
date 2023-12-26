using System;
using System.Collections.Generic;
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
            int n = 1000;
            LinearAlgebraEntitiesCreator creator = new LinearAlgebraEntitiesCreator(n);
            var solver = new LinearSystemSolver(creator.A, creator.BVector);
            var res = solver.Solve();

            Console.WriteLine();

            //Теперь посчитаем погрешность 
            Vector<double> trueX = Vector<double>.Build.Dense(n); ; // верное решение 
            for (int i = 0; i < n; i++)
            {
                trueX[i] = 1;
            }

            var numerator = (trueX - res).Norm(2);
            var denominator = trueX.Norm(2);

            Console.WriteLine("Относительная погрешность:");
            Console.WriteLine(numerator / denominator);

            Console.WriteLine("2-норма разности:");
            Console.WriteLine(numerator);

        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectionAlgorithm_diploma_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LinearAlgebraEntitiesCreator creator = new LinearAlgebraEntitiesCreator(3);
            var solver = new LinearSystemSolver(creator.A, creator.BVector);



        }
    }
}

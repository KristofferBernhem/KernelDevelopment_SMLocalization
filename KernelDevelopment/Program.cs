﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MathNet.Numerics;
//using MathNet.Numerics.LinearAlgebra;
namespace KernelDevelopment
{
    class Program
    {
        static void Main(string[] args)
        {
           // add.Execute();
       //     medianFilteringInterpolateSecond.Execute();   // IN USE!
  //          findMaxima.Execute();
        //    gaussFit.Execute();
         //   driftCorr.Execute();
            filterImage.Execute();




            //vectorAdd.Execute();
            //prepareGauss.Execute();
           // medianFiltering.Execute(); // test median filtering.
            //medianFilteringInterpolate.Execute(); // test median filtering with interpolation.
            
            //medianFilteringShort.Execute(); // test median filtering.
            
            //gaussFitShort.Execute();
            
            //TestGPU.Execute();
            // http://numerics.mathdotnet.com/Matrix.html for details:

            // simple scheme for quick gauss fit using least square (matlab code functional). Use this to get initial guess.
 /*           var M = Matrix<double>.Build;
            var m = M.Random(3, 4);
            var n = Vector<double>.Build.Random(3);
            Console.Out.WriteLine(m.ToString());
            Console.Out.WriteLine(n.ToString());
            var y = m.TransposeThisAndMultiply(m).Inverse() * m.TransposeThisAndMultiply(n);
            Console.Out.WriteLine(y.ToString());
            Console.ReadKey(); // keep console up.*/
        }
    }
}

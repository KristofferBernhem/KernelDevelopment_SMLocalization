﻿using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KernelDevelopment
{
    class filterImage
    {
        /*
         * GTX1080 card: 
         */ 
        public static void Execute()
        {
            // Initialize.
            CudafyModule km = CudafyTranslator.Cudafy();
            km.GenerateDebug = true;
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            gpu.LoadModule(km);

            // cudart.dll must be accessible!
            GPGPUProperties prop = null;
            try
            {
                prop = gpu.GetDeviceProperties(true);
            }
            catch (DllNotFoundException)
            {
                prop = gpu.GetDeviceProperties(false);
            }
            double[] timers= { 0, 0, 0, 0, 0 , 0,0,0};
            ushort count = 0;
           int[] Ntests = { 100};//, 1000, 5000, 10000, 20000 };
            //int[] Ntests = { 30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000 };
         //   for (ushort i = 0; i < Ntests.Length; i++)
          //  {
                

                // Profiling:
                Stopwatch watch = new Stopwatch();
                watch.Start();
            
            
                                                          
                int fH = 8;
                int fW = 8;
                
               
                
         /*       int[] data = {   1824, 1840, 1808, 1744, 1824, 1824, 1808, 1744, 
                                  1760, 1824, 1824, 1728, 1728, 1792, 1760, 1696, 
                                  1776, 1728, 1760, 1792, 1760, 1840, 1856, 1744, 
                                  1792, 1792, 1760, 1760, 1792, 1840, 1776, 1872, 
                                  1824, 1824, 1760, 1760, 1792, 1792, 1888, 2000, 
                                  1936, 1840, 1888, 1936, 1888, 1808, 1728, 1728, 
                                  1744, 1904, 1936, 1744, 1856, 1776, 1824, 1888,
                                  1744, 1904, 1936, 1744, 1856, 1776, 1824, 1888};
            */    int[] data = {0, 0, 0, 0, 0, 0, 0, 0, 
                                0, 0, 0, 0, 0, 0, 0, 0, 
                                0, 0, 1000, 0, 0, 0, 0, 0, 
                                0, 0, 0, 0, 0, 0, 0, 0, 
                                0, 0, 0, 0, 0, 0, 0, 0, 
                                0, 0, 0, 0, 0, 0, 0, 0, 
                                0, 0, 0, 0, 0, 0, 0, 0,
                                0, 0, 0, 0, 0, 0, 0, 0, };
            
                int kernelSize = 5;
             /*   double[] kernel = { 0.0015257568383789054, 0.003661718759765626, 0.02868598630371093, 0.0036617187597656254, 0.0015257568383789054, 
                                    0.003661718759765626, 0.008787890664062511, 0.06884453115234379, 0.00878789066406251, 0.003661718759765626, 
                                    0.02868598630371093, 0.06884453115234379, 0.5393295900878906, 0.06884453115234378, 0.02868598630371093,
                                    0.0036617187597656254, 0.00878789066406251, 0.06884453115234378, 0.008787890664062508, 0.0036617187597656254, 
                                    0.0015257568383789054, 0.003661718759765626, 0.02868598630371093, 0.0036617187597656254, 0.0015257568383789054};
            */
                float[] kernel = { 0.0015257568383789054F, 0.003661718759765626F, 0.02868598630371093F, 0.0036617187597656254F, 0.0015257568383789054F, 
                                0.003661718759765626F, 0.008787890664062511F, 0.06884453115234379F, 0.00878789066406251F, 0.003661718759765626F, 
                                0.02868598630371093F, 0.06884453115234379F, 0.5393295900878906F, 0.06884453115234378F, 0.02868598630371093F,
                                0.0036617187597656254F, 0.00878789066406251F, 0.06884453115234378F, 0.008787890664062508F, 0.0036617187597656254F, 
                                0.0015257568383789054F, 0.003661718759765626F, 0.02868598630371093F, 0.0036617187597656254F, 0.0015257568383789054F}; // 5x5 bicubic Bspline filter.
		
                int[] deviceData = gpu.CopyToDevice(data);
                int[] deviceOutput = gpu.Allocate<int>(data.Length); // 25 
                float[] deviceKernel = gpu.CopyToDevice(kernel);
                int N_squared = (int)(Math.Sqrt(data.Length / (fH * fW)));

                //gpu.Launch(new dim3(N_squared, N_squared), 1).filterKernel(deviceData, fW, fH, deviceKernel, kernelSize, deviceOutput);

                int N = data.Length / (fH * fW);
                int blockSize = 256;
                int gridSize = (N + blockSize - 1) / blockSize;
                gridSize = gridSize * 2;
                gpu.Launch(gridSize, blockSize).filterKernel(N,deviceData, fW, fH, deviceKernel, kernelSize, deviceOutput);
                int[] output = new int[data.Length];                
                gpu.CopyFromDevice(deviceOutput, output);           
                for (int h = 0; h < output.Length; h++)
                {
                    Console.Out.Write(data[h] + " ");
                    if (h % fH == fH - 1)
                        Console.Out.WriteLine("");
                }
                Console.Out.WriteLine("*********************************");
                for (int h = 0; h < output.Length; h ++)
                {
                    Console.Out.Write(output[h]+ " ");
                   if (h%fH == fH-1)
                        Console.Out.WriteLine("");
                }
                           
                    //Profile:
                    watch.Stop();
                Console.WriteLine( watch.ElapsedMilliseconds + " total time");
                
                // Clear gpu.
                gpu.FreeAll();
                gpu.HostFreeAll();

                // profiling.
                count++;

           
            // profiling.
           

            Console.ReadKey(); // keep console up.
        } // Execute()     
        

    /*
     * Datainput as: {X1Y1}{X2Y1}...
     */ 
        [Cudafy]

        public static void filterKernel(GThread thread, int n, int[] data, int frameWidth, int frameHeight, float[] kernel, int kernelSize, int[] output)
        {
            // int idx = thread.blockIdx.x + thread.gridDim.x * thread.blockIdx.y;           
           //  if (idx < data.Length / (frameWidth * frameHeight))  // if current idx points to a location in input.
            int indexStart = thread.blockIdx.x * thread.blockDim.x + thread.threadIdx.x;
            int stride = thread.blockDim.x * thread.gridDim.x;
            int idx = indexStart;                        
            while (idx < n)
            {
                
              int halfKernel = (int)(Math.Ceiling((double)(kernelSize / 2)));
                int frameStart = idx * frameWidth * frameHeight;
                for (int i = frameStart; i < (idx + 1) * frameWidth * frameHeight; i++)
                {
                    float filtered = 0;
                    int k = i - (halfKernel) * (frameWidth + 1);                                                 
                    int loopC = 0;
                    int j = 0;
                    while (k <= i + (halfKernel) * (frameWidth)) // loop over all relevant pixels. use this loop to extract data based on single indexing defined centers.
                    {
                        if (k < (idx + 1) * frameWidth * frameHeight && k >= (idx) * frameWidth * frameHeight)
                            filtered += data[k] * kernel[j];
                        k++;
                        loopC++;
                        j++;
                        if (loopC == kernelSize)
                        {
                            k += (frameWidth - kernelSize);
                            loopC = 0;
                        }                             
                    }
                    output[i] = (int)(filtered);
                }
     /*           for (int xi = 0; xi < frameWidth; xi++)
                {
                    for (int yi = 0; yi < frameHeight; yi++)
                    {
                      //  float swap = 0;
                        int outputIndex = xi + yi * frameHeight + frameStart;
                        if (outputIndex < (idx + 1) * frameWidth * frameHeight && outputIndex >= (idx) * frameWidth * frameHeight)
                        {
                            for (int i = -halfKernel; i <= halfKernel; i++)
                            {
                                if (xi + i < frameWidth && xi + i >= 0)
                                {
                                    for (int j = -halfKernel; j <= halfKernel; j++)
                                    {
                                        if (yi + j < frameHeight && yi + j >= 0)
                                        {
                                            int index = (xi + i) + (yi + j) * frameHeight + frameStart;
                                            if (index < (idx + 1) * frameWidth * frameHeight && index >= (idx) * frameWidth * frameHeight)                                            
                                            {                                                                                                
                                                int kernelIdx = (i + kernelSize / 2) + (j + kernelSize / 2)* (kernelSize);
                                                if (kernelIdx < kernel.Length)
                                                {
                                                    float temp = (float)(data[index] * kernel[kernelIdx]);
                                                    output[outputIndex] += temp;
                                                }                                       
                                                //swap += (float)(data[index] * kernel[kernelIdx]);
                                            }
                                        }
                                    }
                                }
                            }
                           }
                              
                    }
                } // main inner section                 */

                idx += stride;
            } // idx check
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Classificadores
{
    public class Flower
    {
        public double[] Features;
        public int Class { get; set; }
        public Flower(double sepalLenght, double sepalWidth, double petalLenght, double petalWidth, string name)
        {
            Features = new double[4];
            Features[0] = sepalLenght;
            Features[1] = sepalWidth;
            Features[2] = petalLenght;
            Features[3] = petalWidth;
            if (name == "Iris-setosa")
                Class = 0;
            if (name == "Iris-versicolor")
                Class = 1;
            if (name == "Iris-virginica")
                Class = 2;
        }   

        public static double Distance(double[] a, double[] b)
        {
            double sum = 0;
            for (int i = 0; i < a.Length; i++)
                sum += Math.Pow(a[i] - b[i], 2);
            return Math.Sqrt(sum);
        }

        public static int Classify(double[] unknown, List<Flower> trainData, int numClasses, int k)
        {
            int n = trainData.Count;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0;  i < n; i++)
            {
                IndexAndDistance current = new IndexAndDistance();
                double dist = Distance(unknown, trainData[i].Features);
                current.idx = i;
                current.dist = dist;
                info[i] = current;
            }
            Array.Sort(info);     

            int result = Vote(info, trainData, numClasses, k);
            return result;  
        } 

        private static int Vote(IndexAndDistance[] info, List<Flower> trainData, int numClasses, int k)
        {
            int[] votes = new int[numClasses];  
            for (int i = 0; i < k; ++i) 
            {
                int idx = info[i].idx;  
                int c = trainData[idx].Class; 
                ++votes[c];
            }

            int bestClass = 0;
            int theChosenOne = 0;
            for (int j = 0; j < numClasses; ++j)
            {
                if (votes[j] > bestClass)
                {
                     bestClass = votes[j];
                        theChosenOne = j;
                }
            }
            return theChosenOne;
        }

        public static List<Flower> GetFlowers()
            {
                List<Flower> flores = new List<Flower>();
                var colunas = File.ReadAllLines(@"C:\Flor.txt");

                foreach (string coluna in colunas)
                {
                    var linha = coluna.Split(",");
                    var flor = new Flower(Convert.ToDouble(linha[0]), Convert.ToDouble(linha[1]), Convert.ToDouble(linha[2]), Convert.ToDouble(linha[3]), linha[4]);
                    flores.Add(flor);
                }
                return flores;
            }
        }
}

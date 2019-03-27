using Classificadores.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Classificadores
{
    public class Flower
    {
        public double[] Features;
        public int Class { get; set; }
        public string Name { get; set; }        
        public bool Trocar { get; set; }
        public bool Trocado { get; set; }


        public Flower(double sepalLenght, double sepalWidth, double petalLenght, double petalWidth, string name)
        {
            Features = new double[4];
            Features[0] = sepalLenght;
            Features[1] = sepalWidth;
            Features[2] = petalLenght;
            Features[3] = petalWidth;
            Class = GetFlowerCode(name);
            Name = name;
            Trocar = false;
            Trocado = false;
        }

        public static int GetFlowerCode(string name)
        {
            int flor = 0;
            if (name == "Iris-setosa")
                flor = 0;
            if (name == "Iris-versicolor")
                flor = 1;
            if (name == "Iris-virginica")
                flor = 2;
            return flor;
        }

        public static string GetFlowerName(int code)
        {
            string flor = "";
            if (code == 0)
                flor = "Iris-setosa";
            if (code == 1)
                flor = "Iris-versicolor";
            if (code == 2)
                flor = "Iris-virginica";
            return flor;
        }

        public static double Distance(double[] a, double[] b)
        {
            double sum = 0;
            for (int i = 0; i < a.Length; i++)
                sum += Math.Pow(a[i] - b[i], 2);
            return Math.Sqrt(sum);
        }

        public static string Classify(double[] unknown, List<Flower> trainData, int numClasses, int k)
        {
            int n = trainData.Count;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0; i < n; i++)
            {
                IndexAndDistance current = new IndexAndDistance();
                double dist = Distance(unknown, trainData[i].Features);
                current.idx = i;
                current.dist = dist;
                info[i] = current;
            }
            Array.Sort(info);

            int result = Vote(info, trainData, numClasses, k);
            return GetFlowerName(result);
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


        public static List<List<Flower>> GetFlowerLists()
        {
            List<List<Flower>> florezinhas = new List<List<Flower>>();

            List<Flower> dataSet = Flower.GetFlowers();
            foreach (var flower in dataSet.GroupBy(f => f.Name))
            {
                var list = new List<Flower>();
                foreach (Flower flor in flower)
                {
                    list.Add(flor);
                }
                florezinhas.Add(list);
            }
            return florezinhas;
        }
    }
}
    
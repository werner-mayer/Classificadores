using Classificadores.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Accord.Math.Distances;

namespace Classificadores
{
    public class Seed
    {
        public double[] Features;
        public int Class { get; set; }
        public string Name { get; set; }        
        public bool Trocar { get; set; }
        public bool Trocado { get; set; }

        public Seed(double x1, double x2, double x3, double x4, double x5, double x6, double x7, int name)
        {
            Features = new double[7];
            Features[0] = x1;
            Features[1] = x2;
            Features[2] = x3;
            Features[3] = x4;
            Features[4] = x5;
            Features[5] = x6;
            Features[6] = x7;
            Class = ClassToIndex(name);
            Name = name.ToString();            
            Trocar = false;
            Trocado = false;
        }
        
        public int ClassToIndex(int name)
        {
            if (name == 1)
                name = 0;
            if (name == 2)
                name = 1;
            if (name == 3)
                name = 2;
            return name;            
        }

        public static int IndexToClass(int name)
        {
            if (name == 0)
                return 1;
            if (name == 1)
                return 2;
            if (name == 2)
                return 3;
            return name;
        }

        public static double Distance(double[] a, double[] b, bool euclidiana)
        {
            double sum = 0;
            if (euclidiana)
            {
                for (int i = 0; i < a.Length; i++)
                    sum += Math.Pow(a[i] - b[i], 2);
                return Math.Sqrt(sum);
            }
            else
                return Accord.Math.Distance.Manhattan(a, b);
        }

        public static int Classify(double[] unknown, List<Seed> trainData, int numClasses, int k, bool euclidiana)
        {
            int n = trainData.Count;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0; i < n; i++)
            {
                IndexAndDistance current = new IndexAndDistance();
                double dist = Distance(unknown, trainData[i].Features, euclidiana);
                current.idx = i;
                current.dist = dist;
                info[i] = current;
            }
            Array.Sort(info);

            int result = Vote(info, trainData, numClasses, k);
            return result;
        }

        private static int Vote(IndexAndDistance[] info, List<Seed> trainData, int numClasses, int k)
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

        public static List<Seed> GetSeeds()
        {
            List<Seed> flores = new List<Seed>();
            var colunas = File.ReadAllLines(@"C:\Seeds.txt");

            foreach (string coluna in colunas)
            {
                var linha = coluna.Split(",");
                var flor = new Seed(Convert.ToDouble(linha[0]), Convert.ToDouble(linha[1]), Convert.ToDouble(linha[2]), Convert.ToDouble(linha[3]), Convert.ToDouble(linha[4]), Convert.ToDouble(linha[5]), Convert.ToDouble(linha[6]), Convert.ToInt32(linha[7]));
                flores.Add(flor);
            }
            return flores;
        }

        public static List<List<Seed>> GetSeedsList()
        {
            List<List<Seed>> seedzinhas = new List<List<Seed>>();

            List<Seed> dataSet = GetSeeds();
            foreach (var seeds in dataSet.GroupBy(f => f.Name))
            {
                var rnd = new Random();

                var query =
                    from i in seeds
                    let r = rnd.Next()
                    orderby r
                    select i;

                var shuffled = query.ToList();
                var list = new List<Seed>();
                foreach (Seed seed in shuffled)
                {
                    list.Add(seed);
                }
                seedzinhas.Add(list);
            }
            return seedzinhas;
        }
    }
}
    
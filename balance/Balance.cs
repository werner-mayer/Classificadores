using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Classificadores
{
    public class Balance
    {
        public double[] Features;
        public int Class { get; set; }
        public string Name { get; set; }
        public bool Trocar { get; set; }

        public Balance(double leftWeigth, double leftDistance, double rightWeigth, double rightDistance, string name)
        {
            Features = new double[4];
            Features[0] = leftWeigth;
            Features[1] = leftDistance;
            Features[2] = rightWeigth;
            Features[3] = rightDistance;
            Class = GetBalanceCode(name);
            Name = name;
            Trocar = false;
        }

        public static int GetBalanceCode(string name)
        {
            int balance = 0;
            if (name == "B")
                balance = 0;
            if (name == "L")
                balance = 1;
            if (name == "R")
                balance = 2;
            return balance;
        }

        public static string GetBalanceName(int code)
        {
            string balance = "";
            if (code == 0)
                balance = "B";
            if (code == 1)
                balance = "L";
            if (code == 2)
                balance = "R";
            return balance;
        }

        public static double Distance(double[] a, double[] b)
        {
            double sum = 0;
            for (int i = 0; i < a.Length; i++)
                sum += Math.Pow(a[i] - b[i], 2);
            return Math.Sqrt(sum);
        }

        public static string Classify(double[] unknown, List<Balance> trainData, int numClasses, int k)
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
            return GetBalanceName(result);
        }

        private static int Vote(IndexAndDistance[] info, List<Balance> trainData, int numClasses, int k)
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
        public static List<Balance> GetBalances()
        {
            List<Balance> balances = new List<Balance>();
            var colunas = File.ReadAllLines(@"C:\Balance.txt");

            foreach (string coluna in colunas)
            {
                var linha = coluna.Split(",");
                var balance = new Balance(Convert.ToDouble(linha[1]), Convert.ToDouble(linha[2]), Convert.ToDouble(linha[3]), Convert.ToDouble(linha[4]), linha[0]);
                balances.Add(balance);
            }
            return balances;
        }


        public static List<List<Balance>> GetBalanceList()
        {
            List<List<Balance>> listabalances = new List<List<Balance>>();

            List<Balance> dataSet = Balance.GetBalances();
            foreach (var balances in dataSet.GroupBy(f => f.Name))
            {
                var list = new List<Balance>();
                foreach (Balance balance in balances)
                {
                    list.Add(balance);
                }
                listabalances.Add(list);
            }
            return listabalances;
        }
    }
}

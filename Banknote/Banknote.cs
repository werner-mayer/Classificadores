using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Classificadores
{
    public class Banknote
    {
        public double[] Features;
        public int Class { get; set; }
        public int Name { get; set; }        
        public bool Trocar { get; set; }

        public Banknote(double x1, double x2, double y1, double y2, int name)
        {
            Features = new double[4];
            Features[0] = x1;
            Features[1] = x2;
            Features[2] = y1;
            Features[3] = y2;
            Class = name;    
            Trocar = false;        
     
        }

        public static double Distance(double[] a, double[] b)
        {
            double sum = 0;
            for (int i = 0; i < a.Length; i++)
                sum += Math.Pow(a[i] - b[i], 2);
            return Math.Sqrt(sum);
        }

        public static int Classify(double[] unknown, List<Banknote> trainData, int numClasses, int k)
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
            return result;
        }

        private static int Vote(IndexAndDistance[] info, List<Banknote> trainData, int numClasses, int k)
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
                if (votes[j] >= bestClass)
                {
                    bestClass = votes[j];
                    theChosenOne = j;
                }
            }
            return theChosenOne;
        }
        public static List<Banknote> GetNotes()
        {
            List<Banknote> notes = new List<Banknote>();
            var colunas = File.ReadAllLines(@"C:\Banknote.txt");

            foreach (string coluna in colunas)
            {
                var linha = coluna.Split(",");
                var nota = new Banknote(Convert.ToDouble(linha[0]), Convert.ToDouble(linha[1]), Convert.ToDouble(linha[2]), Convert.ToDouble(linha[3]), Convert.ToInt32(linha[4]));
                notes.Add(nota);
            }
            return notes;
        }


        public static List<List<Banknote>> GetNoteLists()
        {
            List<List<Banknote>> notazinhas = new List<List<Banknote>>();

            List<Banknote> dataSet = Banknote.GetNotes();
            foreach (var notas in dataSet.GroupBy(f => f.Name))
            {
                var list = new List<Banknote>();
                foreach (Banknote nota in notas)
                {
                    list.Add(nota);
                }
                notazinhas.Add(list);
            }
            return notazinhas;
        }
    }
}
    
using System;
using System.Collections.Generic;
using System.Linq;

namespace Classificadores
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Flower> flores = Flower.GetFlowers();
            var floresDistintas = flores.Select(x => x.Class).Distinct();
            List<Flower> trainData = new List<Flower>();
            List<Flower> ShuffleFlores2 = new List<Flower>();
            List<Flower> ShuffleFlores3 = new List<Flower>();
            for (int i = 0; i < flores.Count; i++)
            {
                if (i <= 12 || (i >= 50 && i < 62) || (i >= 100 && i <= 112))
                    trainData.Add(flores[i]);
                if ((i > 12 && i <= 24) || (i >= 62 && i <= 74) || (i > 112 && i <= 124))
                    ShuffleFlores2.Add(flores[i]);
                if ((i > 24 && i < 50) || (i > 74 && i < 100) || (i > 124))
                    ShuffleFlores3.Add(flores[i]);
            }

            var rand = new Random();
            trainData = trainData.OrderBy(x => rand.Next()).ToList();
            ShuffleFlores2 = ShuffleFlores2.OrderBy(x => rand.Next()).ToList();
            int numClasses = 3;
            int k = 1;
            int acertos = 0;
            int j = 1;

            foreach (Flower flower in ShuffleFlores2)
            {
                int Class = Flower.Classify(flower.Features, trainData, numClasses, k);
                string flor = "";

                if (Class == 0)
                    flor = "Iris-setosa";
                if (Class == 1)
                    flor = "Iris-versicolor";
                if (Class == 2)
                    flor = "Iris-virginica";

                Console.WriteLine(j++ + " - " + flor);

                if (flower.Class == Class)
                    acertos++;        
            }

            Console.WriteLine($"\nTotal de testes: {ShuffleFlores2.Count}");
            Console.WriteLine($"\nTotal de acertos: {acertos}" );
            Console.WriteLine($"\nPorcentagem de acertos: {100 * acertos / ShuffleFlores2.Count}%");
            Console.WriteLine("");
        }
    }
}

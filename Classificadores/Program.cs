using System;
using System.Collections.Generic;
using System.Linq;

namespace Classificadores
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Flower> dataSet = Flower.GetFlowers();
            List<Flower> trainData = new List<Flower>();
            List<Flower> avalicao = new List<Flower>();
            List<Flower> teste = new List<Flower>();

            for (int i = 0; i < dataSet.Count; i++)
            {
                if (i <= 12 || (i >= 50 && i < 62) || (i >= 100 && i <= 112))
                    trainData.Add(dataSet[i]);
                if ((i > 12 && i <= 24) || (i >= 62 && i <= 74) || (i > 112 && i <= 124))
                    avalicao.Add(dataSet[i]);
                if ((i > 24 && i < 50) || (i > 74 && i < 100) || (i > 124))
                    teste.Add(dataSet[i]);
            }

            var rand = new Random();
            trainData = trainData.OrderBy(x => rand.Next()).ToList();
            avalicao = avalicao.OrderBy(x => rand.Next()).ToList();
            teste = teste.OrderBy(x => rand.Next()).ToList();

            int numClasses = 3;
            int k = 1;
            int acertos = 0;
            int j = 1;

            foreach (Flower flower in teste)
            {             
                string flor = Flower.Classify(flower.Features, trainData, numClasses, k);
           
                if (flower.Class == Flower.GetFlowerCode(flor))
                    acertos++;

                Console.WriteLine(j++ + " - " + flor);
            }

            Console.WriteLine($"\nTotal de testes: {teste.Count}");
            Console.WriteLine($"\nTotal de acertos: {acertos}" );
            Console.WriteLine($"\nPorcentagem de acertos: {100 * acertos / teste.Count}%");
            Console.WriteLine("");
        }
    }
}

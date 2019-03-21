using System;
using System.Collections.Generic;
using System.Linq;

namespace Classificadores
{
    class Program
    {
        static void Main(string[] args)
        {
            
            List<Flower> trainData = new List<Flower>();
            List<Flower> avaliacao = new List<Flower>();
            List<Flower> teste = new List<Flower>();

            List<List<Flower>> florezinhas = Flower.GetFlowerLists();

            foreach (var flower in florezinhas)
            {
                for(var i = 0; i < flower.Count; i++)
                {
                    if (i < flower.Count / 4 )
                        trainData.Add(flower[i]);
                    if (i > flower.Count / 4 && i < flower.Count / 2)
                        avaliacao.Add(flower[i]);
                    if(i >= flower.Count / 2)
                        teste.Add(flower[i]);
                }
            }

            int numClasses = 3;
            int k = 5;
            int acertos = 0;
            int j = 1;

            for(int g = 0; g < avaliacao.Count; g++)
            {
                string flor = Flower.Classify(avaliacao[g].Features, trainData, numClasses, k);
                if (avaliacao[g].Class != Flower.GetFlowerCode(flor))
                    trainData[g] = avaliacao[g];
            }

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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Classificadores
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Seed> trainData = new List<Seed>();
            List<Seed> avaliacao = new List<Seed>();
            List<Seed> teste = new List<Seed>();

            List<List<Seed>> seedzinhas = Seed.GetSeedsList();

            foreach (var seed in seedzinhas)
            {
                for (var i = 0; i < seed.Count; i++)
                {
                    if (i < seed.Count / 4)
                        trainData.Add(seed[i]);
                    if (i > seed.Count / 4 && i < seed.Count / 2)
                        avaliacao.Add(seed[i]);
                    if (i >= seed.Count / 2)
                        teste.Add(seed[i]);
                }
            }

            int numClasses = 3;
            int k = 1;
            int acertos = 0;           

            List<List<Seed>> trainList = new List<List<Seed>>();

            int qtdTreinos = 200;
            int[] contador = new int[qtdTreinos];

            for (var i = 0; i < qtdTreinos; i++)
            {
                var g = 0;
                avaliacao = avaliacao.OrderBy(a => Guid.NewGuid()).ToList();
                trainData = trainData.OrderBy(a => Guid.NewGuid()).ToList();
                foreach (var semente in avaliacao)
                {                    
                    int seed = Seed.Classify(semente.Features, trainData, numClasses, k);

                    if (semente.Class != seed)
                    {
                        if (semente.Trocar == false && trainData[g].Trocar == false)
                        {
                            trainData[g] = avaliacao.Where(x => x.Class == trainData[g].Class && x.Trocar == false).FirstOrDefault();
                            semente.Trocar = true;
                            trainData[g].Trocar = true;
                            contador[i]++;
                        }
                    }
                    g++;
                }
                trainList.Add(trainData);
             }
            
            int bestClass = int.MaxValue;
            int theChosenOne = 0;

            for (var x = 0; x < qtdTreinos; x++)
            {
                if (contador[x] < bestClass)
                {
                    bestClass = contador[x];
                    theChosenOne = x;
                }
            }

            var bestTrainData = trainList[theChosenOne];

            foreach (Seed seed in teste)
            {
                int semente = Seed.Classify(seed.Features, bestTrainData, numClasses, k);

                if (seed.Class == semente)
                    acertos++;
                Console.WriteLine(Seed.IndexToClass(semente));
            }

            Console.WriteLine($"\nTotal de testes: {teste.Count}");
            Console.WriteLine($"\nTotal de acertos: {acertos}");
            Console.WriteLine($"\nPorcentagem de acertos: {100 * acertos / teste.Count}%");
            Console.WriteLine("");
        }
    }
}


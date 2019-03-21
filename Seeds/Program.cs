using System;
using System.Collections.Generic;

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
                for(var i = 0; i < seed.Count; i++)
                {
                    if (i < seed.Count / 4 )
                        trainData.Add(seed[i]);
                    if (i > seed.Count / 4 && i < seed.Count / 2)
                        avaliacao.Add(seed[i]);
                    if(i >= seed.Count / 2)
                        teste.Add(seed[i]);
                }
            }                    

            int numClasses = 3;
            int k = 3;
            int acertos = 0;
            int j = 1;

            List<List<Seed>> trainList = new List<List<Seed>>();

            int qtdTreinos = 10;
            int[] contador = new int[qtdTreinos];

           for (var i = 0; i < qtdTreinos; i++)
            {
                for (int g = 0; g < avaliacao.Count; g++)
                {
                    int seed = Seed.Classify(avaliacao[g].Features, trainData, numClasses, k);
                    if (avaliacao[g].Class != seed)
                    {                        
                        if(trainData[g].Trocar == false)
                        {
                            trainData[g].Trocar = true;
                            var aux  = trainData[g];
                            trainData[g] = avaliacao[g];

                            if (avaliacao[g].Trocar == false)
                            {
                                avaliacao[g] = aux;
                                avaliacao[g].Trocar = true;
                            }
                            contador[i]++;
                        }
                    }
                }
                trainList.Add(trainData);
            }

            int bestClass = int.MaxValue;
            int theChosenOne = 0;

            for (var i = 0; i < qtdTreinos; i++)
            {
                if(contador[i] < bestClass )
                {
                    bestClass = contador[i];
                    theChosenOne = i;
                }
            }

            var bestTrainData = trainList[theChosenOne];

            foreach (Seed seed in teste)
            {
                int semente = Seed.Classify(seed.Features, bestTrainData, numClasses, k);

                if (seed.Class == semente)          
                    acertos++;
            
                Console.WriteLine(j++ + " - " + Seed.IndexToClass(semente));
            }

            Console.WriteLine($"\nTotal de testes: {teste.Count}");
            Console.WriteLine($"\nTotal de acertos: {acertos}" );
            Console.WriteLine($"\nPorcentagem de acertos: {100 * acertos / teste.Count}%");
            Console.WriteLine("");
        }
    }
}

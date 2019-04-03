using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Classificadores
{
    class Program
    {
        static void Main(string[] args)
        {

            var media = new int[30];
            var tamanhoTestes = 0;

            for (var knn = 0; knn < media.Length; knn++)
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
                tamanhoTestes = teste.Count();


                int numClasses = 3;
                int k = 1;                

                List<List<Seed>> trainList = new List<List<Seed>>();

                int[] treinos = new int[10];

                for (var i = 0; i < treinos.Length; i++)
                {
                    var g = 0;
                    foreach (var semente in avaliacao)
                    {
                        int seed = Seed.Classify(semente.Features, trainData, numClasses, k, false);

                        if (semente.Class != seed)
                        {
                            trainData[g].Trocar = true;
                            treinos[i]++;
                        }
                        g++;
                    }

                    for (var jj = 0; jj < avaliacao.Count; jj++)
                    {
                        if (trainData[jj].Trocar == true && trainData[jj].Trocado == false)
                        {
                            var listaDeClasses = avaliacao.Where(c => c.Class == trainData[jj].Class && c.Trocado == false).ToList();
                            listaDeClasses.OrderBy(a => Guid.NewGuid()).ToList();
                            var aux = listaDeClasses.FirstOrDefault();
                            var auxAvaliacao = trainData[jj];
                            trainData[jj] = aux;
                            aux = auxAvaliacao;
                            listaDeClasses.ElementAt(0).Features = aux.Features;
                            trainData[jj].Trocado = true;
                            listaDeClasses.ElementAt(0).Trocado = true;
                        }
                    }

                    for (var ij = 0; ij < avaliacao.Count; ij++)
                    {
                        trainData[ij].Trocar = false;
                        trainData[ij].Trocado = false;
                        avaliacao[ij].Trocar = false;
                        avaliacao[ij].Trocado = false;
                    }
                    trainList.Add(trainData);
                }

                int bestClass = int.MaxValue;
                int theChosenOne = 0;

                for (var x = 0; x < treinos.Length; x++)
                {
                    if (treinos[x] < bestClass)
                    {
                        bestClass = treinos[x];
                        theChosenOne = x;
                    }
                }

                var bestTrainData = new List<Seed>();
                bestTrainData = trainList[theChosenOne];

                int acertos = 0;
                foreach (Seed seed in teste)
                {
                    int semente = Seed.Classify(seed.Features, bestTrainData, numClasses, k, false);

                    if (seed.Class == semente)
                        acertos++;
                }
                media[knn] = acertos;
            }

            var mediaManhattan = media.Sum() / media.Length;
            var desvioaPadraoManhattan = Math.Round(StandardDeviation.Calc(media), 1);
            Console.WriteLine("\nDistância Manhattan\n");
            Console.WriteLine($"\nPorcentagem média de acertos: {100 * mediaManhattan / tamanhoTestes}%");
            Console.WriteLine("Média de acertos: " + mediaManhattan + "/" + tamanhoTestes);
            Console.WriteLine("Desvio Padrão: " + desvioaPadraoManhattan);


            media = new int[30];

            for (var knn = 0; knn < media.Length; knn++)
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
                tamanhoTestes = teste.Count();


                int numClasses = 3;
                int k = 1;

                List<List<Seed>> trainList = new List<List<Seed>>();

                int[] treinos = new int[10];

                for (var i = 0; i < treinos.Length; i++)
                {
                    var g = 0;
                    foreach (var semente in avaliacao)
                    {
                        int seed = Seed.Classify(semente.Features, trainData, numClasses, k, true);

                        if (semente.Class != seed)
                        {
                            trainData[g].Trocar = true;
                            treinos[i]++;
                        }
                        g++;
                    }

                    for (var jj = 0; jj < avaliacao.Count; jj++)
                    {
                        if (trainData[jj].Trocar == true && trainData[jj].Trocado == false)
                        {
                            var listaDeClasses = avaliacao.Where(c => c.Class == trainData[jj].Class && c.Trocado == false).ToList();
                            listaDeClasses.OrderBy(a => Guid.NewGuid()).ToList();
                            var aux = listaDeClasses.FirstOrDefault();
                            var auxAvaliacao = trainData[jj];
                            trainData[jj] = aux;
                            aux = auxAvaliacao;
                            listaDeClasses.ElementAt(0).Features = aux.Features;
                            trainData[jj].Trocado = true;
                            listaDeClasses.ElementAt(0).Trocado = true;
                        }
                    }

                    for (var ij = 0; ij < avaliacao.Count; ij++)
                    {
                        trainData[ij].Trocar = false;
                        trainData[ij].Trocado = false;
                        avaliacao[ij].Trocar = false;
                        avaliacao[ij].Trocado = false;
                    }
                    trainList.Add(trainData);
                }

                int bestClass = int.MaxValue;
                int theChosenOne = 0;

                for (var x = 0; x < treinos.Length; x++)
                {
                    if (treinos[x] < bestClass)
                    {
                        bestClass = treinos[x];
                        theChosenOne = x;
                    }
                }

                var bestTrainData = new List<Seed>();
                bestTrainData = trainList[theChosenOne];

                int acertos = 0;
                foreach (Seed seed in teste)
                {
                    int semente = Seed.Classify(seed.Features, bestTrainData, numClasses, k, true);

                    if (seed.Class == semente)
                        acertos++;
                }
                media[knn] = acertos;
            }

            var mediaEuclidiana = media.Sum() / media.Length;
            var desvioaPadraoEuclidiana = Math.Round(StandardDeviation.Calc(media), 1);
            Console.WriteLine("\nDistância Euclidiana\n");
            Console.WriteLine($"\nPorcentagem média de acertos: {100 * mediaEuclidiana / tamanhoTestes}%");
            Console.WriteLine("Média de acertos: " + mediaEuclidiana + "/" + tamanhoTestes);
            Console.WriteLine("Desvio Padrão: " + desvioaPadraoEuclidiana);
            Console.WriteLine("O melhor classificador é: ");

            Console.ReadLine();



        }    
            
    }
}


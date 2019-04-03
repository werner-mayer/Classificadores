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
                List<Flower> trainData = new List<Flower>();
                List<Flower> avaliacao = new List<Flower>();
                List<Flower> teste = new List<Flower>();
                List<List<Flower>> Flowerzinhas = Flower.GetFlowerLists();

                foreach (var Flower in Flowerzinhas)
                {
                    for (var i = 0; i < Flower.Count; i++)
                    {
                        if (i < Flower.Count / 4)
                            trainData.Add(Flower[i]);
                        if (i > Flower.Count / 4 && i < Flower.Count / 2)
                            avaliacao.Add(Flower[i]);
                        if (i > Flower.Count / 2)
                            teste.Add(Flower[i]);
                    }
                }
                tamanhoTestes = teste.Count();


                int numClasses = 3;
                int k = 3;

                List<List<Flower>> trainList = new List<List<Flower>>();

                int[] treinos = new int[20];

                for (var i = 0; i < treinos.Length; i++)
                {
                    var g = 0;
                    foreach (var florre in avaliacao)
                    {                     
                        string flore = Flower.Classify(florre.Features, trainData, numClasses, k, false);

                        if (florre.Class != Flower.GetFlowerCode(flore))
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
                            var listaDeClasses = avaliacao.Where(c => c.Class == trainData[jj].Class && c.Trocado == false);
                            if (listaDeClasses.Any())
                            {
                                var aux = listaDeClasses.FirstOrDefault();
                                var auxAvaliacao = trainData[jj];
                                trainData[jj] = aux;
                                aux = auxAvaliacao;
                                trainData[jj].Trocado = true;
                                listaDeClasses.ElementAt(0).Features = aux.Features;
                                listaDeClasses.ElementAt(0).Trocado = true;
                            }
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

                var bestTrainData = new List<Flower>();
                bestTrainData = trainList[theChosenOne];

                int acertos = 0;
                foreach (Flower florrrr in teste)
                {
                    string flore = Flower.Classify(florrrr.Features, bestTrainData, numClasses, k, false);

                    if (florrrr.Class == Flower.GetFlowerCode(flore))
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
                List<Flower> trainData = new List<Flower>();
                List<Flower> avaliacao = new List<Flower>();
                List<Flower> teste = new List<Flower>();
                List<List<Flower>> Flowerzinhas = Flower.GetFlowerLists();

                foreach (var Flower in Flowerzinhas)
                {
                    for (var i = 0; i < Flower.Count; i++)
                    {
                        if (i < Flower.Count / 4)
                            trainData.Add(Flower[i]);
                        if (i > Flower.Count / 4 && i < Flower.Count / 2)
                            avaliacao.Add(Flower[i]);
                        if (i > Flower.Count / 2)
                            teste.Add(Flower[i]);
                    }
                }
                tamanhoTestes = teste.Count();


                int numClasses = 3;
                int k = 3;

                List<List<Flower>> trainList = new List<List<Flower>>();

                int[] treinos = new int[20];

                for (var i = 0; i < treinos.Length; i++)
                {
                    var g = 0;
                    foreach (var florrr in avaliacao)
                    {
                        string flore = Flower.Classify(florrr.Features, trainData, numClasses, k, true);

                        if (florrr.Class != Flower.GetFlowerCode(flore))
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
                            var listaDeClasses = avaliacao.Where(c => c.Class == trainData[jj].Class && c.Trocado == false);
                            if (listaDeClasses.Any())
                            {
                                var aux = listaDeClasses.FirstOrDefault();
                                var auxAvaliacao = trainData[jj];
                                trainData[jj] = aux;
                                aux = auxAvaliacao;
                                trainData[jj].Trocado = true;
                                listaDeClasses.ElementAt(0).Features = aux.Features;
                                listaDeClasses.ElementAt(0).Trocado = true;
                            }
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

                var bestTrainData = new List<Flower>();
                bestTrainData = trainList[theChosenOne];

                int acertos = 0;
                foreach (Flower bal in teste)
                {
                    string balanco = Flower.Classify(bal.Features, bestTrainData, numClasses, k, true);

                    if (bal.Class == Flower.GetFlowerCode(balanco))
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

            Console.ReadLine();
        }
    }
}

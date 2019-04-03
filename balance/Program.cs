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
                List<Balance> trainData = new List<Balance>();
                List<Balance> avaliacao = new List<Balance>();
                List<Balance> teste = new List<Balance>();
                List<List<Balance>> Balancezinhas = Balance.GetBalanceList();

                foreach (var Balance in Balancezinhas)
                {
                    for (var i = 0; i < Balance.Count; i++)
                    {
                        if (i < Balance.Count / 4)
                            trainData.Add(Balance[i]);
                        if (i > Balance.Count / 4 && i <= Balance.Count / 2)
                            avaliacao.Add(Balance[i]);
                        if (i > Balance.Count / 2)
                            teste.Add(Balance[i]);
                    }
                }
                tamanhoTestes = teste.Count();


                int numClasses = 3;
                int k = 19;

                List<List<Balance>> trainList = new List<List<Balance>>();

                int[] treinos = new int[10];

                for (var i = 0; i < treinos.Length; i++)
                {
                    var g = 0;
                    foreach (var bal in avaliacao)
                    {
                        string balance = Balance.Classify(bal.Features, trainData, numClasses, k, false);

                        if (bal.Class != Balance.GetBalanceCode(balance))
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

                var bestTrainData = new List<Balance>();
                bestTrainData = trainList[theChosenOne];

                int acertos = 0;
                foreach (Balance bal in teste)
                {
                    string balanco = Balance.Classify(bal.Features, bestTrainData, numClasses, k, false);

                    if (bal.Class == Balance.GetBalanceCode(balanco))
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
                List<Balance> trainData = new List<Balance>();
                List<Balance> avaliacao = new List<Balance>();
                List<Balance> teste = new List<Balance>();
                List<List<Balance>> Balancezinhas = Balance.GetBalanceList();

                foreach (var Balance in Balancezinhas)
                {
                    for (var i = 0; i < Balance.Count; i++)
                    {
                        if (i < Balance.Count / 4)
                            trainData.Add(Balance[i]);
                        if (i > Balance.Count / 4 && i <= Balance.Count / 2)
                            avaliacao.Add(Balance[i]);
                        if (i > Balance.Count / 2)
                            teste.Add(Balance[i]);
                    }
                }
                tamanhoTestes = teste.Count();


                int numClasses = 3;
                int k = 19;

                List<List<Balance>> trainList = new List<List<Balance>>();

                int[] treinos = new int[10];

                for (var i = 0; i < treinos.Length; i++)
                {
                    var g = 0;
                    foreach (var bal in avaliacao)
                    {
                        string balance = Balance.Classify(bal.Features, trainData, numClasses, k, true);

                        if (bal.Class != Balance.GetBalanceCode(balance))
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

                var bestTrainData = new List<Balance>();
                bestTrainData = trainList[theChosenOne];

                int acertos = 0;
                foreach (Balance bal in teste)
                {
                    string balanco = Balance.Classify(bal.Features, bestTrainData, numClasses, k, true);

                    if (bal.Class == Balance.GetBalanceCode(balanco))
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

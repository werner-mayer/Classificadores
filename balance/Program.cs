using System;
using System.Collections.Generic;
using System.Linq;

namespace Classificadores
{
    class Program
    {

        static void Main(string[] args)
        {

            List<Balance> trainData = new List<Balance>();
            List<Balance> avaliacao = new List<Balance>();
            List<Balance> teste = new List<Balance>();

            List<List<Balance>> balancezinhos = Balance.GetBalanceList();

            foreach (var Balance in balancezinhos)
            {
                for (var i = 0; i < Balance.Count; i++)
                {
                    if (i < Balance.Count / 4)
                        trainData.Add(Balance[i]);
                    if (i > Balance.Count / 4 && i <= Balance.Count / 2)
                        avaliacao.Add(Balance[i]);
                    if (i >= Balance.Count / 2)
                        teste.Add(Balance[i]);
                }
            }

            int numClasses = 3;
            int k = 97; 
            int acertos = 0;
            int j = 1;

            for (int g = 0; g < avaliacao.Count; g++)
            {
                string flor = Balance.Classify(avaliacao[g].Features, trainData, numClasses, k);
                if (avaliacao[g].Class != Balance.GetBalanceCode(flor))
                    trainData[g] = avaliacao[g];
            }

            foreach (Balance Balance in teste)
            {
                string flor = Balance.Classify(Balance.Features, trainData, numClasses, k);

                if (Balance.Class == Balance.GetBalanceCode(flor))
                    acertos++;

                Console.WriteLine(j++ + " - " + flor);
            }

            Console.WriteLine($"\nTotal de testes: {teste.Count}");
            Console.WriteLine($"\nTotal de acertos: {acertos}");
            Console.WriteLine($"\nPorcentagem de acertos: {100 * acertos / teste.Count}%");
            Console.WriteLine("");
        }
    }
}

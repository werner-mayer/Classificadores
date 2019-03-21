using System;
using System.Collections.Generic;
using System.Linq;

namespace Classificadores
{    class Program
    {
        static void Main(string[] args)
        {
            List<Banknote> trainData = new List<Banknote>();
            List<Banknote> avaliacao = new List<Banknote>();
            List<Banknote> teste = new List<Banknote>();

            List<List<Banknote>> florezinhas = Banknote.GetNoteLists();

            foreach (var Banknote in florezinhas)
            {
                for (var i = 0; i < Banknote.Count; i++)
                {
                    if (i < Banknote.Count / 4)
                        trainData.Add(Banknote[i]);
                    if (i > Banknote.Count / 4 && i < Banknote.Count / 2)
                        avaliacao.Add(Banknote[i]);
                    if (i >= Banknote.Count / 2)
                        teste.Add(Banknote[i]);
                }
            }

            int numClasses = 2;
            int k = 303;
            int acertos = 0;
            int j = 1;

            for (int g = 0; g < avaliacao.Count; g++)
            {
                int note = Banknote.Classify(avaliacao[g].Features, trainData, numClasses, k);
                if (avaliacao[g].Class != note)
                    trainData[g] = avaliacao[g];
            }

            foreach (Banknote Banknote in teste)
            {
                int note = Banknote.Classify(Banknote.Features, trainData, numClasses, k);

                if (Banknote.Class == note)
                    acertos++;

                Console.WriteLine(j++ + " - " + note);
            }

            Console.WriteLine($"\nTotal de testes: {teste.Count}");
            Console.WriteLine($"\nTotal de acertos: {acertos}");
            Console.WriteLine($"\nPorcentagem de acertos: {100 * acertos / teste.Count}%");
            Console.WriteLine("");
        }
    }
}

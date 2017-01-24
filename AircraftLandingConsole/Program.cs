using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AircraftLanding;
using System.Globalization;


namespace AircraftLanding
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">primeiro parametro: nome do arquivo; segundo parametro: num maximo iteracoes;
        /// terceiro parametro: alfa
        /// </param>
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Linha de comando correta é: AirLandingConsole [nome-do-arquivo] [numero-maximo-iteracoes] [alfa]");
                }
                else if (args.Length == 3)
                {
                    string nomeArquivo = string.Empty;
                    if (!String.IsNullOrEmpty(args[0]))
                    {
                        nomeArquivo = args[0];
                    }

                    int numeroIteracoes = Convert.ToInt32(args[1]);
                    decimal alfa = Convert.ToDecimal(args[2], new CultureInfo("en-US"));

                    AircraftLandingParser alp = new AircraftLandingParser(nomeArquivo);
                    alp.InputAirlandFile();

                    GRASP.GRASP grasp = new GRASP.GRASP();
                    grasp.DadosProblema = alp.planes;
                    AircraftLanding.GRASP.Solucao sol = grasp.grasp(numeroIteracoes, alfa);

                    Console.WriteLine("Sequencia de aterrisagens: ");
                    Console.WriteLine();
                    foreach (var item in sol.SeqAterrisagens)
                    {
                        Console.Write("Aviao: {0} - Tempo: {1}", item.Aviao, item.Tempo);
                        Console.WriteLine();
                    }
                    Console.WriteLine("Solucao: {0} ", sol.ValorSolucao);
                }
                else
                {
                    Console.WriteLine("Linha de comando correta é: AirLandingConsole [nome-do-arquivo] [numero-maximo-iteracoes] [alfa]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

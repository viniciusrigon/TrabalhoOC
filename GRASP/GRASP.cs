using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using AircraftLanding;

namespace AircraftLanding.GRASP
{
    public class Aterrisagem
    {
        private int aviao;
        public int Aviao
        {
            get { return aviao; }
            set { aviao = value; }
        }

        private int tempo;
        public int Tempo
        {
            get { return tempo; }
            set { tempo = value; }
        }
    }

    public class Candidato
    {
        private int aviao;
        public int Aviao
        {
            get { return aviao; }
            set { aviao = value; }
        }

        private int tempo;
        public int Tempo
        {
            get { return tempo; }
            set { tempo = value; }
        }

        private decimal multa;
        public decimal Multa
        {
            get { return multa; }
            set { multa = value; }
        }
    }

    public class Solucao
    {
        private List<Aterrisagem> seqAterrisagens;
        public List<Aterrisagem> SeqAterrisagens
        {
            get { return seqAterrisagens; }
            set { seqAterrisagens = value; }
        }

        private List<Candidato> listaCandidatos;
        public List<Candidato> ListaCandidatos
        {
            get { return listaCandidatos; }
            set { listaCandidatos = value; }
        }
                
        private decimal valorSolucao;
        public decimal ValorSolucao
        {
            get { return valorSolucao; }
            set { valorSolucao = value; }
        }

        public Solucao()
        {
            seqAterrisagens = new List<Aterrisagem>();
            listaCandidatos = new List<Candidato>();
            //listaRestritaCandidatos = new List<int>();
        }
        
    }

    public class GRASP
    {        
        private Random rand = new Random(0);

        private List<Plane> dadosProblema;
        public List<Plane> DadosProblema
        {
            get { return dadosProblema; }
            set { dadosProblema = value; }
        }

        private Solucao copiaSolucao(Solucao sol)
        {
            Solucao solCopia = new Solucao();

            foreach (var item in sol.SeqAterrisagens)
            {
                solCopia.SeqAterrisagens.Add(item);
            }

            foreach (var item in sol.ListaCandidatos)
            {
                solCopia.ListaCandidatos.Add(item);
            }

            solCopia.ValorSolucao = sol.ValorSolucao;
           
            return solCopia;
        }

        private Solucao buscaLocal(Solucao sol)
        {
            //Repete
            //pegar a solução(S), remover um avião aleatoriamente e substituir por outro um avião com o mesmo id

            /*calcular o valor da nova solução(S'),
                     }   se S > S' 
                      S=S'
                senão
                  descarta S'
                até S nao melhorar mais*/
            Solucao novaSolucao = new Solucao();
            int no_update = 0;
            do
            {
                int aviaoEscolhido = rand.Next(0, sol.SeqAterrisagens.Count);
                sol.SeqAterrisagens.RemoveAll(i => i.Aviao.Equals(aviaoEscolhido));

                novaSolucao = copiaSolucao(sol);
                
                Candidato candidato = sol.ListaCandidatos.FirstOrDefault(i => i.Aviao.Equals(aviaoEscolhido));
                Aterrisagem aterrisagem = new Aterrisagem();
                aterrisagem.Aviao = candidato.Aviao;
                aterrisagem.Tempo = candidato.Tempo;

                sol.ValorSolucao += candidato.Multa;
                sol.SeqAterrisagens.Add(aterrisagem);

                if (sol.ValorSolucao < novaSolucao.ValorSolucao)
                {
                    sol = copiaSolucao(novaSolucao);                    
                }
                else
                {
                    no_update++;
                    novaSolucao = null;                    
                }
                
            } while (no_update < 300);

            return sol;
           
        }
                
        private Solucao greedyRandomizedSolution(decimal alfa)
        {
            Solucao sol = new Solucao();

            // key : id do avião
            // valor : tempo de pouso
            //List<KeyValuePair<int, decimal>> listaCandidatos = new List<KeyValuePair<int, decimal>>();
            //Dictionary<int, decimal> listaCandidatos = new Dictionary<int, decimal>();

            /*
             xi: tempo de pouso
             ai: tempo pousa antes do ideal
             bi: tempo pouso depois do ideal
             ti: tempo ideal (TT)
             ei: tempo antes do pouso (ET)
             li: tempo depois do pouso (LT)
             * 
             * a[i]<=t[i] - e[i];
             * 0<=b[i]<=l[i] - t[i];
             * x[i]=t[i] - a[i] + b[i];
             1. tempo do primeiro = tempo ideal do primeiro.
             2. tempo do segundo = (> tempo ideal do segundo, 
             *  (+ tempo do primeiro, tempo de espera entre o primeiro e o segundo dado pela matriz S))
             * 
             */

            List<Candidato> listaCandidatos = new List<Candidato>();

            for (int i = 0; i < DadosProblema.Count; i++)
            {
                int tempoInicial = DadosProblema[i].ET;
                int tempoFinal = DadosProblema[i].LT;
                for (int j = tempoInicial; j < tempoFinal; j++)
                {
                    Candidato candidato = new Candidato();

                    candidato.Aviao = i;
                    candidato.Tempo = j;
                    if (candidato.Tempo < DadosProblema[i].TT)
                    {
                        decimal ai = DadosProblema[i].TT - j;
                        candidato.Multa = DadosProblema[i].pE * ai;
                    }
                    else if (candidato.Tempo > DadosProblema[i].TT)
                    {
                        decimal bi = j - DadosProblema[i].TT;
                        candidato.Multa = DadosProblema[i].pL * bi;
                    }

                    listaCandidatos.Add(candidato);
                }                
            }

            foreach (Candidato item in listaCandidatos)
            {
                sol.ListaCandidatos.Add(item);
            }

            List<int> avioesEscolhidos = new List<int>();
            while (listaCandidatos.Count > 0)
            {
                List<Candidato> listaRestritaCandidatos = criarLRC(alfa, listaCandidatos);
                bool trancaDaMatrizS = true;
                Candidato candidatoEscolhido = new Candidato();
                int aviaoEscolhido, valorMatrizS, tempoMinimo;
                while (trancaDaMatrizS)
                {
                    do
                    {
                        candidatoEscolhido = listaRestritaCandidatos[rand.Next(0, listaRestritaCandidatos.Count)];
                        aviaoEscolhido = candidatoEscolhido.Aviao;
                    } while (avioesEscolhidos.Contains(aviaoEscolhido));
                    if (avioesEscolhidos.Count > 0)
                    {
                        valorMatrizS = dadosProblema.Find(a => a.idPlane == avioesEscolhidos[avioesEscolhidos.Count - 1] + 1).S[aviaoEscolhido];
                        tempoMinimo = sol.SeqAterrisagens[sol.SeqAterrisagens.Count - 1].Tempo + valorMatrizS;
                        if (candidatoEscolhido.Tempo > tempoMinimo)
                        {
                            trancaDaMatrizS = false;
                            avioesEscolhidos.Add(aviaoEscolhido);
                        }
                        if (!listaRestritaCandidatos.Exists(c => c.Tempo > tempoMinimo && c.Aviao == aviaoEscolhido))
                        {
                            return sol;
                        }
                    }
                    else
                    {
                        trancaDaMatrizS = false;
                        avioesEscolhidos.Add(aviaoEscolhido);
                    }
                }

                Aterrisagem aterrisagem = new Aterrisagem();
                aterrisagem.Aviao = candidatoEscolhido.Aviao;
                aterrisagem.Tempo = candidatoEscolhido.Tempo;
                sol.ValorSolucao += candidatoEscolhido.Multa;
                sol.SeqAterrisagens.Add(aterrisagem);
                listaCandidatos.RemoveAll(i => i.Aviao.Equals(candidatoEscolhido.Aviao) || i.Tempo < candidatoEscolhido.Tempo);
            }


            return sol;
          
        }

        private static List<Candidato> criarLRC(decimal alfa, List<Candidato> listaCandidatos)
        {
            listaCandidatos.Sort(delegate(Candidato um, Candidato dois) { return um.Multa.CompareTo(dois.Multa); });

            List<Candidato> listaRestritaCandidatos = new List<Candidato>();
            foreach (var item in listaCandidatos)
            {
                if (listaRestritaCandidatos.Count < (listaCandidatos.Count * alfa))
                {
                    listaRestritaCandidatos.Add(item);
                }
            }
            return listaRestritaCandidatos;
        }

        public Solucao grasp(int numIter, decimal pAlfa)
        {

            Solucao solucaoInicial = new Solucao();

            solucaoInicial.ValorSolucao = decimal.MaxValue;

            Solucao solucao = new Solucao();

            int num_iter = 0;
            
            while (num_iter <= numIter)
            {
                solucao = greedyRandomizedSolution(pAlfa);
                solucao = buscaLocal(solucao);
                if (solucao.ValorSolucao < solucaoInicial.ValorSolucao && solucao.SeqAterrisagens.Count == dadosProblema.Count)
                {                    
                    solucaoInicial.ValorSolucao = solucao.ValorSolucao;
                }

                num_iter++;
            }

            return solucao;

        }
    }
}

using Pastel;
using System;
using System.Collections.Generic;

namespace AS2021_4H_SIR_BartoliniLiam_Hamming
{
    class Program
    {
        static List<char> sequenza;
        static int nSpazi = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Bartolini Liam, Hamming");

            string strInviato;
            bool isValid = true;
            do
            {
                Console.Write("Inserisci una parola (solo 0 e 1): ");
                strInviato = Console.ReadLine();

                // Controllo l'Input
                for (int i = 0; i < strInviato.Length; i++)
                {
                    if (strInviato[i] == '0' || strInviato[i] == '1')
                        isValid = true;
                    else
                    {
                        isValid = false;
                        Console.WriteLine("Input errato sono accettati solamente 0 e 1".Pastel("#FF0000"));
                        break;
                    }
                }

            } while (!isValid);

            InserimentoSpazi(strInviato);
            for (int i = 0; i < nSpazi; i++)
                InserisciBitParita((int)Math.Pow(2, i));
            Console.WriteLine($"Codifica di Hamming della parola in ingresso:\n{Stampa(sequenza)}");

            Console.Write("\nInserisci la parola ricevuta (comprensiva di codice di Hamming), inserendovi al massimo un errore: ");
            string strRicevuto = Console.ReadLine();

            int ris = Comparazione(strRicevuto);
            Console.WriteLine($"Il bit sbagliato si trova alla posizione {ris}");
            Console.WriteLine($"Inviato:\t\t\t\t" + $"{Stampa(ris - 1, ls: sequenza)}");
            Console.WriteLine($"Ricevuto:\t\t\t\t" + $"{Stampa(ris - 1, s: strRicevuto)}");
            SistemazioneErrore(ris - 1, ref strRicevuto);
            Console.WriteLine($"Ricostruzione della parola corretta:\t" + $"{Stampa(ris - 1, s: strRicevuto)}");

        }

        /// <summary>
        /// Campara le stringhe, quella inviata con quella ricevuta
        /// </summary>
        /// <param name="ricevuto"></param>
        /// <returns>Ritorna -1 se le stringhe hanno più di un errore, 0 se non ci sono errori, altrimenti ritorna la posizione dell'errore</returns>
        static int Comparazione(string ricevuto)
        {
            int retVal = 0;

            for (int i = 0; i < nSpazi; i++)
                if (!CalcoloParita((int)Math.Pow(2, i), ricevuto))
                    retVal += (int)Math.Pow(2, i);

            return retVal;
        }

        static void SistemazioneErrore(int pos, ref string s)
        {
            char[] chared = s.ToCharArray();
            if (chared[pos] == '1')
                chared[pos] = '0';
            else
                chared[pos] = '1';

            s = new string(chared);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <returns>True se il numero di bit è pari altrimenti ritorna false</returns>
        static bool CalcoloParita(int start, string msg)
        {
            int cont = 0;
            var msgLs = new List<char>(msg.ToCharArray());

            for (int i = start - 1; i < msgLs.Count && i < i + start; i += start * 2)
                for (int j = i; j < i + start && j < msgLs.Count; j++)
                    if (msgLs[j] == '1') cont++;

            return cont % 2 == 0;
        }

        /// <summary>
        /// Inserisce li spazi nelle posizioni delle parità
        /// </summary>
        /// <param name="strInviato"></param>
        static void InserimentoSpazi(string strInviato)
        {
            sequenza = new List<char>(strInviato.ToCharArray());
            int esp = 0;
            for (int i = 0; i < sequenza.Count; i++)
                if (i == (int)Math.Pow(2, esp) - 1)
                {
                    sequenza.Insert(i, '_');
                    nSpazi++;
                    esp++;
                }
        }

        /// <summary>
        /// Conta il numero di bit a 1
        /// </summary>
        /// <param name="start">Numero del bit di parità one-based</param>
        /// <returns>Il numero di bit a 1</returns>
        static void InserisciBitParita(int start)
        {
            int cont = 0;

            for (int i = start - 1; i < sequenza.Count; i += start * 2)
            {
                //Console.WriteLine("indice: " + i);
                for (int j = i; j < i + start && j < sequenza.Count; j++)
                {
                    if (sequenza[j] == '1')
                        cont++;
                    //Console.WriteLine(sequenza[j]);
                }
            }
            InserisciParita(cont);
        }

        static void InserisciParita(int cont)
        {
            if (cont % 2 == 0)
                sequenza[sequenza.IndexOf('_')] = '0';
            else
                sequenza[sequenza.IndexOf('_')] = '1';
        }

        static string Stampa(List<char> ls)
        {
            string s = "";
            int cont = 0;
            for (int i = 0; i < ls.Count; i++)
            {
                if (i == (int)Math.Pow(2, cont) - 1)
                {
                    s += $"{ls[i]} ".Pastel("#00FF00");
                    cont++;
                }
                else
                    s += $"{ls[i]} ";
            }

            return s;
        }

        static string Stampa(int pos, string s = "", List<char> ls = null)
        {
            string toPrint = "";
            int cont = 0;
            if (ls != null)
            {
                for (int i = 0; i < ls.Count; i++)
                {
                    // Coloro i bit di parità
                    if (i == (int)Math.Pow(2, cont) - 1)
                    {
                        if (i == pos)
                            toPrint += $"{ls[i]} ".Pastel("#FF0000");
                        else
                            toPrint += $"{ls[i]} ".Pastel("#24fc03");
                        cont++;
                        continue;
                    }

                    toPrint += $"{ls[i]} ";
                }
            }
            else if (s != "")
            {
                for (int i = 0; i < s.Length; i++)
                {
                    // Coloro i bit di parità
                    if (i == (int)Math.Pow(2, cont) - 1)
                    {
                        if (i == pos)
                            toPrint += $"{s[i]} ".Pastel("#FF0000");
                        else
                            toPrint += $"{s[i]} ".Pastel("#24fc03");

                        cont++;
                        continue;
                    }

                    toPrint += $"{s[i]} ";
                }
            }

            return toPrint;
        }
    }
}
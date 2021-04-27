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

            string strInviato = ControlloInput("Inserisci una parola (solo 0 e 1): ");
            Console.WriteLine($"Codifica di Hamming della parola in ingresso:\n{CalcoloHamming(strInviato)}");
            string strRicevuto = ControlloInput("Inserisci la parola ricevuta (comprensiva di codice di Hamming), inserendovi al massimo un errore: ");
            int ris = Comparazione(strRicevuto);

            Console.WriteLine($"Il bit sbagliato si trova alla posizione {ris}");
            Console.WriteLine($"Inviato:\t\t\t\t" + $"{Stampa(ris, ls: sequenza)}");
            Console.WriteLine($"Ricevuto:\t\t\t\t" + $"{Stampa(ris, s: strRicevuto)}");
            
            // Sistemo l'errore passando la stringa per riferimento
            string strRicevutoFixed = SistemazioneErrore(ris, strRicevuto);
            Console.WriteLine($"Ricostruzione della parola corretta:\t" + $"{Stampa(ris, s: strRicevutoFixed)}");
        }

        /// <summary>
        /// Permette di controllare che l'input sia composto solo da 0 e 1
        /// </summary>
        /// <param name="msg">Messaggio da visualizzare</param>
        /// <returns>Una stringa contente l'input</returns>
        static string ControlloInput(string msg)
        {
            string strInput;
            bool isValid = true;
            do
            {
                Console.Write(msg);
                strInput = Console.ReadLine();

                // Controllo l'Input
                for (int i = 0; i < strInput.Length; i++)
                {
                    if (strInput[i] == '0' || strInput[i] == '1')
                        isValid = true;
                    else
                    {
                        isValid = false;
                        Console.WriteLine("Input errato sono accettati solamente 0 e 1".Pastel("#FF0000"));
                        break;
                    }
                }

            } while (!isValid);

            return strInput;
        }

        /// <summary>
        /// Calcola hamming per la stringa passata
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Ritorna la string in ingresso con hamming</returns>
        static string CalcoloHamming(string s)
        {
            /*
             * InserimentoSpazi(strInviato);
            for (int i = 0; i < nSpazi; i++)
                InserisciBitParita((int)Math.Pow(2, i));
            Console.WriteLine($"Codifica di Hamming della parola in ingresso:\n{Stampa(sequenza)}");
             */
            InserimentoSpazi(s);
            for (int i = 0; i < nSpazi; i++)
                InserisciBitParita((int)Math.Pow(2, i));
            return Stampa(sequenza);
        }

        /// <summary>
        /// Campara le stringhe, quella inviata con quella ricevuta
        /// </summary>
        /// <param name="ricevuto"></param>
        /// <returns>Ritorna -1 se le stringhe hanno più di un errore, 0 se non ci sono errori, altrimenti ritorna la posizione dell'errore</returns>
        static int Comparazione(string ricevuto)
        {
            int retVal = -1;

            for (int i = 0; i < nSpazi; i++)
                if (!CalcoloParita((int)Math.Pow(2, i), ricevuto))
                    retVal += (int)Math.Pow(2, i);

            if (retVal != -1) retVal += 1;
            return retVal;
        }

        static string SistemazioneErrore(int pos, string s)
        {
            if (pos == -1) return s;

            pos -= 1; // Lo porto 0-based per poterlo usare con i vettori
            char[] chared = s.ToCharArray();
            if (chared[pos] == '1')
                chared[pos] = '0';
            else
                chared[pos] = '1';

            return new string(chared);
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
        /// <param name="s">Stringa alla quale aggiungere i bit di parità</param>
        static void InserimentoSpazi(string s)
        {
            sequenza = new List<char>(s.ToCharArray());
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
                for (int j = i; j < i + start && j < sequenza.Count; j++)
                    if (sequenza[j] == '1')
                        cont++;

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

        /// <summary>
        /// Stampa l'eventuale errore all'interno della lista/string
        /// </summary>
        /// <param name="pos">Indice dell'errore 1-based</param>
        /// <param name="s">Evenutale stringa</param>
        /// <param name="ls">Evenutale lista da stampare</param>
        /// <returns>Una stringa con i bit colorati nelle posizioni importanti</returns>
        static string Stampa(int pos, string s = "", List<char> ls = null)
        {
            string toPrint = "";
            pos -= 1; // Porto la posizioni in 0-based
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
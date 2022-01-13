//GIOCO PACMAN
//Si sviluppi in C# il gioco PacMan (che potete scaricare e provare dai vari play store) senza
//implementare l’interfaccia grafica ma semplicemente usando la console.
//Il gioco viene eseguito su una griglia (a voi la scelta della dimensione del labirinto) e vengono
//utilizzate le frecce direzionali della tastiera per indicare la direzione di movimento del PacMan.
//Prevedere 3 livelli di gioco:
//1.Facile: 3 fantasmi a velocità media
//2. Intermedio: 3 fantasmi a velocità alta
//3. Difficile: 3 fantasmi a velocità alta e intelligenti (che si muovono in base alla posizione del
//PacMan).
//Ogni utente prima di giocare deve inserire il proprio nome.
//Punteggio, nome e livello raggiunto dal giocatore vengono salvati su un file di testo.
//Visualizzare su console:
//1.Punteggio corrente
//2.Vite a disposizione
//3. Record corrente 


using System;
using System.Threading;
using System.Collections.Generic;
using PACMAN.Scene;
using PACMAN.Musica;
using System.Text;
using System.IO;

namespace PACMAN
{
    internal class PACMAN
    {
        #region VARIABILI
        const String Muro = "X";
        const String PacManDX = "\u15e7";
        const String PacManSX = "ᗤ";
        static String ActivePacMan = new String(PacManDX);
        const String FantasmaChar = "ᗣ";
        const String Bonus = "ò";
        const String Pallino = "·";
        const String Mangia = "•";
        static object _lock = new object();
        static int punti = 0;
        static bool pacmanMangiato = false;
        static int difficoltà = 0;
        static List<int[]> posizioniFantasmi = new List<int[]>()
        {
            new int[2] { 15, 8 },
            new int[2] { 15, 21 },
            new int[2] { 16, 8 },
            new int[2] { 16, 21 },
        };
        static List<int[]> posizioniFantasmiIniziali = new List<int[]>()
        {
            new int[2] { 15, 8 },
            new int[2] { 15, 21 },
            new int[2] { 16, 8 },
            new int[2] { 16, 21 },
        };
        static int[] posizione = new int[2] { 1, 1 };
        static String[,] griglia = new String[30, 30]
        {
            { "X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X"},//0
            { "X"," ","","","","","","","","","","","","","","X","","","","","","","","","","","","","","X",},//1
            { "X","","","","","","","","","","","","","","","X","","","","","","","","","","","","","","X",},//2
            { "X","","","X","X","X","","","","X","X","X","","","","","","","X","X","X","","","","X","X","X","","","X",},//3
            { "X","","","X","X","X","","","","X","X","X","","","","","","","X","X","X","","","","X","X","X","","","X",},//4
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//5
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//6
            { "X","","","X","X","X","","","","X","X","°","","","","","","","°","X","X","","","","X","X","X","","","X",},//7
            { "X","","","","","","","","","X","X","X","X","","","","","X","X","X","X","","","","","","","","","X",},//8
            { "X","","","","","","","","","X","X","","","","","","","","","X","X","","","","","","","","","X",},//9
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//10
            { "X","X","X","","","","","","","","","","","","","","","","","","","","","","","","","X","X","X",},//11
            { " "," ","X","","","","","","","","","","","","","","","","","","","","","","","","","X"," "," ",},//12
            { " "," ","X","","","","","","","","","X","X","X","X","X","X","X","X","","","","","","","","","X"," "," ",},//13
            { "X","X","X","","","","","","","","","X","X","X","X","X","X","X","X","","","","","","","","","X","X","X",},//14
            { " "," "," ","","","","","","","","","X","X","X","X","X","X","X","X","","","","","","","",""," "," "," ",},//15
            { "X","X","X","","","X","X","","","","","X","X","X","X","X","X","X","X","","","","","X","X","","","X","X","X",},//16
            { " "," ","X","","","X","X","","","","","X","X","X","X","X","X","X","X","","","","","X","X","","","X"," "," ",},//17
            { " "," ","X","","","X","X","","","","","","","","","","","","","","","","","X","X","","","X"," "," ",},//18
            { "X","X","X","","","","","","","","","","","","","","","","","","","","","","","","","X","X","X",},//19
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//20
            { "X","","X","X","X","","","","","","","","","X","X","X","X","X","","","","","","","","X","X","X","","X",},//21
            { "X","","","","X","","","","","","","","","","","X","","","","","","","","","","X","","","","X",},//22
            { "X","","","","X","","","","","","","","","","","X","","","","","","","","","","X","","","","X",},//23
            { "X","X","X","","","","","","","","","","","","","","","","","","","","","","","","","X","X","X",},//24
            { "X","","","","","","","","","","","","","X","X","X","X","X","","","","","","","","","","","","X",},//25
            { "X","","","","","°","X","","","","","","","","","X","","","","","","","","X","°","","","","","X",},//26
            { "X","","X","X","X","X","X","X","","","","","","","","X","","","","","","","X","X","X","X","X","X","","X",},//27
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//28
            { "X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X"},//29
        };
        static String[,] grigliainiziale = new String[30, 30]
        {
            { "X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X"},//0
            { "X"," ","","","","","","","","","","","","","","X","","","","","","","","","","","","","","X",},//1
            { "X","","","","","","","","","","","","","","","X","","","","","","","","","","","","","","X",},//2
            { "X","","","X","X","X","","","","X","X","X","","","","","","","X","X","X","","","","X","X","X","","","X",},//3
            { "X","","","X","X","X","","","","X","X","X","","","","","","","X","X","X","","","","X","X","X","","","X",},//4
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//5
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//6
            { "X","","","X","X","X","","","","X","X","°","","","","","","","°","X","X","","","","X","X","X","","","X",},//7
            { "X","","","","","","","","","X","X","X","X","","","","","X","X","X","X","","","","","","","","","X",},//8
            { "X","","","","","","","","","X","X","","","","","","","","","X","X","","","","","","","","","X",},//9
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//10
            { "X","X","X","","","","","","","","","","","","","","","","","","","","","","","","","X","X","X",},//11
            { " "," ","X","","","","","","","","","","","","","","","","","","","","","","","","","X"," "," ",},//12
            { " "," ","X","","","","","","","","","X","X","X","X","X","X","X","X","","","","","","","","","X"," "," ",},//13
            { "X","X","X","","","","","","","","","X","X","X","X","X","X","X","X","","","","","","","","","X","X","X",},//14
            { " "," "," ","","","","","","","","","X","X","X","X","X","X","X","X","","","","","","","",""," "," "," ",},//15
            { "X","X","X","","","X","X","","","","","X","X","X","X","X","X","X","X","","","","","X","X","","","X","X","X",},//16
            { " "," ","X","","","X","X","","","","","X","X","X","X","X","X","X","X","","","","","X","X","","","X"," "," ",},//17
            { " "," ","X","","","X","X","","","","","","","","","","","","","","","","","X","X","","","X"," "," ",},//18
            { "X","X","X","","","","","","","","","","","","","","","","","","","","","","","","","X","X","X",},//19
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//20
            { "X","","X","X","X","","","","","","","","","X","X","X","X","X","","","","","","","","X","X","X","","X",},//21
            { "X","","","","X","","","","","","","","","","","X","","","","","","","","","","X","","","","X",},//22
            { "X","","","","X","","","","","","","","","","","X","","","","","","","","","","X","","","","X",},//23
            { "X","X","X","","","","","","","","","","","","","","","","","","","","","","","","","X","X","X",},//24
            { "X","","","","","","","","","","","","","X","X","X","X","X","","","","","","","","","","","","X",},//25
            { "X","","","","","°","X","","","","","","","","","X","","","","","","","","X","°","","","","","X",},//26
            { "X","","X","X","X","X","X","X","","","","","","","","X","","","","","","","X","X","X","X","X","X","","X",},//27
            { "X","","","","","","","","","","","","","","","","","","","","","","","","","","","","","X",},//28
            { "X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X","X"},//29
        };
        static string[,] grigliadef = new string[30, 30];
        static bool mangiabili = false;
        static int numerovite = 3;
        static bool fine = false;
        static int livello = 1;
        static int bonusMangiati = 0;
        static int fantasmiMangiati = 0;
        const string path = "../../../data.csv";
        #endregion
        #region GESTIONE GRIGLIA
        static void GeneraGriglia()
        {
            Random r = new Random();
            int nBonus = 0;
            for (int i = 0; i < griglia.GetLength(0); i++)
            {
                for (int j = 0; j < griglia.GetLength(1); j++)
                {
                    if (griglia[i, j] == "")
                    {
                        if (r.Next(1, 600) == 265 && bonusMangiati < 4 && nBonus < 4)
                        {
                            griglia[i, j] = Bonus;
                            nBonus++;
                        }
                        else
                        {
                            griglia[i, j] = Pallino;
                        }
                    }
                    else if (griglia[i, j].Equals("X"))
                    {
                        griglia[i, j] = Muro;
                    }
                    else if (griglia[i, j].Equals("°"))
                    {
                        griglia[i, j] = Mangia;
                    }
                }
            }
            for (int i = 0; i < grigliadef.GetLength(0); i++)
            {
                for (int j = 0; j < grigliadef.GetLength(1); j++)
                {
                    if (griglia[i, j].Equals(Pallino) || griglia[i, j].Equals(Bonus) || griglia[i, j].Equals(Mangia))
                    {
                        grigliadef[i, j] = Pallino;
                    }
                    else
                    {
                        grigliadef[i, j] = " ";
                    }
                }
            }
        }
        static void StampaGriglia()
        {
            for (int i = 0; i < griglia.GetLength(0); i++)
            {
                for (int j = 0; j < griglia.GetLength(1); j++)
                {
                    switch (griglia[i, j])
                    {
                        case Muro:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;
                        case FantasmaChar:
                            if (!mangiabili)
                                Console.ForegroundColor = ConsoleColor.Magenta;
                            else
                                Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case PacManDX:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case PacManSX:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Bonus:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case Mangia:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                    }
                    Console.Write($"{griglia[i, j]} ");
                }
                Console.WriteLine();
            }
        }
        static void AggiornaGriglia(object obj)
        {
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.UTF8;
            Giocatore g = obj as Giocatore;
            int n = 1;
            Sottofondo s = new Sottofondo(ref fine);
            new Thread(s.Play).Start();
            while (!fine)
            {
                if (pacmanMangiato)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(Morto.Stringa);
                    Console.ReadLine();
                    lock (_lock)
                    {
                        pacmanMangiato = false;
                    }
                    Console.Clear();
                }
                lock (_lock)
                {
                    if (punti - n * 10000 > 0)
                    {
                        n++;
                        numerovite++;
                    }
                    if (numerovite == 0)
                    {
                        fine = true;
                    }
                    if (FineLivello())
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.Clear();
                        Console.WriteLine($"Livello superato con successo!");
                        livello++;
                        lock (_lock)
                        {
                            griglia = grigliainiziale;
                            posizioniFantasmi = posizioniFantasmiIniziali;
                            posizione = new int[2] { 0, 0 };
                        }
                        Thread.Sleep(2000);
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                lock (_lock)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"{g.Nome} - Punti: {punti} - Vite: {numerovite} - Livello: {livello}");
                    StampaGriglia();
                }
                Thread.Sleep(100);
            }
            Console.Clear();
            if (g.Record < punti)
            {
                Console.WriteLine($"Nuovo record {punti}!!!");
                g.Record = punti;
                AggiornaRecord(g);
                Thread.Sleep(2000);
            }
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Fine.Stringa);
            Console.Read();
        }
        #endregion
        #region GESTIONE EVENTI E CONTROLLI NEL GIOCO
        static void GestisciMangiabili()
        {
            lock (_lock)
            {
                mangiabili = true;
            }
            Thread.Sleep(new Random().Next(7000, 10000));
            lock (_lock)
            {
                mangiabili = false;
                fantasmiMangiati = 0;
            }
        }
        static bool FineLivello()
        {
            for (int i = 0; i < griglia.GetLength(0); i++)
            {
                for (int j = 0; j < griglia.GetLength(1); j++)
                {
                    if (grigliadef[i, j].Equals(Pallino))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static void FantasmaMangiaPacMan()
        {
            lock (_lock)
            {
                if (!pacmanMangiato)
                {
                    pacmanMangiato = true;
                    inp = ConsoleKey.C;
                    numerovite--;
                }
            }
        }
        static void PacManMangiaFantasma(int index)
        {
            lock (_lock)
            {
                griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                posizioniFantasmi[index] = new List<int>(posizioniFantasmiIniziali[index]).ToArray();
                fantasmiMangiati++;
                punti += 200 * fantasmiMangiati;
                griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = FantasmaChar;
            }
        }
        #endregion
        #region GESTIONE PACMAN E INPUT GIOCATORE
        static ConsoleKey inp = ConsoleKey.C;
        static void InputGiocatore()
        {
            if (Console.KeyAvailable)
            {
                inp = Console.ReadKey().Key;
            }
            switch (inp)
            {
                case ConsoleKey.LeftArrow:
                    lock (_lock)
                    {
                        ActivePacMan = new String(PacManSX);
                    }
                    if ((posizione[0], posizione[1]) == (15, 0))
                    {
                        griglia[posizione[0], posizione[1]] = " ";
                        posizione[0] = 15;
                        posizione[1] = 29;
                    }
                    else if (griglia[posizione[0], posizione[1] - 1] != Muro)
                    {
                        lock (_lock)
                        {
                            griglia[posizione[0], posizione[1]] = " ";
                            grigliadef[posizione[0], posizione[1]] = " ";
                        }
                        posizione[1]--;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (griglia[posizione[0] - 1, posizione[1]] != Muro)
                    {
                        lock (_lock)
                        {
                            griglia[posizione[0], posizione[1]] = " ";
                            grigliadef[posizione[0], posizione[1]] = " ";
                        }
                        posizione[0]--;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    lock (_lock)
                    {
                        ActivePacMan = new String(PacManDX);
                    }
                    if ((posizione[0], posizione[1]) == (15, 29))
                    {
                        griglia[posizione[0], posizione[1]] = " ";
                        posizione[0] = 15;
                        posizione[1] = 0;
                    }
                    else if (griglia[posizione[0], posizione[1] + 1] != Muro)
                    {
                        lock (_lock)
                        {
                            griglia[posizione[0], posizione[1]] = " ";
                            grigliadef[posizione[0], posizione[1]] = " ";
                        }
                        posizione[1]++;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (griglia[posizione[0] + 1, posizione[1]] != Muro)
                    {
                        lock (_lock)
                        {
                            griglia[posizione[0], posizione[1]] = " ";
                            grigliadef[posizione[0], posizione[1]] = " ";
                        }
                        posizione[0]++;
                    }
                    break;
                case ConsoleKey.Escape:
                    lock (_lock)
                    {
                        fine = true;
                    }
                    return;
                default:
                    break;
            }
        }
        static void PacMan(object obj)
        {
            while (!fine)
            {
                InputGiocatore();
                switch (griglia[posizione[0], posizione[1]])
                {
                    case Pallino:
                        lock (_lock)
                        {
                            punti += 10;
                            griglia[posizione[0], posizione[1]] = ActivePacMan;
                        }
                        break;
                    case FantasmaChar:
                        if (!mangiabili)
                        {
                            FantasmaMangiaPacMan();
                            while (pacmanMangiato) ;
                            lock (_lock)
                            {
                                griglia[posizione[0], posizione[1]] = " ";
                                posizione = new int[2]
                                {
                                1, 1
                                };
                                griglia[posizione[0], posizione[1]] = ActivePacMan;
                            }
                        }
                        else
                        {
                            int indexOfGhost = 10;
                            lock (_lock)
                            {
                                for (int i = 0; i < posizioniFantasmi.Count; i++)
                                {
                                    if ((posizioniFantasmi[i][0], posizioniFantasmi[i][1]) == (posizione[0], posizione[1]))
                                    {
                                        indexOfGhost = i;
                                        break;
                                    }
                                }
                            }
                            if (indexOfGhost != 10)
                            {
                                PacManMangiaFantasma(indexOfGhost);
                            }
                        }
                        break;
                    case Bonus:
                        lock (_lock)
                        {
                            bonusMangiati++;
                            punti += bonusMangiati * 200;
                            griglia[posizione[0], posizione[1]] = ActivePacMan;
                        }
                        break;
                    case Mangia:
                        new Thread(GestisciMangiabili).Start();
                        break;
                    default:
                        lock (_lock)
                        {
                            griglia[posizione[0], posizione[1]] = ActivePacMan;
                        }
                        break;
                }
                Thread.Sleep(500);
            }
        }
        #endregion
        #region FANTASMA
        static void MuoviFantasma(int index)
        {
            Random r = new Random();
            int inp = r.Next(1, 5);
            switch (inp)
            {
                case 1:
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] - 1] != FantasmaChar && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] - 1] != Muro && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] - 1] != Mangia)
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][1]--;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManDX || griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManSX)
                            {
                                if (!mangiabili)
                                    FantasmaMangiaPacMan();
                                else
                                    PacManMangiaFantasma(index);
                            }
                        }
                    }
                    break;
                case 2:
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != FantasmaChar && griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != Muro && griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != Mangia)
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizione[0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][0]--;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManDX || griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManSX)
                            {
                                if (!mangiabili)
                                    FantasmaMangiaPacMan();
                                else
                                    PacManMangiaFantasma(index);
                            }
                        }
                    }
                    break;
                case 3:
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] + 1] != FantasmaChar && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] + 1] != Muro && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] + 1] != Mangia)
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][1]++;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManDX || griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManSX)
                            {
                                if (!mangiabili)
                                    FantasmaMangiaPacMan();
                                else
                                    PacManMangiaFantasma(index);
                            }
                        }
                    }
                    break;
                case 4:
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != FantasmaChar && griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != Muro && griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != Mangia)
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][0]++;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManDX || griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManSX)
                            {
                                if (!mangiabili)
                                    FantasmaMangiaPacMan();
                                else
                                    PacManMangiaFantasma(index);
                            }
                        }
                    }
                    break;
            }
            lock (_lock)
            {
                griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = FantasmaChar;
            }
        }
        static void Rincorri(int index, int distanzaX, int distanzaY)
        {
            if (distanzaY > distanzaX)
            {
                if (distanzaY > 0)
                {
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != FantasmaChar && griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != Muro && griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != Mangia)
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizione[0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][0]++;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManDX || griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManSX)
                            {
                                FantasmaMangiaPacMan();
                            }
                        }
                    }
                }
                else
                {
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != FantasmaChar && griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != Muro && griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != Mangia)
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizione[0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][0]--;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManDX || griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManSX)
                            {
                                FantasmaMangiaPacMan();
                            }
                        }
                    }
                }
            }
            else
            {
                if (distanzaX > 0)
                {
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != FantasmaChar && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] + 1] != Muro && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] + 1] != Mangia)
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][1]++;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManDX || griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManSX)
                            {
                                FantasmaMangiaPacMan();
                            }
                        }
                    }
                }
                else
                {
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] - 1] != FantasmaChar && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] - 1] != Muro && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] - 1] != Mangia)
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][1]--;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManDX || griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == PacManSX)
                            {
                                FantasmaMangiaPacMan();
                            }
                        }
                    }
                }
            }
        }
        static void Scappa(int index, int distanzaX, int distanzaY)
        {
            if (distanzaY > distanzaX)
            {
                if (distanzaY > 0)
                {
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != "6" && griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != "X" && griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != "°")
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizione[0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][0]--;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == "O")
                            {
                                PacManMangiaFantasma(index);
                            }
                        }
                    }
                }
                else
                {
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != "6" && griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != "X" && griglia[posizioniFantasmi[index][0] - 1, posizioniFantasmi[index][1]] != "°")
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizione[0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][0]++;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == "O")
                            {
                                PacManMangiaFantasma(index);
                            }
                        }
                    }
                }
            }
            else
            {
                if (distanzaX > 0)
                {
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] - 1] != "6" && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] - 1] != "X" && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] - 1] != "°")
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][1]--;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == "O")
                            {
                                PacManMangiaFantasma(index);
                            }
                        }
                    }
                }
                else
                {
                    lock (_lock)
                    {
                        if (griglia[posizioniFantasmi[index][0] + 1, posizioniFantasmi[index][1]] != "6" && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] + 1] != "X" && griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1] + 1] != "°")
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                            posizioniFantasmi[index][1]++;
                            if (griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] == "O")
                            {
                                PacManMangiaFantasma(index);
                            }
                        }
                    }
                }
            }
        }
        static void Fantasma(object obj)
        {
            Thread.Sleep(100);
            int index = (int)obj;
            Random r = new Random();
            switch (difficoltà)
            {
                case 1:
                    while (!fine)
                    {
                        MuoviFantasma(index);
                        if (pacmanMangiato)
                        {
                            while (pacmanMangiato) ;
                            lock (_lock)
                            {
                                griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                                posizioniFantasmi[index][0] = posizioniFantasmiIniziali[index][0];
                                posizioniFantasmi[index][1] = posizioniFantasmiIniziali[index][1];
                                griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = FantasmaChar;
                            }
                        }
                        Thread.Sleep(750);
                    }
                    break;
                case 2:
                    while (!fine)
                    {
                        MuoviFantasma(index);
                        if (pacmanMangiato)
                        {
                            while (pacmanMangiato) ;
                            lock (_lock)
                            {
                                griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                                posizioniFantasmi[index][0] = posizioniFantasmiIniziali[index][0];
                                posizioniFantasmi[index][1] = posizioniFantasmiIniziali[index][1];
                                griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = FantasmaChar;
                            }
                        }
                        Thread.Sleep(500);
                    }
                    break;
                case 3:
                    while (!fine)
                    {
                        (int distanzaY, int distanzaX) = (posizione[0] - posizioniFantasmi[index][0], posizione[1] - posizioniFantasmi[index][1]);
                        if (!mangiabili)
                        {
                            Rincorri(index, distanzaX, distanzaY);
                            if (pacmanMangiato)
                            {
                                while (pacmanMangiato) ;
                                lock (_lock)
                                {
                                    griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = grigliadef[posizioniFantasmi[index][0], posizioniFantasmi[index][1]];
                                    posizioniFantasmi[index][0] = posizioniFantasmiIniziali[index][0];
                                    posizioniFantasmi[index][1] = posizioniFantasmiIniziali[index][1];
                                    griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = FantasmaChar;
                                }
                            }
                        }
                        else
                        {
                            Scappa(index, distanzaX, distanzaY);
                        }
                        lock (_lock)
                        {
                            griglia[posizioniFantasmi[index][0], posizioniFantasmi[index][1]] = FantasmaChar;
                        }
                        Thread.Sleep(500);
                    }
                    break;
            }
        }
        #endregion
        #region GESTIONE UTENTE
        static Giocatore Login()
        {
            Console.WriteLine($"Hai già giocato prima?[si, no]");
            string resp = Console.ReadLine();
            Giocatore player = null;
            if (resp.Equals("si"))
            {
                Console.WriteLine("Nome utente?");
                string nome = Console.ReadLine();
                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] currentLine = reader.ReadLine().Split(';');
                        if (currentLine[0].Equals(nome))
                        {
                            player = new(currentLine);
                        }
                    }
                }
                if (player == null)
                {
                    Console.WriteLine("Impossibile effettuare il login!");
                    Thread.Sleep(300);
                    Console.WriteLine("Riprovare");
                    Thread.Sleep(300);
                    Console.Clear();
                    player = Login();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Login effettuato con successo...");
                    Thread.Sleep(1500);
                }
            }
            else
            {
                Console.WriteLine("Come ti chiami?");
                string nome = Console.ReadLine();
                player = new Giocatore { Nome = nome, Record = 0 };
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(player.ToString());
                }
            }
            Console.Clear();
            return player;
        }
        static void AggiornaRecord(Giocatore n)
        {
            List<Giocatore> players = new List<Giocatore>();
            players.Add(n);
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string[] riga = reader.ReadLine().Split(';');
                    if (!riga[0].Equals(n.Nome))
                    {
                        players.Add(new Giocatore
                        {
                            Record = int.Parse(riga[1]),
                            Nome = riga[0]
                        });
                    }
                }
            }
            using (var writer = new StreamWriter(path))
            {
                foreach (Giocatore p in players)
                {
                    writer.WriteLine(p.ToString());
                }
            }
        }
        #endregion
        static void Main(string[] args)
        {
            Console.Title = "PACMAN";
            Console.Clear();
            #region INTRODUZIONE
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Avvio.Stringa);
            Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("O -> PacMan");
            Console.WriteLine("6 -> Fantasmi");
            Console.WriteLine("° -> Bonus [ti permette di mangiare i fantasmi]");
            Console.WriteLine(". -> Pallini devi finirli per passare al livello successivo");
            Console.WriteLine("X -> Muri");
            Console.WriteLine("Premi invio per iniziare...");
            Console.ReadKey();
            Console.Clear();
            #endregion
            #region FASE PRE-GIOCO (Login generazione della griglia e scelta difficoltà)
            var a = Login();
            Console.WriteLine("Seleziona difficoltà[1,2,3]");
            difficoltà = int.Parse(Console.ReadLine());
            GeneraGriglia();
            #endregion
            #region AVVIO THREAD DEL GIOCO
            Thread pacman = new(PacMan);
            Thread griglia = new(AggiornaGriglia);
            griglia.Start(a);
            pacman.Start();
            int numero_fantasmi = difficoltà == 2 ? 4 : 2;
            for (int i = 0; i < numero_fantasmi; i++)
            {
                new Thread(Fantasma).Start(i);
            }
            #endregion
        }
    }
}

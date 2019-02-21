using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using ConvertPlus;
using System.Threading;

namespace wanderX
{
    class Program
    {
        private static byte randomX;
        private static byte randomY;

        static List<int> posix = new List<int>();
        static List<int> posiy = new List<int>();

        static bool add = false;
        static bool school;
        static bool beep;
        static bool autoRun;

        static byte spielfeld;
        static Einstellungen mySettings = new Einstellungen();
        static bool gameOver = false;
        static bool beenden = false;
        static bool neuStarten;
        static int score = 0;
        public static List<int> Posiy { get => posiy; set => posiy = value; }

        static void Main(string[] args)
        {
            //try
            // {


            Console.Title = "Snake";
            Console.CursorVisible = false;
            ConsoleKey eingabe = ConsoleKey.B;
            Console.OutputEncoding = Encoding.Unicode;
            posix.Add(1);
            Posiy.Add(2);
            Load();
            if (mySettings.Spielfeldgroeße <= 0)
            {
                mySettings.Sound = false;
                mySettings.OldSmiley = false;
                mySettings.Autorun = true;
                mySettings.Spielfeldgroeße = 20;
            }
            beep = mySettings.Sound;
            school = mySettings.OldSmiley;
            autoRun = mySettings.Autorun;
            spielfeld = (byte)mySettings.Spielfeldgroeße;

            bool lehrerMode = false;

            NewRandom();

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.SetWindowSize(spielfeld + 50, spielfeld + 3);
            Console.SetBufferSize(spielfeld + 50, spielfeld + 3);


            Reload();
            do
            {
                Console.CursorVisible = false;

                Console.SetCursorPosition(spielfeld + 1, spielfeld / 2);
                Console.Write("score: {0}", score);

                SetSnake();
                if (eingabe == ConsoleKey.F5) eingabe = ConsoleKey.B;
                WriteToConsole(randomX, randomY, "O");
                if (autoRun == true)
                {
                    if (300 - (score * 5) > 100)
                    {
                        System.Threading.Thread.Sleep(300 - (score * 5));
                    }
                    else System.Threading.Thread.Sleep(100);
                }

                else eingabe = ConsoleKey.B;

                if (Console.KeyAvailable == true || eingabe == ConsoleKey.B)
                {
                    eingabe = Console.ReadKey(true).Key;
                }

                switch (eingabe)//prüfe eingabe
                {
                    case ConsoleKey.LeftArrow://Beginn Bewegungen
                    case ConsoleKey.A:
                        GoLeft();
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        GoRight();
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        GoUp();
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        GoDown();
                        break; //Ende Bewegungen
                    case ConsoleKey.F5://reload
                        Reload();
                        break;
                    case ConsoleKey.L://toggle Lehrermode
                    case ConsoleKey.Enter:
                        lehrerMode = !lehrerMode;
                        break;
                    case ConsoleKey.Escape:
                        HilfeEinfuehrung();
                        eingabe = ConsoleKey.B;
                        break;
                }

                if (lehrerMode == true)//teste ob O in der Nähe
                {
                    DoLehrermode();
                }
                else
                {
                    if (posix[0] == randomX && Posiy[0] == randomY) //Teste ob O gefressen
                    {
                        score++;

                        SetNewO();
                        add = true;
                        if (beep == true) Beep();
                    }
                }
                for (int i = score; i > 0 && gameOver == false && add == false; i--)//prüfe TestForGameOver
                {
                    gameOver = TestForGameOver(posix[0], Posiy[0], posix[i], Posiy[i]);
                }
                if (gameOver == true)
                {
                    HilfeEinfuehrung();
                }
            } while (beenden == false); //TestForGameOver
                                        //EXIT
                                        // }
                                        // catch
                                        // {
                                        //    Console.Clear();
                                        //    Console.WriteLine("Ooups, da es ist ein Fehler aufgetreten. Bitte schließen Sie dieses Fenster.");
                                        //    Console.ReadKey(true);
                                        //}

        }





        private static void GoLeft()//gehe links
        {
            if (posix[0] <= 1)
            {
                posix.Insert(0, spielfeld - 1);
            }
            else
            {
                posix.Insert(0, posix[0] - 1);
            }
            Posiy.Insert(0, Posiy[0]);
            RemoveLastBodypart();
        }
        private static void GoRight()//gehe rechts
        {
            if (posix[0] >= spielfeld - 1)
            {
                posix.Insert(0, 1);
            }
            else
            {
                posix.Insert(0, posix[0] + 1);
            }
            Posiy.Insert(0, Posiy[0]);
            RemoveLastBodypart();
        }
        private static void GoUp()//gehe hinauf
        {
            if (Posiy[0] <= 2)
            {
                Posiy.Insert(0, spielfeld - 1);
            }
            else
            {
                Posiy.Insert(0, Posiy[0] - 1);
            }
            posix.Insert(0, posix[0]);
            RemoveLastBodypart();
        }
        private static void GoDown()//gehe hinunter
        {
            if (Posiy[0] >= spielfeld - 1)
            {
                Posiy.Insert(0, 2);
            }
            else
            {
                Posiy.Insert(0, Posiy[0] + 1);
            }
            posix.Insert(0, posix[0]);
            RemoveLastBodypart();
        }
        private static void ChangeSize()//Change Size
        {
            try
            {
                Console.Clear();
                Console.Write("Spielfeldgröße: ");
                spielfeld = Parse.ToByte(Console.ReadLine());
                NewRandom();
                Console.SetWindowSize(spielfeld + 50, spielfeld + 3);
                Console.SetBufferSize(spielfeld + 50, spielfeld + 3);
                Console.Clear();
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Dies ist keine Zahl!");
                Console.ReadKey(true);
            }
        }
        private static void Reload()//reload
        {
            Console.Clear();
            DrawSpielfeld();
        }
        private static void HilfeEinfuehrung()//Einführung
        {
            ConsoleKey inputForChangeSettings;
            int auswahl = 1;
            Console.Clear();
            if (gameOver == false)
            {
                do
                {
                    DrawSettings(auswahl);
                    inputForChangeSettings = Console.ReadKey(true).Key;
                    switch (inputForChangeSettings)
                    {
                        case ConsoleKey.UpArrow:
                            auswahl = SettingUp(auswahl);
                            break;
                        case ConsoleKey.DownArrow:
                            auswahl = SettingDown(auswahl);
                            break;
                        case ConsoleKey.Enter:
                            inputForChangeSettings = ChangeSettings(auswahl);
                            break;
                    }
                } while (inputForChangeSettings != ConsoleKey.Escape);
            }
            else
            {
                do
                {
                    DrawSettings(auswahl);
                    inputForChangeSettings = Console.ReadKey(true).Key;
                    switch (inputForChangeSettings)
                    {
                        case ConsoleKey.UpArrow:
                            auswahl = SettingUp(auswahl);
                            break;
                        case ConsoleKey.DownArrow:
                            auswahl = SettingDown(auswahl);
                            break;
                        case ConsoleKey.Enter:
                            inputForChangeSettings = ChangeSettings(auswahl);
                            break;
                    }
                } while (beenden == false && neuStarten == false);
                neuStarten = false;
            }


            Save();
            Reload();

        }
        private static void DrawSettings(int auswahl)
        {
            Console.CursorVisible = false;
            if (gameOver == false)
            {
                WriteToConsole(0, 0, "Dies ist ein Snake Spiel, welches mit den Tasten 'W' für hinauf, 'A' für rechts,'S' für hinunter und 'D' für links verwendet. Alternativ sind die Pfeiltasten möglich.");
                Console.WriteLine();
            }
            else
            {
                WriteToConsole(0, 0, "Game Over!");
                Console.WriteLine();
                Console.WriteLine("score: {0}", score);
            }
            Console.WriteLine();
            Console.WriteLine();
            if (auswahl == 1)
            {
                Markieren();
            }
            Console.WriteLine("Ton: {0}", beep);
            Demarkieren();
            Console.WriteLine();
            if (auswahl == 2)
            {
                Markieren();
            }
            Console.WriteLine("Black smiley: {0}", school);
            Demarkieren();
            Console.WriteLine();
            if (auswahl == 3)
            {
                Markieren();
            }
            Console.WriteLine("Automatisch gehen: {0}", autoRun);
            Demarkieren();
            Console.WriteLine();
            if (auswahl == 4)
            {
                Markieren();
            }
            Console.WriteLine("Retromode");
            Demarkieren();
            Console.WriteLine();
            if (auswahl == 5)
            {
                Markieren();
            }
            Console.WriteLine("Neu starten");//reset
            Demarkieren();
            Console.WriteLine();
            if (auswahl == 6)
            {
                Markieren();
            }
            Console.WriteLine("Spielfeldfröße ändern");
            Console.WriteLine("Aktuelle Spielfeldgröße: {0}", spielfeld);
            Demarkieren();
            Console.WriteLine();
            if (auswahl == 7)
            {
                Markieren();
            }
            Console.WriteLine("Beenden");
            Demarkieren();
            if (gameOver == false)
            {
                Console.WriteLine();
                if (auswahl == 8)
                {
                    Markieren();
                }
                Console.WriteLine("Fortsetzen");
                Demarkieren();
            }

        }
        private static void Markieren()
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
        }
        private static void Demarkieren()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        private static int SettingUp(int auswahl)
        {
            if (auswahl <= 1)
            {
                if (gameOver == false) return 8;
                else return 7;
            }

            else
            {
                auswahl--;
                return auswahl;
            }
        }
        private static int SettingDown(int auswahl)
        {
            if ((auswahl >= 8) || ((gameOver == true) && (auswahl >= 7))) return 1;
            else
            {
                auswahl++;
                return auswahl;
            }
        }
        private static ConsoleKey ChangeSettings(int auswahl)
        {
            switch (auswahl)
            {
                case 1:
                    beep = !beep;
                    mySettings.Sound = beep;
                    Console.Clear();
                    if (beep == true) Beep();
                    return ConsoleKey.R;
                case 2:
                    school = !school;
                    mySettings.OldSmiley = school;
                    Console.Clear();
                    if (beep == true) Beep();
                    return ConsoleKey.R;
                case 3:
                    autoRun = !autoRun;
                    mySettings.Autorun = autoRun;
                    if (beep == true) Beep();
                    Console.Clear();
                    return ConsoleKey.R;
                case 4:
                    if (beep == true) Beep();
                    DasWanderndeX();
                    return ConsoleKey.R;
                case 5:
                    if (beep == true) Beep();
                    neuStarten = true;
                    Reset();
                    return ConsoleKey.Escape;
                case 6:
                    ChangeSize();
                    mySettings.Spielfeldgroeße = spielfeld;
                    if (beep == true) Beep();
                    return ConsoleKey.R;
                case 7:
                    beenden = true;
                    if (beep == true) Beep();
                    return ConsoleKey.Escape;
                case 8:
                    if (beep == true) Beep();
                    return ConsoleKey.Escape;
            }
            return ConsoleKey.R;
        }

        private static void Reset()
        {
            posix.Clear();
            Posiy.Clear();
            posix.Add(1);
            Posiy.Add(2);
            score = 0;
            add = false;
            gameOver = false;
        }
        static void WriteToConsole(int posx, int posy, string text = " ")//schreibt auf die Console
        {
            Console.SetCursorPosition(posx, posy);
            Console.Write(text);
            return;
        }
        static bool TestForGameOver(int headX, int headY, int bodyX, int bodyY)//prüfe TestForGameOver
        {
            if (headX == bodyX && headY == bodyY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void DrawSpielfeld()//Zeichne Spielfeld
        {
            if (gameOver == false)
            {
                WriteToConsole(0, 0, "Für das Menü und Hilfe Escape drücken!");
                int a = spielfeld;
                for (int i = 1; i <= spielfeld; i++)
                {
                    WriteToConsole(i, 1, "" + (Char)9608);
                    WriteToConsole(0, a, "" + (Char)9608);
                    WriteToConsole(a, spielfeld, "" + (Char)9608);
                    WriteToConsole(spielfeld, i, "" + (Char)9608);
                    a--;
                    System.Threading.Thread.Sleep(15);
                }
            }
        }

        private static void SetNewO()//setze neues O
        {
            Console.CursorVisible = false;
            NewRandom();
            for (int i = score - 1; i >= 0; i--)
            {
                if (posix[i] == randomX && Posiy[i] == randomY)
                {
                    WriteToConsole(randomX, randomY);
                    NewRandom();
                    i = score;
                }
            }
            WriteToConsole(randomX, randomY);

        }
        static void NewRandom()//generiert neue random Coords
        {
            Random random = new Random();
            randomX = (byte)random.Next(1, spielfeld - 1);
            randomY = (byte)random.Next(2, spielfeld - 1);
        }
        static void SetSnake()//Zeichne die Schlange
        {
            for (int i = 0; i < posix.Count; i++)
            {
                Console.SetCursorPosition(posix[i], Posiy[i]);
                if (i == 0)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (school == false) Console.Write('\u263a');
                    else Console.Write("" + /*(Char)2*/'\u263b');
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("X");
                }
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        static void RemoveLastBodypart()
        {
            if (add == false)//entferne den letzten Körperteil
            {
                WriteToConsole(posix[score + 1], Posiy[score + 1], " ");
                posix.RemoveAt(score + 1);
                Posiy.RemoveAt(score + 1);
            }
            add = false;//setzt add auf false(wenn ein O eingesammelt wird, da Snake verlängert werden muss)
        }

        static void DoLehrermode()
        {
            if (posix[0] + 1 == randomX && Posiy[0] == randomY)
            {
                SetNewO();
            }
            if (posix[0] - 1 == randomX && Posiy[0] == randomY)
            {
                SetNewO();
            }
            if (posix[0] == randomX && Posiy[0] + 1 == randomY)
            {
                SetNewO();
            }
            if (posix[0] == randomX && Posiy[0] - 1 == randomY)
            {
                //WriteToConsole(randomX, randomY);
                //randomX = random.Next(2, spielfeld - 1);
                //randomY = random.Next(2, spielfeld - 1);
                SetNewO();
            }
        }
        static void DasWanderndeX()
        {
            Console.Title = "Das wandernde X";
            char eingabe;
            int posx = 2;

            int posy = 2;

            spielfeld = 20;
            NewRandom();

            int score = 0;
            Console.CursorVisible = false;
            bool lehrerMode = false;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Clear();
            WriteToConsole(1, 0, "Um den Retromode zu verlassen 'Q'drücken");
            for (int i = 1; i <= spielfeld; i++)
            {
                WriteToConsole(i, 1, "#");
                WriteToConsole(1, i, "#");
                WriteToConsole(i, spielfeld, "#");
                WriteToConsole(spielfeld, i, "#");
            }
            do
            {

                Console.SetCursorPosition(spielfeld + 1, 11);
                Console.Write("score:" + score); Console.SetCursorPosition(posx, posy);
                Console.Write("X");
                Console.SetCursorPosition(randomX, randomY);
                Console.Write("O");
                eingabe = Console.ReadKey(true).KeyChar;
                WriteToConsole(posx, posy, " ");
                eingabe = char.ToLower(eingabe);
                if (posy > 2)
                {
                    if (eingabe == 'w')
                    {
                        posy--;
                    }
                }
                if (posx > 2)
                {
                    if (eingabe == 'a')
                    {
                        posx--;
                    }
                }
                if (posy < spielfeld - 1)
                {
                    if (eingabe == 's')
                    {
                        posy++;
                    }
                }
                if (posx < spielfeld - 1)
                {
                    if (eingabe == 'd')
                    {
                        posx++;
                    }
                }
                if (lehrerMode == true)
                {
                    if (posx + 1 == randomX && posy == randomY)
                    {
                        WriteToConsole(randomX, randomY, " ");
                        NewRandom();
                    }
                    if (posx - 1 == randomX && posy == randomY)
                    {
                        WriteToConsole(randomX, randomY, " ");
                        NewRandom();
                    }
                    if (posx == randomX && posy + 1 == randomY)
                    {
                        WriteToConsole(randomX, randomY, " ");
                        NewRandom();
                    }
                    if (posx == randomX && posy - 1 == randomY)
                    {
                        WriteToConsole(randomX, randomY, " ");
                        NewRandom();
                    }
                }
                else
                {
                    if (posx == randomX && posy == randomY)
                    {
                        score++;
                        NewRandom();
                        Beep();
                    }
                }
                if (posx == randomX && posy == randomY)
                {
                    score++;
                    NewRandom();
                    Beep();
                    //  Console.CursorVisible = false;
                }
                if (eingabe == 'r') score = 0;
                if (eingabe == 'l') lehrerMode = !lehrerMode;

            } while (eingabe != 'q');
            Console.Title = "Snake";
            Console.Clear();
            return;
        }
        public static void Beep()
        {
            Thread beep = new Thread(DoBeep);
            beep.Start();

        }
        private static void DoBeep()
        {
            Console.Beep();
        }

        static string _path = ".//Snake.Xml";
        public static void Save()
        {

            FileStream file;
            /*FileStream*/
            file = new FileStream(Directory.GetCurrentDirectory() + _path, FileMode.Create);
            //file = File.Create(Directory.GetCurrentDirectory() + "//wanderXSettings.Xml"); //Erstellt in dem aktuellen verzeichnis ...

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Einstellungen));
            writer.Serialize(file, mySettings);

            file.Close();
        }

        public static void Load()
        {

            try
            {
                FileStream file = new FileStream(Directory.GetCurrentDirectory() + _path, FileMode.Open);
                System.Xml.XmlReader reader = XmlReader.Create(file);
                //System.IO.File.ReadAllText("file.Xml");
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Einstellungen));
                mySettings = (Einstellungen)serializer.Deserialize(reader);
                file.Close();
            }
            catch
            {
                File.Create(_path);
                autoRun = mySettings.Autorun = true;
                school = mySettings.OldSmiley = false;
                beep = mySettings.Sound = true;
                mySettings.Spielfeldgroeße = 20;
                spielfeld = 20;
                while (!File.Exists(_path))
                {

                }
                Save();
            }




        }
    }
}
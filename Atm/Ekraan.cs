using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Atm
{
    class Ekraan
    {
        public static int Width => Console.WindowWidth;
        public static int Height => Console.WindowHeight;

        public static String NL => Environment.NewLine;

        private static int MenyyLaius = Width - 30;

        public static void SeaMenyyLaius(int uusLaius)
        {
            MenyyLaius = Math.Min(Width, uusLaius);
        }

        public static char Menyy(List<string> valikud, List<char> vastused)
        {
            if (valikud.Count != vastused.Count) return '#';

            Console.WriteLine();
            string MenyyRida = "";
            int i = 0;
            foreach (string valik in valikud)
            {

                MenyyRida = vastused[i] + ". " + valik;
                Ekraan.KeskelRV(MenyyRida, MenyyLaius);
                i++;
            }

            MenyyRida = " Sisesta valik: <" + string.Join(',', vastused.ToArray()) + ">";
            Ekraan.TyhjeRidu(1);
            Ekraan.KeskelRV(MenyyRida, MenyyLaius);
            char vastus = ' ';
            string midaVastati = "";
            do
            {
                midaVastati = Ekraan.KysiKeskelRV(" ::> ", MenyyLaius);
                midaVastati += " ";
                midaVastati = midaVastati.ToUpper();
                vastus = midaVastati[0];
            } while (!vastused.Any(s => s.Equals(vastus)));

            return vastus;
        }

        public static void Puhtaks()
        {
            Console.Clear();
        }

        public static void Kiri(ConsoleColor värv)
        {
            Console.ForegroundColor = värv;
        }

        public static void Taust(ConsoleColor värv)
        {
            Console.BackgroundColor = värv;
        }

        /// <summary>
        /// Värv tagasi tavaliseks (ehk hall kiri mustal taustal)
        /// </summary>
        public static void Tavaline()
        {
            Console.ResetColor();
        }

        public static void KeskelRV(string message, int width = 0)
        {
            if (width > 0)
            {
                width = Math.Min(width, Width);
                message = message.PadRight(width);
            }

            Console.CursorLeft = (Width / 2) - (message.Length / 2);
            Console.WriteLine(message);
        }

        public static void Keskel(string message, int width = 0)
        {
            if (width > 0)
            {
                width = Math.Min(width, Width);
                message = message.PadRight(width);
            }

            Console.CursorLeft = (Width / 2) - (message.Length / 2);
            Console.Write(message);
        }

        public static void Joon(char lineChar = '-', int width = 0)
        {
            width = Math.Max(width, 0);
            width = Math.Min(Width, width);
            KeskelRV(new string(lineChar, width));
        }

        public static void TyhjeRidu(int lines = 1)
        {
            if (lines < 1) lines = 1;

            if (lines > (Height - Console.CursorTop)) lines = (Height - Console.CursorTop);

            for (int i = 0; i < lines; i++) Console.WriteLine();
        }

        public static void Tiitel(string message)
        {
            Console.Title = message;
        }

        public static string Kysi(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public static string KysiKeskelRV(string kysimus, int Laius = 0)
        {
            if (Laius == 0)
            {
                Laius = kysimus.Length;
            }
            Keskel(kysimus, Laius);
            Mine(LeiaRida(), LeiaVeerg() - (Laius - kysimus.Length));
            return Console.ReadLine();
        }

        public static void Mine(int row, int col)
        {
            col = col < 0 ? Width - Math.Min(Math.Abs(col), Width) : Math.Min(col, Width);
            row = row < 0 ? Height - Math.Min(Math.Abs(row), Height) : Math.Min(row, Height);
            Console.CursorLeft = col;
            Console.CursorTop = row;
        }



        public static void Fikseeritud()
        {
            Console.BufferHeight = Height;
            Console.BufferWidth = Width;
        }

        public static void Paus()
        {
            TyhjeRidu(1);
            Kiri(ConsoleColor.DarkYellow);
            KeskelRV("Jätkamiseks vajuta <ENTER>.");
            Tavaline();
            Console.ReadLine();
        }

        public static int LeiaRida() { return Console.CursorTop; }
        public static int LeiaVeerg() { return Console.CursorLeft; }


    }
}

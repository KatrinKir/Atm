using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Atm
{
    class App
    {


        public static char RegistreeriLogin()
        {

            Andmed info = new Andmed(Program.AndmeFailiNimi);

            if (info.KasutajateArv() == 0)
            {
                return 'R';
            }

            List<string> menyyValikud = new List<string>();
            List<char> menyyVastused = new List<char>();

            menyyValikud.Add("Registreerimine");
            menyyVastused.Add('R');

            menyyValikud.Add("Logi sisse");
            menyyVastused.Add('L');

            return Ekraan.Menyy(menyyValikud, menyyVastused);
        }


        public static List<string> Registreerimine()
        {
            List<string> regAndmed = new List<string>();
            Andmed info = new Andmed(Program.AndmeFailiNimi);

            string vastus = "";
            bool Viga = false;
            do
            {
                if (Viga)
                {
                    Ekraan.Kiri(ConsoleColor.Red);
                    Ekraan.KeskelRV("VIGA! Selline kasutaja on juba olemas!");
                    Ekraan.Tavaline();
                }
                vastus = Ekraan.KysiKeskelRV("Uue kasutaja nimi     : ", Ekraan.Width - 30);
                Viga = true;
            } while (info.KasutajaOlemas(vastus) || vastus.Length == 0);
            regAndmed.Add(vastus);
            Viga = false;
            do
            {
                if (Viga)
                {
                    Ekraan.Kiri(ConsoleColor.Red);
                    Ekraan.KeskelRV("VIGA! PIN kood tohib sisaldada vaid 4 sümbolit!");
                    Ekraan.Tavaline();
                }
                vastus = Ekraan.KysiKeskelRV("Uue kasutaja PIN (4 kohta) : ", Ekraan.Width - 30);
                Viga = true;
            } while (vastus.Length != 4);
            regAndmed.Add(vastus);

            return regAndmed;
        }

        public static string LogiSisse()
        {
            Andmed info = new Andmed(Program.AndmeFailiNimi);

            string kasutaja = "";
            string salaS6na = "";
            kasutaja = Ekraan.KysiKeskelRV("KASUTAJA : ");
            salaS6na = Ekraan.KysiKeskelRV("PIN KOOD : ");

            if (info.KasutajaLogin(kasutaja, salaS6na))
            {
                return kasutaja;
            }
            else
            {
                Ekraan.Kiri(ConsoleColor.Yellow);
                Ekraan.KeskelRV("VIGA! Sisselogimine ebaõnnestus!");
                Ekraan.Tavaline();
                Ekraan.Paus();
                return "";
            }

        }


        public static void Pealdis()
        {
            Ekraan.Puhtaks();
            Ekraan.Fikseeritud();
            Ekraan.Tiitel("PANGAAUTOMAAT");
            Ekraan.TyhjeRidu(3);
            Ekraan.KeskelRV("P A N G A A U T O M A A T");
            Ekraan.KeskelRV("Versioon 1.0");
            Ekraan.KeskelRV("Tegija: Katrin");
            Ekraan.Joon('=', Ekraan.Width - 20);
            Ekraan.TyhjeRidu(1);
        }


    }
}

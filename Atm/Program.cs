using System;
using System.Collections.Generic;

namespace Atm
{
    class Program
    {
        public const string AndmeFailiNimi = @"C:\Users\katri\source\repos\Atm\Atm\Kasutaja_Andmed.csv";



        static void Main(string[] args)
        {

            Andmed info = new Andmed(AndmeFailiNimi);
            List<string> TooMenyy = new List<string>();
            List<char> TooValikud = new List<char>();

            TooMenyy.Add("Konto jääk");
            TooValikud.Add('1');
            TooMenyy.Add("Raha välja");
            TooValikud.Add('2');
            TooMenyy.Add("Raha sisse");
            TooValikud.Add('3');
            TooMenyy.Add("Konto info");
            TooValikud.Add('4');
            TooMenyy.Add("Logi välja");
            TooValikud.Add('5');
            TooMenyy.Add("Programm kinni");
            TooValikud.Add('X');



            bool running = true;
            string aktiivneKasutaja = "";


            while (running)
            {

                // 1. Kasutaja loomine või sisse logimine
                App.Pealdis();

                char valik = App.RegistreeriLogin();
                Ekraan.TyhjeRidu(2);

                if (valik == 'R')
                {
                    List<string> uusKasutaja = App.Registreerimine();
                    info.UusKasutaja(uusKasutaja[0], uusKasutaja[1]);
                    continue;
                }
                else
                {
                    aktiivneKasutaja = App.LogiSisse();
                    if (aktiivneKasutaja.Length == 0)
                    {
                        continue;
                    }
                }

                // 2. Programmi töötamine
                App.Pealdis();
                // Tekita valikute menüü ja 
                do
                {
                    App.Pealdis();
                    valik = Ekraan.Menyy(TooMenyy, TooValikud);
                    valik = Char.ToUpper(valik);

                    switch (valik)
                    {
                        case '1':
                            // konto jääk
                            Ekraan.TyhjeRidu(1);
                            float kontoJaak = info.KasutajaSaldo(aktiivneKasutaja);
                            Ekraan.KeskelRV("Kasutaja '" + aktiivneKasutaja + "' konto jääk on: " + kontoJaak.ToString());
                            Ekraan.TyhjeRidu(2);
                            Ekraan.Paus();
                            break;

                        case '2':
                            // Raha välja
                            Ekraan.TyhjeRidu(1);
                            //float RahaValja = info.KasutajaSaldo(aktiivneKasutaja);
                            float J22k = info.KasutajaSaldo(aktiivneKasutaja);
                            Ekraan.KeskelRV("Hetkel on teie arvel : " + J22k);
                            string soov1S = Ekraan.KysiKeskelRV("Kui palju soovid raha välja võtta? : ");
                            float soov1F = float.Parse(soov1S);
                            if (soov1F > J22k)
                            {
                                Ekraan.Kiri(ConsoleColor.DarkRed);
                                Ekraan.KeskelRV("Teil pole piisavalt raha kontol.");
                            } else
                            {
                                J22k = info.KasutajaSaldo(aktiivneKasutaja, -soov1F);
                                Ekraan.Kiri(ConsoleColor.Green);
                                Ekraan.KeskelRV("Raha on kontol nüüd" + J22k);
                            }
                            Ekraan.Tavaline();
                            Ekraan.Paus();
                            break;

                        case '3':
                            // Raha sisse
                            Ekraan.TyhjeRidu(1);
                            string soov2S = Ekraan.KysiKeskelRV("Kui palju te soovite sisestada? : ");
                            float soov2F = float.Parse(soov2S);
                            J22k = info.KasutajaSaldo(aktiivneKasutaja, soov2F);

                            Ekraan.Kiri(ConsoleColor.Green);
                            Ekraan.KeskelRV("Raha on kontol nüüd" + J22k);

                            Ekraan.Tavaline();
                            Ekraan.Paus();
                            break;

                        case '4':
                            // Kasutaja info
                            Ekraan.TyhjeRidu(1);
                            List<string> KasutajaInfo = info.KasutajaInfo(aktiivneKasutaja);
                            Ekraan.KeskelRV("Kasutaja nimi : " + KasutajaInfo[0], 40 );
                            Ekraan.KeskelRV("Konto nr      : " + KasutajaInfo[2], 40);
                            Ekraan.KeskelRV("Konto jääk    : " + KasutajaInfo[3], 40);
                            Ekraan.TyhjeRidu(1);
                            Ekraan.Paus();
                            break;

                        case '5':
                            // Logi välja
                            aktiivneKasutaja = "";
                            break;

                        case 'X':
                            aktiivneKasutaja = "";
                            running = false;
                            break;
                        default:
                            Ekraan.KeskelRV("Tehtud valik : " + valik);
                            break;
                    }
                } while (aktiivneKasutaja.Length != 0);

                aktiivneKasutaja = "";

            } // while running

        } // void MAIN

    }

}

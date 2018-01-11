using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Atm.Lisad;
using System.Globalization;

namespace Atm
{
    public class Andmed
    {
        private string failiNimi = ""; //klassi loomisel tuleb tühi nimi

        private static Random random = new Random(); // Konto nr

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="andmeFail"></param>
        public Andmed(string andmeFail)
        {
            failiNimi = andmeFail;
        }

        /// <summary>
        /// Leiame kasutajate arvu süsteemis.
        /// Selleks loeme läbi CSV failis olevad read ja lahutame maha 1. rea. (See on päis)
        /// </summary>
        /// <returns></returns>
        public int KasutajateArv()
        {
            int i = 0; // Ridade lugeja
            using (CsvFileReader lugeja = new CsvFileReader(failiNimi))
            {
                CsvRow rida = new CsvRow();
                while (lugeja.ReadRow(rida))
                {
                    i++;
                }
                lugeja.Close(); // Sulgeme faili lugeja.
            }
            return i - 1; // vastus on 1-võrra väiksem kui kõik read kokku.

        }

        /// <summary>
        /// Loome uue kasutaja.
        /// Kontrllime, kas sellise nimega kasutajat juba olemas ei ole.
        /// Kui on anname vastseks FALSE. Kui õnnestus luua, on vastuseks TRUE.
        /// </summary>
        /// <param name="Kasutaja">Kasutaja nimi</param>
        /// <param name="Pin">Kasutaja PIN kood</param>
        /// <returns></returns>
        public bool UusKasutaja(string Kasutaja, string Pin)
        {
            string kontoNr = LeiaVabaKontoNr();
            if (!KasutajaOlemas(Kasutaja))
            {
                // lisame uue kasutaja, kui seda ei eksisteeri
                using (CsvFileWriter writer = new CsvFileWriter(failiNimi, true))
                {
                    CsvRow row = new CsvRow(); // loome uue rea
                    row.Add(Kasutaja);  // esimesena lisame ritta kasutaja nime
                    row.Add(Pin);       // teiseks pin koodi
                    row.Add(kontoNr);   // kolmandaks leiame kasutajale konto numbri
                    row.Add("0");       // Anname kasutajale 0 saldo

                    writer.WriteRow(row); // kirjutame rea andmefaili
                    writer.Close();
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Rahandus
        /// </summary>
        /// <param name="kasutaja"></param>
        /// <returns></returns>
        public float KasutajaSaldo(string kasutaja)
        {
            using (CsvFileReader lugeja = new CsvFileReader(failiNimi))
            {
                CsvRow rida = new CsvRow();
                while (lugeja.ReadRow(rida))
                {
                    if (rida[0] == kasutaja)
                    {
                        string summa = rida[3];
                        lugeja.Close();
                        return float.Parse(summa, CultureInfo.InvariantCulture.NumberFormat);
                    }
                }
                lugeja.Close();
            }

            return -1;
        }

        /// <summary>
        /// Muudame kasutaja saldot. Vähendamisel anname summa miinusega.
        /// Kui pole piisavalt raha kontol saame tulemuseks -1.
        /// </summary>
        /// <param name="kasutaja"></param>
        /// <param name="SaldoMuutus"></param>
        /// <returns></returns>
        public float KasutajaSaldo(string kasutaja, float SaldoMuutus)
        {
            float j22k = KasutajaSaldo(kasutaja);


            if (SaldoMuutus < 0 && Math.Abs(SaldoMuutus) > j22k)
            {
                return -1;
            }
            j22k += SaldoMuutus;

            // Kõigepealt loeme CSV faili sisse ja muudame soovitud rea.
            using (CsvFileReader lugeja = new CsvFileReader(failiNimi))
            {
                CsvRow rida = new CsvRow();
                int i = 0;
                while (lugeja.ReadRow(rida))
                {
                    if (rida[0] == kasutaja)
                    {
                        rida[3] = j22k.ToString().Replace(',', '.');
                    }
                    using (CsvFileWriter writer = new CsvFileWriter(failiNimi + ".txt", i != 0))
                    {
                        writer.WriteRow(rida);
                        writer.Close();
                    }
                    i++;
                }
                lugeja.Close();
            }

            // Nimetame loodud uue faili ümber vanaks failiks.
            if (File.Exists(failiNimi))
            {
                File.Delete(failiNimi);
            }
            File.Move(failiNimi + ".txt", failiNimi);


            return j22k;
        }

        /// <summary>
        /// Leiab antud kasutaja txt failist.
        /// Kui kasutajat pole siis tagasta tühjus.
        /// </summary>
        /// <param name="kasutaja"></param>
        /// <returns></returns>
        public string KasutajaKonto(string kasutaja)
        {
            using (CsvFileReader lugeja = new CsvFileReader(failiNimi))
            {
                CsvRow rida = new CsvRow();
                int i = 0;
                while (lugeja.ReadRow(rida))
                {
                    if (i > 0 && rida[0] == kasutaja)
                    {
                        lugeja.Close();
                        return rida[2];
                    }
                    i++;
                }
                lugeja.Close();
            }
            return "";
        }


        /// <summary>
        /// Tgastab kogu info kui kasutaja eksisteerib (PIN, jäägi jne)
        /// </summary>
        /// <param name="kasutaja"></param>
        /// <returns></returns>
        public List<string> KasutajaInfo(string kasutaja)
        {
            using (CsvFileReader lugeja = new CsvFileReader(failiNimi))
            {
                CsvRow rida = new CsvRow();
                int i = 0;
                while (lugeja.ReadRow(rida))
                {
                    if (i > 0 && rida[0] == kasutaja)
                    {
                        lugeja.Close();
                        return rida;
                    }
                    i++;
                }
                lugeja.Close();
            }
            List<string> vastus = new List<string>();
            vastus.Add("");
            vastus.Add("");
            vastus.Add("");
            vastus.Add("");
            return vastus;
        }

        /// <summary>
        /// Kontrollime, kas kasutaja info on olemas CSV failis
        /// </summary>
        /// <param name="kasutaja"></param>
        /// <returns></returns>
        public bool KasutajaOlemas(string kasutaja)
        {
            using (CsvFileReader lugeja = new CsvFileReader(failiNimi))
            {
                CsvRow rida = new CsvRow();
                int i = 0;
                while (lugeja.ReadRow(rida))
                {
                    if (i > 0 && rida[0] == kasutaja)
                    {
                        lugeja.Close();
                        return true;
                    }
                    i++;
                }
                lugeja.Close();
            }
            return false;
        }

        /// <summary>
        /// Kontrollime kasutaja sisselogimis andmeid.
        /// </summary>
        /// <param name="kasutaja"></param>
        /// <param name="Pin"></param>
        /// <returns></returns>
        public bool KasutajaLogin(string kasutaja, string Pin)
        {
            using (CsvFileReader lugeja = new CsvFileReader(failiNimi))
            {
                CsvRow rida = new CsvRow();
                int i = 0;
                string PinKood = "";
                while (lugeja.ReadRow(rida))
                {
                    if (i > 0)
                    {
                        PinKood = rida[1];
                        if (rida[0] == kasutaja && PinKood == Pin)
                        {
                            return true;
                        }
                    }
                    i++;
                }
                lugeja.Close();
            }
            return false;
        }

        /// <summary>
        /// Kontrollime, et kas sellise nimega kasutaja konto on juba olemas.
        /// </summary>
        /// <param name="kontoId"></param>
        /// <returns></returns>
        public bool KasutajaKontoOlemas(string kontoId)
        {
            // Avame CSV faili lugemiseks
            using (CsvFileReader lugeja = new CsvFileReader(failiNimi))
            {
                CsvRow rida = new CsvRow();
                int i = 0; // see on ridade loendamiseks, et ei kontrollitaks kasutajat esimesest reast.

                // Loeme faili rida haaval ja kontrollime, et ega 
                // esimene väärtus (kasutaja) juba ei ole olemas
                while (lugeja.ReadRow(rida))
                {
                    if (i > 0 && rida[3] == kontoId)
                    {
                        lugeja.Close();
                        return true;
                    }
                    i++;
                }
                lugeja.Close();
            }
            return false;
        }

        /// <summary>
        /// Leiame uue konto numbri, mida keegi pole veel kasutusele võtnud
        /// </summary>
        /// <returns>Kontonumber</returns>
        public string LeiaVabaKontoNr()
        {
            string konto = LooKontoNumber();
            while (KasutajaKontoOlemas(konto))
            {
                konto = LooKontoNumber();
            }
            return konto;
        }


        /// <summary>
        /// Loome arve numbri kujul AA00000000000000000000
        /// Vajadusel võid formaati muuta, muutes CharsCount ja NrsCount väärtuseid.
        /// </summary>
        /// <returns></returns>
        public static string LooKontoNumber()
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string nrs = "0123456789";
            const int CharsCount = 2;
            const int NrsCount = 20;

            string kontoId = new string(Enumerable.Repeat(chars, CharsCount)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            kontoId += new string(Enumerable.Repeat(nrs, NrsCount)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return kontoId;
        }





        // ///////////////////////////////////////////////////////////////////////////////////////
        // ///////////////////////////////////////////////////////////////////////////////////////
        //  _   _ _   _ _     _ _     
        // | \ | (_) (_|_)   | (_)    
        // |  \| | __ _ _  __| |_ ___ 
        // | . ` |/ _` | |/ _` | / __|
        // | |\  | (_| | | (_| | \__ \
        // |_| \_|\__,_|_|\__,_|_|___/
        //                            
        // ///////////////////////////////////////////////////////////////////////////////////////
        // ///////////////////////////////////////////////////////////////////////////////////////

        // See on näide kuidas lugeda kõik andmed CSV failist
        public void LoeCSV()
        {
            // Read sample data from CSV file and output to screen
            using (CsvFileReader reader = new CsvFileReader(failiNimi))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    foreach (string s in row)
                    {
                        Console.Write(s);
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }
        } // LoeCSV

        // See on näide kuidas kirjutada andmed tagasi CSV faili
        public void KirjutaCSV()
        {
            // Write sample data to CSV file
            using (CsvFileWriter writer = new CsvFileWriter("WriteTest.csv"))
            {
                for (int i = 0; i < 100; i++)
                {
                    CsvRow row = new CsvRow();
                    for (int j = 0; j < 5; j++)
                        row.Add(String.Format("Column{0}", j));
                    writer.WriteRow(row);
                }
            }
        } // KirjutaCSV

    }
}

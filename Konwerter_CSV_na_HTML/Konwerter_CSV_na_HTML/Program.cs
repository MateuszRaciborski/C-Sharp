//Mateusz Raciborski, ZUT 2018/2019
//konwerter pliku CSV na plik HTML
//tworzy tabelę w HTML ze wskazanego pliku CSV
//public static class Stala jest do wpisania znaku dzielącego w pliku CSV

using System;
using System.IO;
using System.Linq; //do kolekcji obiektów ( .ToArray() )


public static class Stala //ustala znak podziału w pliku CSV
{
    public const char rozdzielacz = ';';
}

namespace Konwerter_CSV_na_HTML
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string sciezka_pliku_csv = @"pliki\plik_csv.csv";
            string sciezka_pliku_html = @"pliki\plik_html.html";
            
            //Utworzenie pliku HTML
            if (!File.Exists(sciezka_pliku_html))
            {
                File.CreateText(sciezka_pliku_html);
            }
            else
            {
                //Wyczyszczenie tego pliku jeśli coś tam jest
                File.WriteAllText(sciezka_pliku_html, string.Empty);
            }


            //pobranie wszystkich linii z pliku CSV
            var plik_CSV = File.ReadAllLines(sciezka_pliku_csv);

            //utworzenie nowego obiektu klasy CSV, żeby zrobić przetworzenie wyżej załadowanej treści pliku
            przetwarzanie_pliku_CSV CSV = new przetwarzanie_pliku_CSV();

            //początek i koniec pliku HTML
            poczatek_pliku_HTML poczatek = new poczatek_pliku_HTML();
            koniec_pliku_HTML koniec = new koniec_pliku_HTML();


            string plik_HTML = poczatek.poczatek_pliku();
            plik_HTML += CSV.Przetwarzanie_CSV(plik_CSV);
            plik_HTML += koniec.koniec_pliku();

            //wysłanie do pliku HTML
            File.WriteAllText(sciezka_pliku_html, plik_HTML);

            Console.WriteLine("\n\n+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+\n\n");
            Console.WriteLine("Plik CSV przetworzony do pliku HTML:\n");
            Console.WriteLine(plik_HTML);

            Console.WriteLine(Environment.NewLine + Environment.NewLine + "Naciśnij dowolny przycisk, aby wyjść...");
            Console.ReadKey();
        }
    }


    class przetwarzanie_pliku_CSV
    {
        public string Przetwarzanie_CSV(String[] plik_CSV)
        {
            string przetworzony_CSV = "<center><table>";
            int licznik = 0;

            Console.WriteLine("Plik CSV:\n");

            foreach (var pojedyncza_linia_pliku_CSV in plik_CSV)
            {
                
                Console.WriteLine(pojedyncza_linia_pliku_CSV);
                if (licznik == 0) //sprawdza czy nagłówek pliku
                {
                    var rozdzielona_pojedyncza_linia_z_pliku_CSV = pojedyncza_linia_pliku_CSV.Split(Stala.rozdzielacz).ToArray();

                    przetworzony_CSV += "<thead><tr>";

                    foreach (var jedna_wartosc_z_linii in rozdzielona_pojedyncza_linia_z_pliku_CSV)
                    {
                        przetworzony_CSV += "<th>" + jedna_wartosc_z_linii + "</th>";
                    }

                    przetworzony_CSV += "</tr></thead>";
                    licznik++;
                }
                else
                {
                    var rozdzielona_pojedyncza_linia_z_pliku_CSV = pojedyncza_linia_pliku_CSV.Split(';').ToArray();

                    przetworzony_CSV += "<tbody><tr>";

                    foreach (var jedna_wartosc_z_linii in rozdzielona_pojedyncza_linia_z_pliku_CSV)
                    {
                        przetworzony_CSV += "<td>" + jedna_wartosc_z_linii + "</td>";
                    }

                    przetworzony_CSV += "</tr></tbody>";
                }
            }
            przetworzony_CSV += "</center></table>";
            return przetworzony_CSV;
        }
    }

    class poczatek_pliku_HTML
    {
        public string poczatek_pliku()
        {
            string poczatek_pliku = "<!DOCTYPE html>" +
                "<html lang='pl'>" +
                "<head>" +
                "<meta charset='UTF-8'>" +
                "<title>Konwerter CSV na HTML</title>" +

                "<style>" +

                "body {background-color: beige;}" +
                "body {font-size:25px;}" +

                "table, th, td {border-collapse: collapse; padding-left: 10px; padding-right: 10px; padding-top: 5px; padding-bottom: 5px;}" +
                "table, th, td {border: 1px solid black;}" +

                "</style>" +

                "</head>" +
                "<body>" +
                "<center><h2>Plik CSV przekonwertowany w języku C# na tabelę w HTML:</h2></center></br>";

            return poczatek_pliku;
        }
    }


    class koniec_pliku_HTML
    {
        public string koniec_pliku()
        {
            string koniec_pliku = "</body>" +
                "</html>";
            return koniec_pliku;
        }
    }
}
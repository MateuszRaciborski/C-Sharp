//Base64 służy do kodowania ciągu bajtów za pomocą ciągu znaków.
//Kodowanie to przypisuje 64 wybranym znakom wartości od 0 do 63.
//Ciąg bajtów poddawany kodowaniu dzielony jest na grupy po 3 bajty.
//Ponieważ bajt ma 8 bitów, grupa 3 bajtów składa się z 24 bitów.
//Każdą taką grupę dzieli się następnie na 4 jednostki 6-bitowe,
//więc istnieją dokładnie 64 możliwe wartości każdej z tych jednostek.

//Jednostkom przypisywane są odpowiednie znaki na podstawie arbitralnie ustalonego kodowania.
//Jeśli rozmiar wejściowego ciągu bajtów nie jest wielokrotnością liczby 3,
//to stosowane jest dopełnianie – na końcu wynikowego ciągu dodawana
//jest taka liczba symboli dopełnienia (ang. pad), aby ten miał długość podzielną przez 4.


using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KonwerterBase64
{
    class Zwroc_bajty
    {
        public byte[] Tablica_z_bajtami(string sciezka_pliku_czytanego)
        {
            FileStream plik_czytany = File.OpenRead(sciezka_pliku_czytanego);
            byte[] bajty = System.IO.File.ReadAllBytes(sciezka_pliku_czytanego);
            plik_czytany.Close();
            return bajty;
        }
    }


    class Zwroc_string_z_danych_z_pliku
    {
        public string Tablica_danych_z_pliku(string sciezka_pliku_czytanego)
        {
            FileStream plik_czytany = File.OpenRead(sciezka_pliku_czytanego);
            string dane_z_pliku = System.IO.File.ReadAllText(sciezka_pliku_czytanego);
            return dane_z_pliku;
        }
    }


    class Konwersja
    {
        char[] tablica_znakow_base64 = new char[64]{
            'A','B','C','D','E','F','G','H','I','J','K','L','M',
            'N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m',
            'n','o','p','q','r','s','t','u','v','w','x','y','z',
            '0','1','2','3','4','5','6','7','8','9','+','/'};

        public string Konwersja_do_base64(byte[] dane_do_konwersji)
        {
            int b = 0;
            int dlugosc_danych = dane_do_konwersji.Length;
            int div = dlugosc_danych / 3;
            int mod = dlugosc_danych % 3;

            int dlugosc_danych_base64 = div * 4 + (mod == 0 ? 0 : 4);
            char[] dane_skonwertowane_do_base64 = new char[dlugosc_danych_base64];


            for (int i = 0; i < div * 3; i += 3)
            {
                dane_skonwertowane_do_base64[b] = tablica_znakow_base64[(byte)((dane_do_konwersji[i] & 0xfc) >> 2)];
                dane_skonwertowane_do_base64[b + 1] = tablica_znakow_base64[(byte)(((dane_do_konwersji[i] & 0x03) << 4) | (dane_do_konwersji[i + 1] & 0xf0) >> 4)];
                dane_skonwertowane_do_base64[b + 2] = tablica_znakow_base64[(byte)(((dane_do_konwersji[i + 1] & 0x0f) << 2) | (dane_do_konwersji[i + 2] & 0xc0) >> 6)];
                dane_skonwertowane_do_base64[b + 3] = tablica_znakow_base64[(byte)((dane_do_konwersji[i + 2] & 0x3f))];
                b += 4;
            }

            switch (mod)
            {
                case 1:
                    dane_skonwertowane_do_base64[b] = tablica_znakow_base64[(byte)((dane_do_konwersji[dlugosc_danych - 1] & 0xfc) >> 2)];
                    dane_skonwertowane_do_base64[b + 1] = tablica_znakow_base64[(byte)((dane_do_konwersji[dlugosc_danych - 1] & 0x03) << 4)];
                    dane_skonwertowane_do_base64[b + 2] = '=';
                    dane_skonwertowane_do_base64[b + 3] = '=';
                    b += 4;
                    break;

                case 2:
                    dane_skonwertowane_do_base64[b] = tablica_znakow_base64[(byte)((dane_do_konwersji[dlugosc_danych - 2] & 0xfc) >> 2)];
                    dane_skonwertowane_do_base64[b + 1] = tablica_znakow_base64[(byte)(((dane_do_konwersji[dlugosc_danych - 2] & 0x03) << 4) | (dane_do_konwersji[dlugosc_danych - 1] & 0xf0) >> 4)];
                    dane_skonwertowane_do_base64[b + 2] = tablica_znakow_base64[(byte)((dane_do_konwersji[dlugosc_danych - 1] & 0x0f) << 2)];
                    dane_skonwertowane_do_base64[b + 3] = '=';
                    b += 4;
                    break;
            }

            return new string(dane_skonwertowane_do_base64);
        }


        public byte[] Konwersja_do_txt(string plik_wynikowy)
        {

            char[] plik_tekstowy = plik_wynikowy.ToCharArray();
            int dlugosc_plik_tekstowy = plik_tekstowy.Length;


            int plik_tekstowy_czesc = plik_tekstowy.Length / 4;
            int paddingCount = 0;

            for (int x = 0; x < 2; x++)
            {
                if (plik_wynikowy[dlugosc_plik_tekstowy - x - 1] == '=')
                    paddingCount++;
            }

            int dlugosc_pliku_tekstowego_z_padding = plik_tekstowy_czesc * 3;

            byte[] bufor = new byte[dlugosc_plik_tekstowy];
            byte[] plik_po_konwersji = new byte[dlugosc_pliku_tekstowego_z_padding];

            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < dlugosc_plik_tekstowy; i++)
            {
                bufor[i] = Konwersja_liter_na_bajty(plik_tekstowy[i]);
            }

            for (int i = 0; i < plik_tekstowy_czesc; i++)
            {
                plik_po_konwersji[i * 3] = (byte)(((bufor[i * 4 + 1] & 0x30) >> 4) | (bufor[i * 4] << 2));
                plik_po_konwersji[i * 3 + 1] = (byte)(((bufor[i * 4 + 2] & 0x3c) >> 2) | ((bufor[i * 4 + 1] & 0xf) << 4));
                plik_po_konwersji[i * 3 + 2] = (byte)(bufor[i * 4 + 3] | ((bufor[i * 4 + 2] & 0x3) << 6));
            }

            int nowa_dlugosc_pliku_wynikowego = dlugosc_pliku_tekstowego_z_padding - paddingCount;

            byte[] wynik = new byte[nowa_dlugosc_pliku_wynikowego];

            for (int i = 0; i < nowa_dlugosc_pliku_wynikowego; i++)
            {
                wynik[i] = plik_po_konwersji[i];
            }

            return wynik;
        }

        public byte Konwersja_liter_na_bajty(char litera_do_konwersji)
        {
            if (litera_do_konwersji == '=')
                return 0;
            else
            {
                for (int i = 0; i < 64; i++)
                {
                    if (tablica_znakow_base64[i] == litera_do_konwersji)
                        return (byte)i;
                }
                return 0;
            }
        }
    }


    class Wyswietl_plik_w_konsoli
    {
        public string wyswietl_plik(string plik_czytany)
        {
            String linia_pliku;
            List<string> lista_linii = new List<string>();

            StreamReader czytaj = new StreamReader(plik_czytany);

            while (!czytaj.EndOfStream)
            {
                //Read line
                linia_pliku = czytaj.ReadLine();

                //Write a line of text
                Console.WriteLine(linia_pliku);
            }
            czytaj.Close();

            return "";
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //ŚCIEŻKI DO PLIKÓW
            string sciezka_do_pliku_konwertowanego_do_base64 = (@"C:\Users\Mateusz\Desktop\Programowanie sieciowe\base64\KonwerterBASE64\dane_do_kodowania\tekst.txt");
            string sciezka_do_zapisu_pliku_przekonwertowanego_do_base64 = (@"C:\Users\Mateusz\Desktop\Programowanie sieciowe\base64\KonwerterBASE64\dane_do_kodowania\zakodowany.b64");

            string sciezka_do_pliku_base64_konwertowanego_do_txt = (@"C:\Users\Mateusz\Desktop\Programowanie sieciowe\base64\KonwerterBASE64\dane_do_kodowania\zakodowany.b64");
            string sciezka_do_zapisu_pliku_przekonwertowanego_do_txt = (@"C:\Users\Mateusz\Desktop\Programowanie sieciowe\base64\KonwerterBASE64\dane_do_kodowania\zdekodowany.txt");
            //ŚCIEŻKI DO PLIKÓW


            string dzialanie_switch;
            string zakodowany_plik_do_base64;
            string plik_wejsciowy;

            //tworzenie instancji (obiektu) klas
            Zwroc_bajty dane_w_bajtach = new Zwroc_bajty();
            Konwersja konwersja_danych = new Konwersja();
            Zwroc_string_z_danych_z_pliku dane_string = new Zwroc_string_z_danych_z_pliku();
            Wyswietl_plik_w_konsoli wyswietl = new Wyswietl_plik_w_konsoli();
            //tworzenie instancji (obiektu) klas

            do
            {
                dzialanie_switch = string.Empty;
                Console.Write("Dziń dybry. To jest koder/dekoder dla Base64. Korzystaj do woli!\n");
                Console.Write("Przed rozpoczęciem zabawy możesz zmienić lokalizację plików, używanych przez program. Zrobisz to w sekcji Class Program.\n\n");
                Console.Write("k - Kodowanie pliku \nd - Dekodowanie pliku \nw - Wyjście... \n\n");
                Console.Write("Wybierz operację do wykonania: ");
                dzialanie_switch = Console.ReadLine();


                switch (dzialanie_switch)
                {
                    case "k":
                        Console.WriteLine("\nŚcieżka pliku kodowanego:\n" + sciezka_do_pliku_konwertowanego_do_base64 + "\n");
                        Console.WriteLine("Jego zawartość:");
                        plik_wejsciowy = wyswietl.wyswietl_plik(sciezka_do_pliku_konwertowanego_do_base64);

                        File.WriteAllText(
                            sciezka_do_zapisu_pliku_przekonwertowanego_do_base64,
                            zakodowany_plik_do_base64 = konwersja_danych.Konwersja_do_base64(dane_w_bajtach.Tablica_z_bajtami(sciezka_do_pliku_konwertowanego_do_base64))
                            );

                        Console.WriteLine("\nPlik zakodowany w B64:\n" + zakodowany_plik_do_base64 + "\n\nZapisano do pliku:\n" + sciezka_do_zapisu_pliku_przekonwertowanego_do_base64 + "\n\nKodowanie zakończone pomyślnie.\n\n========================================\n\n");
                        break;

                    case "d":
                        Console.WriteLine("\nŚcieżka pliku dekodowanego:\n" + sciezka_do_pliku_base64_konwertowanego_do_txt + "\n");
                        Console.WriteLine("Jego zawartość:");
                        plik_wejsciowy = wyswietl.wyswietl_plik(sciezka_do_pliku_base64_konwertowanego_do_txt);

                        System.IO.File.WriteAllBytes(
                            sciezka_do_zapisu_pliku_przekonwertowanego_do_txt.Replace(@"\", "\\"),
                            konwersja_danych.Konwersja_do_txt(dane_string.Tablica_danych_z_pliku(sciezka_do_pliku_base64_konwertowanego_do_txt))
                            );

                        Console.WriteLine("Plik zdekodowany z B64:");
                        Console.WriteLine(wyswietl.wyswietl_plik(sciezka_do_zapisu_pliku_przekonwertowanego_do_txt));
                        Console.WriteLine("\nZapisano do pliku:\n" + sciezka_do_zapisu_pliku_przekonwertowanego_do_txt + "\n\nDekodowanie zakończone pomyślnie.\n\n========================================\n\n");
                        
                        break;

                    case "w":
                        Console.WriteLine("\nŻegnaj :(\n");
                        break;
                }


            } while (dzialanie_switch != "w");

            Console.WriteLine("Naciśnij dowolny klawisz, aby zamknać to okno...");
            Console.ReadKey();
        }
    }
}

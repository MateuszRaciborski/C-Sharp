//Mateusz Raciborski, ZUT 2018/2019
//Operacje na plikach

using System;
using System.IO;
using System.Text;

namespace Operacje_na_plikach
{
    class Program
    {
        static void Main(string[] args)
        {
            //część pierwsza zadania
            //weź plik zakodowany w unicode i zapisz jako zakodowany w utf8

            Console.WriteLine(  "Dziń dybry.\nTo jest program który potrafi zmienić kodowanie pliku z formatu Unicode na ASCII.\n" +
                                "Drugą rzeczą jaką ten program potrafi to jest przeszukanie podanego folderu i podfolderów i zebranie poszukiwanych plików w jednym folderze.\n\n" +
                                "Jeśli przeczytałeś, naciśnij dowolny klawisz, by przejść dalej.");
            Console.ReadKey();
            Console.WriteLine("\n\n\nCzęść pierwsza - zmiana kodowania pliku tekstowego.\n");


            string sciezka_pliku_oryginalnego = @"pliki\plik_unicode.txt";
            string sciezka_pliku_zmienionego = @"pliki\plik_ascii.txt";

            File.Delete(sciezka_pliku_zmienionego);

            string zawartosc_pliku_oryginalnego = File.ReadAllText(sciezka_pliku_oryginalnego);
            File.WriteAllText(sciezka_pliku_zmienionego, zawartosc_pliku_oryginalnego, Encoding.UTF8);


            //info o kodowaniu pliku oryginalnego
            using (var czytaj_kodowanie = new StreamReader(sciezka_pliku_oryginalnego))
            {
                czytaj_kodowanie.Peek();
                var kodowanie = czytaj_kodowanie.CurrentEncoding;
                FileInfo info_o_pliku = new FileInfo(sciezka_pliku_oryginalnego);

                Console.WriteLine("Plik oryginalny:\nNazwa pliku: " + info_o_pliku.Name + ", kodowanie: " + kodowanie);
            }

            Console.WriteLine("Zawartość pliku w formacie Unicode:\n");
            Console.Write(zawartosc_pliku_oryginalnego.ToString());


            //info o kodowaniu pliku zmienionego
            using (var czytaj_kodowanie = new StreamReader(sciezka_pliku_zmienionego))
            {
                czytaj_kodowanie.Peek();
                var kodowanie = czytaj_kodowanie.CurrentEncoding;
                FileInfo info_o_pliku = new FileInfo(sciezka_pliku_zmienionego);

                Console.WriteLine("\n\nPlik ze zmienionym kodowaniem:\nNazwa pliku: " + info_o_pliku.Name + ", kodowanie: " + kodowanie);
            }

            string zawartosc_pliku_zmienionego = File.ReadAllText(sciezka_pliku_zmienionego);
            Console.WriteLine("Zawartość pliku w formacie ASCII:\n");
            Console.Write(zawartosc_pliku_zmienionego.ToString());


            //część druga zadania
            //z podanego folderu i podfolderów znajdź wszystkie pliki podanego rozszerzenia
            //i przenieś je do podanego folderu z nazwą "data utworzenia.rozszerzenie"
            //EDIT
            //pliki są nazywane według schematu: "ROK-MIESIĄC-DZIEŃ(utworzenia), GODZINA(utworzenia), NAZWA(pliku).ROZSZERZENIE"
            //bo gdy daty utworzenia są takie same, to pliki się nadpisują
            //EDIT

            Console.WriteLine("\n\n\n\n+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+");
            Console.WriteLine("Aby przejść dalej naciśnij dowolny klawisz.");
            Console.ReadKey();
            Console.WriteLine("\n\n\nCzęść druga programu wykonuje przeniesienie znalezionych plików do jednego folderu.\n");

            string folder_do_zebrania_plików = @"pliki\zebrane pliki";
            string folder_do_szukania_plików = @"pliki\folder z plikami";
            string wyszukiwane_rozszerzenie_pliku = ".txt";

            Console.WriteLine("Pliki będą szukane w folderze i podfolderach: " + folder_do_szukania_plików);
            Console.WriteLine("Znalezione pliki będą skopiowane do folderu: " + folder_do_zebrania_plików);
            Console.WriteLine("Wyszukiwane będą pliki o nazwie " + wyszukiwane_rozszerzenie_pliku);

            Console.WriteLine("\nAby przejść dalej naciśnij dowolny klawisz.");
            Console.ReadKey();
            Console.WriteLine("\n\n");

            //utworzenie folderu do zebrania plików
            System.IO.Directory.CreateDirectory(folder_do_zebrania_plików);

            //usunięcie plików w folderze który ma je zebrać, żeby nie było błędów "plik istnieje"
            System.IO.DirectoryInfo folder_do_zebrania_plików_info = new DirectoryInfo(folder_do_zebrania_plików);

            foreach (FileInfo file in folder_do_zebrania_plików_info.GetFiles())
            {
                file.Delete();
            }

            //skopiowanie plików z folderu głównego
            string[] folder_glowny = Directory.GetFiles(folder_do_szukania_plików, "*" + wyszukiwane_rozszerzenie_pliku);
            Console.WriteLine("\nLiczba plików txt w folderze głównym to {0}.", folder_glowny.Length);

            foreach (string sciezka_pojedynczego_pliku in folder_glowny)
            {
                string data_utworzenia_pliku = File.GetCreationTime(sciezka_pojedynczego_pliku).ToString();
                File.Copy(sciezka_pojedynczego_pliku, folder_do_zebrania_plików.Replace(@"\", @"\\") + "\\" + data_utworzenia_pliku.Replace(" ", ", ").Replace(":", ".") + ", " + Path.GetFileName(sciezka_pojedynczego_pliku), true);
            }

            //skopiowanie plików z podfolderów
            string[] podfoldery = Directory.GetDirectories(folder_do_szukania_plików);
            Console.WriteLine("Liczba folderów w folderze przeszukiwanym to {0}.", podfoldery.Length);

            foreach (string folder in podfoldery)
            {
                Console.WriteLine("\nTeraz przeszukiwany folder: " + folder);
                string[] pliki_w_podfolderach = Directory.GetFiles(folder, "*" + wyszukiwane_rozszerzenie_pliku);
                Console.WriteLine("Liczba plików txt w podfolderze to {0}.", pliki_w_podfolderach.Length);

                foreach (string sciezka_pojedynczego_pliku in pliki_w_podfolderach)
                {
                    Console.WriteLine(sciezka_pojedynczego_pliku);
                    FileInfo info_o_pliku_w_podkatalogu = new FileInfo(sciezka_pojedynczego_pliku);
                    string data_utworzenia_pliku = File.GetCreationTime(sciezka_pojedynczego_pliku).ToString();

                    File.Copy(sciezka_pojedynczego_pliku, folder_do_zebrania_plików.Replace(@"\", @"\\") + "\\" + data_utworzenia_pliku.Replace(" ", ", ").Replace(":", ".") + ", " + Path.GetFileName(sciezka_pojedynczego_pliku), true);
                }
            }

            Console.WriteLine("\n\nZakończono wyszukiwanie plików, spójrz do folderu " + folder_do_zebrania_plików);
            Console.WriteLine("\n\nNaciśnij dowolny przycisk, aby wyjść...");
            Console.ReadKey();
        }
    }
}
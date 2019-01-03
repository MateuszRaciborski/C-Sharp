using System;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;

namespace POP3
{
    class Program
    {
        int nowa_liczba_wiadomosci = 0;
        int liczba_wiadomosci = 0;


        public void polacz(string serwer_poczty, string nazwa_uzytkownika, string haslo, int port_serwera)
        {
            string pop3_gotowy_lub_nie = string.Empty;
            string odpowiedz_nazwa_uzytkownika = string.Empty;
            string odpowiedz_haslo = string.Empty;
            string odpowiedz_wiadomosci = string.Empty;
            string zamkniecie_polaczenia = string.Empty;
            try
            {

                TcpClient client = new TcpClient();
                client.Connect(serwer_poczty, port_serwera);

                SslStream ssl = new SslStream(client.GetStream());
                ssl.AuthenticateAsClient(serwer_poczty);

                using (StreamReader sr = new StreamReader(ssl))
                {
                    using (StreamWriter sw = new StreamWriter(ssl))
                    {

                        //odpowiedź od serwera POP3 gotowy do pracy lub nie
                        pop3_gotowy_lub_nie = sr.ReadLine();
                        Console.WriteLine("POP3 gotowość: " + pop3_gotowy_lub_nie);

                        //podanie nazwy użytkownika i wyczyszczenie strumienia
                        sw.WriteLine("USER " + nazwa_uzytkownika);
                        sw.Flush();
                        odpowiedz_nazwa_uzytkownika = sr.ReadLine();
                        Console.WriteLine("Nazwa użytkownika: " + odpowiedz_nazwa_uzytkownika);

                        //podanie hasła
                        sw.WriteLine("PASS " + haslo);
                        sw.Flush();
                        odpowiedz_haslo = sr.ReadLine();
                        Console.WriteLine("Hasło: " + odpowiedz_haslo + "\n");

                        //sprawdzenie wiadomości //można tu dać inne polecenie, na przykład LIST, RETR, TOP
                        sw.WriteLine("UIDL");
                        sw.Flush();

                        nowa_liczba_wiadomosci = 0;

                        while ((odpowiedz_wiadomosci = sr.ReadLine()) != null)
                        {
                            if (odpowiedz_wiadomosci == "." || odpowiedz_wiadomosci.IndexOf("-ERR") != -1)
                            {
                                break;
                            }

                            if (odpowiedz_wiadomosci != "+OK")
                            {
                                nowa_liczba_wiadomosci++;
                            }
                            Console.WriteLine("Odebrana poczta: " + odpowiedz_wiadomosci);
                        }


                        //sprawdzenie czy są nowe wiadomości
                        if (liczba_wiadomosci == 0)
                        {
                            liczba_wiadomosci = nowa_liczba_wiadomosci;
                        }
                        else if (nowa_liczba_wiadomosci > liczba_wiadomosci)
                        {
                            Console.WriteLine("Otrzymano nową pocztę.");
                            liczba_wiadomosci = nowa_liczba_wiadomosci;
                        }
                        else if (nowa_liczba_wiadomosci <= liczba_wiadomosci)
                        { }

                        Console.WriteLine("Aktualna liczba wiadomości: " + liczba_wiadomosci + "\n");

                        sw.WriteLine("QUIT");
                        sw.Flush();
                        zamkniecie_polaczenia = sr.ReadLine();
                        Console.WriteLine("Zamknięcie połączenia: " + zamkniecie_polaczenia + "\nAby zamknąć to okno, naciśnij dowolny przycisk.\n" + "+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+\n");
                    }
                }
            }
            catch (InvalidOperationException err)
            {
                Console.WriteLine("Error: " + err.ToString());
            }
        }



        static void Main(string[] args)
        {
            string[] POP3_config = File.ReadAllLines(@"C:\Users\Mateusz\Desktop\Programowanie sieciowe\pop3\POP3\POP3\POP3.config");

            //string info = POP3_config[0];
            string serwer_poczty        = POP3_config[1];
            string nazwa_uzytkownika    = POP3_config[2];
            string haslo                = POP3_config[3];
            int port_serwera            = Int32.Parse(POP3_config[4]);
            int czas_odswiezania        = Int32.Parse(POP3_config[5]);

            Console.WriteLine("Witaj w prymitywnym programie do odbioru wiadomości mailowych za pomocą POP3.\nMateusz Raciborski 2019.\n");

            Program POP3 = new Program();
            Task.Run(async () =>
            {
                while (true)
                {
                    POP3.polacz(serwer_poczty, nazwa_uzytkownika, haslo, port_serwera);
                    await Task.Delay(czas_odswiezania);
                }
            });

            Console.ReadKey();
        }
    }
}
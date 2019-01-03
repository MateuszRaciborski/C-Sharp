using System;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;

namespace SMTP
{
    class ClientSMTP
    {
        public string KonwertujDoBase64(string tekst)
        {
            byte[] bajty = Encoding.UTF8.GetBytes(tekst);
            string tekst_skonwertowany = Convert.ToBase64String(bajty);
            return tekst_skonwertowany;
        }

        public void SMTP_wyslij_wiadomosc(string nazwa_uzytkownika, string haslo, string serwer, int port, string adres_odbiorcy, string temat_wiadomosci, string tresc_wiadomosci)
        {
            string odpowiedzi_serwera = string.Empty;
            DateTime dzisiejsza_data = DateTime.Today;

            TcpClient klient_tcp = new TcpClient();
            klient_tcp.Connect(serwer, port);

            SslStream ssl = new SslStream(klient_tcp.GetStream());
            ssl.AuthenticateAsClient(serwer);

            using (StreamReader sr = new StreamReader(ssl))
            {
                using (StreamWriter sw = new StreamWriter(ssl))
                {
                    //przywitanie (rozszerzone funkcje SMTP)
                    sw.WriteLine("EHLO " + serwer);
                    sw.Flush();

                    //metoda logowania PLAIN, może być też LOGIN
                    sw.WriteLine("AUTH PLAIN " + KonwertujDoBase64(string.Format("\0" + nazwa_uzytkownika + "\0" + haslo)));
                    sw.Flush();

                    sw.WriteLine("MAIL FROM:<" + nazwa_uzytkownika + ">");
                    sw.Flush();

                    sw.WriteLine("RCPT TO:<" + adres_odbiorcy + ">");
                    sw.Flush();


                    //treść wiadomości
                    sw.WriteLine("DATA ");
                    sw.WriteLine("DATE: " + dzisiejsza_data.ToString());
                    sw.Flush();

                    sw.WriteLine("FROM: " + nazwa_uzytkownika);
                    sw.Flush();

                    sw.WriteLine("TO: " + adres_odbiorcy);
                    sw.Flush();

                    sw.WriteLine("SUBJECT: " + temat_wiadomosci);
                    sw.Flush();

                    sw.WriteLine(tresc_wiadomosci);
                    sw.Flush();

                    sw.WriteLine(".");
                    sw.Flush();



                    sw.WriteLine("QUIT");
                    sw.Flush();

                    while ((odpowiedzi_serwera = sr.ReadLine()) != null)
                    {

                        if (odpowiedzi_serwera == "." || odpowiedzi_serwera.IndexOf("-ERR") != -1)
                        {
                            break;
                        }

                        if (odpowiedzi_serwera.IndexOf("-ERR") != -1)
                        {
                            break;
                        }
                        Console.WriteLine("Serwer mówi: " + odpowiedzi_serwera);
                    }

                    Console.WriteLine("\n=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=\n");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] dane_konfiguracyjne = File.ReadAllLines("SMTP.config");
            string dzialanie_switch;
            string temat_wiadomosci;

            //dane konfiguracyjne
            string info = dane_konfiguracyjne[0];
            string serwer = dane_konfiguracyjne[1];
            string nazwa_uzytkownika = dane_konfiguracyjne[2];
            string haslo = dane_konfiguracyjne[3];
            int port = Int32.Parse(dane_konfiguracyjne[4]);
            int czas_odswiezania = Int32.Parse(dane_konfiguracyjne[5]);
            string adres_nadawcy, adres_odbiorcy, tresc_wiadomosci;

            //wysyłanie
            ClientSMTP klient_SMTP = new ClientSMTP();
            do
            {
                Console.WriteLine("Wybierz działanie:");
                Console.WriteLine("s - wyślij wiadomość;");
                Console.WriteLine("q - zamknij program.");

                dzialanie_switch = string.Empty;
                dzialanie_switch = Console.ReadLine();
                switch (dzialanie_switch)
                {
                    case "s":
                        Console.Write("\nPodaj adres email odbiorcy: ");
                        adres_odbiorcy = Console.ReadLine();
                        adres_nadawcy = nazwa_uzytkownika;
                        Console.Write("Podaj temat wiadomości: ");
                        temat_wiadomosci = Console.ReadLine();
                        Console.Write("Podaj treść wiadomości: ");
                        tresc_wiadomosci = Console.ReadLine();

                        klient_SMTP.SMTP_wyslij_wiadomosc(nazwa_uzytkownika, haslo, serwer, port, adres_odbiorcy, temat_wiadomosci, tresc_wiadomosci);
                        break;

                    case "q":
                        Console.WriteLine("Żegnaj na zawsze :(");
                        break;
                }
            } while (dzialanie_switch != "q");

            Console.ReadKey();
        }
    }
}
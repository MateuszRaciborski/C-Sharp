//LOGIN 2b2012ob17
//HASŁO Mateusz123
//SERWER ftp://cba.pl
//FUNKCJE ZE STRONY https://www.codeproject.com/Tips/443588/%2FTips%2F443588%2FSimple-Csharp-FTP-Class
/*if(){host = "ftp://cba.pl";
                        user = "2b2012ob17";
                        pass = "Mateusz123";
                        directory = "/mateuszcsharp.cba.pl/mateusz";

                        Console.WriteLine("\nLISTA FOLDERÓW\n");

                        //JEDNO ZAPYTANIE DO FTP
                        FtpWebRequest ftpRequest3 = (FtpWebRequest)WebRequest.Create(host + directory);
                        ftpRequest3.Method = WebRequestMethods.Ftp.PrintWorkingDirectory; //wyświetla aktualną lokację, gdzie jestem
                        ftpRequest3.Credentials = new NetworkCredential(user, pass);
                        FtpWebResponse response3 = (FtpWebResponse)ftpRequest3.GetResponse();
                        Stream responseStream3 = response3.GetResponseStream();
                        StreamReader streamReader = new StreamReader(responseStream3);
                        Console.WriteLine(streamReader.ReadToEnd());
                        Console.WriteLine($"Directory List Complete, status {response3.StatusDescription}");
                        streamReader.Close();
                        response3.Close();
                        //JEDNO ZAPYTANIE DO FTP}*/

using System;
using System.IO;
using System.Linq;
using System.Net;

namespace FTP
{
    class Program
    {
        static void Main(string[] args)
        {
            //ŚCIEŻKI DO PLIKÓW
            //string sciezka_do_pliku_wgrywanego = (@"pliki\plik_do_wgrania.txt");
            string sciezka_do_pliku_wgrywanego = (@"pliki\png.png");
            //string sciezka_do_pliku_pobieranego = (@"pliki\plik_ściągnięty.txt");
            string sciezka_do_pliku_pobieranego = (@"pliki\png_download.png");

            Console.WriteLine("LOGOWANIE DO FTP\n");
            /* Create Object Instance */
            FTP klientFTP = new FTP("ftp://cba.pl", "2b2012ob17", "Mateusz123");

            string aktualna_lokacja = "/";
            string wybor = "";
            do
            {

                Console.WriteLine("\n\nWybierz działanie na serwerze FTP:");
                Console.WriteLine("1 - wyświetl zawartość katalogu roboczego");
                Console.WriteLine("2 - wyświetl zawartość katalogu roboczego ze szczegółami");
                Console.WriteLine("3 - zmiana katalogu roboczego");
                Console.WriteLine("4 - stwórz katalog");
                Console.WriteLine("5 - usuń katalog");
                Console.WriteLine("6 - wgraj plik na serwer");
                Console.WriteLine("7 - pobierz plik z serwera");
                Console.WriteLine("8 - usuń plik z serwera");
                Console.WriteLine("9 - zmień nazwę pliku");
                Console.WriteLine("10 - pokaż rozmiar pliku");
                Console.WriteLine("11 - wyświetla datę i godzinę utworzenia pliku");
                Console.WriteLine("12 - wyświetl zawartość katalogu głównego");
                Console.WriteLine("q - zamknij program.");

                Console.WriteLine("\nTWÓJ AKTUALNY FOLDER ROBOCZY TO: " + aktualna_lokacja);
                Console.WriteLine("\nTwój wybór: ");
                wybor = string.Empty;
                wybor = Console.ReadLine();
                Console.WriteLine(" ");

                switch (wybor)
                {
                    case "1":
                        Console.WriteLine("ZAWARTOŚĆ KATALOGU ROBOCZEGO:\n");
                        string[] prostaListaKatalogu = klientFTP.prostaListaKatalogu(aktualna_lokacja);
                        for (int i = 0; i < prostaListaKatalogu.Count(); i++) { Console.WriteLine(prostaListaKatalogu[i]); }
                        break;


                    case "2":
                        Console.WriteLine("ZAWARTOŚĆ KATALOGU ROBOCZEGO ZE SZCZEGÓŁAMI:\n");
                        string[] szczegolowaListaKatalogu = klientFTP.szczegolowaListaKatalogu(aktualna_lokacja);
                        for (int i = 0; i < szczegolowaListaKatalogu.Count(); i++) { Console.WriteLine(szczegolowaListaKatalogu[i]); }
                        break;


                    case "3":
                        Console.WriteLine("\nZMIANA AKTUALNEGO KATALOGU ROBOCZEGO:\nPodaj pełną ścieżkę katalogu do którego chcesz przejść (podawanie zakończ ukośnikiem /):");
                        aktualna_lokacja = Console.ReadLine();
                        break;


                    case "4":
                        Console.WriteLine("STWORZENIE KATALOGU:\nPodaj nazwę katalogu, który zostanie utworzony w katalogu roboczym:\n");
                        string nazwa_nowego_katalogu = Console.ReadLine();
                        klientFTP.stworzKatalog(aktualna_lokacja + nazwa_nowego_katalogu);
                        break;


                    case "5":
                        Console.WriteLine("USUNIĘCIE KATALOGU:\nPodaj nazwę katalogu, który zostanie usunięty z katalogu roboczego:\n");
                        string nazwa_katalogu = Console.ReadLine();
                        klientFTP.usunKatalog(aktualna_lokacja + nazwa_katalogu);
                        break;


                    case "6":
                        Console.WriteLine("WGRANIE PLIKU:\nPodaj nazwę pliku, jaką będzie on miał na serwerze:\n");
                        string nazwa_pliku_nowy = Console.ReadLine();
                        klientFTP.wgraj(aktualna_lokacja + nazwa_pliku_nowy, sciezka_do_pliku_wgrywanego);
                        break;


                    case "7":
                        Console.WriteLine("POBIERANIE PLIKU:\nPodaj nazwę pliku znajdującego się w folderze roboczym, który chcesz pobrać:");
                        string nazwa_pliku_pobieranego = Console.ReadLine();
                        klientFTP.pobierz(aktualna_lokacja + nazwa_pliku_pobieranego, sciezka_do_pliku_pobieranego);
                        break;


                    case "8":
                        Console.WriteLine("USUNIĘCIE PLIKU:\n");
                        string nazwa_pliku_usuwanego = Console.ReadLine();
                        klientFTP.usun(aktualna_lokacja + nazwa_pliku_usuwanego);
                        break;


                    case "9":
                        Console.WriteLine("ZMIANA NAZWY PLIKU:\nPodaj pełną nazwę pliku któremu chcesz zmienić nazwę:\n");
                        string nazwa_pliku_zmienianego = Console.ReadLine();
                        Console.WriteLine("Podaj pełną nową nazwę pliku:\n");
                        string nowa_nazwa = Console.ReadLine();
                        klientFTP.zmienNazwe(aktualna_lokacja + nazwa_pliku_zmienianego, nowa_nazwa);
                        break;


                    case "10":
                        Console.WriteLine("ROZMIAR PLIKU:\n");
                        string nazwa_pliku_rozmiar = Console.ReadLine();
                        string rozmiarPliku = klientFTP.pobierzRozmiarPliku(aktualna_lokacja + nazwa_pliku_rozmiar);
                        Console.WriteLine(rozmiarPliku);
                        break;


                    case "11":
                        Console.WriteLine("DATA I GODZINA UTWORZENIA PLIKU:\nPodaj nazwę pliku:\n");
                        string nazwa_pliku_czas = Console.ReadLine();
                        string plikDataCzas = klientFTP.pobierzCzasDataUtworzeniaPliku(aktualna_lokacja + nazwa_pliku_czas);
                        Console.WriteLine(plikDataCzas);
                        break;


                    case "12":
                        Console.WriteLine("ZAWARTOŚĆ KATALOGU GŁÓWNEGO /:\n");
                        prostaListaKatalogu = klientFTP.szczegolowaListaKatalogu("/");
                        for (int i = 0; i < prostaListaKatalogu.Count(); i++) { Console.WriteLine(prostaListaKatalogu[i]); }
                        break;


                    case "q":
                        klientFTP = null;
                        Console.WriteLine("Żegnaj na zawsze :(");
                        break;
                }
            } while (wybor != "q");

            Console.WriteLine("Naciśnij dowolny klawisz, aby zamknać to okno...");
            Console.ReadKey();
        }
    }

    class FTP
    {
        private string host = null;
        private string user = null;
        private string pass = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        private int bufferSize = 2048;

        /* Construct Object */
        public FTP(string hostIP, string userName, string password) { host = hostIP; user = userName; pass = password; }

        /* Download File */
        public void pobierz(string remoteFile, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                ftpStream = ftpResponse.GetResponseStream();
                /* Open a File Stream to Write the Downloaded File */
                FileStream localFileStream = new FileStream(localFile, FileMode.Open, FileAccess.ReadWrite);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Upload File */
        public void wgraj(string remoteFile, string localFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                FileStream localFileStream = new FileStream(localFile, FileMode.Open, FileAccess.ReadWrite);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Delete File */
        public void usun(string deleteFile)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Rename File */
        public void zmienNazwe(string currentFileNameAndPath, string newFileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + currentFileNameAndPath);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                /* Rename the File */
                ftpRequest.RenameTo = newFileName;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Create a New Directory on the FTP Server */
        public void stworzKatalog(string newDirectory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + newDirectory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        public void usunKatalog(string directory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }

        /* Get the Date/Time a File was Created */
        public string pobierzCzasDataUtworzeniaPliku(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { fileInfo = ftpReader.ReadToEnd(); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Created Date Time */
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* Get the Size of a File */
        public string pobierzRozmiarPliku(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string fileInfo = null;
                /* Read the Full Response Stream */
                try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return File Size */
                return fileInfo;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return "";
        }

        /* List Directory Contents File/Folder Name Only */
        public string[] prostaListaKatalogu(string directory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }

        /* List Directory Contents in Detail (Name, Size, Created, etc.) */
        public string[] szczegolowaListaKatalogu(string directory)
        {
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                try { string[] directoryList = directoryRaw.Split("|".ToCharArray()); return directoryList; }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Return an Empty string Array if an Exception Occurs */
            return new string[] { "" };
        }
    }
}
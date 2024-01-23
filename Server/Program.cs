using MusicalyAdminApp.API.APISQL;
using MusicalyAdminApp.API.APISQL.Taules;
using MusicPlayerLibrary.Crypto;
using MusicPlayerLibrary.GestioPDF;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

class ServerProgram
{
    static int PORT = 999;
    static TcpListener listener;
    static TcpClient ActualClient;
    static String PublicKeyString;
    static String ListaPedida;
    static String PDFEncriptado = "C:\\Users\\Moha\\source\\PDFCrypted.pdf";
    static String PDFSignat = "C:\\Users\\Moha\\source\\PDFsignat.pdf";
    static String RutaPublicKey = "C:\\Users\\Moha\\source\\PublicKeyFile.pem";
    static String RutaCertificado = "C:\\Users\\Moha\\source\\certMuhammad.pfx";
    static String CertPass = "1234";

    public static void Main()
    {
        listener = new TcpListener(IPAddress.Any, PORT);
        listener.Start();
        Console.WriteLine("Socket Inciado y escuchando...");

        try
        {
            ActualClient = listener.AcceptTcpClient();
            Console.WriteLine("Conexió correcta amb el client");

            Listener();

            // Generar datos para el PDF
            GenerarPDFAsync().GetAwaiter().GetResult();

            // Encriptar PDF
            var AESKey = Encryption.EncryptPDF(PDFSignat, PDFEncriptado);

            // Retornar PDF amb consulta i encriptada amb clau pública
            Sender(PDFEncriptado, AESKey);


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            // Seveix per tancar el programa
            listener.Stop();
        }
    }

    private static async Task GenerarPDFAsync()
    {
        try
        {
            // Obtener la lista de canciones de forma asíncrona
            Apisql api = new Apisql();
            string jsonString = "";

            if (ListaPedida.Equals("1"))
            {
                List<Song> songs = await api.GetSongs();
                var data = new
                {
                    Cancions = songs
                };

                // Serializar el objeto a una cadena JSON
                jsonString = JsonSerializer.Serialize(data);

            }
            else if (ListaPedida.Equals("2"))
            {
                List<Extension> extensions = await api.GetExtensions();
                var data = new
                {
                    Extensions = extensions
                };

                // Serializar el objeto a una cadena JSON
                jsonString = JsonSerializer.Serialize(data);
            }
            else if (ListaPedida.Equals("3"))
            {
                List<PlayList> playlists = await api.GetPlaylists();
                var data = new
                {
                    Playlists = playlists
                };

                // Serializar el objeto a una cadena JSON
                jsonString = JsonSerializer.Serialize(data);
            }
            else if (ListaPedida.Equals("4"))
            {
                List<Instrument> instruments = await api.GetInstruments();
                var data = new
                {
                    Instruments = instruments
                };

                // Serializar el objeto a una cadena JSON
                jsonString = JsonSerializer.Serialize(data);
            }
            else
            {
                var data = new
                {
                    Error = "No existeix opció!"
                };
            }

            // Crear PDF
            CreatePDF.CrearPDFSignat(PDFSignat, jsonString, CertPass, RutaCertificado);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar el PDF: {ex.Message}");
        }
    }

    private static void Sender(string rutaArxiu, byte[] aeskey)
    {
        try
        {
            // Obtenir la referencia del fluxa de xarxa del client
            NetworkStream networkStream = ActualClient.GetStream();
            networkStream.Write(aeskey, 0, aeskey.Length);

            // Llegir l'arxiu en blocs i enviarlo el client
            using (FileStream fileStream = File.OpenRead(rutaArxiu))
            {
                int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                int bytesRead;
                
                // Enviar l'arxiu del client en blocs
                while ((bytesRead = fileStream.Read(buffer, 0, bufferSize)) > 0)
                {
                    networkStream.Write(buffer, 0, bytesRead);
                }
            }

            Console.WriteLine($"Archivo {rutaArxiu} encriptado enviado exitosamente.");

            // Tancar la conexió
            networkStream.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al procesar cliente: {ex.Message}");
        }
    }

    private static void Listener()
    {
        try
        {
            // Obtenir la referencia al fluxa de xarxa del cliente
            NetworkStream networkStream = ActualClient.GetStream();

            // Inicializar el tamany del buffer
            const int bufferSize = 1_024;
            byte[] buffer = new byte[bufferSize];
            int bytesRead = networkStream.Read(buffer, 0, bufferSize);

            if(bytesRead > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string[] messageSplited = message.Split("|");
                ListaPedida = messageSplited[0];
                PublicKeyString = messageSplited[1];
                Console.Write("Clave Recibida! La clave publica es: " + PublicKeyString);
                Console.Write("La lista que se ha pedido es: " + ListaPedida);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en el listener del servidor: {ex.Message}");
        }
    }
}
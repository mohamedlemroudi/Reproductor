using MusicPlayerLibrary.Crypto;
using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;

class ClientProgram
{

    static void Main()
    {
        //CreatePairKey();
        ConnectToServer();
    }

    static void ConnectToServer()
    {
        try
        {
            TcpClient client = new TcpClient("172.23.2.155", 2009); // Debes especificar la misma IP y puerto que el servidor
                                                                    //Console.WriteLine("Conectado al servidor.");

            string publicKeyFilePath = "Dades\\clave_publica.txt";
            RSA publicKey = RSACrypt.LoadPublicKey(publicKeyFilePath);

            // Convierte la clave pública a una cadena
            string publicKeyString = Convert.ToBase64String(publicKey.ExportRSAPublicKey());

            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // Lee la cadena enviada por el servidor
                string serverMessage = reader.ReadLine();
                Console.WriteLine($"{serverMessage}");

                // Envía la clave pública al servidor
                writer.WriteLine(publicKeyString);
                writer.Flush();

                // Lee el contenido del archivo PDF cifrado
                /*string pdfContent = reader.ReadLine();
                RecivePDF(pdfContent);*/
            }

            // Cierra la conexión con el servidor
            //client.Close();
        }
        catch (Exception e)
        {
            // Maneja las excepciones según sea necesario
            Console.WriteLine($"Error al conectar al servidor: {e.Message}");
        }
    }

    private static void RecivePDF(string? pdfContent)
    {
        if (!string.IsNullOrEmpty(pdfContent))
        {
            byte[] pdfData = Convert.FromBase64String(pdfContent);
            string pdfFilePath = "Dades\\PDFrebut.pdf";
            File.WriteAllBytes(pdfFilePath, pdfData);
            Console.WriteLine("Archivo PDF recibido exitosamente.");
        }
        else
        {
            Console.WriteLine("Error: PDF content received is null or empty.");
        }
    }

    static void CreatePairKey()
    {
        string publicKeyFilePath = "Dades\\clave_publica.txt";
        string privateKeyFilePath = "Dades\\clave_privada.txt";

        try
        {
            // Llama a este método para generar un nuevo par de claves y guardarlas en archivos
            RSACrypt.GenerateAndSaveKeyPair("Dades\\clave_publica.txt", "Dades\\clave_privada.txt");

            Console.WriteLine("Claus generades i guardades exitosament.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
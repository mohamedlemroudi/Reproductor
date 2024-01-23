using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MusicPlayerLibrary.Crypto
{
    public class Encryption
    {
        static public string certRute = "C:\\Users\\Moha\\source\\certMuhammad.pfx";
        static public string certPass = "1234";
        static public string PublicKeyRoute = "C:\\Users\\Moha\\source\\PublicKeyFile";

        public static byte[] EncryptPDF(string rutaPDF, string PDFEncriptado)
        {
            try
            {
                // Obtenir la clau pública RSA des del certificat
                var pk = RSACrypt.LoadPublicKey(PublicKeyRoute).ExportParameters(false);

                // Encriptar la clau AES amb la clau pública RSA
                X509Certificate2 certificat = new X509Certificate2(certRute, certPass);

                // Llegir el contingut del arxiu PDF
                byte[] contingutPDF = File.ReadAllBytes(rutaPDF);
                byte[] clauAesEncriptada;

                // Encriptar el contingut del PDF amb AES
                using (FileStream fsInput = new FileStream(rutaPDF, FileMode.Open))
                {
                    using (FileStream fsOutput = new FileStream(PDFEncriptado, FileMode.Create))
                    {
                        using (AesManaged aes = new AesManaged())
                        {
                            aes.GenerateKey();
                            byte[] aesIV = new byte[16];
                            aes.IV = aesIV;
                            clauAesEncriptada = RSACrypt.EncryptAESKey(aes.Key, pk);
                            // Perform encryption
                            ICryptoTransform encryptor = aes.CreateEncryptor();
                            using (CryptoStream cs = new CryptoStream(fsOutput, encryptor, CryptoStreamMode.Write))
                            {
                                fsInput.CopyTo(cs);
                            }
                        }
                    }
                }

                Console.WriteLine("Documento encriptado con éxito.");

                // Devolver les clau generades
                return clauAesEncriptada;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al encriptar el PDF: {ex.Message}");
                throw;
            }
        }

        public static void DecryptPDF(string rutaPDF, string PDFDesencriptado, byte[] claveAesEncriptada, string certRuta, string certPass)
        {
            try
            {
                X509Certificate2 certificado = new X509Certificate2(certRuta, certPass);
                byte[] claveAesDesencriptada = RSACrypt.DecryptAESKeyWithPrivateKey(claveAesEncriptada, certificado);
                byte[] contenidoEncriptadoAES = File.ReadAllBytes(rutaPDF);

                using (FileStream fsInput = new FileStream(rutaPDF, FileMode.Open))
                {
                    using (FileStream fsOutput = new FileStream(PDFDesencriptado, FileMode.Create))
                    {
                        using (AesManaged aes = new AesManaged())
                        {

                            aes.Key = claveAesDesencriptada;
                            byte[] aesIV = new byte[16];
                            aes.IV = aesIV;


                            ICryptoTransform decryptor = aes.CreateDecryptor();
                            using (CryptoStream cs = new CryptoStream(fsOutput, decryptor, CryptoStreamMode.Write))
                            {
                                fsInput.CopyTo(cs);
                            }
                        }
                    }
                }

                Console.WriteLine("Documento desencriptado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al desencriptar el PDF: {ex.Message}");
                throw;
            }
        }
    }
}

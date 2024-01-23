using iText.Bouncycastle.Crypto;
using iText.Bouncycastle.X509;
using iText.Commons.Bouncycastle.Cert;
using iText.Forms.Fields.Properties;
using iText.Forms.Form.Element;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Signatures;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerLibrary.Crypto
{
    public class Sign
    {
        #region Private attributes

        private X509Certificate2? certificate;
        private Certificates.CertificateInfo? certificateInfo;
        private Pkcs12Store pkcs12Store = new Pkcs12StoreBuilder().Build();
        private string storeAlias = "";

        #endregion

        /// <summary>
        /// Init class certificate attributes with the disk certificate
        /// </summary>
        /// <param name="pfxFileName">Certificate file disk path</param>
        /// <param name="pfxPassword">Certificate password</param>
        public void InitCertificate(string pfxFileName, string pfxPassword)
        {
            certificate = new X509Certificate2(pfxFileName, pfxPassword);

            pkcs12Store.Load(new FileStream(pfxFileName, FileMode.Open, FileAccess.Read), pfxPassword.ToCharArray());
            foreach (string currentAlias in pkcs12Store.Aliases)
            {
                if (pkcs12Store.IsKeyEntry(currentAlias))
                {
                    storeAlias = currentAlias;
                    break;
                }
            }
            certificateInfo = Certificates.CertificateInfo.FromCertificate(pfxFileName, pfxPassword);
        }

        /// <summary>
        /// Sign pdf document and save result to disk.
        /// This method puts digital signature inside pdf document
        /// </summary>
        /// <param name="inputFileName">Input pdf file path to sign</param>
        /// <param name="outputFileName">Ouput pdf file path to save the result file</param>
        /// <param name="showSignature">If signatature is visible in pdf document</param>
        public void SignPdf(string inputFileName, string outputFileName, bool showSignature)
        {
            AsymmetricKeyParameter key = pkcs12Store.GetKey(storeAlias).Key;

            X509CertificateEntry[] chainEntries = pkcs12Store.GetCertificateChain(storeAlias);
            IX509Certificate[] chain = new IX509Certificate[chainEntries.Length];
            for (int i = 0; i < chainEntries.Length; i++)
                chain[i] = new X509CertificateBC(chainEntries[i].Certificate);
            PrivateKeySignature signature = new PrivateKeySignature(new PrivateKeyBC(key), "SHA256");

            using (PdfReader pdfReader = new PdfReader(inputFileName))
            using (FileStream result = File.Create(outputFileName))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                if (showSignature)
                {
                    CreateSignatureApperanceField(pdfSigner);
                }

                pdfSigner.SignDetached(signature, chain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);
            }
        }

        /// <summary>
        /// Sign filedisk file with the global class certificate
        /// </summary>
        /// <param name="inputFileName">Filedisk input file path to sign</param>
        /// <param name="outputFileName">Filedisk output file path to save the result</param>
        public void SignFile(string inputFileName, string outputFileName)
        {
            if (certificate != null)
            {
                byte[] inputBytes = File.ReadAllBytes(inputFileName);
                byte[] outputBytes = SignDocument(certificate, inputBytes);

                File.WriteAllBytes(outputFileName, outputBytes);
            }
        }

        /// <summary>
        /// Returns SHA-256 HASH from input byte array
        /// </summary>
        /// <param name="input">Input byte array to obtain SHA-256 HASH</param>
        /// <returns>SHA-256 HASH</returns>
        public string SHA256Hash(byte[] input)
        {
            using (SHA256 sHA256 = SHA256.Create())
            {

                var r = sHA256.ComputeHash(input);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < r.Length; i++)
                {
                    builder.Append(r[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Sign byte array document with the certificate
        /// </summary>
        /// <param name="certificate">Certificated used to sign the document</param>
        /// <param name="document">Document byte array to sign</param>
        /// <returns>Byte array with the signed document</returns>
        internal static byte[] SignDocument(X509Certificate2 certificate, byte[] document)
        {
            ContentInfo contentInfo = new ContentInfo(document);
            SignedCms signedCms = new SignedCms(contentInfo, false);
            CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, certificate);
            signedCms.ComputeSignature(signer);

            return signedCms.Encode();
        }

        /// <summary>
        /// Adds signature field rectangle inside pdf document
        /// </summary>
        /// <param name="pdfSigner">PdfSigner used to sign document</param>
        internal void CreateSignatureApperanceField(PdfSigner pdfSigner)
        {
            var pdfDocument = pdfSigner.GetDocument();
            var pageRect = pdfDocument.GetPage(1).GetPageSize();
            var size = new PageSize(pageRect);
            pdfDocument.AddNewPage(size);
            var totalPages = pdfDocument.GetNumberOfPages();
            float yPos = pdfDocument.GetPage(totalPages).GetPageSize().GetHeight() - 100;
            float xPos = 0;
            Rectangle rect = new Rectangle(xPos, yPos, 200, 100);

            pdfSigner.SetFieldName("signature");

            SignatureFieldAppearance appearance = new SignatureFieldAppearance(pdfSigner.GetFieldName())
                    .SetContent(new SignedAppearanceText()
                        .SetSignedBy(certificateInfo?.Organization)
                        .SetReasonLine("" + " - " + "")
                        .SetLocationLine("Location: " + certificateInfo?.Locality)
                        .SetSignDate(pdfSigner.GetSignDate()));

            pdfSigner.SetPageNumber(totalPages).SetPageRect(rect)
                    .SetSignatureAppearance(appearance);

        }
        /// <summary>
        ///     Function used to return the certificate info of a signed PDF
        /// </summary>
        /// <param name="pdfPath">Pdf file path</param>
        public int CertificateFromPdf(string pdfPath)
        {
            try
            {

                using (PdfReader pdfReader = new PdfReader(pdfPath))
                {
                    PdfDocument pdfDocument = new PdfDocument(pdfReader);
                    SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
                    foreach (string signatureName in signatureUtil.GetSignatureNames())
                    {
                        Console.WriteLine($"Signature Name: {signatureName}");

                        PdfPKCS7 pdfPKCS7 = signatureUtil.ReadSignatureData(signatureName);

                        Console.WriteLine();
                        return 1;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return -1;
            }
        }
        /// <summary>
        /// Returns SHA-256 HASH from input string
        /// </summary>
        /// <param name="input">Input string to obtain SHA-256 HASH</param>
        /// <returns>SHA-256 HASH</returns>
        public string SHA256HashFromString(string input)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            return SHA256Hash(inputBytes);
        }

        /// <summary>
        /// Signs a PDF without saving in disk
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <param name="outputFileName"></param>
        /// <param name="showSignature"></param>
        public void SignPdfStream(MemoryStream inputFileName, string outputFileName, bool showSignature)
        {
            AsymmetricKeyParameter key = pkcs12Store.GetKey(storeAlias).Key;

            X509CertificateEntry[] chainEntries = pkcs12Store.GetCertificateChain(storeAlias);
            IX509Certificate[] chain = new IX509Certificate[chainEntries.Length];
            for (int i = 0; i < chainEntries.Length; i++)
                chain[i] = new X509CertificateBC(chainEntries[i].Certificate);
            PrivateKeySignature signature = new PrivateKeySignature(new PrivateKeyBC(key), "SHA256");

            using (PdfReader pdfReader = new PdfReader(inputFileName))
            using (FileStream result = File.Create(outputFileName))
            {
                PdfSigner pdfSigner = new PdfSigner(pdfReader, result, new StampingProperties().UseAppendMode());

                if (showSignature)
                {
                    CreateSignatureApperanceField(pdfSigner);
                }

                pdfSigner.SignDetached(signature, chain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);
            }
        }

        // NOVES FUNCIONALITATS
        public void CreateSignPDF(String certPath, String certPass, String output, String PdfRoute)
        {
            using (X509Certificate2 cert = new X509Certificate2(certPath, certPass))
            {
                byte[] bytes = File.ReadAllBytes(PdfRoute);
                InitCertificate(certPath, certPass);
                SignPdf(PdfRoute, output, true);
            }
        }
    }
}

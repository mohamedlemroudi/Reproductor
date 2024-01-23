using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using MusicPlayerLibrary.Crypto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerLibrary.GestioPDF
{
    /// <summary>
    /// Class used to Create and Add information to a PDF
    /// </summary>
    public class CreatePDF
    {
        /// <summary>
        /// This function allows you to create a new PDF
        /// </summary>
        /// <param name="rutaPDF"> Path for the PDF file (.../.../PDFfile.pdf)</param>
        /// <returns></returns>
        public static string Create(string rutaPDF)
        {
            try
            {
                using (PdfWriter writer = new PdfWriter(rutaPDF))
                using (PdfDocument pdf = new PdfDocument(writer))

                return "Creat";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "Error";
            }
        }

        public static void CreatePDFWithJsonList(string rutaPdf, string jsonList)
        {
            try
            {
                dynamic jsonData = JsonConvert.DeserializeObject<ExpandoObject>(jsonList);

                // Crear el documento PDF
                using (PdfWriter writer = new PdfWriter(rutaPdf))
                {
                    using (PdfDocument pdfDocument = new PdfDocument(writer))
                    {
                        using (Document document = new Document(pdfDocument))
                        {
                            // Agregar cada propiedad del objeto dinámico al PDF de manera estructurada
                            foreach (var property in jsonData)
                            {
                                string propertyName = property.Key;
                                object propertyValue = property.Value;

                                if (propertyValue is List<object>)
                                {
                                    // Si el valor es una lista, formatearla como una lista en el PDF
                                    document.Add(new Paragraph($"{propertyName}:"));
                                    var list = (List<object>)propertyValue;
                                    foreach (var item in list)
                                    {
                                        // Mostrar cada propietat del objecta de la lista
                                        var listItem = (ExpandoObject)item;
                                        foreach (var listItemProperty in listItem)
                                        {
                                            document.Add(new ListItem($"{listItemProperty.Key}: {listItemProperty.Value}"));
                                        }
                                        document.Add(new Paragraph(""));
                                        document.Add(new Paragraph(""));
                                    }
                                }
                                else
                                {
                                    // Si no es una lista, agregar como un párrafo normal
                                    document.Add(new Paragraph($"{propertyName}: {propertyValue}"));
                                }
                            }
                        }
                    }
                }
                Console.WriteLine("PDF Creat correctament");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al crear el PDF con la lista JSON: " + e.Message);
            }
        }

        public static void CrearPDFSignat(string rutaOutputPDFSignado, string jsonList,
            string Certpass, string CertificateRoute)
        {
            try
            {
                String rutaPDF = "C:\\Users\\Moha\\source\\PDFsignat.pdf";
                CreatePDFWithJsonList(rutaPDF, jsonList);
                Sign sign = new Sign();
                sign.CreateSignPDF(CertificateRoute, Certpass, rutaOutputPDFSignado, rutaPDF);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}

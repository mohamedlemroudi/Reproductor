using MusicPlayerLibrary.Certificates;
using MusicPlayerLibrary.Crypto;
using MusicPlayerLibrary.GestioPDF;

namespace Testing
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }


        /// <summary>
        /// the test that are below this comment are the ones used to test the functions to crate the pdf,designate the info of the certificate, and then sign it 
        /// </summary>
        [Test]
        public void CrearPDF()
        {
            string rutaCompleta = AppContext.BaseDirectory+"CarpetaPDF\\InfoCançons.pdf";

            string resultado = CreatePDF.Create(rutaCompleta);
            Assert.AreEqual(resultado, "Creat");
        }
        [Test]
        public void CertificatInfoName()
        {
            // Pas 1 crear certificat and info
            var nom = "Alhuerto";
            var organitzacio = "MiCasa";
            var localitat = "Miami";

            CertificateInfo certificateInfo = new CertificateInfo();

            certificateInfo.CommonName = nom;
            certificateInfo.Organization = organitzacio;
            certificateInfo.Locality = localitat;

            Autosigned.GeneratePfx("CarpetaPDF\\certificat.pfx", "123456", certificateInfo);

            // Pas 2 cridar FromCertificate
            CertificateInfo info2 = CertificateInfo.FromCertificate("CarpetaPDF\\certificat.pfx", "123456");
            // Pas 3 compararar dades
            Assert.AreEqual(certificateInfo.CommonName, info2.CommonName);
            Assert.AreEqual(certificateInfo.Locality, info2.Locality);
        }
        [Test]
        public void CertificatInfoOrganització()
        {
            //Utilizant la info de CertificatInfoName()
            var organitzacio = "MiCasa";

            CertificateInfo info = CertificateInfo.FromCertificate("CarpetaPDF\\certificat.pfx", "123456");

            Assert.AreEqual(organitzacio, info.Organization);
        }
        [Test]
        public void SignarPDF()
        {
            bool visible = true;
            Sign s = new Sign();
            s.InitCertificate("CarpetaPDF\\certificat.pfx", "123456");
            s.SignPdf("CarpetaPDF\\InfoCançons.pdf", "CarpetaPDF\\fitxerfirmat.pdf", visible);

            Sign s2 = new Sign();
            int validador = s2.CertificateFromPdf("CarpetaPDF\\fitxerfirmat.pdf");

            Assert.AreEqual(1, validador);

        }
    }
}
using ApiMongoMusica.Classes.Models;
using mba.BooksLibrary.Client.Exceptions;
using mla.ApiMusica.Model;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace mba.BooksLibrary.Client
{
    /*class Program
    {
        const string APIRULBASE = "http://localhost:5050/api/v1/";
        const string APIRULLIBRARY = APIRULBASE + "Libraries";
        static async Task Main()
        {
            
            for(int i=1;i<=20;i++) {
                Library library = new Library();
                library.Name = "Gran d'Olot - " + i; 
                await CreateLibrary(library);
            }
            
            await GetAllLibraries(0,9);
        }

        static async Task CreateLibrary(Library library)
        {
            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage response = null;
                try {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(library), System.Text.Encoding.UTF8, "application/json");
                    response = await client.PostAsync(APIRULLIBRARY, content);
                } 
                catch (HttpRequestException ex) { new ApiCallException("Error en la petició HTTP", ex); }
                catch (Exception ex) { new ApiCallException("Error ", ex); }

                if (response.IsSuccessStatusCode) {
                    string result = await response.Content.ReadAsStringAsync();
                    // S'hauria de comprovar que el resultat és vàlid
                }
                else {
                    string result = await response.Content.ReadAsStringAsync();
                    new ApiLibraryException(result);
                }
                
            }
        }
        static async Task GetAllLibraries(int start, int limit)
        {
            using (HttpClient client = new HttpClient()) {
                try {
                    HttpResponseMessage response = await client.GetAsync(APIRULLIBRARY + "/start/" + start + "/limit/" + limit);

                    if (response.IsSuccessStatusCode) {
                        string content = await response.Content.ReadAsStringAsync();
                        content = content.Replace(",", "\n");
                        Console.WriteLine(content);
                    }
                    else throw new ApiLibraryException("Error en la petició HTTP. Ruta no vàlida. Codi d'estat: " + response.StatusCode);
                }
                catch (HttpRequestException ex)
                {
                    new ApiCallException("Error en la petició HTTP", ex);
                }
                catch (Exception ex)
                {
                    new ApiCallException("Error ", ex);
                }
            }
        }
    }*/

    /*class Program
    {
        const string APIRULBASE = "http://localhost:5180/api/";
        const string APIRULLAUDIO = APIRULBASE + "Audio";

        static async Task Main()
        {
            string rutaMusica = "C:\\Users\\Moha\\source\\x.mp3";  // Reemplaza con la ruta de tu archivo de música
            await CreateAudio(rutaMusica);
        }

        static async Task CreateAudio(string rutaMusica)
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] contenidoMusica = File.ReadAllBytes(rutaMusica);
                //string nombreArchivo = Path.GetFileName(rutaMusica);

                Audio audio = new Audio
                {
                    Name = "Nombre del Audio",  // Reemplaza con el nombre deseado
                    Content = contenidoMusica
                };

                HttpResponseMessage response = null;
                try
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(audio), Encoding.UTF8, "application/json");
                    //response = await client.PostAsync(APIRULLAUDIO, content);
                    response = await client.PostAsJsonAsync(APIRULLAUDIO, audio);

                }
                catch (HttpRequestException ex)
                {
                    new ApiCallException("Error en la petición HTTP", ex);
                }
                catch (Exception ex)
                {
                    new ApiCallException("Error ", ex);
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    // Verifica que el resultado sea válido si es necesario
                }
                else
                {
                    string result = await response?.Content.ReadAsStringAsync();
                    new ApiLibraryException(result ?? "Error en la petición HTTP");
                }
            }
        }
    }*/


    /*class Program
    {
        const string APIRULBASE = "http://localhost:5180/api/";
        const string APIRULLAUDIO = APIRULBASE + "Audio";

        static async Task Main()
        {
            // GET: Obtener todos los audios
            await GetAllAudios();
        }

        static async Task GetAllAudios()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = null;
                try
                {
                    // Realizar solicitud GET para obtener todos los audios
                    response = await client.GetAsync(APIRULLAUDIO);
                }
                catch (HttpRequestException ex)
                {
                    new ApiCallException("Error en la petición HTTP", ex);
                }
                catch (Exception ex)
                {
                    new ApiCallException("Error ", ex);
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    // Leer la respuesta como una lista de nombres de archivos
                    List<string> audioNames = await response.Content.ReadFromJsonAsync<List<string>>();

                    // Guardar todos los archivos en la carpeta especificada
                    if (audioNames != null && audioNames.Any())
                    {
                        foreach (var audioName in audioNames)
                        {
                            await DownloadAndSaveAudio(audioName);
                        }
                    }
                }
                else
                {
                    string result = await response?.Content.ReadAsStringAsync();
                    new ApiLibraryException(result ?? "Error en la petición HTTP");
                }
            }
        }

        static async Task DownloadAndSaveAudio(string audioName)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = null;
                try
                {
                    // Realizar solicitud GET para obtener el contenido del audio
                    response = await client.GetAsync($"{APIRULLAUDIO}/{audioName}");
                }
                catch (HttpRequestException ex)
                {
                    new ApiCallException("Error en la petición HTTP", ex);
                }
                catch (Exception ex)
                {
                    new ApiCallException("Error ", ex);
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    // Guardar el contenido del audio en el archivo local
                    byte[] audioContent = await response.Content.ReadAsByteArrayAsync();
                    string filePath = Path.Combine(@"C:\Users\Moha\source\repos", $"{audioName}.mp3");
                    File.WriteAllBytes(filePath, audioContent);
                }
                else
                {
                    string result = await response?.Content.ReadAsStringAsync();
                    new ApiLibraryException(result ?? "Error en la petición HTTP");
                }
            }
        }
    }*/

    /*class Program
    {
        const string APIMONGOMUSIC = "http://localhost:5042/api/";
        const string APILLETRES = APIMONGOMUSIC + "Lletres";

        static async Task Main()
        {
            await GetAllLletres();
        }

        private static async Task GetAllLletres()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await client.GetAsync(APILLETRES);
                }
                catch (HttpRequestException ex)
                {
                    new ApiCallException("Error en la petición HTTP", ex);
                }
                catch (Exception ex)
                {
                    new ApiCallException("Error ", ex);
                }

                if (response != null && response.IsSuccessStatusCode)
                {
                    List<Lletres> lletres = await response.Content.ReadFromJsonAsync<List<Lletres>>();
                    if (lletres != null && lletres.Any())
                    {
                        foreach (var lletra in lletres)
                        {
                            Console.WriteLine(lletra.Lyrics);
                        }
                    }
                }
                else
                {
                    string result = await response?.Content.ReadAsStringAsync();
                    new ApiLibraryException(result ?? "Error en la petición HTTP");
                }
            }
        }
    }*/

    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json; // Asegúrate de tener esta referencia para la serialización/deserialización JSON

    class Program
    {
        static async Task Main()
        {
            // Crear una instancia de la letra que deseas enviar
            Lletres nuevaLetra = new Lletres
            {
                Lyrics = "Aquí van las letras de la canción..."
                // Puedes configurar otros campos según la estructura de tu clase Lletres
            };

            // URL de la API de MongoDB donde realizarás el POST
            string apiUrl = "http://localhost:5042/api/Lletres";

            // Convertir la letra a formato JSON
            string jsonLetra = JsonConvert.SerializeObject(nuevaLetra);

            // Configurar la solicitud HTTP POST
            using (HttpClient client = new HttpClient())
            using (HttpContent content = new StringContent(jsonLetra, Encoding.UTF8, "application/json"))
            {
                // Realizar la solicitud POST
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta (si es necesario)
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Solicitud POST exitosa. Respuesta: " + responseBody);
                }
                else
                {
                    Console.WriteLine("Error en la solicitud POST. Código de estado: " + response.StatusCode);
                }
            }
        }
    }

}
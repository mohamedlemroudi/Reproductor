using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using MusicalyAdminApp.API.APISQL.Taules;
using Newtonsoft.Json;


namespace MusicalyAdminApp.API.APISQL
{

    public class Apisql: IDisposable
    {
        private readonly HttpClient client;

        /// <summary>
        ///  creation to the connection to the api that has acces to the Database
        /// </summary>
        public Apisql()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5095/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// method to do an asyncronous get request and later return the response as a string
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error en la solicitud GET: {ex.Message}");
                throw; 
            }
        }

        /// <summary>
        /// methot to get all the songs that we have on the database 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Song>> GetSongs()
        {
            try
            {
                string endpoint = "api/Song/";
                string songsJson = await GetAsync(endpoint);

                // Deserializa el JSON a una lista de objetos Song
                var songs = JsonConvert.DeserializeObject<List<Song>>(songsJson);

                return songs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener canciones: {ex.Message}");
                return new List<Song>();
            }
        }

        /// <summary>
        /// This method performs an HTTP PUT request, handle success or failure, and return the content of the response as a string.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="jsonContent"></param>
        /// <returns></returns>
        public async Task<string> PutAsync(string endpoint, string jsonContent)
        {
            try
            {
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error en la solicitud PUT: {ex.Message}");
                throw; // Puedes lanzar una excepción personalizada si lo prefieres
            }
        }

        /// <summary>
        /// uses the putasync method to make a put ont the table song
        /// </summary>
        /// <param name="updatedSong"></param>
        /// <returns></returns>
        public async Task<string> PutSong(Song updatedSong)
        {
            try
            {
                string endpoint = $"api/Song/{updatedSong.UID}";
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(updatedSong);
                return await PutAsync(endpoint, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la canción: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// with the getasync performs a get from the table extension and shows all of them 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Extension>> GetExtensions()
        {
            try
            {
                string endpoint = "api/extension/";

                string extensionsJson = await GetAsync(endpoint);

                // Deserializa el JSON a una lista de objetos Song
                var extensions = JsonConvert.DeserializeObject<List<Extension>>(extensionsJson);

                return extensions;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener extensiones: {ex.Message}");
                return new List<Extension>();
            }
        }

        /// <summary>
        /// sends the respective "UID" of the song and then uses the getasync method to get the data of that especific extension
        /// </summary>
        /// <param name="extensionId"></param>
        /// <returns></returns>
        public async Task<string> GetExtensionById(int extensionId)
        {
            try
            {
                string endpoint = $"api/extension/{extensionId}";
                return await GetAsync(endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la extensión: {ex.Message}");
                return string.Empty;
            }
        }
        /// <summary>
        ///  in this methodwe use it to update 
        /// </summary>
        /// <param name="updatedExtension"></param>
        /// <returns></returns>
        public async Task<string> PutExtension(Extension updatedExtension)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(updatedExtension.Name))
                {
                    Console.WriteLine("El nombre de la extensión no puede estar vacío o nulo.");
                    return string.Empty;
                }
                string endpoint = $"api/extensions/{updatedExtension.Name}";
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(updatedExtension);
                return await PutAsync(endpoint, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la extensión: {ex.Message}");
                return string.Empty;
            }
        }
        // taula playlist

        /// <summary>
        ///  method to get all the data of all playlists 
        /// </summary>
        /// <returns></returns>
        public async Task<List<PlayList>> GetPlaylists()
        {
            try
            {
                string endpoint = "api/playlist/";

                string playlistsJson = await GetAsync(endpoint);

                // Deserializa el JSON a una lista de objetos Song
                var playlists = JsonConvert.DeserializeObject<List<PlayList>>(playlistsJson);

                return playlists;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener playlists: {ex.Message}");
                return new List<PlayList>();
            }
        }

        /// <summary>
        /// mthod to get a playlist by its id
        /// </summary>
        /// <param name="dispositiu"></param>
        /// <param name="playlistName"></param>
        /// <returns></returns>
        public async Task<string> GetPlaylistById(string dispositiu, string playlistName)
        {
            try
            {
                // El endpoint podría ser algo como "api/playlists/{Dispositiu}/{PlaylistName}" para obtener una playlist específica
                string endpoint = $"api/playlists/{Uri.EscapeDataString(dispositiu)}/{Uri.EscapeDataString(playlistName)}";
                return await GetAsync(endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la playlist: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// method to update data on the database 
        /// </summary>
        /// <param name="updatedPlaylist"></param>
        /// <returns></returns>
        public async Task<string> PutPlaylist(PlayList updatedPlaylist)
        {
            try
            {
                // El endpoint podría ser algo como "api/playlists/{Dispositiu}/{PlaylistName}" para actualizar una playlist específica
                string endpoint = $"api/playlists/{Uri.EscapeDataString(updatedPlaylist.Dispositiu)}/{Uri.EscapeDataString(updatedPlaylist.PlaylistName)}";
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(updatedPlaylist);
                return await PutAsync(endpoint, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la playlist: {ex.Message}");
                return string.Empty;
            }
        }


        //taula  instruments 

        /// <summary>
        /// method to get all the instruments
        /// </summary>
        /// <returns></returns>
        public async Task<List<Instrument>> GetInstruments()
        {
            try
            {
                string endpoint = "api/instrument/";

                string instrumentsJson = await GetAsync(endpoint);

                // Deserializa el JSON a una lista de objetos Song
                var instruments = JsonConvert.DeserializeObject<List<Instrument>>(instrumentsJson);

                return instruments;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener instrumentos: {ex.Message}");
                return new List<Instrument>();
            }
        }

        /// <summary>
        /// method to get an instrument by its name 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<string> GetInstrumentByName(string name)
        {
            try
            {
                // El endpoint podría ser algo como "api/instruments/{Name}" para obtener un instrumento específico
                string endpoint = $"api/instruments/{Uri.EscapeDataString(name)}";
                return await GetAsync(endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el instrumento: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        ///  method to modify the data of an instrument 
        /// </summary>
        /// <param name="updatedInstrument"></param>
        /// <returns></returns>
        public async Task<string> PutInstrument(Instrument updatedInstrument)
        {
            try
            {
                // Verifica que el nombre del instrumento no sea nulo ni vacío
                if (string.IsNullOrWhiteSpace(updatedInstrument.Name))
                {
                    Console.WriteLine("El nombre del instrumento no puede estar vacío o nulo.");
                    return string.Empty;
                }

                // El endpoint podría ser algo como "api/instruments/{Name}" para actualizar un instrumento específico
                string endpoint = $"api/instruments/{Uri.EscapeDataString(updatedInstrument.Name)}";
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(updatedInstrument);
                return await PutAsync(endpoint, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el instrumento: {ex.Message}");
                return string.Empty;
            }
        }


        // taula band 

        /// <summary>
        ///  method to get all the bands
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetBands()
        {
            try
            {
                string endpoint = "api/bands";
                return await GetAsync(endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener bandas: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// method to get a band by its name
        /// </summary>
        /// <param name="bandName"></param>
        /// <returns></returns>
        public async Task<string> GetBandByName(string bandName)
        {
            try
            {
                // El endpoint podría ser algo como "api/bands/{BandName}" para obtener una banda específica
                string endpoint = $"api/bands/{Uri.EscapeDataString(bandName)}";
                return await GetAsync(endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la banda: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// method to update the data of a band 
        /// </summary>
        /// <param name="updatedBand"></param>
        /// <returns></returns>
        public async Task<string> PutBand(Band updatedBand)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(updatedBand.Name))
                {
                    Console.WriteLine("El nombre de la banda no puede estar vacío o nulo.");
                    return string.Empty;
                }

                // El endpoint podría ser algo como "api/bands/{Name}" para actualizar una banda específica
                string endpoint = $"api/bands/{Uri.EscapeDataString(updatedBand.Name)}";
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(updatedBand);
                return await PutAsync(endpoint, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar la banda: {ex.Message}");
                return string.Empty;
            }
        }

        // tabla musician

        /// <summary>
        /// method to get all the musicians 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMusicians()
        {
            try
            {
                string endpoint = "api/musicians";
                return await GetAsync(endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener músicos: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// method to get a musician by its name 
        /// </summary>
        /// <param name="musicianName"></param>
        /// <returns></returns>
        public async Task<string> GetMusicianByName(string musicianName)
        {
            try
            {
                // El endpoint podría ser algo como "api/musicians/{Name}" para obtener un músico específico
                string endpoint = $"api/musicians/{Uri.EscapeDataString(musicianName)}";
                return await GetAsync(endpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el músico: {ex.Message}");
                return string.Empty;
            }
        }
        /// <summary>
        /// method to update the data of a musician 
        /// </summary>
        /// <param name="updatedMusician"></param>
        /// <returns></returns>
        public async Task<string> PutMusician(Musician updatedMusician)
        {
            try
            {
                // El endpoint podría ser algo como "api/musicians/{Name}" para actualizar un músico específico
                string endpoint = $"api/musicians/{Uri.EscapeDataString(updatedMusician.Name)}";
                string jsonContent = System.Text.Json.JsonSerializer.Serialize(updatedMusician);
                return await PutAsync(endpoint, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el músico: {ex.Message}");
                return string.Empty;
            }
        }
        public void Dispose()
        {
            client.Dispose();
        }

    }
}

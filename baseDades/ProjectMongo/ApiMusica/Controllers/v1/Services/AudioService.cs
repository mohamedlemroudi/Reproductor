using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;

namespace mla.ApiMusica.Services
{
    public class AudioService
    {
        private readonly GridFSBucket _gridFs;

        public AudioService(IMongoDatabase database)
        {
            _gridFs = new GridFSBucket(database);
        }

        public async Task<ObjectId> GuardarAudioAsync(string name, byte[] content)
        {
            return await _gridFs.UploadFromBytesAsync(name, content);
        }

        public async Task<byte[]> ObtenerAudioAsync(string name)
        {
            var filtro = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, name);
            var archivo = await _gridFs.Find(filtro).FirstOrDefaultAsync();

            if (archivo != null)
            {
                using (var stream = await _gridFs.OpenDownloadStreamAsync(archivo.Id))
                {
                    var memoria = new MemoryStream();
                    await stream.CopyToAsync(memoria);
                    return memoria.ToArray();
                }
            }

            return null;
        }

        public async Task<List<string>> ObtenerTodosLosNombresDeAudiosAsync()
        {
            var names = await _gridFs.Find(Builders<GridFSFileInfo>.Filter.Empty)
                                        .ToListAsync();

            return names.Select(x => x.Filename.ToString()).ToList();
        }
    }
}

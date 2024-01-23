using ApiMongoMusica.Classes.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiMongoMusica.Classes.Services
{
    public class LletresService
    {
        private readonly IMongoCollection<Lletres> _lletra;

        public LletresService(IOptions<MongoMusicInfoSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            if (string.IsNullOrEmpty(settings.Value.LletresCollectionName))
            {
                throw new ArgumentNullException(nameof(settings.Value.LletresCollectionName), "El nombre de la colección no puede ser nulo o vacío.");
            }

            _lletra = database.GetCollection<Lletres>(settings.Value.LletresCollectionName);

        }

        public async Task<List<Lletres>> GetAsync() =>
            await _lletra.Find(_ => true).ToListAsync();
        
        public async Task<Lletres> GetAsync(string id) =>
            await _lletra.Find(l => l.Id == id).FirstOrDefaultAsync();
        
        public async Task<Lletres> CreateAsync(Lletres lletra)
        {
            await _lletra.InsertOneAsync(lletra);
            return lletra;
        }

        public async Task UpdateAsync(string id, Lletres lletraIn) =>
            await _lletra.ReplaceOneAsync(l => l.Id == id, lletraIn);
        
        public async Task RemoveAsync(Lletres lletraIn) =>
            await _lletra.DeleteOneAsync(l => l.Id == lletraIn.Id);
    }
}
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiMongoMusica.Classes.Models
{
    public class Lletres
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Lyrics { get; set; }

        public Lletres()
        {
            // Inicializar Lyrics con un valor predeterminado o vacío
            Lyrics = string.Empty;
        }
    }
}
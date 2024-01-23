using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;


namespace mba.BooksLibrary.Client.Exceptions {
    class ApiLibraryException : Exception {
        public string Category { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public ApiLibraryException() : base() { }
        public ApiLibraryException(string message) : base(message) { 
            
            ApiLibraryException error = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiLibraryException>(message);
            Console.WriteLine(error.Category + " - " + error.Code + " - " + error.Message);
        }
        public ApiLibraryException(string message,Exception innerException) : base(message,innerException) { 
            // A partir d'un objecte JSON, creem un objecte anònim amb les propietats que ens interessen i les assignem a un objecte anònim
            object objectJson = Newtonsoft.Json.JsonConvert.DeserializeObject(message);
            var json = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(objectJson.ToString(), new { Category="", Code = 0, Errmsg = "" } );
            Console.WriteLine(message + " - " + innerException.Message);
        }
    }

}
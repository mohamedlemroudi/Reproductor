namespace mla.ApiMusica.Classes {
    public class ApiMusicaDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;

        public string AudioCollectionName { get; set; } = null!;
    }
}
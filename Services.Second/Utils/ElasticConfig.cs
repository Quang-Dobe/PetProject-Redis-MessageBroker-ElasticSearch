namespace Services.Second.Utils
{
    public class ElasticConfig
    {
        public ElasticConfig(IConfiguration configuration)
        {
            Url = configuration["ElasticConfig:Url"];
            UserName = configuration["ElasticConfig:UserName"];
            Password = configuration["ElasticConfig:Password"];
            CertificateFingerprint = configuration["ElasticConfig:CertificateFingerprint"];
        }

        public string Url { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string CertificateFingerprint { get; set; }
    }
}

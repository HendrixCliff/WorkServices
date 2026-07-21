namespace WorkServices.Infrastructure.Persistence.Configurations;


public sealed class PaystackSettings
{
      public string SecretKey { get; set; } = string.Empty;

    public string PublicKey { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = string.Empty;

    public string CallbackUrl { get; set; } = string.Empty;
}
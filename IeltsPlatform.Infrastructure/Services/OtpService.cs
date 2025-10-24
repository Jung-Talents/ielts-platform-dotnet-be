namespace IeltsPlatform.Infrastructure.Services;

public interface IOtpService
{
    string GenerateOtp();
}

public class OtpService : IOtpService
{
    private readonly Random _random = new Random();

    public string GenerateOtp()
    {
        return _random.Next(100000, 999999).ToString();
    }
}

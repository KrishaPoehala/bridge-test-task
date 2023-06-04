namespace Bridge.Infrastructure.Options;

public class RateLimiterOptions
{
   public bool AutoReplenishment { get; set; }
   public int PermitLimit { get; set; }
   public int QueueLimit { get; set; }
   public int WindowInSeconds { get; set; }
}

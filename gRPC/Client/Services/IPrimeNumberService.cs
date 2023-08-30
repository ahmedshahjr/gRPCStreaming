namespace Client.Services
{
    public interface IPrimeNumberService
    {
        Task HasPrimeNumber(CancellationToken ct);
    }
}

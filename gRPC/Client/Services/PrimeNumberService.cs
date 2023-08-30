using Client_Protos;
using Grpc.Core;

namespace Client.Services
{
    public class PrimeNumberService : IPrimeNumberService
    {
        private readonly PrimeNumberProtoStreaming.PrimeNumberProtoStreamingClient _client;
        public PrimeNumberService(PrimeNumberProtoStreaming.PrimeNumberProtoStreamingClient client)
        {
            _client = client;
        }
        public async Task HasPrimeNumber(CancellationToken ct)
        {
            var failedRequest = new List<string>();
            try
            {

                long id = 1;
                double startTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;


                using (var call = _client.HasPrimerNumber(headers: new Metadata { new Metadata.Entry("requestid", $"{id}") }))
                {

                    var responseTask = Task.Run(async () =>
                    {
                        await foreach (var message in call.ResponseStream.ReadAllAsync(ct))
                        {
                            double currentTime = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                            var totalTimeDifference = (currentTime - startTime) % 60;
                            Console.WriteLine($"Reponse for id: {message.Id} with request number {message.Number} is prime number{message.Isprimenumber}. total round trip {totalTimeDifference} in seconds.");
                        }
                    });

                    while (!ct.IsCancellationRequested)
                    {
                        Random rnd = new Random();
                        await call.RequestStream.WriteAsync(new PrimeNumberRequest
                        {
                            Id = id,
                            Timestamp = startTime,
                            Number = rnd.Next(1, 10000),
                        });
                        id++;
                    }
                    await call.RequestStream.CompleteAsync();
                    //   await responseTask;

                }
            }
            catch (RpcException rpcException) when (rpcException.StatusCode == StatusCode.Unavailable)
            {
                failedRequest.Add(rpcException.ToString());
            }
            finally
            {

                Console.WriteLine(string.Join("/n,", failedRequest));
                failedRequest.Clear();
            }
        }
    }
}

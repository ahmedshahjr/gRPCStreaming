using Grpc.Core;
using gRPC_Server_Protos;

namespace Server.Services
{
    public class PrimeNumberService : PrimeNumberProtoStreaming.PrimeNumberProtoStreamingBase
    {
        public PrimeNumberService()
        {

        }
        public async override Task HasPrimerNumber(
         IAsyncStreamReader<PrimeNumberRequest> requestStream,
         IServerStreamWriter<PrimeNumberResponse> responseStream,
         ServerCallContext context)
        {
            var primeNumbers = new List<long>();
            var requestId = context.RequestHeaders.Single(x => x.Key == "requestid").Value;
            var received = 0;
            // Create a timer that ticks every second
            System.Threading.Timer timer = new System.Threading.Timer(PrintPrimeNumber, primeNumbers, 0, 1000);

            try
            {

                while (await requestStream.MoveNext())
                {
                    received++;
                    if (context.CancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    var responseMessage = new PrimeNumberResponse
                    {
                        Id = requestStream.Current.Id,
                        Number = requestStream.Current.Number,
                        Timestamp = requestStream.Current.Timestamp,
                    };
                    responseMessage.Isprimenumber = IsPrime(requestStream.Current.Number);
                    if (responseMessage.Isprimenumber)
                    {
                        primeNumbers.Add(requestStream.Current.Number);
                    }
                    await responseStream.WriteAsync(responseMessage);
                }

            }
            catch (RpcException ex)
            {
                throw;

            }
            finally
            {
                Console.WriteLine($"Total Request Received are {received}");
                timer.Dispose();
            }

        }
        private void PrintPrimeNumber(object state)
        {
            var primeNumbers = (List<long>)state;
            if (primeNumbers.Count >= 10)
            {
                var topTenPrimeNumbers = primeNumbers
                      .OrderByDescending(x => x)
                      .Distinct()
                      .Take(10)
                      .ToList();
                var primeNumber = string.Join(",", topTenPrimeNumbers);
                Console.WriteLine($"Top Ten Prime Numbers are {primeNumber}");
                primeNumbers.Clear();
            }
        }
        public static bool IsPrime(long number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }
    }
}

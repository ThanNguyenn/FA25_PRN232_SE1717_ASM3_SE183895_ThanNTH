
using EVServiceCenter.gRPCService.ThanNTH.Protos;
using Grpc.Net.Client;

Console.WriteLine("Hello, World!");

var channel = GrpcChannel.ForAddress("https://localhost:7296");

var grpcClient = new CenterPartThanNthGRPC.CenterPartThanNthGRPCClient(channel);

Console.WriteLine("Get all items");

var centerParts = grpcClient.GetAllAsync(new EmptyRequest());

if (centerParts.Items.Count > 0)
{
    foreach (var item in centerParts.Items)
    {
        Console.WriteLine($"{item.CenterPartThanNthid}-{item.Description}-{item.PartId}");
    }
}
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

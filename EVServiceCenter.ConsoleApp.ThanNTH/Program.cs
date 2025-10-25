using EVServiceCenter.gRPCService.ThanNTH.Protos;
using Grpc.Net.Client;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.BackgroundColor = ConsoleColor.Black;
Console.Clear();

using var channel = GrpcChannel.ForAddress("https://localhost:7296");
var grpcClient = new CenterPartThanNthGRPC.CenterPartThanNthGRPCClient(channel);

bool exit = false;

while (!exit)
{
    ShowMenu();
    Console.Write(">> Select an option (1-6): ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            GetAllCenterParts();
            break;
        case "2":
            GetCenterPartById();
            break;
        case "3":
            CreateCenterPart();
            break;
        case "4":
            UpdateCenterPart();
            break;
        case "5":
            DeleteCenterPart();
            break;
        case "6":
            if (ConfirmExit())
            {
                RunShutdownSequence();
                exit = true;
            }
            break;
        default:
            Console.WriteLine("Invalid choice. Try again.");
            break;
    }

    if (!exit)
    {
        Console.WriteLine("\n>> Press any key to return to menu...");
        Console.ReadKey();
        Console.Clear();
    }
}

void ShowMenu()
{
    DrawHeader();
    Console.WriteLine("1. Get all center parts");
    Console.WriteLine("2. Get center part by ID");
    Console.WriteLine("3. Create center part");
    Console.WriteLine("4. Update center part");
    Console.WriteLine("5. Delete center part");
    Console.WriteLine("6. Exit");
    Console.WriteLine();
}

void GetAllCenterParts()
{
    Console.Clear();
    DrawHeader();

    var progressTask = Task.Run(() => ProgressBar("Fetching all center parts: "));

    var response = grpcClient.GetAllAsync(new EmptyRequest());

    progressTask.Wait();

    Thread.Sleep(500);
    Console.WriteLine();
    if (response.Items.Count == 0)
    {
        Console.WriteLine("No items found.");
        return;
    }

    foreach (var item in response.Items)
    {
        Console.WriteLine($"Id: {item.CenterPartThanNthid} - Description: {item.Description} - Part ID: {item.PartId}");
    }
}

void GetCenterPartById()
{
    Console.Clear();
    DrawHeader();
    Console.Write(">> Enter ID: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var progressTask = Task.Run(() => ProgressBar("Fetching center part: "));

    var response = grpcClient.GetByIdAsync(new CenterPartThanNthIdRequest { CenterPartThanNthid = id });

    progressTask.Wait();

    Thread.Sleep(500);

    Console.WriteLine();

    if (response == null || response.CenterPartThanNthid == 0)
    {
        Console.WriteLine("Item not found.");
        return;
    }

    Console.WriteLine($"Id: {response.CenterPartThanNthid} - Description: {response.Description} - Part ID: {response.PartId}");
}
void CreateCenterPart()
{
    Console.Clear();
    DrawHeader();
    var newPart = new CenterPartThanNth();

    Console.Write(">> Enter Part ID (integer): ");
    newPart.PartId = int.TryParse(Console.ReadLine(), out int partId) ? partId : 0;

    Console.Write(">> Enter Center ID (integer): ");
    newPart.CenterId = int.TryParse(Console.ReadLine(), out int centerId) ? centerId : 0;

    Console.Write(">> Enter Available Quantity (integer): ");
    newPart.AvailableQuantity = int.TryParse(Console.ReadLine(), out int availableQty) ? availableQty : 0;

    Console.Write(">> Enter Minimum Quantity (integer): ");
    newPart.MinimumQuantity = int.TryParse(Console.ReadLine(), out int minQty) ? minQty : 0;

    Console.Write(">> Enter Description: ");
    newPart.Description = Console.ReadLine();

    Console.Write(">> Enter Part Status: ");
    newPart.PartStatus = Console.ReadLine();

    newPart.IsDeleted = false;
    newPart.CreateDate = DateTime.UtcNow.ToString("O");
    newPart.UpdateDate = DateTime.UtcNow.ToString("O");

    var progressTask = Task.Run(() => ProgressBar("Creating center part: "));
    var response = grpcClient.CreateAsync(newPart);
    progressTask.Wait(); 
    Thread.Sleep(500);

    Console.WriteLine();

    if (response.Result == 1)
    {
        Console.WriteLine("Center part created successfully.");
    }
    else
    {
        Console.WriteLine($"Failed to create center part. Result code: {response.Result}");
    }
}

void UpdateCenterPart()
{
    Console.Clear();
    DrawHeader();
    Console.Write("$ Enter ID of the center part to update: ");

    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var progressTask = Task.Run(() => ProgressBar("Fetching existing part: "));
    var existingPart = grpcClient.GetByIdAsync(new CenterPartThanNthIdRequest { CenterPartThanNthid = id });
    progressTask.Wait();
    Thread.Sleep(500);
    Console.WriteLine();

    if (existingPart == null || existingPart.CenterPartThanNthid == 0)
    {
        Console.WriteLine("Item not found.");
        return;
    }

    Console.WriteLine("Current Center Part Details:");
    Console.WriteLine($"ID: {existingPart.CenterPartThanNthid}");
    Console.WriteLine($"Description: {existingPart.Description}");
    Console.WriteLine($"Available Qty: {existingPart.AvailableQuantity}");
    Console.WriteLine($"Minimum Qty: {existingPart.MinimumQuantity}");
    Console.WriteLine($"Part Status: {existingPart.PartStatus}");
    Console.WriteLine();

    Console.Write(">> Do you want to update this center part? (y/n): ");
    var confirm = Console.ReadLine()?.Trim().ToLower();
    if (confirm != "y" && confirm != "yes")
    {
        Console.WriteLine("Update cancelled.");
        return;
    }

    Console.Write(">> Enter new Description (leave blank to keep current): ");
    var desc = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(desc)) existingPart.Description = desc;

    Console.Write(">> Enter new Available Quantity (leave blank to keep current): ");
    var qty = Console.ReadLine();
    if (int.TryParse(qty, out int newQty)) existingPart.AvailableQuantity = newQty;

    Console.Write(">> Enter new Minimum Quantity (leave blank to keep current): ");
    var min = Console.ReadLine();
    if (int.TryParse(min, out int newMin)) existingPart.MinimumQuantity = newMin;

    Console.Write(">> Enter new Part Status (leave blank to keep current): ");
    var status = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(status)) existingPart.PartStatus = status;

    existingPart.UpdateDate = DateTime.UtcNow.ToString("O");

    progressTask = Task.Run(() => ProgressBar("Updating center part: "));
    var response = grpcClient.UpdateAsync(existingPart);
    progressTask.Wait();
    Thread.Sleep(500);
    Console.WriteLine();

    if (response.Result == 1)
    {
        Console.WriteLine("Center part updated successfully.");
    }
    else
    {
        Console.WriteLine($"Failed to update center part. Result code: {response.Result}");
    }
}


void DeleteCenterPart()
{
    Console.Clear();
    DrawHeader();

    Console.Write(">> Enter ID of the center part to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var progressTask = Task.Run(() => ProgressBar("Fetching existing part: "));
    var existingPart = grpcClient.GetByIdAsync(new CenterPartThanNthIdRequest { CenterPartThanNthid = id });
    progressTask.Wait();
    Thread.Sleep(500);
    Console.WriteLine();

    if (existingPart == null || existingPart.CenterPartThanNthid == 0)
    {
        Console.WriteLine("No center part found with that ID.");
        return;
    }

    Console.WriteLine("Current Center Part Details:");
    Console.WriteLine($"ID: {existingPart.CenterPartThanNthid}");
    Console.WriteLine($"Description: {existingPart.Description}");
    Console.WriteLine($"Part ID: {existingPart.PartId}");
    Console.WriteLine($"Center ID: {existingPart.CenterId}");
    Console.WriteLine($"Available Qty: {existingPart.AvailableQuantity}");
    Console.WriteLine($"Minimum Qty: {existingPart.MinimumQuantity}");
    Console.WriteLine($"Status: {existingPart.PartStatus}");
    Console.WriteLine();

    Console.Write(">> Are you sure you want to delete this record? (y/n): ");
    var confirm = Console.ReadLine()?.Trim().ToLower();

    if (confirm != "y" && confirm != "yes")
    {
        Console.WriteLine("Deletion cancelled.");
        return;
    }

    progressTask = Task.Run(() => ProgressBar("Deleting center part: "));
    var response = grpcClient.DeleteAsync(new CenterPartThanNthIdRequest { CenterPartThanNthid = id });
    progressTask.Wait();
    Thread.Sleep(500);
    Console.WriteLine();

    if (response.Result == 1)
    {
        Console.WriteLine("Center part deleted successfully.");
    }
    else
    {
        Console.WriteLine($"Failed to delete center part. Result code: {response.Result}");
    }
}

bool ConfirmExit()
{
    Console.Clear();
    DrawHeader();
    Console.Write(">> Are you sure you want to exit? (y/n): ");
    var confirmExit = Console.ReadLine()?.Trim().ToLower();
    if (confirmExit != "y" && confirmExit != "yes")
    {
        Console.WriteLine("Exit cancelled.");
        Thread.Sleep(500);
        return false;
    }
    return true;
}

void RunShutdownSequence()
{
    Console.Clear();
    string[] shutdownLogs = new string[]
    {
        "Shutting down services...",
        "Closing database connections...",
        "Terminating gRPC channels...",
        "Saving user session...",
        "Clearing caches...",
        "Powering off subsystems...",
        "Goodbye!"
    };

    Random rand = new Random();

    foreach (var line in shutdownLogs)
    {
        Console.WriteLine(line);
        Thread.Sleep(rand.Next(500, 701));
    }

    Console.ResetColor();
}

void DrawHeader()
{
    Console.ForegroundColor = ConsoleColor.DarkMagenta;
    Console.WriteLine(@"
███████╗██╗   ██╗ ██████╗███████╗███╗   ██╗████████╗███████╗██████╗ 
██╔════╝██║   ██║██╔════╝██╔════╝████╗  ██║╚══██╔══╝██╔════╝██╔══██╗
█████╗  ██║   ██║██║     █████╗  ██╔██╗ ██║   ██║   █████╗  ██████╔╝
██╔══╝  ╚██╗ ██╔╝██║     ██╔══╝  ██║╚██╗██║   ██║   ██╔══╝  ██╔══██╗
███████╗ ╚████╔╝ ╚██████╗███████╗██║ ╚████║   ██║   ███████╗██║  ██║
╚══════╝  ╚═══╝   ╚═════╝╚══════╝╚═╝  ╚═══╝   ╚═╝   ╚══════╝╚═╝  ╚═╝
");
    Console.WriteLine("             CENTER PART MANAGEMENT SYSTEM v3.9");
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
}

void ProgressBar(string message)
{
    var rnd = new Random();
    int time = rnd.Next(1500, 2500);

    Console.Write($"{message} [");
    int totalBlocks = 20;
    for (int i = 0; i < totalBlocks; i++)
    {
        Console.Write("█");
        Thread.Sleep(time / totalBlocks);
    }
    Console.WriteLine("] Done.");
}

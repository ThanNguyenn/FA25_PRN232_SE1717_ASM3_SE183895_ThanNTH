using EVServiceCenter.gRPCService.ThanNTH.Protos;
using Grpc.Net.Client;
using System.Collections.Generic;
using System.Linq;

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

// MODIFIED to use the new PrintTable helper
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

    PrintTable(response.Items);
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

    DrawInfoPanel(response, "ITEM DETAILS");
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
    Console.Write(">> Enter ID of the center part to update: ");

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

    // Use the new info panel helper
    DrawInfoPanel(existingPart, "REVIEWING PART FOR UPDATE");
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


// MODIFIED to use the new DrawInfoPanel helper
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

    // Use the new info panel helper
    DrawInfoPanel(existingPart, "REVIEWING PART FOR DELETION");
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
    Console.WriteLine("               CENTER PART MANAGEMENT SYSTEM v5.5");
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



void PrintTable(IEnumerable<CenterPartThanNth> items)
{
    int idWidth = Math.Max("ID".Length, items.Max(i => i.CenterPartThanNthid.ToString().Length)) + 2;
    int descWidth = Math.Max("Description".Length, items.Max(i => (i.Description ?? "N/A").Length)) + 2;
    int partIdWidth = Math.Max("Part ID".Length, items.Max(i => i.PartId.ToString().Length)) + 2;
    int centerIdWidth = Math.Max("Center ID".Length, items.Max(i => i.CenterId.ToString().Length)) + 2;
    int qtyWidth = Math.Max("Qty".Length, items.Max(i => i.AvailableQuantity.ToString().Length)) + 2;
    int statusWidth = Math.Max("Status".Length, items.Max(i => (i.PartStatus ?? "N/A").Length)) + 2;

    Console.ForegroundColor = ConsoleColor.Cyan;
    string topBorder = $"┌{new string('─', idWidth)}┬{new string('─', descWidth)}┬{new string('─', partIdWidth)}┬{new string('─', centerIdWidth)}┬{new string('─', qtyWidth)}┬{new string('─', statusWidth)}┐";
    string headers = $"│{"ID".PadRight(idWidth)}│{"Description".PadRight(descWidth)}│{"Part ID".PadRight(partIdWidth)}│{"Center ID".PadRight(centerIdWidth)}│{"Qty".PadRight(qtyWidth)}│{"Status".PadRight(statusWidth)}│";
    string midBorder = $"├{new string('─', idWidth)}┼{new string('─', descWidth)}┼{new string('─', partIdWidth)}┼{new string('─', centerIdWidth)}┼{new string('─', qtyWidth)}┼{new string('─', statusWidth)}┤";
    string botBorder = $"└{new string('─', idWidth)}┴{new string('─', descWidth)}┴{new string('─', partIdWidth)}┴{new string('─', centerIdWidth)}┴{new string('─', qtyWidth)}┴{new string('─', statusWidth)}┘";

    Console.WriteLine(topBorder);
    Console.WriteLine(headers);
    Console.WriteLine(midBorder);

    Console.ForegroundColor = ConsoleColor.White;
    foreach (var item in items)
    {
        string id = item.CenterPartThanNthid.ToString().PadRight(idWidth);
        string desc = (item.Description ?? "N/A").PadRight(descWidth);
        string partId = item.PartId.ToString().PadRight(partIdWidth);
        string centerId = item.CenterId.ToString().PadRight(centerIdWidth);
        string qty = item.AvailableQuantity.ToString().PadRight(qtyWidth);
        string status = (item.PartStatus ?? "N/A").PadRight(statusWidth);

        Console.WriteLine($"│{id}│{desc}│{partId}│{centerId}│{qty}│{status}│");
    }

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(botBorder);
    Console.ForegroundColor = ConsoleColor.Green;
}


void DrawInfoPanel(CenterPartThanNth part, string title)
{
    var lines = new List<string>
    {
        $"ID:                {part.CenterPartThanNthid}",
        $"Description:       {part.Description ?? "N/A"}",
        $"Part ID:           {part.PartId}",
        $"Center ID:         {part.CenterId}",
        $"Available Qty:     {part.AvailableQuantity}",
        $"Minimum Qty:       {part.MinimumQuantity}",
        $"Part Status:       {part.PartStatus ?? "N/A"}",
        $"Created:           {part.CreateDate}",
        $"Last Updated:      {part.UpdateDate}"
    };

    int maxWidth = lines.Max(l => l.Length) + 4; 
    string titleFormatted = $" {title} ";
    int titlePadding = (maxWidth - titleFormatted.Length) / 2;
    string titleBar = $"{new string('─', titlePadding)}{titleFormatted}{new string('─', maxWidth - titleFormatted.Length - titlePadding)}";

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"┌{titleBar}┐");

    Console.ForegroundColor = ConsoleColor.White;
    foreach (var line in lines)
    {
        Console.WriteLine($"│  {line.PadRight(maxWidth - 4)}  │");
    }

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"└{new string('─', maxWidth)}┘");
    Console.ForegroundColor = ConsoleColor.Green; 
}
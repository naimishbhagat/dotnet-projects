using FileApp.Models;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using FileApp.Data;
using System.Text.Json;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
DataContextDapper dapper = new DataContextDapper(config);

// Computer myComputer = new Computer(){
//     Motherboard = "Z690",
//     HasWifi = true,
//     HasLTE = false,
//     Price = 943.87m,
//     VideoCard ="RTX 2026"
// };

// string sql = @"INSERT INTO TutorialAppSchema.Computer (
//     Motherboard,
//     HasWifi,
//     HasLTE,
//     Price,
//     VideoCard
// ) VALUES ('"+ myComputer.Motherboard 
//     + "','" + myComputer.HasWifi
//     + "','" + myComputer.HasLTE
//     + "','" + myComputer.Price
//     + "','" + myComputer.VideoCard
// + "')\n";
// File.WriteAllText("log.txt", sql);

// using StreamWriter openFile = new("log.txt",append: true);
// openFile.WriteLine(sql);
// openFile.close();
string computersJson = File.ReadAllText("ComputersSnake.json");
Mapper mapper = new Mapper(new MapperConfiguration((cfg) =>
{
    cfg.CreateMap<ComputerSnake, Computer>()
       .ForMember(destination => destination.ComputerId, options =>
       options.MapFrom(source => source.computer_id))
        .ForMember(destination => destination.Motherboard, options =>
       options.MapFrom(source => source.motherboard))
       .ForMember(destination => destination.CPUCores, options =>
       options.MapFrom(source => source.cpu_cores))
        .ForMember(destination => destination.HasWifi, options =>
       options.MapFrom(source => source.has_wifi))
        .ForMember(destination => destination.HasLTE, options =>
       options.MapFrom(source => source.has_lte))
        .ForMember(destination => destination.Price, options =>
       options.MapFrom(source => source.price))
       .ForMember(destination => destination.VideoCard, options =>
       options.MapFrom(source => source.video_card));
}, NullLoggerFactory.Instance));
IEnumerable<ComputerSnake>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ComputerSnake>>(computersJson);
if (computersSystem != null)
{
    IEnumerable<Computer> computerResult = mapper.Map<IEnumerable<Computer>>(computersSystem);
    foreach (Computer computer in computerResult)
    {
        Console.WriteLine(computer.Motherboard);
    }
}

string EscapeSingleQuote(string input)
{
    string output = input.Replace("'", "''");
    return output;
}

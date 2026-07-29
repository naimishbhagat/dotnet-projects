using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelApp.Data;
using ModelsApp.Models;

namespace ModelsApp
{
    
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            DataContextDapper dapper = new DataContextDapper(config);
            DataContextEF entityFramework = new DataContextEF(config);
            string sqlCommand = "SELECT GETDATE()";
            DateTime rightNow = dapper.LoadDataSingle<DateTime>(sqlCommand);
            Console.WriteLine(rightNow);
            Computer myComputer = new Computer(){
                Motherboard = "Z690",
                HasWifi = true,
                HasLTE = false,
                Price = 943.87m,
                VideoCard ="RTX 2026"
            };

            entityFramework.Add(myComputer);
            entityFramework.SaveChanges();
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
            // + "')";
            // Console.WriteLine(sql);
            // int result = dapper.ExecuteSqlWithRowCount(sql);
            // Console.WriteLine(result);

            string sqlSelect = @"
            SELECT 
                Computer.ComputerId,
                Computer.Motherboard,
                Computer.HasWifi,
                Computer.HasLTE,
                Computer.Price,
                Computer.VideoCard
            FROM TutorialAppSchema.Computer";

            IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);
            
            foreach(Computer comp in computers)
            {
                Console.WriteLine("'"+ comp.ComputerId 
                    + "','" + comp.Motherboard
                    + "','" + comp.HasWifi
                    + "','" + comp.HasLTE
                    + "','" + comp.Price
                    + "','" + comp.VideoCard
                + "'");
            }

            IEnumerable<Computer>? computersEF = entityFramework.Computer?.ToList<Computer>();
            if(computersEF != null)
            {
                foreach(Computer comp in computersEF)
                {
                    Console.WriteLine("'"+ comp.ComputerId 
                        + "','" + comp.Motherboard
                        + "','" + comp.HasWifi
                        + "','" + comp.HasLTE
                        + "','" + comp.Price
                        + "','" + comp.VideoCard
                    + "'");
                }
            }
            

            // Console.WriteLine(myComputer.Motherboard);
            // Console.WriteLine(myComputer.VideoCard);
            // Console.WriteLine(myComputer.Price);
        }
    }
}
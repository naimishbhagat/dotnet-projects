using System.Data;
using System.Globalization;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelApp.Data;
using ModelsApp.Models;
using Newtonsoft.Json;

namespace ModelsApp
{
    
    internal class Program
    {
        static void Main(string[] args)
        {
            // IConfiguration config = new ConfigurationBuilder()
            //     .AddJsonFile("appsettings.json")
            //     .Build();

            // DataContextDapper dapper = new DataContextDapper(config);
            // DataContextEF entityFramework = new DataContextEF(config);
            // string sqlCommand = "SELECT GETDATE()";
            // DateTime rightNow = dapper.LoadDataSingle<DateTime>(sqlCommand);
            // Console.WriteLine(rightNow);
            // Computer myComputer = new Computer(){
            //     Motherboard = "Z690",
            //     HasWifi = true,
            //     HasLTE = false,
            //     Price = 943.87m,
            //     VideoCard ="RTX 2026"
            // };

            // entityFramework.Add(myComputer);
            // entityFramework.SaveChanges();
            // // string sql = @"INSERT INTO TutorialAppSchema.Computer (
            // //     Motherboard,
            // //     HasWifi,
            // //     HasLTE,
            // //     Price,
            // //     VideoCard
            // // ) VALUES ('"+ myComputer.Motherboard 
            // //     + "','" + myComputer.HasWifi
            // //     + "','" + myComputer.HasLTE
            // //     + "','" + myComputer.Price
            // //     + "','" + myComputer.VideoCard
            // // + "')";
            // // Console.WriteLine(sql);
            // // int result = dapper.ExecuteSqlWithRowCount(sql);
            // // Console.WriteLine(result);

            // string sqlSelect = @"
            // SELECT 
            //     Computer.ComputerId,
            //     Computer.Motherboard,
            //     Computer.HasWifi,
            //     Computer.HasLTE,
            //     Computer.Price,
            //     Computer.VideoCard
            // FROM TutorialAppSchema.Computer";

            // IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);
            
            // foreach(Computer comp in computers)
            // {
            //     Console.WriteLine("'"+ comp.ComputerId 
            //         + "','" + comp.Motherboard
            //         + "','" + comp.HasWifi
            //         + "','" + comp.HasLTE
            //         + "','" + comp.Price
            //         + "','" + comp.VideoCard
            //     + "'");
            // }

            // IEnumerable<Computer>? computersEF = entityFramework.Computer?.ToList<Computer>();
            // if(computersEF != null)
            // {
            //     foreach(Computer comp in computersEF)
            //     {
            //         Console.WriteLine("'"+ comp.ComputerId 
            //             + "','" + comp.Motherboard
            //             + "','" + comp.HasWifi
            //             + "','" + comp.HasLTE
            //             + "','" + comp.Price
            //             + "','" + comp.VideoCard
            //         + "'");
            //     }
            // }
            
            IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            DataContextDapper dataContextDapper = new DataContextDapper(config);

            string tableCreateSql = System.IO.File.ReadAllText("Users.sql");
            dataContextDapper.ExecuteSql(tableCreateSql);

            string usersJson = System.IO.File.ReadAllText("Users.json");

            IEnumerable<Users>? users = JsonConvert.DeserializeObject<IEnumerable<Users>>(usersJson);

            if (users != null)
            {
                using (IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
                {
                    string sql = "SET IDENTITY_INSERT TutorialAppSchema.Users ON;"
                                    + "INSERT INTO TutorialAppSchema.Users (UserId"
                                    + ",FirstName"
                                    + ",LastName"
                                    + ",Email"
                                    + ",Gender"
                                    + ",Active)"
                                    + "VALUES";
                    foreach (Users singleUser in users)
                    {
                        string sqlToAdd = "(" + singleUser.UserId
                                    + ", '" + singleUser.FirstName?.Replace("'", "''")
                                    + "', '" + singleUser.LastName?.Replace("'", "''")
                                    + "', '" + singleUser.Email?.Replace("'", "''")
                                    + "', '" + singleUser.Gender
                                    + "', '" + singleUser.Active
                                    + "'),";

                        if ((sql + sqlToAdd).Length > 4000)
                        {
                            dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                            sql = "SET IDENTITY_INSERT TutorialAppSchema.Users ON;"
                                    + "INSERT INTO TutorialAppSchema.Users (UserId"
                                    + ",FirstName "
                                    + ",LastName"
                                    + ",Email"
                                    + ",Gender"
                                    + ",Active)"
                                    + "VALUES";
                        }
                        sql += sqlToAdd;
                    }
                    dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                }
            }
            dataContextDapper.ExecuteSQL("SET IDENTITY_INSERT TutorialAppSchema.Users OFF");

            string userSalaryJson = System.IO.File.ReadAllText("UserSalary.json");

            IEnumerable<UserSalary>? userSalary = JsonConvert.DeserializeObject<IEnumerable<UserSalary>>(userSalaryJson);

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TutorialAppSchema.UserSalary");

            if (userSalary != null)
            {
                using (IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
                {
                    string sql = "INSERT INTO TutorialAppSchema.UserSalary (UserId"
                                    + ",Salary)"
                                    + "VALUES";
                    foreach (UserSalary singleUserSalary in userSalary)
                    {
                        string sqlToAdd = "(" + singleUserSalary.UserId
                                    + ", '" + singleUserSalary.Salary.ToString("0.00", CultureInfo.InvariantCulture)
                                    + "'),";
                        if ((sql + sqlToAdd).Length > 4000)
                        {
                            dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                            sql = "INSERT INTO TutorialAppSchema.UserSalary (UserId"
                                    + ",Salary)"
                                    + "VALUES";
                        }
                        sql += sqlToAdd;
                    }
                    dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                }
            }

            string userJobInfoJson = System.IO.File.ReadAllText("UserJobInfo.json");

            IEnumerable<UserJobInfo>? userJobInfo = JsonConvert.DeserializeObject<IEnumerable<UserJobInfo>>(userJobInfoJson);

            dataContextDapper.ExecuteSQL("TRUNCATE TABLE TutorialAppSchema.UserJobInfo");

            if (userJobInfo != null)
            {
                using (IDbConnection dbConnection = new SqlConnection(config.GetConnectionString("DefaultConnection")))
                {
                    string sql = "INSERT INTO TutorialAppSchema.UserJobInfo (UserId"
                                    + ",Department"
                                    + ",JobTitle)"
                                    + "VALUES";
                    foreach (UserJobInfo singleUserJobInfo in userJobInfo)
                    {
                        string sqlToAdd = "(" + singleUserJobInfo.UserId
                                    + ", '" + singleUserJobInfo.Department
                                    + "', '" + singleUserJobInfo.JobTitle
                                    + "'),";
                        if ((sql + sqlToAdd).Length > 4000)
                        {
                            dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                            sql = "INSERT INTO TutorialAppSchema.UserJobInfo (UserId"
                                    + ",Department"
                                    + ",JobTitle)"
                                    + "VALUES";
                        }
                        sql += sqlToAdd;
                    }
                    dataContextDapper.ExecuteProcedureMulti(sql.Trim(','), dbConnection);
                }
            }
            Console.WriteLine("SQL Seed Completed Successfully");

            // Console.WriteLine(myComputer.Motherboard);
            // Console.WriteLine(myComputer.VideoCard);
            // Console.WriteLine(myComputer.Price);
        }
    }
}
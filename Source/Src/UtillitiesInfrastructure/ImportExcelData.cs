using ClosedXML.Excel;
using Domain;
using Domain.MyVote;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UtilitiesInfrastructure
    {
    public class ImportExcelDataUtility
        {
        //import excel data for constituency
        public static async Task ImportExcelData(string filePath)
            {
            try
                {
                // Replace with your actual connection string details
                string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Vote24March;TrustServerCertificate=True;Integrated Security=True;";

                // Create the ApplicationDbContext instance directly
                var dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(connectionString)
                    .Options);
                using (var excel = new XLWorkbook(filePath))
                    {
                    var worksheet = excel.Worksheet(1); // Assuming data is in the first worksheet

                    // Skip header row
                    var dataRows = worksheet.RowsUsed().Skip(1);

                    foreach (var row in dataRows)
                        {
                        var entity = new VoteConstituency
                            {
                            Id = int.Parse(row.Cell(1).Value.ToString()),
                            CurrentMemberName = (string)row.Cell(2).Value,
                            CurrentMemberParty = (string)row.Cell(3).Value,
                            Name = (string)row.Cell(4).Value, // Assuming Name is in column 1
                            State = (string)row.Cell(5).Value,
                            CurrentMemberTerms = (string)row.Cell(7).Value,
                            // MemberNamesEarlierOthers = (string)row.Cell(7).Value,
                            };

                        // Optional: Validate data before adding to context
                        // ...

                        dbContext.VoteConstituencies.Add(entity);
                        }

                    var result = await dbContext.SaveChangesAsync();
                    Console.WriteLine($"Data imported successfully! with {result} changes");
                    }
                }
            catch (Exception e)
                {
                Console.WriteLine(e.ToString());
                }
            Console.ReadLine();
            }
        }
    }

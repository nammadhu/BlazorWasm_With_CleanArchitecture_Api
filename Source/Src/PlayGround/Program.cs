using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using UtilitiesInfrastructure;

namespace PlayGround
    {
    internal class Program
        {
        static async Task Main(string[] args)
            {
            Console.WriteLine("Hello, World!");

            await ImportExcelDataUtility.ImportExcelData(@"D:\repos\news\nammadhu\Notes\ConstituenciesShortened.xlsx");

            }
        }
    }

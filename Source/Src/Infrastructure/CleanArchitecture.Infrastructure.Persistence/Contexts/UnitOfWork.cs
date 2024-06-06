using CleanArchitecture.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence.Contexts;
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
        {
        public async Task<bool> SaveChangesAsync()
            {
            try
                {
                return await dbContext.SaveChangesAsync() > 0;
                }
            catch (System.Exception e)
                {
                Console.Write(e.ToString());
                throw;
                }

            }
        public bool SaveChanges()
            {
            try
                {
                return dbContext.SaveChanges() > 0;
                }
            catch (Exception e)
                {
                Console.Write(e.ToString());
                throw;
                }

            }
        }

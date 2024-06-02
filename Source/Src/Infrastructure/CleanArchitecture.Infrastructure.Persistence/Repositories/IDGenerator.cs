using CleanArchitecture.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using MyTown.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Persistence.Repositories
    {
    public class IDGenerator<TEntity>(ApplicationDbContext context, string idColumnName) : IIDGenerator<TEntity> where TEntity : class
        {
        public int GetNextID()
            {
            // Fetch the current maximum ID from the database for the specific table
            var maxId = context.Set<TEntity>().Max(entity => EF.Property<int>(entity, idColumnName));
            return  maxId+1; // Ensure returned ID is greater than both internal state and DB max
            }
        }

    }

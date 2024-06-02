using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTown.Application.Interfaces
    {
    public interface IIDGenerator<TEntity> where TEntity : class
        {
        int GetNextID();
        }

    }

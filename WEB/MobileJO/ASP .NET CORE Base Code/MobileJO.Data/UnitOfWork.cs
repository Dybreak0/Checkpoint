using MobileJO.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public DbContext Database { get; private set; }

        public UnitOfWork(BaseCodeEntities serviceContext)
        {
            Database = serviceContext;
        }

        public void SaveChanges()
        {
            try
            {
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}

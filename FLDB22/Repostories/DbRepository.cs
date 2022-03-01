using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace FLDB22.Repostories
{
    internal class DbRepository
    {
        private string _connectionString;

        public DbRepository()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<DbRepository>()
                        .Build();

            _connectionString = config.GetConnectionString("dbConn");
        }
    }
}

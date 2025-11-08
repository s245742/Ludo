using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Ludo.Services
{
    public abstract class ServiceBase
    {
        protected readonly string connectionString;

        protected ServiceBase()
        {
            // Get the connection string from App.config file
            connectionString = ConfigurationManager
                .ConnectionStrings["LudoDb"]
                .ConnectionString;
        }


    }
}

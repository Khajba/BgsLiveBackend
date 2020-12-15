using Bgs.Bll.Abstract;
using Bgs.DataConnectionManager.SqlServer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Dal
{
    public class IUserRepository: SqlServerRepository, IUserService
    {
        private const string _schemaInternalUser = "InternalUser";

        public IUserRepository(IConfiguration configuration)
             : base(configuration, configuration.GetConnectionString("MainDatabase"))
        {

        }
    }
}

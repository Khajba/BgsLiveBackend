using Bgs.Dal.Abstract;
using Bgs.DataConnectionManager.SqlServer;
using Bgs.DataConnectionManager.SqlServer.Extensions;
using Bgs.Live.Common.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Dal
{
    public class InternalUserRepository : SqlServerRepository, IInternalUserRepository
    {
        private const string _schemaInternalUser = "InternalUser";

        public InternalUserRepository(IConfiguration configuration)
             : base(configuration, configuration.GetConnectionString("MainDatabase"))
        {

        }
        public InternalUser GetUserByCredentials(string email, string password)
        {
            using (var cmd = GetSpCommand($"{_schemaInternalUser}.GetUserByCredentials"))
            {
                cmd.AddParameter("Email", email);
                cmd.AddParameter("Password", password);

                return cmd.ExecuteReaderSingle<InternalUser>();
            }
        }
    }
}

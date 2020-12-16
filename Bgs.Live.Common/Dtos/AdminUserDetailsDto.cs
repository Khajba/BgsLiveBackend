using System.Collections.Generic;

namespace Bgs.Live.Common.Dtos
{
    public class AdminUserDetailsDto
    {
        public UserDto UserDetails { get; set; }

        public IEnumerable<TransactionDto> Transactions { get; set; }
    }
}

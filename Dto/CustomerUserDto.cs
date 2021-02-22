using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class CustomerUserDto : UserDto
    {
        public Guid CustomerId { get; set; }
    }
}

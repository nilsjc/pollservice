using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record PollDTO
    {
        required public string Key { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record QuestionDTO
    {
        required public string QuestionKey { get; set; }
        required public string Text { get; set; }
    }
}
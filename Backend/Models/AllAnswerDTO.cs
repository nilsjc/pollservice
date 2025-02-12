using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record AllAnswerDTO(string pollKey, List<string> Questions, List<int> Answers);
}
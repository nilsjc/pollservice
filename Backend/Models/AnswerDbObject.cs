using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public record AnswerDbObject(string PollKey, Dictionary<string, int> Answers, string user);
}
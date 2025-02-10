using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.interfaces
{
    public interface IAnswerService
    {
        Task PostAnswers(AnswersDTO dto, string ipaddress);
    }
}
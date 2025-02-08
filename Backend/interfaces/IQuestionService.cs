using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend.interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDTO>> SelectQuestions(string key);
        Task InsertQuestion(QuestionDTO question);
    }
}
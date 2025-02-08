using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend.database
{
    public class QuestionService : IQuestionService
    {
        public async Task<IEnumerable<QuestionDTO>> GetQuestions(string key)
        {
            IEnumerable<QuestionDTO> result = new List<QuestionDTO>
            {
                new QuestionDTO
                {
                    QuestionKey = key,
                    Text = "lufs lufs"
                }
            };
            return result;
        }

        public void InsertQuestion(QuestionDTO question, string key)
        {
            throw new NotImplementedException();
        }
    }
}
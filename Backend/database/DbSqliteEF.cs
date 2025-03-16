using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.database
{
    public class DbSqliteEF : IDatabaseService
    {
        public Task<int> CountQuestions(string pollkey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllAnswers(string pollId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllPolls()
        {
            throw new NotImplementedException();
        }

        public Task InsertManyQuestions(QuestionDbObject questions)
        {
            throw new NotImplementedException();
        }

        public Task InsertPoll(string name)
        {
            throw new NotImplementedException();
        }

        public Task InsertQuestion(QuestionDTO question)
        {
            throw new NotImplementedException();
        }

        public Task PostAnswers(AnswerDbObject answers)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuestionDTO>> SelectQuestions(string pollKey)
        {
            throw new NotImplementedException();
        }
    }
}
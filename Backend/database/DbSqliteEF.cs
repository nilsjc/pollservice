using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.OpenApi.Validations;

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

        public async Task InsertManyQuestions(QuestionDbObject questions)
        {
            using var db = new PollContext();
            var poll = db.Polls.SingleOrDefault(x => x.name == questions.pollKey);
            if(poll is null)
            {
                return;
            }

            List<tbl_question> questionsList = [];
            foreach(var q in questions.Questions)
            {
                questionsList.Add(new tbl_question
                {
                    tbl_poll_id = poll.id,
                    qkey = q.Key,
                    text = q.Value
                });
            }
            db.Questions.AddRange(questionsList);
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
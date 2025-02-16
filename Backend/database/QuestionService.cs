using Backend.interfaces;
using Backend.Models;

namespace Backend.database
{
    public class QuestionService : IQuestionService
    {
        private readonly IDatabaseService _service;

        public QuestionService(IDatabaseService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<QuestionDTO>> SelectQuestions(string key)
        {
            var result = await _service.SelectQuestions(key);
            return result;
        }

        public async Task InsertQuestion(QuestionDTO question)
        {
            await _service.InsertQuestion(question);
        }

        public async Task InsertClientQuestion(ClientQuestionDTO questDTO)
        {
            Dictionary<string,string> quest = new();
            foreach(var q in questDTO.questions)
            {
                quest.Add(q.key, q.data);

            }
            var dbObject = new QuestionDbObject(quest, questDTO.key);
            await _service.InsertManyQuestions(dbObject);
        }
    }
}
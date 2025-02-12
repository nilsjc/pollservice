using Backend.interfaces;
using Backend.Models;
namespace Backend.database
{
    public class AnswerService : IAnswerService
    {
        private readonly IDatabaseService service;
        public AnswerService(IDatabaseService databaseService)
        {
            service = databaseService;
        }

        public async Task<AllAnswerDTO> GetAllAnswers(string pollKey)
        {
            var allAnswers = await service.GetAllAnswers(pollKey);
            const char SPACE = ' ';
            string pollName = string.Empty;
            List<string> questions = [];
            List<int> answers = [];
            foreach(var ans in allAnswers)
            {
                var values = ans.Split(SPACE);
                pollName = values[1];
                questions.Add(values[2]);
                answers.Add(int.Parse(values[3]));
            }
            return new AllAnswerDTO(pollName, questions, answers);
        }

        public async Task<int> PostAnswers(AnswersDTO dto, string ipaddress, int answCount)
        {
            var keys = await service.CountQuestions(dto.PollKey);
            if(keys != answCount)
            {
                return -1;
            }
            var answers = dto.QKeys.Zip(dto.Answers, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);
            var answerDbObject = new AnswerDbObject(dto.PollKey, answers, dto.User);
            await service.PostAnswers(answerDbObject);
            return 0;
        }
    }
}
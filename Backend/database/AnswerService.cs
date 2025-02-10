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
        public async Task PostAnswers(AnswersDTO dto, string ipaddress)
        {
            var answers = dto.QKeys.Zip(dto.Answers, (k, v) => new { k, v })
              .ToDictionary(x => x.k, x => x.v);
            var answerDbObject = new AnswerDbObject(dto.PollKey, answers, dto.User);
            await service.PostAnswers(answerDbObject);
        }
    }
}
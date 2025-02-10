using Backend.Models;

namespace Backend.database
{
    public interface IDatabaseService
    {
        Task InsertQuestion(QuestionDTO question);
        Task<IEnumerable<QuestionDTO>> SelectQuestions(string pollKey);
        Task InsertPoll(string name);
        Task<IEnumerable<string>> GetAllPolls();
        Task PostAnswer(Answer answer);
    }
}
using Backend.Models;

namespace Backend.interfaces
{
    public interface IQuestionService
    {
        Task<IEnumerable<QuestionDTO>> SelectQuestions(string key);
        Task InsertQuestion(QuestionDTO question);
        Task InsertClientQuestion(ClientQuestionDTO questDTO);
    }
}
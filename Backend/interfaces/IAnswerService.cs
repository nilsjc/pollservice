using Backend.Models;

namespace Backend.interfaces
{
    public interface IAnswerService
    {
        Task<int> PostAnswers(AnswersDTO dto, string ipaddress, int answerCount);
        Task<AllAnswerDTO> GetAllAnswers(string pollKey);
    }
}
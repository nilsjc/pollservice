using Backend.Models;

namespace Backend.interfaces
{
    public interface IAnswerService
    {
        Task PostAnswers(AnswersDTO dto, string ipaddress);
        Task<AllAnswerDTO> GetAllAnswers(string pollKey);
    }
}
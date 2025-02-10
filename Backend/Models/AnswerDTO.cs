namespace Backend.Models
{
    public class AnswersDTO()
    {
        required public string PollKey { get; set; }
        required public List<int> Answers { get; set; }
        required public List<string> QKeys { get; set; }
        required public string User { get; set; }
    }
}
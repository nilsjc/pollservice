namespace Backend.Models
{
    public class Question
    {
        public string key { get; set; }
        public string data { get; set; }
    }

    public class ClientQuestionDTO
    {
        public List<Question> questions { get; set; }
        public string key { get; set; }
    }


}
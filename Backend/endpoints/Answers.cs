using Backend.interfaces;
using Backend.Models;

namespace Backend.endpoints
{
    public static class Answers
    {
        const string CorruptDataMessage = "Corrupt data";
        public static void RegisterAnswersEndpoints(this WebApplication app)
        {
            app.MapPost("/Answers", PostAnswers)
                .WithName("PostAnswers")
                .WithOpenApi();
            
            app.MapGet("/Answers", GetAllAnswers)
                .WithName("GetAllAnswers")
                .WithOpenApi();
        }
        static async Task<IResult> PostAnswers(AnswersDTO answerDTO, IAnswerService service, HttpContext context)
        {
            string ipaddress = context.Connection.RemoteIpAddress.ToString();
            int answCount = answerDTO.Answers.Count();
            if(answCount != answerDTO.QKeys.Count())
            {
                return TypedResults.BadRequest(CorruptDataMessage);
            }
            int result = await service.PostAnswers(answerDTO, ipaddress, answCount);
            if(result!=0)
            {
                return TypedResults.BadRequest(CorruptDataMessage);
            }
            return TypedResults.Ok();
        }

        static async Task<IResult> GetAllAnswers(string pollKey, IAnswerService service)
        { 
            var result = await service.GetAllAnswers(pollKey);
            if(string.IsNullOrEmpty(result.pollKey))
            {
                return TypedResults.NotFound($"{pollKey} not found");
            }
            return TypedResults.Ok(result);
        }
    }
}
using Backend.interfaces;
using Backend.Models;

namespace Backend.endpoints
{
    public static class Answers
    {
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
            await service.PostAnswers(answerDTO, ipaddress);
            return TypedResults.Ok();
        }

        static async Task<IResult> GetAllAnswers(string pollKey, IAnswerService service)
        { 
            var result = service.GetAllAnswers(pollKey); 
            return TypedResults.Ok(result);
        }
    }
}
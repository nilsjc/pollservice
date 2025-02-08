using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.interfaces;
using Backend.Models;

namespace Backend.endpoints
{
    public static class Questions
    {
        public static void RegisterQuestionsEndpoints(this WebApplication app)
        {
            app.MapGet("/Questions", GetQuestions)
                .WithName("GetQuestions")
                .WithOpenApi();

            app.MapPost("/Questions", PostQuestion)
                .WithName("PostQuestions")
                .WithOpenApi();
        }
        static async Task<IResult> GetQuestions(string pollKey, IQuestionService service)
        {
            if((pollKey.Split(' ')).Length > 1)
            {
                return TypedResults.BadRequest("no spaces in pollKey name allowed");
            }
            var result = await service.SelectQuestions(pollKey);
            return TypedResults.Ok(result);
        }
        static async Task<IResult> PostQuestion(QuestionDTO qdto, IQuestionService service)
        {
            await service.InsertQuestion(qdto);
            return TypedResults.Ok();
        }
    }
            
}
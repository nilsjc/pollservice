using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        }
        static async Task<IResult> PostAnswers(AnswersDTO answerDTO, IAnswerService service, HttpContext context)
        {
            string ipaddress = context.Connection.RemoteIpAddress.ToString();
            await service.PostAnswers(answerDTO, ipaddress);
            return TypedResults.Ok();
        }
    }
}
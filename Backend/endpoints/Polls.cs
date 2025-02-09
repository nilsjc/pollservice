using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.database;

namespace Backend.endpoints
{
    public static class Polls
    {
        public static void RegisterPollsEndpoints(this WebApplication app)
        {
            app.MapPost("/Polls", PostPolls)
                .WithName("InsertPolls")
                .WithOpenApi();
            
            app.MapGet("/Polls", GetPolls)
                .WithName("GetPolls")
                .WithOpenApi();

        }
        static async Task<IResult> PostPolls(string pollKey, IDatabaseService service)
        {
            if((pollKey.Split(' ')).Length > 1)
            {
                return TypedResults.BadRequest("no spaces in pollKey name allowed");
            }
            await service.InsertPoll(pollKey);
            return TypedResults.Ok();
        }
        static async Task<IResult> GetPolls(IDatabaseService service)
        {
            var result = await service.GetAllPolls();
            return TypedResults.Ok(result);
        }
    }
}
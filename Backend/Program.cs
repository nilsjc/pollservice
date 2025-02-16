using Backend;
using Backend.database;
using Backend.endpoints;
using Backend.interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IDatabaseService>(s => new DbSqliteEngine("questions02"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#if DEBUG
app.UseCors(builder => builder
 .AllowAnyOrigin()
 .AllowAnyMethod()
 .AllowAnyHeader()
);
#endif

app.RegisterQuestionsEndpoints();
app.RegisterPollsEndpoints();
app.RegisterAnswersEndpoints();

app.Run();

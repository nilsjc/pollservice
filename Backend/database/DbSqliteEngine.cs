using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.Data.Sqlite;

namespace Backend.database
{
    public class DbSqliteEngine(string dbName)
    {
        public async Task InsertQuestion(QuestionDTO question)
        {
            using (var connection = new SqliteConnection($"Data Source={dbName}"))

        }
        public async Task<IEnumerable<QuestionDTO>> SelectQueries(string pollKey)
        {
            List<QuestionDTO> questions = [];
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @$"
                    SELECT questionkey, text
                    FROM tblQueries WHERE pollkey = @pollkey
                ";
                command.Parameters.AddWithValue("@pollkey", pollKey);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    questions.Add(
                        new QuestionDTO(
                            reader.GetString(0),
                            reader.GetString(1)));
                }
            }
            return questions;
        }
    }
}
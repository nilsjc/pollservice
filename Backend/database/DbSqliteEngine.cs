using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.Data.Sqlite;

namespace Backend.database
{
    public class DbSqliteEngine : IDatabaseService
    {
        private readonly string DbName;
        public DbSqliteEngine(string dbName)
        {
            DbName = dbName;
            CreateDbSchema();
        }
        private void CreateDbSchema()
        {
            using(var connection = new SqliteConnection($"Data source={DbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"CREATE TABLE IF NOT EXISTS tbl_poll 
                (id INTEGER PRIMARY KEY, 
                name TEXT);

                CREATE TABLE IF NOT EXISTS tbl_question 
                (id INTEGER PRIMARY KEY,
                tbl_poll_id INTEGER, 
                qkey TEXT, 
                text TEXT, 
                FOREIGN KEY(tbl_poll_id) REFERENCES tbl_poll(id)
                );

                CREATE TABLE IF NOT EXISTS tbl_user 
                (id INTEGER PRIMARY KEY,
                name TEXT);

                CREATE TABLE IF NOT EXISTS tbl_answer
                (id INTEGER PRIMARY KEY,
                tbl_user_id INTEGER,
                tbl_question_id INTEGER,
                value INTEGER,
                datetime DATETIME,
                FOREIGN KEY(tbl_user_id) REFERENCES tbl_user(id),
                FOREIGN KEY(tbl_question_id) REFERENCES tbl_question(id)
                );";
                command.ExecuteNonQuery();
            }
            
        }
        public async Task InsertQuestion(QuestionDTO question)
        {
            using (var connection = new SqliteConnection($"Data Source={DbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @$"
                    INSERT INTO tblQuestions (questionkey, text, pollKey)
                    VALUES(@qkey, @qtext, @pollkey)
                ";
                command.Parameters.AddWithValue("@key", question.QuestionKey);
                command.Parameters.AddWithValue("@text", question.Text);
                command.Parameters.AddWithValue("@pollkey", question.PollKey);
            }
        }
        public async Task<IEnumerable<QuestionDTO>> SelectQuestions(string pollKey)
        {
            List<QuestionDTO> questions = [];
            using (var connection = new SqliteConnection($"Data Source={DbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @$"
                    SELECT questionkey, text, pollkey
                    FROM tblQuestions WHERE pollkey = @pollkey
                ";
                command.Parameters.AddWithValue("@pollkey", pollKey);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    questions.Add(
                        new QuestionDTO(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2)));
                }
            }
            return questions;
        }

        public async Task InsertPoll(string name)
        {
            using (var connection = new SqliteConnection($"Data Source={DbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @$"
                    INSERT INTO tbl_poll (name)
                    VALUES(@name)
                ";
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();
            }
        }
        public async Task<IEnumerable<string>> GetAllPolls()
        {
            List<string> results = [];
            using (var connection = new SqliteConnection($"Data Source={DbName}"))
            {
              connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @$"
                    SELECT name FROM tbl_poll
                ";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(
                            reader.GetString(0));
                }
            }
            return results;
        }
    }
}
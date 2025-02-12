using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
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
                name TEXT UNIQUE);

                CREATE TABLE IF NOT EXISTS tbl_question 
                (id INTEGER PRIMARY KEY,
                tbl_poll_id INTEGER, 
                qkey TEXT UNIQUE, 
                text TEXT, 
                FOREIGN KEY(tbl_poll_id) REFERENCES tbl_poll(id)
                );

                CREATE TABLE IF NOT EXISTS tbl_user 
                (id INTEGER PRIMARY KEY,
                name TEXT UNIQUE);

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
                int pollId = 1;
                var command = connection.CreateCommand();
                command.CommandText =
                @$"SELECT id FROM tbl_poll WHERE name = @pollkey";
                command.Parameters.AddWithValue("@pollkey", question.PollKey);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pollId = reader.GetInt32(0);
                    }
                }
                command.CommandText =
                @$"
                    INSERT INTO tbl_question (qkey, text, tbl_poll_id)
                    VALUES(@qkey, @qtext, @pollid)
                ";
                command.Parameters.AddWithValue("@qkey", question.QuestionKey);
                command.Parameters.AddWithValue("@qtext", question.Text);
                command.Parameters.AddWithValue("@pollid", pollId);
                command.ExecuteNonQuery();
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
                    SELECT qkey, text
                    FROM tbl_question 
                    INNER JOIN tbl_poll WHERE tbl_poll.id = tbl_question.tbl_poll_id
                    AND tbl_poll.name = @pollKey
                ";
                command.Parameters.AddWithValue("@pollKey", pollKey);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    questions.Add(
                        new QuestionDTO(
                            reader.GetString(0),
                            reader.GetString(1),
                            pollKey));
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
        public async Task PostAnswers(AnswerDbObject answersObj)
        {
            using (var connection = new SqliteConnection($"Data Source={DbName}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                // if(CheckIfUserExists(command, answersObj.userName)==0)
                // {

                // }
                var userId = CreateUserIfNotExists(command, answersObj.userName);
                var questionIds = GetQuestionQkeyWithIds(command, answersObj.PollKey);
                command.CommandText =
                @$"
                    INSERT INTO tbl_answer (tbl_user_id, tbl_question_id, value)
                    VALUES 
                ";

                StringBuilder sb = new();
                int count = 1;
                var dateTimeNow = DateTime.Now.ToString();
                foreach (var answer in answersObj.Answers)
                {
                    sb.Append($"($userid{count}, $qid{count}, $vote{count}),");
                    count++;
                }
                sb.Remove(sb.Length-1,1); // removing the last comma
                sb.Append(";");
                command.CommandText += sb.ToString();
                int x = 1;
                foreach (var answer in answersObj.Answers)
                {
                    command.Parameters.AddWithValue($"$userid{x}", userId);
                    var questionId = questionIds.SingleOrDefault(x => x.Item2 == answer.Key);
                    command.Parameters.AddWithValue($"$qid{x}", questionId.Item1);
                    command.Parameters.AddWithValue($"$vote{x}", answer.Value);
                    //command.Parameters.AddWithValue("@datetime", dateTimeNow);
                    x++;
                }
                command.ExecuteNonQuery();
            }
        }
        private List<(int,string)> GetQuestionQkeyWithIds(SqliteCommand command, string pollKey)
        {
            command.CommandText = 
            @"SELECT tbl_question.id, tbl_question.qkey FROM tbl_question 
            INNER JOIN tbl_poll 
            WHERE tbl_poll.id = tbl_question.tbl_poll_id 
            AND tbl_poll.name = @pollname";

            command.Parameters.AddWithValue("@pollname", pollKey);

            List<(int,string)> result = [];
            using(var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add((reader.GetInt32(0),reader.GetString(1)));
                }
            }
            return result;
        }

        private int GetQuestionId(SqliteCommand command, string pollKey, string qkey)
        {
            command.CommandText = 
            @"SELECT id FROM tbl_question 
            INNER JOIN tbl_poll 
            WHERE tbl_poll.id = tbl_question.tbl_poll_id 
            AND tbl_poll.name = @pollname
            AND tbl_question.qkey = @qkey";

            command.Parameters.AddWithValue("@pollname", pollKey);
            command.Parameters.AddWithValue("@qkey", qkey);

            int result = 0;
            using(var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            return result;
        }
        private int CheckIfUserExists(SqliteCommand command, string userName)
        {
            int result = -1;
            command.CommandText = "SELECT exists(SELECT 1 FROM tbl_user WHERE name = @name) AS row_exists;";
            command.Parameters.AddWithValue("@name", userName);
            using(var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a new user if not exists. Will always return the user id.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userName"></param>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private int CreateUserIfNotExists(SqliteCommand command, string userName)
        {
            int result = 1;
            command.CommandText = "SELECT exists(SELECT 1 FROM tbl_user WHERE name = @name) AS row_exists;";
            command.Parameters.AddWithValue("@name", userName);
            using(var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = reader.GetInt32(0);
                }
            }
            if(result == 0)
            {
                command.CommandText = 
                @"INSERT INTO tbl_user (name) 
                VALUES ('@username');";

                command.Parameters.AddWithValue("@username", userName);
                command.ExecuteNonQuery();
            }
            int id = 0;
            command.CommandText = "SELECT id FROM tbl_user WHERE name = $usrnme";
            // command.CommandText = "SELECT id FROM tbl_user WHERE name = '@name';";
            command.Parameters.AddWithValue("$usrnme", userName);
            // string tmp = command.CommandText.ToString();
            // foreach (SqliteParameter p in command.Parameters) {
            //     tmp = tmp.Replace('@' + p.ParameterName.ToString(),"'" + p.Value.ToString() + "'");
            // }
            using(var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
            }
            return id;
        }

        public async Task<IEnumerable<string>> GetAllAnswers(string pollId)
        {
            List<string> result = [];
            using (var connection = new SqliteConnection($"Data Source={DbName}"))
            {
              connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @$"
                    SELECT tbl_user.name, tbl_poll.name, tbl_question.qkey, SUM(tbl_answer.value)  
                    FROM tbl_answer 
                    INNER JOIN tbl_user , tbl_question , tbl_poll 
                    WHERE tbl_user.id = tbl_answer.tbl_user_id
                    AND tbl_question.id = tbl_answer.tbl_question_id
                    AND tbl_poll.id = tbl_question.tbl_poll_id
                    AND tbl_poll.name = $pollKey
                    GROUP BY tbl_question.qkey
                ";
                command.Parameters.AddWithValue("$pollKey", pollId);
                using var reader = command.ExecuteReader();
                const char SPACE = ' ';
                while (reader.Read())
                {
                    StringBuilder sb = new();
                    sb.Append(reader.GetString(0));
                    sb.Append(SPACE);
                    sb.Append(reader.GetString(1));
                    sb.Append(SPACE);
                    sb.Append(reader.GetString(2));
                    sb.Append(SPACE);
                    sb.Append(reader.GetInt32(3));
                    sb.Append(SPACE);
                    result.Add(sb.ToString());
                }
            }
            return result;
        }
    }
}
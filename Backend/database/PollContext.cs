using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backend.database
{
    public class PollContext : DbContext
    {
        public DbSet<tbl_poll> Polls { get; set; }
        public DbSet<tbl_question> Questions { get; set; }
        public DbSet<tbl_user> Users { get; set; }
        public DbSet<tbl_answer> Answers { get; set;}
        public string DbPath { get; }
        public PollContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "questions03");
        }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

    }
    public class tbl_poll
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<tbl_question> tbl_Questions { get; set; }
    }
    public class tbl_question
    {
        public int id { get; set;}
        public int tbl_poll_id { get; set;}
        public string qkey { get; set;}
        public string text { get; set;}
        public List<tbl_answer> tbl_Answers { get; set; }
    }

    public class tbl_user
    {
        public int id { get; set;}
        public string name { get; set; }
        public List<tbl_answer> tbl_Answers { get; set; }
    }

    public class tbl_answer
    {
        public int id { get; set; }
        public int tbl_user_id { get; set; }
        public int tbl_question_id { get; set; }
        public int value { get; set; }
        public DateTime dateTime { get; set; }

    }
}
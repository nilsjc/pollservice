using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Backend.database
{
    public class PollContext : DbContext
    {
        
    }
    public class tbl_poll
    {
        public int id { get; set;}
        public string name { get; set;}
    }
    public class tbl_question
    {
        public int id { get; set;}
        public int tbl_poll_id { get; set;}
        public string qkey { get; set;}
        public string text { get; set;}
    }

    public class tbl_user
    {
        public string name { get; set; }
    }

    public class tbl_answer
    {
        public int id { get; set; }
        public int tbl_user_id { get; set; }
    }
}
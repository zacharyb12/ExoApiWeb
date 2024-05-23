﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Models.Entity.PostModels
{
    public class BlogPost
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt{ get; set; }

    }
}

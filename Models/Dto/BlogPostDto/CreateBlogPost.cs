using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity.PostModels
{
    public class CreateBlogPost
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }

        public DateOnly CreatedAt { get; set; }

    }
}

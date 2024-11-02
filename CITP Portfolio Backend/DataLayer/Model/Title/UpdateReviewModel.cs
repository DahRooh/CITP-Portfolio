using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model.Title
{
    public class UpdateReviewModel
    {
        public string Username { get; set; }
        public int ReviewId { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; }

    }
}

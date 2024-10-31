using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model.Title
{
    public class CreateReviewModel
    {
        public string Username { get; set; }
        public string ReviewText { get; set; }
        public string TitleId { get; set; }
        public int Rating {  get; set; }
    }
}

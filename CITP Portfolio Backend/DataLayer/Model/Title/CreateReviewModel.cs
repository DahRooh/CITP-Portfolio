using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model.Title
{
    public class CreateReviewModel
    {
        public int Id { get; set; }
        public string ReviewText { get; set; }
        public int Rating {  get; set; }
        public string CaptionText { get; set; }
    }
}

using System.Collections.Generic;
using FunWithin.Models;

namespace FunWithin.Models.ViewModels
{
    public class ReviewListViewModel
    {
        public IEnumerable<Review> Reviews { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentType { get; set; }
    }
}

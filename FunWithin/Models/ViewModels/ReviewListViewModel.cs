using System.Collections.Generic;

namespace FunWithin.Models.ViewModels
{
    public class ReviewListViewModel
    {
        public IEnumerable<Review> Reviews { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentType { get; set; }
    }
}

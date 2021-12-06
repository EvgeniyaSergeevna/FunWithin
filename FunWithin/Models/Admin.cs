using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace FunWithin.Models
{
    public class Admin 
    {
        public List<Review> Reviews { get; set; }
        public User currentUser { get; set; }
        
    }
}

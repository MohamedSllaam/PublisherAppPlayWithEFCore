﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublisherDomain.Entities
{
    public class Blog
    {
   
        public int BlogId { get; set; }
        public string Url { get; set; } 
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }
    }
}

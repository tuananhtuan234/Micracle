﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Entity
{
    public class Category
    {
        public string Id { get; set; }
        public string Brand { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}

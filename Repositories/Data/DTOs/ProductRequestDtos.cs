﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.DTOs
{
    public class ProductRequestDtos
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }       
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}

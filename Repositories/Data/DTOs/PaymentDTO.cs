﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.DTOs
{
    public class PaymentDTO
    {
        public string Method { get; set; }
        public float Amount { get; set; }
        public string OrderId { get; set; }
    }
}

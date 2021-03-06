﻿using System;
using MES.DAL.Enums;

namespace MES.DAL.Entities
{
    public class Repair : IdProvider
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public RepairsVariant RepairsVariant { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
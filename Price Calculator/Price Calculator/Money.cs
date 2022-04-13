using System;
using System.Collections.Generic;
using System.Text;

namespace Price_Calculator
{
    public class Money
    {
        public double amount { get; set; }
       public string currency { get; set; }

        public Money(double amount,string currency) { this.amount = amount; this.currency = currency; }
    }
}

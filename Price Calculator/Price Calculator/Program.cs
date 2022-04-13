using System;
using System.Collections.Generic;
namespace Price_Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cap = "30%";
            Product product1 = new Product("Milk",12345,new Money(20.25,"USD"));
            var tax_dis = new Dictionary<string, int>();
            tax_dis.Add("Tax",21);
            tax_dis.Add("Discount",15);
            product1.PriceDetailsReport(tax_dis,1,cap);// 0 Additive discount ,1 Multiplicative
        }
    }
}

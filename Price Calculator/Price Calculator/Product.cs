using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Price_Calculator
{
    public enum DiscountType { addictive, multiplicative };
    internal class Product
    {   public enum DiscountType { addictive, multiplicative };
        private static int[] specialProductDiscount = { 12345, 785 };
        public string productName { get; set; }
        public int uPC { get; set; }
        public double flatRateTax;
        public double uDiscount;
        public Money basePrice { get; set; }
        public Product(string name, int upc, Money price)
        {
            productName = name;
            basePrice = price;
            uPC = upc;
            flatRateTax = 0.20;
            uDiscount = 0.15;
        }
        public Money totalDiscount = new Money(0,"USD");
        public void PriceDetailsReport(Dictionary<string,int> tax_dis,int discountType,string cap){
            if (tax_dis["Tax"] > 0 && tax_dis["Tax"] < 100)
            {
                flatRateTax = tax_dis["Tax"];
                flatRateTax = flatRateTax / 100;    
            } 
            if (tax_dis.ContainsKey("Discount"))
            {   if (tax_dis["Discount"] > 0 && tax_dis["Discount"] < 100)
                {
                    uDiscount = tax_dis["Discount"];
                    uDiscount = uDiscount / 100;
                }
                bool capFlag = cap.EndsWith("%");
                cap = cap.Substring(0, cap.Length - 1);
                double capValue = (capFlag) ? (basePrice.amount) * (double.Parse(cap) / 100) : double.Parse(cap);
                CalculateTotalPrice(capValue, discountType);
               
            }
            else
            {
                CalculateTotalPriceWithoutDis();
            }
        }
        public void CalculateTotalPriceWithoutDis()
        {  Money taxAmount = ApplyTax();
            Money totalPrice = new Money (0,basePrice.currency);
            totalPrice.amount =basePrice.amount + taxAmount.amount;
            PrintDetailsWithoutDis(taxAmount, totalPrice);

        }
        public void CalculateTotalPrice(double cap,int discountType)
        {
            double remain = basePrice.amount;
            double uDiscountAmount = ApplyUniversalDiscount();
            if (discountType == (int)DiscountType.multiplicative)
            { remain = remain - uDiscountAmount; }
            double sDiscountAmount = ApplySpecialDiscount(basePrice.amount);
            Money taxAmount = ApplyTax();
            Money transport = ApplyTransport();
            Money packaging = ApplyPackaging();
            totalDiscount.amount = sDiscountAmount + uDiscountAmount;
            totalDiscount.amount = (cap < totalDiscount.amount && discountType == (int)DiscountType.addictive) ? cap: totalDiscount.amount;
            Money totalPrice = new Money(0, basePrice.currency);
            totalPrice.amount = basePrice.amount + taxAmount.amount - totalDiscount.amount + packaging.amount + transport.amount;
            PrintDetailsWithDiscount(taxAmount, packaging, transport, totalPrice);
        }
        public Money ApplyTax()
        {
            double taxAmount = basePrice.amount * flatRateTax;
            taxAmount = Math.Round(taxAmount, 4);
            Console.WriteLine(taxAmount);
            return new Money(taxAmount,basePrice.currency);
        }
        
        public double ApplyUniversalDiscount()
        {
            double uDiscountAmount = basePrice.amount * uDiscount;
            uDiscountAmount = Math.Round(uDiscountAmount, 4);
            Console.WriteLine(uDiscountAmount);
            return uDiscountAmount;
        }
        public double ApplySpecialDiscount(double price)
        {
            double sDiscountAmount = price * 0.07;
            sDiscountAmount = Math.Round(sDiscountAmount, 4);
            Console.WriteLine(sDiscountAmount);
            return  sDiscountAmount;
        }
        public Money ApplyPackaging()
        {
           double packaging = basePrice.amount * 0.01;
            packaging = Math.Round(packaging, 4);
            return new Money(packaging,basePrice.currency);   
        }
        public Money ApplyTransport()
        {
            double transport = basePrice.amount * 0.03;
            transport = Math.Round(transport, 4);
            Console.WriteLine(transport); 
            return new Money(transport, basePrice.currency);
        }
        public void PrintDetailsWithDiscount(Money taxAmount,Money packaging,Money transport, Money totalPrice )
        {
            Console.WriteLine(totalDiscount.amount);
                Console.WriteLine($"Price Details Report of {productName}:\n Base Price: {basePrice.amount} {basePrice.currency} \n" +
                           $" Tax Amount: {taxAmount.amount:F2}{taxAmount.currency}\n Discount Amount: {totalDiscount.amount:F2}{totalDiscount.currency} \n Packaging: {packaging.amount:F2}{packaging.currency} \n Transport: {transport.amount:F2}{transport.currency}" +
                           $"\n Total Price: {totalPrice.amount:F2}{totalPrice.currency} ");
        }
        public void PrintDetailsWithoutDis(Money taxAmount, Money totalPrice)
        {
            Console.WriteLine($"Price Details Report of {productName}:\n Base Price: {basePrice.amount} {basePrice.currency} \n" +
        $" Tax Amount: {taxAmount.amount:F2}{taxAmount.currency} \n Total Price: {totalPrice.amount:F2} {totalPrice.currency} ");
        }
    }
}

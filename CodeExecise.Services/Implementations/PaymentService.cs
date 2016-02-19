using System;
using System.Linq;
using System.Security.Cryptography;
using CodeExecise.Services.Interfaces;

namespace CodeExecise.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        public string WhatsYourId()
        {
            return "E4ADE275-03EE-4C86-82A9-6649C11AF920";
        }

        public bool IsCardNumberValid(string cardNumber)
        {
            //// check whether input string is null or empty
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return false;
            }

            // 1.	Starting with the check digit double the value of every other digit 
            // 2.	If doubling of a number results in a two digits number, add up
            //      the digits to get a single digit number. This will results in eight single digit numbers                    
            // 3. Get the sum of the digits
            var sumOfDigits = cardNumber.Where((e) => e >= '0' && e <= '9')
                            .Reverse()
                            .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                            .Sum((e) => e / 10 + e % 10);


            //  If the final sum is divisible by 10, then the credit card number
            //  is valid. If it is not divisible by 10, the number is invalid.            
            return sumOfDigits % 10 == 0;
        }

        public bool IsValidPaymentAmount(long amount)
        {
            return amount > 99 && amount < 99999999;
        }

        public bool CanMakePaymentWithCard(string cardNumber, int expiryMonth, int expiryYear)
        {
            var isValidCardNumber = IsCardNumberValid(cardNumber);

            if (!isValidCardNumber)
                return false;

            var today = DateTime.Today;

            if (expiryMonth < 1 || expiryMonth > 12 || expiryYear < today.Year)
                return false;

            return ((expiryYear * 100) + expiryMonth) >= ((today.Year * 100) + today.Month);
        }
    }
}

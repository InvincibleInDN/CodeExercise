using System;
using System.Linq;
using System.Security.Cryptography;
using CodeExecise.Services.Interfaces;

namespace CodeExecise.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private static byte RandomByte()
        {
            using (var randomizationProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[1];
                randomizationProvider.GetBytes(randomBytes);
                return randomBytes.Single();
            }
        }

        public string WhatsYourId()
        {
            const int length = 10;
            const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var outOfRange = Byte.MaxValue + 1 - (Byte.MaxValue + 1) % alphabet.Length;

            return string.Concat(
                Enumerable
                    .Repeat(0, Int32.MaxValue)
                    .Select(e => RandomByte())
                    .Where(randomByte => randomByte < outOfRange)
                    .Take(length)
                    .Select(randomByte => alphabet[randomByte % alphabet.Length])
            );
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

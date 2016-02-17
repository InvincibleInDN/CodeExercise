using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeExecise.Services.Implementations;
using CodeExercise.Controllers;
using NUnit.Framework;

namespace CodeExercise.Tests.Controllers
{
    [TestFixture]
    public class PaymentControllerTest
    {
        private readonly PaymentController _paymentController;
        public PaymentControllerTest()
        {
            _paymentController = new PaymentController(new PaymentService()) { Request = new HttpRequestMessage() };
            _paymentController.Request.SetConfiguration(new HttpConfiguration());
        }

        #region "WhatsYourId Tests"

        [TestCase(1000000)]
        public void WhatsYourId(int upperLimit)
        {
            var uniqueIds = new List<string>();
            for (var i = 0; i < upperLimit; i++)
            {
                var response = _paymentController.WhatsYourId();
                string uniqueId;
                if (response.TryGetContentValue(out uniqueId))
                    uniqueIds.Add(uniqueId);
            }

            Assert.AreEqual(uniqueIds.Distinct().Count(), upperLimit);
        }

        #endregion

        #region "IsCardNumberValid Tests"

        [TestCase("")]
        [TestCase("   ")]
        [TestCase(null)]
        public void IsCardNumberValid_NullOrWhitespaceCardNumber(string cardNumber)
        {
            var response = _paymentController.IsCardNumberValid(cardNumber);
            bool isValid;
            response.TryGetContentValue(out isValid);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.MethodNotAllowed);
        }

        [TestCase("4526477355367834")]
        [TestCase("4108008877676736")]
        [TestCase("5432326762467667")]
        [TestCase("34524")]
        public void IsCardNumberValid_InvalidCardNumber(string cardNumber)
        {
            var response = _paymentController.IsCardNumberValid(cardNumber);
            bool isValid;
            response.TryGetContentValue(out isValid);

            Assert.IsFalse(isValid);
        }

        [TestCase("378282246310005", TestName = "IsCardNumberValid : AMEX 1")]
        [TestCase("371449635398431", TestName = "IsCardNumberValid : AMEX 2")]
        [TestCase("378734493671000", TestName = "IsCardNumberValid : AMEX Corporate")]
        [TestCase("5610591081018250", TestName = "IsCardNumberValid : Australian Bank Card")]
        [TestCase("30569309025904", TestName = "IsCardNumberValid : Diners Club 1")]
        [TestCase("38520000023237", TestName = "IsCardNumberValid : Diners Club 2")]
        [TestCase("6011111111111117", TestName = "IsCardNumberValid : Discover 1")]
        [TestCase("6011000990139424", TestName = "IsCardNumberValid : Discover 2")]
        [TestCase("3530111333300000", TestName = "IsCardNumberValid : JCB 1")]
        [TestCase("3566002020360505", TestName = "IsCardNumberValid : JCB 2")]
        [TestCase("5555555555554444", TestName = "IsCardNumberValid : MasterCard 1")]
        [TestCase("5105105105105100", TestName = "IsCardNumberValid : MasterCard 2")]
        [TestCase("4111111111111111", TestName = "IsCardNumberValid : Visa 1")]
        [TestCase("4012888888881881", TestName = "IsCardNumberValid : Visa 2")]
        [TestCase("4222222222222", TestName = "IsCardNumberValid : Visa 3")]
        public void IsCardNumberValid_ValidCardNumber(string cardNumber)
        {
            var response = _paymentController.IsCardNumberValid(cardNumber);
            bool isValid;
            response.TryGetContentValue(out isValid);
            Assert.IsTrue(isValid);
        }

        #endregion

        #region "IsValidPaymentAmount tests"

        [TestCase(90)]
        [TestCase(99)]
        [TestCase(0)]
        [TestCase(-8)]
        [TestCase(78)]
        [TestCase(99999999)]
        public void IsValidPaymentAmount_InvalidAmount(long amount)
        {
            var response = _paymentController.IsValidPaymentAmount(amount);
            bool isValid;
            response.TryGetContentValue(out isValid);
            Assert.IsFalse(isValid);            
        }

        [TestCase(100)]
        [TestCase(5000)]
        [TestCase(456398)]
        [TestCase(1234)]
        [TestCase(99999998)]
        public void IsValidPaymentAmount_ValidAmount(long amount)
        {
            var response = _paymentController.IsValidPaymentAmount(amount);
            bool isValid;
            response.TryGetContentValue(out isValid);
            Assert.IsTrue(isValid); 
        }

        #endregion

        #region "CanMakePaymentWithCard tests"

        [TestCase("378282246310005", 1, 2016)]
        [TestCase("378282246317605", 1, 2020)]
        [TestCase("4222222222222", 12, 2015)]
        public void CanMakePaymentWithCard_InvalidTestCases(string cardNumber, int expiryMonth, int expiryYear)
        {
            var response = _paymentController.CanMakePaymentWithCard(cardNumber, expiryMonth, expiryYear);
            bool isValid;
            response.TryGetContentValue(out isValid);
            Assert.IsFalse(isValid);             
        }

        [TestCase("378282246310005", 2, 2016)]
        [TestCase("4012888888881881", 5, 2018)]
        [TestCase("4222222222222", 12, 2020)]
        public void CanMakePaymentWithCard_ValidTestCases(string cardNumber, int expiryMonth, int expiryYear)
        {
            var response = _paymentController.CanMakePaymentWithCard(cardNumber, expiryMonth, expiryYear);
            bool isValid;
            response.TryGetContentValue(out isValid);
            Assert.IsTrue(isValid);             
        }

        #endregion

    }
}

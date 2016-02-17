using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeExecise.Services.Interfaces;

namespace CodeExercise.Controllers
{
    public class PaymentController : ApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public HttpResponseMessage WhatsYourId() // {URL}/api/Payment/WhatsYourId
        {
            return Request.CreateResponse(HttpStatusCode.OK, _paymentService.WhatsYourId());
        }

        [HttpGet]
        public HttpResponseMessage IsCardNumberValid(string cardNumber) // {URL}/api/Payment/IsCardNumberValid?cardNumber=
        {
            return string.IsNullOrWhiteSpace(cardNumber)
                ? Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "cardNumber cannot be null.")
                : Request.CreateResponse(HttpStatusCode.OK, _paymentService.IsCardNumberValid(cardNumber));
        }

        [HttpGet]
        public HttpResponseMessage IsValidPaymentAmount(long amount) // {URL}/api/Payment/IsValidPaymentAmount?amount=
        {
            return Request.CreateResponse(HttpStatusCode.OK, _paymentService.IsValidPaymentAmount(amount));
        }

        public HttpResponseMessage CanMakePaymentWithCard(string cardNumber, int expiryMonth, int expiryYear) // {URL}/api/Payment/CanMakePaymentWithCard?cardNumber=&expiryMonth=&expiryYear
        {
            return string.IsNullOrWhiteSpace(cardNumber)
                ? Request.CreateErrorResponse(HttpStatusCode.MethodNotAllowed, "cardNumber cannot be null.")
                : Request.CreateResponse(HttpStatusCode.OK, _paymentService.CanMakePaymentWithCard(cardNumber, expiryMonth, expiryYear));
        }
    }
}

namespace PamojaWebsite.Services
{
    using Microsoft.Extensions.Options;
    using Stripe;

    public class StripePaymentService
    {
        private readonly StripeOptions _opts;

        public StripePaymentService(StripeOptions opts)
        {
            _opts = opts;
        }

        public async Task<PaymentIntent> CreatePaymentIntentAsync(long amountCents, string currency, string customerEmail)
        {
            customerEmail = string.IsNullOrWhiteSpace(customerEmail) ? "briankarimi@pamojasafeguarding.com" : customerEmail;
            var client = new StripeClient(_opts.SecretKey);
            var service = new PaymentIntentService(client);

            var intent = await service.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = amountCents,
                Currency = currency,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions { Enabled = true },
                ReceiptEmail = customerEmail
            });
            return intent;
        }
    }
}

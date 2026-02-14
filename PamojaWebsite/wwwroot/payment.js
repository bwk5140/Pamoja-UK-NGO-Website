export async function initStripePayment(amountCents, currency, email) {
    const intentResp = await fetch('/api/payments/stripe/intent', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ amountCents, currency, email })
    });
    const { clientSecret, publishableKey } = await intentResp.json();

    const stripe = Stripe(publishableKey);
    const elements = stripe.elements({ clientSecret });

    const paymentElement = elements.create('payment');
    paymentElement.mount('#stripe-payment-element');

    return {
        submit: async () => {
            try {
                const { error } = await stripe.confirmPayment({ elements, redirect: "if_required" , confirmParams: { return_url: window.location.origin + '/payment/?status=success/' } });
                if (error) {
                    console.log("Stripe error:", error.message);
                    return { success: false, error: error.message };
                }
                console.log("Stripe success");
                return { success: true };
            } catch (e) {
                console.error("Submit threw:", e);
                return { success: false, error: e.message };
            }
        }

    };
}

export async function renderPayPal(amount, currency) {
    return new Promise((resolve) => {
        const container = document.getElementById("paypal-buttons");
        if (container) {
            container.innerHTML = ""; // clear old buttons
        }
        paypal.Buttons({
            createOrder: async () => {
                const resp = await fetch('/api/paypal/order', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ amount, currency })
                });
                const { orderId } = await resp.json();
                return orderId;
            },
            onApprove: async (data, actions) => {
                const capture = await fetch('/api/paypal/capture', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ orderId: data.orderID })
                });
                const result = await capture.json();
                resolve(result);
                return actions.order.capture().then(function (details) {
                    // ✅ Redirect after successful payment
                    window.location.href = window.location.origin + "/payment-confirmation?Id=data.orderID";
                });
            },
            onCancel: function (data) {
                // ❌ Redirect if user cancels
                window.location.href = window.location.origin + "/payment-unsuccessful?message=cancelled";
            },
            onError: function (err) {
                // ⚠️ Redirect or show error
                window.location.href = window.location.origin + "/payment-unsuccessful?message=error";
            }
        }).render('#paypal-buttons');
    });
}
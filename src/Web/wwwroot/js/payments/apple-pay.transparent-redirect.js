// https://developer.apple.com/documentation/apple_pay_on_the_web/applepaypaymentauthorizationresult
const SuccessAuthorizationResult  = {
    status: ApplePaySession.STATUS_SUCCESS,
    errors: [],
};

const FailureAuthorizationResult = {
    status: ApplePaySession.STATUS_FAILURE,
    errors: [
        {
            code: "unknown",
            message:
                "An unknown error occurred while processing your payment request. Please contact the tech support to solve this issue.",
        },
    ],
}

async function paymentHandler(event) {
    console.log("Apple Pay Session 'onPaymentAuthorized': ", event.payment);

    // Get the contact details for use, for example to
    // use to create an account for the user.
    if (event.payment.billingContact) {
        console.log(event.payment.billingContact);
    }

    if (event.payment.shippingContact) {
        console.log(event.payment.shippingContact);
    }

    const urlParams = new URLSearchParams(window.location.search);
    const productId = urlParams.get("id");
    const initialPayload = {
        ProductId: +productId,
        PaymentMethod: "ApplePay",
    };

    const initialUrl = document.querySelector("#apple-pay-button").dataset.initialUrl;

    try {
        const initialResponse = await fetch(initialUrl, {
            method: "POST",
            cache: "no-cache",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(initialPayload),
        });

        console.log("AccessCode creation response: ", initialResponse);

        const initialContext = await initialResponse.json();

        console.log("AccessCode creation response body: ", initialContext);

        const paymentPayload = new FormData();
        paymentPayload.append("EWAY_PAYMENTTYPE", "ApplePay");
        paymentPayload.append("EWAY_ACCESSCODE", initialContext.accessCode);
        paymentPayload.append("APPLEPAY_NETWORKTOKEN", event.payment.token.PaymentData);
        const payUrl = `${initialContext.endpoint}/${initialContext.accessCode}`;

        console.log(`Submit payment request to eWAY: ${payUrl}`);

        const paymentResponse = await fetch(payUrl, {
            method: "POST",
            mode: "no-cors",
            body: paymentPayload,
        });

        console.log("Payment response: ", paymentResponse);
        const data = await response.text();
        console.log("Payment response body: ", JSON.parse(data));
    } catch (error) {
        console.log("Error on submiting payment: ", error);
        return FailureAuthorizationResult;
    }

    return SuccessAuthorizationResult;
}

console.log("Apple Pay for transparent redirect payment js file loaded.");

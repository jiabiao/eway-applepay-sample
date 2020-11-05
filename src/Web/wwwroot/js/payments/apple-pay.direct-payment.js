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
    const purchasePayload = {
        ProductId: +productId,
        PaymentMethod: "ApplePay",
        ApplePayToken: event.payment.token,
    };

    const requestInit = {
        method: "POST",
        cache: "no-cache",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(purchasePayload),
    };

    const purchaseUrl = document.querySelector("#apple-pay-button").dataset.purchaseUrl;

    try {
        const response = await fetch(purchaseUrl, requestInit);
        console.log("Payment response: ", response);

        const data = await response.text();
        console.log("Payment response body: ", JSON.parse(data));
    } catch (error) {
        console.log("Error on submiting payment: ", error);
        return FailureAuthorizationResult;
    }

    return SuccessAuthorizationResult;
}

console.log("Apple Pay for direct payment js file loaded.");

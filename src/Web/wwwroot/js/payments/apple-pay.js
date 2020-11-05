const applePayButton = document.querySelector("#apple-pay-button");
const validateUrl = applePayButton.dataset.validationUrl;
const merchantIdentifider = applePayButton.dataset.merchantIdentifider;
const countryCode = applePayButton.dataset.countryCode;
const currencyCode = applePayButton.dataset.currencyCode;

function ApplePay() {
    let applePayOptions = {};

    /**
     * Before displaying an Apple Pay button or creating an Apple Pay session, you must
     * ensure that the Apple Pay JS API is available and enabled on the current device.
     * https://developer.apple.com/documentation/apple_pay_on_the_web/apple_pay_js_api/checking_for_apple_pay_availability
     * @param {function} onSupported - Callback function when Apple Pay can make payments or make payments with active card.
     * @param {function} onSetup - Callback function when Apple Pay can not make payments with active card.
     */
    function checkAvailability(onSupported, onSetup) {
        console.log("Checking the Apple Pay availability...");
        if (window.ApplePaySession && onSupported instanceof Function) {
            if (ApplePaySession.canMakePayments()) {
                return onSupported();
            }

            if (!applePayOptions.merchantIdentifider) {
                throw new Error(
                    "Merchant Identifider is required for Apple Pay."
                );
            }

            ApplePaySession.canMakePaymentsWithActiveCard(
                applePayOptions.merchantIdentifider
            ).then((canMakePayments) => {
                if (canMakePayments) {
                    // Display the Apple Pay button
                    onSupported();
                } else {
                    // Check for the existence of the openPaymentSetup method.
                    if (onSetup instanceof Function) {
                        onSetup();
                    }
                }
            });
        } else {
            const msg = "Apple Pay is not supported by this device/browser.";
            console.warn(msg);
            const warning = document.querySelector("div.apple-pay-error");
            warning.innerHTML = `<strong>${msg}</strong>`;
            warning.classList.remove("d-none");
        }
    }

    /**
     * Setup Apple Pay
     * https://developer.apple.com/documentation/apple_pay_on_the_web/applepaysession/2710269-openpaymentsetup
     */
    function setupApplePay() {
        console.log("Start to setup the Apple Pay...");
        if (ApplePaySession.openPaymentSetup) {
            ApplePaySession.openPaymentSetup(
                applePayOptions.merchantIdentifider
            ).then(function (success) {
                if (success) {
                    removeApplePaySetupButton();
                    displayApplePayButton();
                } else {
                    throw new Error("Failed to setup Apple Pay.");
                }
            });
        }
    }

    function displayApplePayButton() {
        console.log("Displaying the Apple Pay payment button.");
        const button = document.getElementById("apple-pay-button");
        button.addEventListener("click", beginPayment);
        button.setAttribute("lang", applePayOptions.language);
        if (ApplePaySession.openPaymentSetup) {
            button.classList.add(
                "apple-pay-button-with-text",
                "apple-pay-button-black-with-text"
            );
        } else {
            button.classList.add("apple-pay-button", "apple-pay-button-black");
        }
        button.classList.remove("d-none");
    }

    function displayApplePaySetupButton() {
        console.log("Displaying the Apple Pay setup button.");
        const button = document.getElementById("apple-pay-set-up-button");
        button.addEventListener("click", setupApplePay);
        button.classList.remove("d-none");
    }

    function removeApplePaySetupButton() {
        console.log("Removing the Apple Pay setup button.");
        const button = document.getElementById("apple-pay-set-up-button");
        button.classList.add("d-none");
        button.removeEventListener("click", setupApplePay);
    }

    function initialize(options) {
        console.log("Initialize the Apple Pay...");
        if (options instanceof Object) {
            applePayOptions = options;
        } else {
            applePayOptions = {
                storeName: "example",
                merchantIdentifider: "merchant.com.example",
                merchantCapabilities: [
                    "supports3DS",
                    "supportsDebit",
                    "supportsCredit",
                ],
                supportedNetworks: ["visa", "masterCard", "amex", "discover"],
                language: "en",
                countryCode: "AU",
                currencyCode: "AUD",
                paymentHandler: function (event) {
                    console.log("Default action payment handler, you need to write your version when ititialize", event);
                },
            };
        }

        checkAvailability(displayApplePayButton, displayApplePaySetupButton);
    }

    /**
     * Handle payment when the Apple Pay button is clicked/pressed.
     */
    function beginPayment() {
        console.log("Begin the Apple Pay payment...");

        // Get the amount to request from the form and set up
        // the totals and line items for collection and delivery.
        const urlParams = new URLSearchParams(window.location.search);
        const productId = urlParams.get("id");
        const productPrice = document.querySelector(
            `div[data-product-id='${productId}'] > p[data-product-price]`
        ).dataset.productPrice;
        const productName = document.querySelector(
            `div[data-product-id='${productId}'] > h5[data-product-name]`
        ).dataset.productName;

        const skuItem = {
            // https://developer.apple.com/documentation/apple_pay_on_the_web/applepaylineitem
            label: productName + " * 1",
            amount: productPrice,
        };

        const total = {
            label: applePayOptions.storeName,
            amount: productPrice,
        };

        // Create the Apple Pay payment request as appropriate.
        // https://developer.apple.com/documentation/apple_pay_on_the_web/applepaypaymentrequest
        const paymentRequest = {
            applicationData: btoa("Custom application-specific data"),
            merchantCapabilities: applePayOptions.merchantCapabilities,
            supportedNetworks: applePayOptions.supportedNetworks,
            countryCode: applePayOptions.countryCode,
            currencyCode: applePayOptions.currencyCode,
            total: total,
            lineItems: [skuItem],
        };

        console.log(
            "Payment request construction completed, start to build payment session."
        );

        console.debug("payment request: ", paymentRequest);

        // Create the Apple Pay session.
        const session = new ApplePaySession(3, paymentRequest);

        // Setup handler for validation the merchant session.
        session.onvalidatemerchant = function (event) {
            // Create the payload.
            const payload = {
                validationUrl: event.validationURL,
            };
            console.log("Apple Pay session onValidateMerchant: ", payload);

            // Post the payload to the server to validate the merchant session using the merchant certificate.
            fetch(validateUrl, {
                method: "POST",
                cache: "no-cache",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(payload),
            }).then((response) => {
                // Complete validation by passing the merchant session to the Apple Pay session.
                if (!response.ok) {
                    console.error("Merchant validation response", response);
                    throw new Error("Error on merchant validation.");
                }

                response.json().then((sessionData) => {
                    console.log(
                        "Merchant validation response data: ",
                        sessionData
                    );
                    session.completeMerchantValidation(
                        sessionData.merchantSession
                    );
                });
            });
        };

        // Setup handler to receive the token when payment is authorized.
        session.onpaymentauthorized = function (event) {
            const echoConsole = document.getElementById("echoConsole");

            if(echoConsole.dataset.echoMode){
                echoConsole.innerText = JSON.stringify(event.payment.token, null, 2).trim();
                echoConsole.classList.remove('d-none');
            }

            try {
                // Do something with the payment to capture funds and
                // then dismiss the Apple Pay sheet for the session with
                // the relevant status code for the payment's authorization.
                // https://developer.apple.com/documentation/apple_pay_on_the_web/applepaysession/1778012-completepayment
                applePayOptions.paymentHandler(event)
                               .then(authorizationResult => session.completePayment(authorizationResult));
            } catch (error) {
                session.completePayment({
                    status: ApplePaySession.STATUS_FAILURE,
                    errors: [
                        {
                            code: "unknown",
                            message: "An unknown error occurred while processing your payment request. Please contact the tech support to solve this issue.",
                        },
                    ],
                });
                console.log("Error on payment: ", error);
            }

            if (!echoConsole.dataset.echoMode) {
                window.location.href = `payments/result?order=${data.orderId}`;
            }
        };

        // Start the session to display the Apple Pay sheet.
        session.begin();
    }

    return {
        initialize,
    };
}

(function () {
    console.log("Apple Pay js file loaded.");
    ApplePay().initialize({
        storeName: "Monkey Store",
        merchantIdentifider: merchantIdentifider,
        merchantCapabilities: [
            "supports3DS",
            "supportsDebit",
            "supportsCredit",
        ],
        supportedNetworks: ["visa", "masterCard", "amex", "discover"],
        language: "en",
        countryCode: countryCode,
        currencyCode: currencyCode,
        paymentHandler: paymentHandler,
    });
})();

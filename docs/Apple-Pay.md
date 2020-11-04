# Apple Pay

In this article, we will guide you to go through with Apple Pay developing and sandbox testing.

## Before you begin

To enable Apple Pay for you, you need to have an Apple Developer Account that is associated with either the [Apple Developer Program](https://developer.apple.com/programs), or [Apple Deveoper Enterprise Program.](https://developer.apple.com/programs/enterprise)

Your user can only see Apple Pay as a payment method option if they:

+ Use an Apple Pay compatible device.
+ Use Safari if they are paying on the web.
+ Located in a country or region where Apple Pay is [available.](https://support.apple.com/en-us/HT207957)
+ have an exsiting card added to their Apple Pay wallet.

## Process overview

To add the Apple Pay entitlement to your website or mobile application, you need to:

1. Create a merchant identifier in your Apple developer account.
1. Enable the Apple Pay capabilities.
1. Create a payment processing certificate for your merchant identifier, and send the private key to backend payment processor so that they can activate it. Apple Pay uses this certificate to encrypt payment information, and we need to have this certificate to be able to decrypt and process the payment.

    **For web you also need to:**

1. Register and validate your merchant domain with Apple.
1. Create a merchant identity certificate, convert it to a PEM/PFX file with the private key, and upload it to your server. This certificate is used to authenticate communication with the Apple Pay servers.
For information about server requirements for Apple Pay on the web, refer to the Apple Developer portal.

## Step 1: Create your merchant identifier

1. Log in to your Apple Developer account at [https://developer.apple.com](https://developer.apple.com).

1. Follow the Apple Developer Account Help instructions to create a merchant identifier, but note the following:
   + In the step to enter the merchant description and identifier name, make sure that the identifier includes the prefix **merchant.** we recommend creating the identifier in the format of **merchant.{Application Bundle Identifier}**. For example: **merchant.com.eway.monkeystore**
   + For test accounts, we recommend adding **.test** to the identifier, for example: **merchant.com.eway.monkeystore.test**.
   + The **merchant.** prefix is the required by Apple.

Generate the Certificate Signing Request (CSR) from you Keychain Access on your computer. You'll use this CSR to create a payment processing certificate.

## Step 2: Enable Apple Pay capabilites

Log in to your Apple Developer account center and just follow the Apple Developer Help instructions to [enable](https://help.apple.com/developer-account/#/dev4cb6dfbdb?sub=dev3f2891710) Apple Pay.

## Step 3: Create a payment processing certificate

1. Log in to your Apple Developer account center.

1. Follow the developer instructions to [create](https://help.apple.com/developer-account/#/devb2e62b839) a payment processing certificate.
   + In the step to select a merchant identifier, make sure you select the merchant identifier you created in Step 1.
   + Skip the step to create a certificate signing request.
   + In the step to select the certificate signing request file, select the CSR you generate on Step 1.
   + The question **Will payments associated with this Merchant ID be processed exclusively in China?** or similar one's ansower should be **No**.
   + Download and save the generated payment processing certificate (.cer file)

1. You can now start processing Apple Pay payments in your iOS app. For Apple Pay on the web, you need to do the additional steps described below.

## Step 4 (Optional): Register and validate your merchant domain

**This step is only required when you want to enable Apple Pay on the web.**

1. Log in to your Apple Developer account at [https://developer.apple.com](https://developer.apple.com).
1. Follow the Apple Developer Account Help instructions to [register and verify](https://help.apple.com/developer-account/#/dev1731126fb) your e-commerce domain which will host the Apple Pay process app.
    **Note: When selecting the merchant identifier, make sure you select the merchant identifier you created in Step 1.**

## Step 5 (Optional): Create a merchant identify certificate

**This step is only required when you want to enable Apple Pay on the web.**

1. For each transaction, you need to [request an Apple Pay payment session](https://developer.apple.com/documentation/apple_pay_on_the_web/apple_pay_js_api/requesting_an_apple_pay_payment_session) using your Merchant Identity Certificate on your back-end. In this step, you will create that certificate.

1. Log in to your Apple Developer account at [https://developer.apple.com](https://developer.apple.com).

1. Follow the Apple Developer Account Help instructions section [create a merchant identity certificate](https://help.apple.com/developer-account/#/dev1731126fb) to create one.

    > **NOTES:**
    >
    > + When selecting the merchant identifier, make sure you select the merchant identifier you created in Step 1.
    > + Follow the instructions from Apple to create another CSR yourself
    > + Download and save the generated merchant identity certificate (.cer file).
    > + When you have completed the instructions from Apple, add the merchant identity certificate to your keychain by click ther cert file.
    > + Export the certificate from your keychain as a p12 file.
    > + merge the p12 file with cert using the following command in the end of this section:
    > + Upload the the converted file to your server.

```sh

openssl x509 -inform der -in merchant_id.cer -out merchant_id.pem

openssl pkcs12 -nocerts -in apple-pay-test-key.p12 -out apple-pay-test-key.pem

openssl rsa -in apple-pay-test-key.pem -out apple-pay-test.key

openssl pkcs12 -export -in merchant_id.pem -inkey apple-pay-test.key -out merchant_ip12

```

## Step 6: Sandbox Testing

1. Log in to [App Store Connect](https://appstoreconnect.apple.com/).
1. On the homepage, click Users and Access.
1. Under Sandbox, click Testers.
1. Click “+” to set up your tester accounts.
1. Complete the tester information form and click Invite.
1. Sign out of your Apple ID on all testing devices and sign back in with your new sandbox tester account.

    > **Notes:**
    >
    > + Before you sign out of your Apple ID on the testing device, you'd better change the region setting to US
    > + The test card should alawys added from Wallet app by manually input even you added it before.
    > + If you want to debug the iOS app on a real device, you need to use the account with admin privileges to create a provisioning file and code signing cert in the Apple developer center.

## References

+ [Apple Pay in the PassKit](https://developer.apple.com/documentation/passkit/apple_pay)
+ [Apple Pay on the Web](https://developer.apple.com/documentation/apple_pay_on_the_web)
+ [Payment Token Format reference](https://developer.apple.com/library/archive/documentation/PassKit/Reference/PaymentTokenJSON/PaymentTokenJSON.html)
+ [Configuring your envrionment](https://developer.apple.com/documentation/apple_pay_on_the_web/configuring_your_environment)
+ [Sandbox Testing](https://developer.apple.com/apple-pay/sandbox-testing/)
+ [Apple Pay in Xamarin.iOS](https://docs.microsoft.com/en-us/xamarin/ios/platform/apple-pay)
+ [Apple Pay is compatible with these devices](https://support.apple.com/en-us/HT208531)
+ [Everything you ever wanted to know about SSL](http://www.robinhowlett.com/blog/2016/01/05/everything-you-ever-wanted-to-know-about-ssl-but-were-afraid-to-ask/)
+ [Mutual Authentication Protocal for HTTP](https://tools.ietf.org/html/rfc8120)
+ [Configure certificate authentication in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-3.1)

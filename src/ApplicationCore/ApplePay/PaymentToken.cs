/// Apple Pay payment token models. For more details, see the link below:
/// https://developer.apple.com/library/archive/documentation/PassKit/Reference/PaymentTokenJSON/PaymentTokenJSON.html

namespace eWAY.Samples.MonkeyStore.Models.ApplePay
{
    public class PaymentToken
    {
        public string TransactionIdentifier { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentData PaymentData { get; set; }
    }

    public class PaymentData
    {
        /// <summary>
        /// Encrypted payment data.
        /// </summary>
        /// <value>Payment data dictionary, Base64 encoded as a string.</value>
        public string Data { get; set; }

        /// <summary>
        /// Additional version-dependent information used to decrypt and verify the payment.
        /// </summary>
        public PaymentHeader Header { get; set; }

        /// <summary>
        /// Signature of the payment and header data. The signature includes the signing certificate,
        /// its intermediate CA certificate, and information about the signing algorithm.
        /// </summary>
        /// <value>detached PKCS #7 signature, Base64 encoded as string</value>
        public string Signature { get; set; }

        /// <summary>
        /// Version information about the payment token.
        /// The token uses EC_v1 for ECC-encrypted data, and RSA_v1 for RSA-encrypted data.
        /// </summary>
        public string Version { get; set; }
    }

    public class PaymentHeader
    {
        /// <summary>
        /// Optional. Hash of the applicationData property of the original PKPaymentRequest object.
        /// If the value of that property is nil, this key is omitted.
        /// </summary>
        /// <value>SHA–256 hash, hex encoded as a string</value>
        public string ApplicationData { get; set; }

        /// <summary>
        /// The symmetric key wrapped using your RSA public key.
        /// RSA_v1 only.
        /// </summary>
        /// <value>A Base64 encoded string</value>
        public string WrappedKey { get; set; }

        /// <summary>
        /// Ephemeral public key bytes.
        /// EC_v1 only.
        /// </summary>
        /// <value>X.509 encoded key bytes, Base64 encoded as a string</value>
        public string EphemeralPublicKey { get; set; }

        /// <summary>
        /// Hash of the X.509 encoded public key bytes of the merchant’s certificate.
        /// </summary>
        /// <value>SHA–256 hash, Base64 encoded as a string</value>
        public string PublicKeyHash { get; set; }

        /// <summary>
        /// Transaction identifier, generated on the device.
        /// </summary>
        /// <value>A hexadecimal identifier, as a string</value>
        public string TransactionId { get; set; }
    }

    public class PaymentMethod
    {
        public string DisplayName { get; set; }

        public PaymentNetwork Network { get; set; }

        public PaymentMethodType Type { get; set; }
    }

    /// <summary>
    /// Enumerates the types of cards available to Apple Pay.
    /// </summary>
    public enum PaymentMethodType : ulong
    {
        /// <summary>
        /// The card’s type is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// A debit card.
        /// </summary>
        Debit = 1,

        /// <summary>
        /// A credit card.
        /// </summary>
        Credit = 2,

        /// <summary>
        /// A prepaid card.
        /// </summary>
        Prepaid = 3,

        /// <summary>
        /// A store card.
        /// </summary>
        Store = 4
    }

    public enum PaymentNetwork
    {
        /// <summary>
        /// American Express.
        /// </summary>
        Amex,

        /// <summary>
        /// Cartes Bancaires.
        /// </summary>
        CartesBancaires,

        /// <summary>
        /// China Union Pay.
        /// </summary>
        ChinaUnionPay,

        /// <summary>
        /// Discover.
        /// </summary>
        Discover,

        /// <summary>
        /// Electronic funds transfer at point of sale.
        /// </summary>
        Eftpos,

        /// <summary>
        ///  Electron.
        /// </summary>
        Electron,

        /// <summary>
        /// Elo.
        /// </summary>
        Elo,

        /// <summary>
        /// iD.
        /// </summary>
        IdCredit,

        /// <summary>
        /// Interac.
        /// </summary>
        Interac,

        /// <summary>
        /// JCB.
        /// </summary>
        JCB,

        /// <summary>
        ///  MADA.
        /// </summary>
        Mada,

        /// <summary>
        /// Maestro.
        /// </summary>
        Maestro,

        /// <summary>
        /// MasterCard.
        /// </summary>
        MasterCard,

        /// <summary>
        /// Store credit and debit cards.
        /// </summary>
        PrivateLabel,

        /// <summary>
        /// QUICPay.
        /// </summary>
        QuicPay,

        /// <summary>
        /// Suica.
        /// </summary>
        Suica,

        /// <summary>
        /// Visa.
        /// </summary>
        Visa,

        /// <summary>
        /// Visa V Pay.
        /// </summary>
        VPay,
    }
}

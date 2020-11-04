using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace eWAY.Samples.MonkeyStore
{
    public static class ApplePayCertificates
    {
        public static void LoadMerchantIdentifierCertificate(IConfiguration configuration)
        {
            using X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var thumbprint = configuration.GetValue<string>("ApplePay:MerchantIdentifierCertificate:Thumbprint");
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint,
                                                       thumbprint,
                                                       validOnly: false);
            MerchantIdentifierCertificate = certificates[0];
        }

        public static X509Certificate2 MerchantIdentifierCertificate { get; private set; }

        public static string MerchantIdentifier
        {
            get
            {
                // This OID returns the ASN.1 encoded merchant identifier
                var extension = MerchantIdentifierCertificate.Extensions["1.2.840.113635.100.6.32"];
                if (extension == null)
                {
                    return string.Empty;
                }

                // Convert the raw ASN.1 data to a string containing the ID
                return Encoding.ASCII.GetString(extension.RawData).Substring(2);
            }
        }
    }
}

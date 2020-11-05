// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonkeyStore.Options;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MonkeyStore.Services
{
    public class ApplePayCertificateProvider
    {
        private readonly IOptionsMonitor<ApplePayOptions> _optionsMonitor;
        private readonly ILogger<ApplePayCertificateProvider> _logger;
        private readonly Dictionary<string, X509Certificate2> _certificates;

        public ApplePayCertificateProvider(IOptionsMonitor<ApplePayOptions> optionsMonitor,
                                           ILogger<ApplePayCertificateProvider> logger)
        {
            _optionsMonitor = optionsMonitor;
            _logger = logger;
            _certificates = new Dictionary<string, X509Certificate2>();
        }

        public string GetMerchantIdentifier()
        {
            var certificate = GetMerchantIdentityCertificate();
            if (certificate == null)
            {
                return string.Empty;
            }

            // This OID returns the ASN.1 encoded merchant identifier
            var extension = certificate.Extensions["1.2.840.113635.100.6.32"];
            if (extension == null)
            {
                return string.Empty;
            }

            // Convert the raw ASN.1 data to a string containing the ID
            return Encoding.ASCII.GetString(extension.RawData)[2..];
        }

        public X509Certificate2 GetMerchantIdentityCertificate()
        {
            var thumbprint = _optionsMonitor.CurrentValue.Thumbprint;
            _logger.LogDebug($"Get the merchant identity certificate by thumbprint[{thumbprint}]");
            return GetMerchantIdentityCertificate(thumbprint, true);
        }

        public X509Certificate2 GetMerchantIdentityCertificate(string thumbprint, bool reload = false)
        {
            var key = thumbprint.ToUpper();
            _certificates.TryGetValue(key, out var certificate);
            if (certificate == null && reload)
            {
                Load();
                _certificates.TryGetValue(key, out certificate);
            }
            if (certificate == null)
            {
                throw new FileNotFoundException($"Can not loacate the merchant identity certificate with thumbprint[{thumbprint}].");
            }
            return certificate;
        }

        private void Load()
        {
            _certificates.Clear();
            _logger.LogDebug("Start to (re)load merchant identity certificate...");

            foreach (var cert in _optionsMonitor.CurrentValue.Certificates)
            {
                var file = File.ReadAllBytes(cert.FileName);
                var certificate = new X509Certificate2(file, cert.Password, X509KeyStorageFlags.MachineKeySet);
                var key = certificate.Thumbprint.ToUpper();
                _certificates.Add(key, certificate);
                _logger.LogDebug($"Load the certificate with thumbprint[{key}] from '{cert.FileName}'");
            }
            _logger.LogDebug($"{_certificates.Count} count(s) of merchant identity certificate loaded.");
        }
    }
}

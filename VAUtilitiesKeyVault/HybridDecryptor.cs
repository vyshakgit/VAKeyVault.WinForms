using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using System;
using System.Configuration;
using System.Security.Cryptography;
using VAUtilitiesKeyVault.Enum;

namespace VAUtilitiesKeyVault
{
    public static class HybridDecryptor
    {
        public static byte[] Decrypt(
        byte[] encryptedPayload,      
        string keyVaultUrl,
        string primaryKeyName = "va-encryption-key-primary",
        string secondaryKeyName = "va-encryption-key-secondary")
        {
            var parts = CryptoEnvelope.Unpack(encryptedPayload);
            KeySlot slot = KeyPolicy.ActiveSlot;

            System.Diagnostics.Trace.TraceInformation(
$"VAUtilities ActiveSlot resolved to: {slot}");

            string keyName =
                slot == KeySlot.Primary ? primaryKeyName : secondaryKeyName;

            // Load config
            string tenantId = ConfigurationManager.AppSettings["VA_KV_TenantId"];
            string clientId = ConfigurationManager.AppSettings["VA_KV_ClientId"];
            string thumbprint = ConfigurationManager.AppSettings["VA_KV_CertThumbprint"];

            var cert = CertificateLoader.LoadFromStore(thumbprint);

            var credential = new ClientCertificateCredential(
                tenantId,
                clientId,
                cert);

            var cryptoClient = new CryptographyClient(
                new Uri($"{keyVaultUrl}/keys/{keyName}"),
                credential);

            // 1. Decrypt AES key in Azure
            //var unwrapResult = cryptoClient.Decrypt(
            //    EncryptionAlgorithm.RsaOaep,
            //    parts.EncryptedKey);

            var unwrapResult = cryptoClient.Decrypt(
         EncryptionAlgorithm.RsaOaep256,
         parts.EncryptedKey);

            byte[] aesKey = unwrapResult.Plaintext;

            // 2. Decrypt data locally
            using (var aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = parts.IV;

                using (var decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(
                        parts.CipherText, 0, parts.CipherText.Length);
                }
            }
        }
    }
}

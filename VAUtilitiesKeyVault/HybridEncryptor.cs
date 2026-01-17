using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VAUtilitiesKeyVault.Enum;

namespace VAUtilitiesKeyVault
{
    public static class HybridEncryptor
    {
        public static byte[] Encrypt(byte[] data)
        {
            KeySlot slot = KeyPolicy.ActiveSlot;

            System.Diagnostics.Trace.TraceInformation(
    $"VAUtilities ActiveSlot resolved to: {slot}");

            // 1. Load public key
            string pem = EmbeddedPemLoader.LoadPem(slot);
            RSAParameters publicKey = PemPublicKeyLoader.LoadPublicKey(pem);

            byte[] encryptedKey;
            byte[] iv;
            byte[] cipherText;

            // 2. Generate AES key
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                aes.GenerateIV();

                iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor())
                {
                    cipherText = encryptor.TransformFinalBlock(data, 0, data.Length);
                }

                // 3. Encrypt AES key with RSA public key
                //using (var rsa = RSA.Create())
                //{
                //    rsa.ImportParameters(publicKey);
                //    encryptedKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA1);
                //}

                using (var rsa = new RSACng())
                {
                    rsa.ImportParameters(publicKey);
                    encryptedKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.OaepSHA256);
                }
            }

            // 4. Combine output (simple binary envelope)
            return CryptoEnvelope.Pack(encryptedKey, iv, cipherText);
        }
    }
}

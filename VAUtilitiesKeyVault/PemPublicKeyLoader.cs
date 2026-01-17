using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.IO;
using System.Security.Cryptography;

namespace VAUtilitiesKeyVault
{
    public static class PemPublicKeyLoader
    {
        public static RSAParameters LoadPublicKey(string pem)
        {
            using (var reader = new StringReader(pem))
            {
                var pemReader = new PemReader(reader);
                var keyObject = pemReader.ReadObject();

                if (keyObject is RsaKeyParameters)
                {
                    var rsaKey = (RsaKeyParameters)keyObject;
                    return DotNetUtilities.ToRSAParameters(rsaKey);
                }
            }

            throw new CryptographicException("Invalid RSA public key PEM");
        }
    }
}

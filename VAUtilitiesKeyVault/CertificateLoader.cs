using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VAUtilitiesKeyVault
{
    internal static class CertificateLoader
    {
        public static X509Certificate2 LoadFromStore(string thumbprint)
        {
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);

                var certs = store.Certificates
                    .Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certs.Count == 0)
                    throw new CryptographicException("Certificate not found: " + thumbprint);

                return certs[0];
            }
        }
    }
}

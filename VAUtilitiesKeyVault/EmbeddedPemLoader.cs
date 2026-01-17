using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VAUtilitiesKeyVault.Enum;

namespace VAUtilitiesKeyVault
{
    internal static class EmbeddedPemLoader
    {
        public static string LoadPem(KeySlot slot)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName =
                slot == KeySlot.Primary
                ? "VAUtilitiesKeyVault.PublicKeys.va-primary-public.pem"
                : "VAUtilitiesKeyVault.PublicKeys.va-secondary-public.pem";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new FileNotFoundException("Embedded PEM not found: " + resourceName);

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

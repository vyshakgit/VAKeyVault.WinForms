using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VAUtilitiesKeyVault.Enum;

namespace VAUtilitiesKeyVault
{
    internal static class KeyPolicy
    {
        /// <summary>
        /// Single source of truth for active Key Vault key slot.
        /// Defaults to Primary if config is missing or invalid.
        /// </summary>
        public static KeySlot ActiveSlot
        {
            get
            {
                string value = ConfigurationManager.AppSettings["VA_KV_ActiveSlot"];

                if (string.IsNullOrWhiteSpace(value))
                    return KeySlot.Primary;

                return value.Equals("Secondary", StringComparison.OrdinalIgnoreCase)
                    ? KeySlot.Secondary
                    : KeySlot.Primary;
            }
        }
    }
}

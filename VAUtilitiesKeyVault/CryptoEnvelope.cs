using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAUtilitiesKeyVault
{
    internal static class CryptoEnvelope
    {
        public static byte[] Pack(byte[] encryptedKey, byte[] iv, byte[] data)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(encryptedKey.Length);
                bw.Write(encryptedKey);
                bw.Write(iv.Length);
                bw.Write(iv);
                bw.Write(data.Length);
                bw.Write(data);
                return ms.ToArray();
            }
        }

        public static (byte[] EncryptedKey, byte[] IV, byte[] CipherText) Unpack(byte[] payload)
        {
            using (var ms = new MemoryStream(payload))
            using (var br = new BinaryReader(ms))
            {
                var key = br.ReadBytes(br.ReadInt32());
                var iv = br.ReadBytes(br.ReadInt32());
                var data = br.ReadBytes(br.ReadInt32());
                return (key, iv, data);
            }
        }
    }
}

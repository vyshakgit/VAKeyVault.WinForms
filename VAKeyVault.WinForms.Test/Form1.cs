using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VAUtilitiesKeyVault;
using VAUtilitiesKeyVault.Enum;
using System.Configuration;

namespace VAKeyVault.WinForms.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string vaultUrl = ConfigurationManager.AppSettings["VA_KV_VaultUrl"];
                // 1. Input
                string plainText = txtPlain.Text;
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);

                // 2. Encrypt (LOCAL, using embedded public key)
                byte[] encrypted = HybridEncryptor.Encrypt(
                    inputBytes);

                txtEncrypted.Text = Convert.ToBase64String(encrypted);

                // 3. Decrypt (KEY VAULT + LOCAL AES)
                byte[] decrypted = HybridDecryptor.Decrypt(
                    encrypted,
                    vaultUrl);

                string decryptedText = Encoding.UTF8.GetString(decrypted);
                txtDecrypted.Text = decryptedText;

                MessageBox.Show("SUCCESS: Encryption + Decryption worked");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}

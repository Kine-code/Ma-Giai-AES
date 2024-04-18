using Rijndael256;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;


namespace algorithmAES
{
    public partial class Form1 : Form
    {
        private byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private int BlockSize = 128;
        public Form1()
        {
            InitializeComponent();
        }
        private void Browse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;
            textBoxFilename.Text = openFileDialog1.FileName;

            FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            richTextBox1.Text = sr.ReadToEnd();

            sr.Close();
            fs.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;

            FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(richTextBox1.Text);
            sw.Write(txt_DecryptText.Text);

            sw.Close();
            fs.Close();
        }

        private void Encrypt_Click(object sender, EventArgs e)
        {
            var inputText = txt_input.Text;
            var key = textBoxPassword.Text;
            string EncryptText = Rijndael256.Rijndael.Encrypt(inputText, key, KeySize.Aes256);
            txt_encryptText.Text = EncryptText;
            //////////////////////////////////////
            if (textBoxPassword.Text == "") return;
            byte[] bytes = Encoding.Unicode.GetBytes(richTextBox1.Text);
            //Encrypt
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.BlockSize = BlockSize;
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(textBoxPassword.Text));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                }

                richTextBox1.Text = Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        private void Decrypt_Click(object sender, EventArgs e)
        {
            var inputText = txt_encryptText.Text;
            string key = textBoxPassword.Text;
            string DecryptText = Rijndael256.Rijndael.Decrypt(inputText, key, KeySize.Aes256);

            txt_DecryptText.Text = DecryptText;
            ///////////////////////////////////////////////////////////////
            if (textBoxPassword.Text == "") return;

            //Decrypt
            byte[] bytes = Convert.FromBase64String(richTextBox1.Text);
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(textBoxPassword.Text));
            crypt.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[bytes.Length];
                    cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    richTextBox1.Text = Encoding.Unicode.GetString(decryptedBytes);
                    //txt_DecryptText.Text = Encoding.Unicode.GetString(decryptedBytes);
                    return;
                }
            }
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxFilename.Text == "") return;
                File.Delete(textBoxFilename.Text);
                MessageBox.Show("File deleted:\n" + textBoxFilename.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to delete file!\n" + ex.Message);
            }
        } 
        private void btnReset_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Clear();
            this.txt_encryptText.Clear();
            this.txt_DecryptText.Clear();
            this.textBoxPassword.Clear();
            this.txt_input.Clear();


            MessageBox.Show("Reset successful!");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

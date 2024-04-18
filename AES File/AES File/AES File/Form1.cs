using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AES_File
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        AES AES = new AES();
        private void btn_encrypt_Click(object sender, EventArgs e)
        {
            string password = "Congkien";
            
            GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);            
            AES.FileEncrypt(txt_input.Text, password);
           
            AES.ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
            gch.Free();
           
            Console.WriteLine("The given password is surely nothing: " + password);
        }

        private void btn_decrypt_Click(object sender, EventArgs e)
        {
            string password = "Congkien";
           
            GCHandle gch = GCHandle.Alloc(password, GCHandleType.Pinned);
           
            AES.FileDecrypt(txt_input.Text + ".aes", txt_input.Text, password);

           
            AES.ZeroMemory(gch.AddrOfPinnedObject(), password.Length * 2);
            gch.Free();                       
            Console.WriteLine("The given password is surely nothing: " + password);
        }

        private void btn_browser_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                txt_input.Text = dlg.FileName;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}

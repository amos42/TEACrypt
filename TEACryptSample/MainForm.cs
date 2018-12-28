using System;
using System.Windows.Forms;

namespace TEACrypt
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtTEAKey.Text = TEA.GenerateTeaKey();
        }

        private void btnGenKey_Click(object sender, EventArgs e)
        {
            txtTEAKey.Text = TEA.GenerateTeaKey();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            txtEncryptedText.Text = TEA.Encrypt(txtPlainText.Text, txtTEAKey.Text);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            txtPlainText.Text = TEA.Decrypt(txtEncryptedText.Text, txtTEAKey.Text);
        }
    }
}

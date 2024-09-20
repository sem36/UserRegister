using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections;

namespace UserRegister
{
    public partial class Form1 : Form
    {
        List<RegClass> RegClassList = new List<RegClass>();

        string key = "9z$C&F)H@McQfTjW";
        string iv = "H@McQfTjWmZq4t7w";
        public void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }


        public void Cheak(List<RegClass> classRegister, string name)
        {
            if (name == null)
                return;
            for (int a = 0; classRegister.Count > a; a++)
            {
                if (classRegister[a].getName() == name)
                {
                    textBox1.Text = classRegister[a].getName();
                    textBox2.Text = classRegister[a].getNumberGroup();
                    textBox3.Text = classRegister[a].getNumberBook();
                    textBox4.Text = classRegister[a].getAvarScore();
                }
            }
        }
        public static bool IsNullOrWhiteSpace(string value)
        {
            if (value == null)
            {
                return true;
            }
            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public Form1()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            InitializeComponent();
            textBox2.MaxLength = 4;
            textBox4.MaxLength = 3;

            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private string[] ClearList(string line)
        {
            string[] sp = line.Split('\n');

            sp = sp.Except(new string[] { "\r", "", "\0\0\0\0", "\0\0\0", "\0", "\0\0" }).ToArray();
            for (int i = 0; i < sp.Length; i++)
            {
                string[] cash = sp[i].Split(',');
                RegClass std = new RegClass(cash[0], cash[1], cash[2], cash[3]);
                RegClassList.Add(std);
                listBox1.Items.Add(std.getName());
            }
            return sp;
        }

        private void AboutProgrammToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данная программа поможет вам ...", "Info", MessageBoxButtons.OK);
        }

        private void UpLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            string fileText = File.ReadAllText(filename);
            string decryptedText = Decrypt(fileText, key, iv);
            try
            {
                ClearList(decryptedText);
            }
            catch (Exception)
            {
                label5.Text = "ERROR UPLOAD";
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string line = "";

            for (int i = 0; i < RegClassList.Count(); i++)
            {
                line += $"{RegClassList[i].getName()},{RegClassList[i].getNumberGroup()},{RegClassList[i].getNumberBook()},{RegClassList[i].getAvarScore()}\n";
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            string cipherText = Encrypt(line, key, iv);
            File.WriteAllText(filename, cipherText);
        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Delete))
                e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Delete))
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && (e.KeyChar != (char)Keys.Back) && (e.KeyChar != (char)Keys.Delete))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label5.Text = "";
            int index = listBox1.SelectedIndex;
            try
            {
                listBox1.Items.RemoveAt(index);
                RegClassList.RemoveAt(index);
            }
            catch (Exception)
            {
                label5.Text = "ERROR";
            }
            clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label5.Text = "";
            if (!IsNullOrWhiteSpace(textBox1.Text))
            {
                RegClass std = new RegClass(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                int index = listBox1.SelectedIndex;
                if (index != -1)
                {
                    listBox1.Items[index] = std.getName();
                    RegClassList[index] = std;
                }
                clear();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length == 1)
            {
                textBox4.Text += ".";
                textBox4.SelectionStart = textBox4.Text.Length;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsNullOrWhiteSpace(textBox1.Text))
            {
                RegClass std = new RegClass(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                RegClassList.Add(std);
                listBox1.Items.Add(std.getName());
                clear();
            }
            else
            {
                label5.Text = "ERROR";
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            if (listBox1.SelectedItem != null)
                Cheak(RegClassList, listBox1.SelectedItem.ToString());
        }

        private static string Encrypt(string plainText, string key, string iv)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] cipherTextBytes;
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cs.FlushFinalBlock();
                        cipherTextBytes = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
        private static string Decrypt(string cipherText, string key, string iv)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] plainTextBytes;
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        plainTextBytes = new byte[cipherTextBytes.Length];
                        int decryptedByteCount = cs.Read(plainTextBytes, 0, plainTextBytes.Length);
                    }
                }
            }
            return Encoding.UTF8.GetString(plainTextBytes);
        }

        private void среднийБаллToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear();
            button5.Visible = true;
            button4.Visible = true;
            chart1.Visible = true;
            listBox1.Visible = false;
            decimal[] avaragescore = new decimal[RegClassList.Count];
            try
            {
                for (int i = 0; i < RegClassList.Count; i++)
                {
                    avaragescore[i] = Math.Round(decimal.Parse(RegClassList[i].getAvarScore()));
                }

                var dict = new Dictionary<decimal, int>();

                foreach (var value in avaragescore)
                {
                    dict.TryGetValue(value, out int count);
                    dict[value] = count + 1;
                }

                foreach (var person in dict)
                {
                    chart1.Series["Средний Балл"].Points.AddXY(person.Key,person.Value);
                }
            }
            catch (Exception)
            {
                label5.Text = "Error";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Visible = false;
            if (chart1.Visible == false)
            {
                listBox1.Visible = true;
                button4.Visible = false;
                button5.Visible = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        { 
            DateTime dateTime = DateTime.Now;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Сохранить изображение как ...";
            sfd.Filter = "*.png|*.png;|*.jpg|*.jpg";
            sfd.AddExtension = true;
            sfd.FileName = DateTime.Now.ToString("d-M-yyyy HH-mm-ss");
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                chart1.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
    }
}
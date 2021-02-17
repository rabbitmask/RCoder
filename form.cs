using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RCoder
{
    public partial class form : Form
    {
        public form()
        {
            InitializeComponent();
            PanelIsDisplay(0);
        }


        Encoder encoder = new Encoder();
        Decoder decoder = new Decoder();
        RC4 rc4 = new RC4();
        RSA rsa = new RSA();
        AES aes = new AES();
        DES des = new DES();


        private void PanelIsDisplay(int p)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;

            switch (p)
            {
                case 1:  
                    {
                        panel1.Visible = true;
                    }
                    break;
                case 2: 
                    {
                        panel2.Visible = true;
                    }
                    break;
                case 3:
                    {
                        panel3.Visible = true;
                    }
                    break;
                case 4:
                    {
                        panel4.Visible = true;
                    }
                    break;
                default:
                    {
                        panel1.Visible = false;
                        panel2.Visible = false;
                        panel3.Visible = false;
                        panel4.Visible = false;
                    }
                    break;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            PanelIsDisplay(1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            PanelIsDisplay(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PanelIsDisplay(3);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            PanelIsDisplay(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = encoder.Rmd5_16(textBox8.Text);
            textBox2.Text = encoder.Rmd5_32(textBox8.Text);
            textBox3.Text = encoder.Rurl(textBox8.Text);
            textBox4.Text = encoder.Rbase64(textBox8.Text);
            textBox5.Text = encoder.Rhex(textBox8.Text);
            textBox6.Text = encoder.Rsha1(textBox8.Text);
            textBox7.Text = encoder.Rsha256(textBox8.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "BASE64")
            {
                textBox12.Text = decoder.Rbase64(textBox9.Text);
            }
            else if (comboBox1.Text == "URL")
            {
                textBox12.Text = decoder.Rurl(textBox9.Text);
            }
            else if (comboBox1.Text == "HEX")
            {
                textBox12.Text = decoder.Rhex(textBox9.Text);
            }
            else
            {
                try { textBox12.Text = decoder.Rbase64(textBox9.Text);} catch { }
                try { textBox12.Text = decoder.Rurl(textBox9.Text); } catch { }
                try { textBox12.Text = decoder.Rhex(textBox9.Text); } catch { }
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "RC4")
            {
                textBox11.Text = rc4.Encrypt(textBox13.Text,textBox10.Text);
            }
            else if (comboBox2.Text == "AES")
            {
                try
                {
                    textBox11.Text = aes.Encrypt(textBox10.Text, textBox13.Text);
                }
                catch (System.Security.Cryptography.CryptographicException)
                {
                    MessageBox.Show("AES密钥长度要求为16的倍数", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (comboBox2.Text == "DES")
            {
                try
                {
                    textBox11.Text = des.Encrypt(textBox10.Text, textBox13.Text);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("DES密钥长度要求为8: \r\n标准的DES密钥长度为64bit，密钥每个字符占7bit，奇偶校验占1bit", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("算法类型未选择,请指定算法后再进行后续操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "RC4")
            {
                textBox10.Text = rc4.Decrypt(textBox13.Text, textBox11.Text);
            }
            else
            {
                MessageBox.Show("算法类型未选择,请指定算法后再进行后续操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "RSA")
            {
                string[] key = rsa.GetKeys();
                textBox14.Text = key[0];
                textBox17.Text = key[1];
            }
            else
            {
                MessageBox.Show("算法类型未选择,请指定算法后再进行后续操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "RSA")
            {
                textBox19.Text = rsa.Encrypt(textBox15.Text, textBox17.Text);
            }
            else
            {
                MessageBox.Show("算法类型未选择,请指定算法后再进行后续操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "RSA")
            {
                textBox19.Text = rsa.Decrypt(textBox18.Text, textBox14.Text);
            }
            else
            {
                MessageBox.Show("算法类型未选择,请指定算法后再进行后续操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }
    }
}

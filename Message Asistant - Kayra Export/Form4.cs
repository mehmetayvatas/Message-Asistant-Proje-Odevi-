using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Message_Asistant___Kayra_Export
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            this.Top = 0;
            textBox1.TabIndex = 1;
            textBox2.TabIndex = 2;
            richTextBox1.TabIndex = 3;
            button1.TabIndex = 4;
            this.TopMost = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string hedefKlasorAdi = textBox1.Text.ToLower();
            string dosyaAdi = textBox2.Text;
            string dosyaIcerik = richTextBox1.Text;
            bool bul = false;
            string anaKlasorYolu = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar";

            if (Directory.Exists(anaKlasorYolu))
            {
                bul = AramaVeKaydet(anaKlasorYolu, hedefKlasorAdi, dosyaAdi, dosyaIcerik);
            }

            if (bul == false)
            {
                MessageBox.Show("Dükkan Adı Bulunamadı.");
            }

            textBox1.Clear();
            textBox2.Clear();
            richTextBox1.Clear();
            this.Close();
        }

        private bool AramaVeKaydet(string klasorYolu, string hedefKlasorAdi, string dosyaAdi, string dosyaIcerik)
        {
            string[] altKlasorler = Directory.GetDirectories(klasorYolu);

            foreach (string altKlasor in altKlasorler)
            {
                string klasorIsmi = Path.GetFileName(altKlasor).ToLower();

                if (klasorIsmi == hedefKlasorAdi)
                {
                    string dosyaYolu = Path.Combine(altKlasor, dosyaAdi + ".txt");

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(dosyaYolu, false))
                        {
                            writer.WriteLine(dosyaIcerik);
                        }

                        MessageBox.Show("Bilgiler başarıyla dosyaya kaydedildi.");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dosya kaydetme hatası: " + ex.Message);
                        return false;
                    }
                }

                if (AramaVeKaydet(altKlasor, hedefKlasorAdi, dosyaAdi, dosyaIcerik))
                {
                    return true;
                }
            }

            return false;
        }
    }
}

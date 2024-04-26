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

namespace Message_Asistant___Kayra_Export.Properties
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string hedefKlasorAdi = textBox1.Text;
            string dosyaAdi = textBox2.Text;
            string dosyaIcerik = richTextBox1.Text;

            string anaKlasorYolu = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar";

            if (Directory.Exists(anaKlasorYolu))
            {
                AramaVeKaydet(anaKlasorYolu, hedefKlasorAdi, dosyaAdi, dosyaIcerik);
            }
            else
            {
                MessageBox.Show("Klasör yolu bulunamadı.");
            }


        }
        private void AramaVeKaydet(string klasorYolu, string hedefKlasorAdi, string dosyaAdi, string dosyaIcerik)
        {
            string[] altKlasorler = Directory.GetDirectories(klasorYolu);

            foreach (string altKlasor in altKlasorler)
            {
                string klasorIsmi = Path.GetFileName(altKlasor);

                if (klasorIsmi == hedefKlasorAdi)
                {
                    string dosyaYolu = Path.Combine(altKlasor, dosyaAdi + ".txt");
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(dosyaYolu, true))
                        {
                            writer.WriteLine(dosyaIcerik);
                        }
                        MessageBox.Show("Bilgiler başarıyla dosyaya kaydedildi.");
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dosya kaydetme hatası: " + ex.Message);
                    }
                }

                AramaVeKaydet(altKlasor, hedefKlasorAdi, dosyaAdi, dosyaIcerik);
            }
        }
    }
}

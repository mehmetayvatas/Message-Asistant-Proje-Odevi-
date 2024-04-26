using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Message_Asistant___Kayra_Export
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.Top = 0;
            textBox1.TabIndex = 1;
            textBox2.TabIndex = 2;
            richTextBox1.TabIndex = 3;
            button1.TabIndex = 4;
            this.TopMost = true;

            toolStripTextBoxRug.KeyPress += new KeyPressEventHandler(toolStripTextBoxRug_KeyPress);
            toolStripTextBoxCanvas.KeyPress += new KeyPressEventHandler(toolStripTextBoxCanvas_KeyPress);
            toolStripTextBoxPrintedRug.KeyPress += new KeyPressEventHandler(toolStripTextBoxPrintedRug_KeyPress);
            toolStripTextBoxTowel.KeyPress += new KeyPressEventHandler(toolStripTextBoxTowel_KeyPress);
            toolStripTextBoxPillow.KeyPress += new KeyPressEventHandler(toolStripTextBoxPillow_KeyPress);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string hedefKlasorAdi = textBox1.Text.ToLower();
            string dosyaAdi = textBox2.Text;
            string dosyaIcerik = richTextBox1.Text;
            bool bul =false;
            string anaKlasorYolu = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar";

            if (Directory.Exists(anaKlasorYolu))
            {
               bul = AramaVeKaydet(anaKlasorYolu, hedefKlasorAdi, dosyaAdi, dosyaIcerik);
            }
           
            if (bul==false)
            {
                MessageBox.Show("Dükkan Adı Bulunamadı.");
            }

            textBox1.Clear();
            textBox2.Clear();
            richTextBox1.Clear();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            string text = textBox1.Text.ToLower();

            Dictionary<string, string> textToLabel = new Dictionary<string, string>
    {
        { "bgt", "Ikbal" },
        { "hth", "Huseyin" },
        { "ot", "Rasim" },
        { "otc", "Menderes" },
        { "ott", "Menderes" },
        { "pgt", "Erdal" },
        { "sct", "Yeliz" },
        { "st", "Sultan" },
        { "tht", "Mevludiye" },
        { "tta", "Mehmet" },
        { "ttb", "Hasan" },
        { "ttbh", "Muharrem" },
        { "ttbq", "Mehmet Akif" },
        { "ttd", "Ecem" },
        { "ttl", "Ferhat" },
        { "ttp", "Dilek" },
        { "tts", "Bilal" },
        { "tw", "Emel" },
        { "wgt", "Nuray" },
        { "wgts", "Hatice" },
        { "cad", "Bekir" },
        { "caga", "Mihrican" },
        { "ch", "Cengizhan" },
        { "cs", "Aysun" },
        { "rwd", "Gulsun" },
        { "waa", "Cuneyt" },
        { "wac", "Eda" },
        { "waca", "Merve" },
        { "wad", "Fatma" },
        { "wads", "Arife" },
        { "wak", "Mehmet Cihan" },
        { "wam", "Hatice" },
        { "wap", "Nursah" },
        { "wat", "Gaye" },
        { "waw", "Sahin" },
        { "wawd", "Esra" },
        { "wc", "Esme" },
        { "wdh", "Abdullah" },
        { "kpcd", "Ahmet" },
        { "kpd", "Esra" },
        { "kpds", "Nurgul" },
        { "mdp", "Muteber" },
        { "skp", "Oya" },
        { "sp", "Mustafa" },
        { "crd", "Eda" },
        { "hdrs", "Arife" },
        { "pra", "Mihrican" },
        { "prk", "Mehmet Cihan" },
        { "prs", "Cuneyt" },
        { "rads", "Fatma" },
        { "ram", "Hatice" },
        { "rap", "Nursah" },
        { "rat", "Gaye" },
        { "rh", "Cengizhan" },
        { "rrd", "Gulsun" },
        { "wra", "Esme" },
        { "ao", "Elaheh" },
        { "dr", "Esra" },
        { "dvr", "Nurgul" },
        { "hrh", "Huseyin" },
        { "pr", "Hande" },
        { "svr", "Sultan" },
        { "tqr", "Sevilay" },
        { "trbq", "Mehmet Akif" },
        { "trd", "Erdal" },
        { "tre", "Mustafa" },
        { "trl", "Ferhat" },
        { "trn", "Meryem" },
        { "tro", "Muteber" },
        { "trp", "Cilem" },
        { "trs", "Oya" },
        { "trss", "Bilal" },
        { "trw", "Oktay" },
        { "vdr", "Ahmet" },
        { "trm", "Mehmet" },
        { "vrb", "Rasim" }
    };

            if (textToLabel.ContainsKey(text))
            {
                label3.Text = textToLabel[text];
            }
            else
            {
                label3.Text = "";
            }
        }
        private void toolStripTextBoxPrintedRug_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && toolStripTextBoxPrintedRug.Text.Trim() != "")
            {
                string klasorAdi = toolStripTextBoxPrintedRug.Text;
                klasorAdi = klasorAdi.ToUpper();
                string yol = Path.Combine(@"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Printed Rug", klasorAdi);

                try
                {
                    if (Directory.Exists(yol))
                    {
                        MessageBox.Show("Bu Dükkan zaten var", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Directory.CreateDirectory(yol);
                        MessageBox.Show("Dükkan oluşturuldu" , "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dükkan oluşturulurken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                toolStripTextBoxPrintedRug.Clear();
                e.Handled = true;
            }
        }

        private void toolStripTextBoxCanvas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && toolStripTextBoxCanvas.Text.Trim() != "")
            {
                string klasorAdi = toolStripTextBoxCanvas.Text;
                klasorAdi = klasorAdi.ToUpper();
                string yol = Path.Combine(@"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Canvas", klasorAdi);

                try
                {
                    if (Directory.Exists(yol))
                    {
                        MessageBox.Show("Bu Dükkan zaten var" , "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Directory.CreateDirectory(yol);
                        MessageBox.Show("Dükkan oluşturuldu" , "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Klasör oluşturulurken bir hata oluştu" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                toolStripTextBoxCanvas.Clear();
                e.Handled = true;
            }
        }

        private void toolStripTextBoxPillow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && toolStripTextBoxPillow.Text.Trim() != "")
            {
                string klasorAdi = toolStripTextBoxPillow.Text;
                klasorAdi = klasorAdi.ToUpper();
                string yol = Path.Combine(@"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Pillow", klasorAdi);

                try
                {
                    if (Directory.Exists(yol))
                    {
                        MessageBox.Show("Bu Dükkan zaten var" , "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Directory.CreateDirectory(yol);
                        MessageBox.Show("Dükkan oluşturuldu" , "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dükkan oluşturulurken bir hata oluştu" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                toolStripTextBoxPillow.Clear();
                e.Handled = true;
            }
        }

        private void toolStripTextBoxTowel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && toolStripTextBoxTowel.Text.Trim() != "")
            {
                string klasorAdi = toolStripTextBoxTowel.Text;
                klasorAdi = klasorAdi.ToUpper();
                string yol = Path.Combine(@"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Towel", klasorAdi);

                try
                {
                    if (Directory.Exists(yol))
                    {
                        MessageBox.Show("Bu Dükkan zaten var" , "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Directory.CreateDirectory(yol);
                        MessageBox.Show("Dükkan oluşturuldu" , "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dükkan oluşturulurken bir hata oluştu" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                toolStripTextBoxTowel.Clear();
                e.Handled = true;
            }
        }

        private void toolStripTextBoxRug_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && toolStripTextBoxRug.Text.Trim() != "")
            {
                string klasorAdi = toolStripTextBoxRug.Text;
                klasorAdi = klasorAdi.ToUpper();
                string yol = Path.Combine(@"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Rug", klasorAdi);

                try
                {
                    if (Directory.Exists(yol))
                    {
                        MessageBox.Show("Bu Dükkan zaten var" , "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Directory.CreateDirectory(yol);
                        MessageBox.Show("Dükkan oluşturuldu" , "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dükkan oluşturulurken bir hata oluştu" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                toolStripTextBoxRug.Clear();
                e.Handled = true;
            }
        }
    }
}

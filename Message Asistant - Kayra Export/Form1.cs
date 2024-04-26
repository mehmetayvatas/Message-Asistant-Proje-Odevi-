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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Message_Asistant___Kayra_Export

{
    public partial class Form1 : Form
    {
        private string selectedDirectory = "";
        private List<string> orijinalOgeler = new List<string>();
        private readonly Timer directoryCheckTimer;

        private Point originalLocation;
        private bool isFormShifted = false;


        public Form1()
        {
            InitializeComponent();
            textBox2.KeyDown += TextBox2_KeyDown;
            textBox1.TabIndex = 2;
            textBox2.TabIndex = 1;
            listBox1.TabIndex = 3;
            btnMsjEkle.TabIndex = 4;
            this.Top = 0;
            this.AutoScroll = true;

            listBox2.DrawMode = DrawMode.OwnerDrawFixed;
            listBox2.DrawItem += listBox2_DrawItem;


            this.StartPosition = FormStartPosition.Manual;
            this.AutoSize = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Width = (int)(Screen.PrimaryScreen.Bounds.Width * 0.15);
            this.Height = (int)(Screen.PrimaryScreen.Bounds.Height * 0.93
            );

            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            int formLeft = workingArea.Right - this.Width;
            int formTop = workingArea.Bottom - this.Height;
            this.Location = new Point(formLeft, formTop);


            directoryCheckTimer = new Timer
            {
                Interval = 60000
            };
            directoryCheckTimer.Tick += DirectoryCheckTimer_Tick;
            directoryCheckTimer.Start();

            listBox2.MouseUp += new MouseEventHandler(listBox2_MouseUp);
            listBox1.MouseUp += new MouseEventHandler(listBox1_MouseUp);
        }



        private void DirectoryCheckTimer_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedDirectory))
            {
                RefreshListBox(selectedDirectory);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;

            this.Size = new System.Drawing.Size(
                (int)(Screen.PrimaryScreen.Bounds.Width * 0.15),
                (int)(Screen.PrimaryScreen.Bounds.Height * 0.93)
            );


            radioButton1.Checked = true;
            radioButton6.Checked = false;
            radioButton7.Checked = false;
            radioButton8.Checked = false;
        }

        private void KlasorleriVeDosyalariGetir(string anaDizin, string arananKlasor)
        {
            try
            {
                string[] klasorler = Directory.GetDirectories(anaDizin, arananKlasor, SearchOption.AllDirectories);

                if (klasorler.Length > 0)
                {
                    string hedefKlasor = klasorler[0];
                    string[] dosyaYollari = Directory.GetFiles(hedefKlasor, "*.txt");
                    listBox1.Items.Clear();

                    foreach (string dosyaYolu in dosyaYollari)
                    {
                        string dosyaAdi = Path.GetFileNameWithoutExtension(dosyaYolu);
                        listBox1.Items.Add(dosyaAdi);
                        dosyaBilgileri[dosyaAdi] = dosyaYolu;
                    }

                    label1.Text = Path.GetFileName(hedefKlasor);
                }
                else
                {
                    MessageBox.Show("Belirtilen klasör bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }





        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string anaDizin = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\";
                string arananKlasor = textBox2.Text;
                KlasorleriVeDosyalariGetir(anaDizin, arananKlasor);
                orijinalOgeler = listBox1.Items.Cast<string>().ToList();

                textBox2.Clear();
                e.Handled = true;
            }
        }


        private readonly Dictionary<string, string> dosyaBilgileri = new Dictionary<string, string>();


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string arananKelime = textBox1.Text.ToLower();

            if (orijinalOgeler != null)
            {
                listBox1.Items.Clear();
                foreach (string item in orijinalOgeler)
                {
                    if (item.ToLower().Contains(arananKelime))
                    {
                        listBox1.Items.Add(item);
                    }
                }
            }
        }
        private string seciliDosyaAdi = string.Empty;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (index >= 0 && index < listBox1.Items.Count)
            {
                string yeniSecilenDosyaAdi = listBox1.Items[index].ToString();

                if (yeniSecilenDosyaAdi != seciliDosyaAdi)
                {
                    seciliDosyaAdi = yeniSecilenDosyaAdi;

                    if (dosyaBilgileri.TryGetValue(seciliDosyaAdi, out string dosyaYolu))
                    {
                        try
                        {
                            string dosyaIcerik = File.ReadAllText(dosyaYolu);
                            richTextBox1.Text = dosyaIcerik;
                        }
                        catch (Exception ex)
                        {
                            richTextBox1.Text = "Dosya okuma hatası: " + ex.Message;
                        }
                    }
                    else
                    {
                        richTextBox1.Text = "Seçilen dosya bilgileri bulunamadı.";
                    }
                }
            }
            else
            {
                seciliDosyaAdi = string.Empty;
                richTextBox1.Text = string.Empty;
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                try
                {
                    Clipboard.SetText(richTextBox1.Text);
                }
                catch (ArgumentNullException)
                {

                }
            }
        }


        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (listBox2.SelectedIndex != -1)
            {
                string selectedFile = listBox2.SelectedItem.ToString();
                string fileContent = File.ReadAllText(Path.Combine(selectedDirectory, selectedFile + ".txt"));
                richTextBox2.Text = fileContent;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string fileName = textBox3.Text.Trim();

            if (!string.IsNullOrEmpty(fileName))
            {
                CreateTextFile(fileName);
                RefreshListBox(selectedDirectory);
            }
            else
            {
                MessageBox.Show("Dosya adı boş olamaz.");
            }
            textBox3.Clear();
        }


        private string GetUniqueFileName(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string fileExtension = Path.GetExtension(filePath);
            int count = 1;

            string newFileName = filePath;

            while (File.Exists(newFileName))
            {
                newFileName = Path.Combine(directory, fileNameWithoutExtension + "_" + count + fileExtension);
                count++;
            }

            return newFileName;
        }



        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private Form3 form3 = null;
        private void btnMsjEkle_Click(object sender, EventArgs e)
        {
            if (form3 == null || form3.IsDisposed)
            {
                form3 = new Form3
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    TopMost = true
                };

            }

            if (!form3.Visible)
            {
                form3.Show();
            }
        }

        private void CreateTextFile(string fileName)
        {
            if (!string.IsNullOrEmpty(selectedDirectory) && !string.IsNullOrEmpty(fileName))
            {
                string currentDate = DateTime.Now.ToString("dd-MM-yyyy");
                string formattedFileName = fileName.Replace(".", "-").Replace("/", "-");

                try
                {
                    string filePath = Path.Combine(selectedDirectory, $"{formattedFileName} - {currentDate}.txt");

                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Close();
                        RefreshListBox(selectedDirectory);
                    }
                    else
                    {
                        MessageBox.Show("Aynı ada sahip bir dosya zaten var.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya oluşturma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            selectedDirectory = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Mesaj Takibi\Devam Eden";
            RefreshListBox(selectedDirectory);
            richTextBox2.Clear();
        }

        private void RefreshListBox(string directory)
        {
            if (!string.IsNullOrEmpty(directory))
            {
                string[] files = Directory.GetFiles(directory, "*.txt");
                listBox2.Items.Clear();

                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    DateTime addedDate = File.GetCreationTime(filePath);
                    Color backColor = IsOneDayPassed(addedDate) ? Color.Red : Color.White;

                    ListBoxItem item = new ListBoxItem(fileName, backColor);
                    listBox2.Items.Add(item);
                }

                UpdateRadioButtonText(directory);
            }
        }

        private class ListBoxItem
        {
            public string Text { get; set; }
            public Color BackColor { get; set; }

            public ListBoxItem(string text, Color backColor)
            {
                Text = text;
                BackColor = backColor;
            }



            public override string ToString()
            {
                return Text;
            }
        }

        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ListBoxItem item = (ListBoxItem)listBox2.Items[e.Index];

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.RoyalBlue, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(item.BackColor), e.Bounds);
            }

            e.Graphics.DrawString(item.Text, e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
        }

        private void UpdateRadioButtonText(string directory)
        {
            int itemCount = listBox2.Items.Count;

            if (directory.Contains("Devam Eden"))
            {
                radioButton1.Text = $"Genel ({itemCount})";
            }
            else if (directory.Contains("Oktay"))
            {
                radioButton6.Text = $"Oktay Bey ({itemCount})";
            }
            else if (directory.Contains("Bilal"))
            {
                radioButton7.Text = $"Bilal Bey ({itemCount})";
            }
            else if (directory.Contains("Mehmet"))
            {
                radioButton8.Text = $"Mehmet Bey ({itemCount})";
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            selectedDirectory = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Mesaj Takibi\Oktay";
            RefreshListBox(selectedDirectory);
            richTextBox2.Clear();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            selectedDirectory = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Mesaj Takibi\Bilal";
            RefreshListBox(selectedDirectory);
            richTextBox2.Clear();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            selectedDirectory = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Mesaj Takibi\Mehmet";
            RefreshListBox(selectedDirectory);
            richTextBox2.Clear();
        }



        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            AutoSave();
        }

        private void AutoSave()
        {
            try
            {
                if (!string.IsNullOrEmpty(selectedDirectory) && listBox2.SelectedItem != null)
                {
                    string textToSave = richTextBox2.Text;
                    string selectedItem = listBox2.SelectedItem.ToString();
                    string fileName = $"{selectedItem}.txt";
                    string filePath = Path.Combine(selectedDirectory, fileName);

                    File.WriteAllText(filePath, textToSave);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Otomatik kaydetme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isFormShifted)
            {

                int red = 128;
                int green = 128;
                int blue = 255;

                Color renk2 = Color.FromArgb(red, green, blue);


                AnimateForm(originalLocation);
                button2.Text = ">";
                button2.BackColor = renk2;
                button2.ForeColor = Color.Red;
            }
            else
            {
                int red = 255;
                int green = 128;
                int blue = 0;

                Color renk = Color.FromArgb(red, green, blue);

                originalLocation = this.Location;

                AnimateForm(new Point(originalLocation.X + 260, originalLocation.Y));
                button2.Text = "<";
                button2.BackColor = renk;
                button2.ForeColor = Color.DarkGreen;

            }

            isFormShifted = !isFormShifted;

        }


        private readonly int animationStep = 10;
        private Timer animationTimer;
        private void AnimateForm(Point targetLocation)
        {
            animationTimer = new Timer
            {
                Interval = 5
            };
            animationTimer.Tick += (s, args) => AnimateStep(targetLocation);
            animationTimer.Start();
        }

        private void AnimateStep(Point targetLocation)
        {
            if (this.Location != targetLocation)
            {
                int stepX = Math.Sign(targetLocation.X - this.Location.X) * animationStep;
                int stepY = Math.Sign(targetLocation.Y - this.Location.Y) * animationStep;

                this.Location = new Point(this.Location.X + stepX, this.Location.Y + stepY);
            }
            else
            {
                animationTimer.Stop();
                animationTimer.Dispose();
            }
        }






        private void listBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listBox2.IndexFromPoint(e.Location);

                if (index != ListBox.NoMatches)
                {
                    if (listBox2.SelectedIndices.Count <= 1 && !Control.ModifierKeys.HasFlag(Keys.Control))
                    {
                        listBox2.ClearSelected();
                    }

                    listBox2.SelectedIndex = index;

                    ContextMenuStrip contextMenu = new ContextMenuStrip();

                    ToolStripMenuItem tamamlandiMenuItem = new ToolStripMenuItem("Tamamlandı");
                    tamamlandiMenuItem.Click += new EventHandler(TasiMenuItem_Click);
                    contextMenu.Items.Add(tamamlandiMenuItem);

                    ToolStripMenuItem GenelMenuItem = new ToolStripMenuItem("Genel");
                    GenelMenuItem.Click += new EventHandler(GenelMenuItem_Click);
                    contextMenu.Items.Add(GenelMenuItem);

                    ToolStripMenuItem oktayBeyMenuItem = new ToolStripMenuItem("Oktay Bey");
                    oktayBeyMenuItem.Click += new EventHandler(OktayBeyMenuItem_Click);
                    contextMenu.Items.Add(oktayBeyMenuItem);

                    ToolStripMenuItem bilalBeyMenuItem = new ToolStripMenuItem("Bilal Bey");
                    bilalBeyMenuItem.Click += new EventHandler(BilalBeyMenuItem_Click);
                    contextMenu.Items.Add(bilalBeyMenuItem);

                    ToolStripMenuItem mehmetBeyMenuItem = new ToolStripMenuItem("Mehmet Bey");
                    mehmetBeyMenuItem.Click += new EventHandler(MehmetBeyMenuItem_Click);
                    contextMenu.Items.Add(mehmetBeyMenuItem);

                    contextMenu.Show(listBox2, e.Location);
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (!Control.ModifierKeys.HasFlag(Keys.Control))
                {
                    listBox2.ClearSelected();
                }

                int index = listBox2.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    listBox2.SelectedIndex = index;
                }
            }
        }

        private void TasiMenuItem_Click(object sender, EventArgs e)
        {
            TaşıTamamlananKlasörüne();
        }

        private void GenelMenuItem_Click(object sender, EventArgs e)
        {
            TaşıGenel();
        }

        private void OktayBeyMenuItem_Click(object sender, EventArgs e)
        {
            TaşıOktayBey();
        }

        private void BilalBeyMenuItem_Click(object sender, EventArgs e)
        {
            TaşıBilalBey();
        }

        private void MehmetBeyMenuItem_Click(object sender, EventArgs e)
        {
            TaşıMehmetBey();
        }

        private void TaşıGenel()
        {
            if (listBox2.SelectedItems.Count > 0)
            {
                string targetDirectory = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Mesaj Takibi\Devam Eden";

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                try
                {
                    ListBoxItem[] selectedItems = new ListBoxItem[listBox2.SelectedItems.Count];
                    for (int i = 0; i < listBox2.SelectedItems.Count; i++)
                    {
                        selectedItems[i] = (ListBoxItem)listBox2.SelectedItems[i];
                    }

                    foreach (var selectedItem in selectedItems)
                    {
                        string selectedFile = selectedItem.Text;
                        string sourceFilePath = Path.Combine(selectedDirectory, $"{selectedFile}.txt");
                        string targetFilePath = Path.Combine(targetDirectory, $"{selectedFile}.txt");

                        targetFilePath = GetUniqueFileName(targetFilePath);

                        File.Move(sourceFilePath, targetFilePath);
                    }

                    RefreshListBox(selectedDirectory);
                    richTextBox2.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Taşıma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen taşımak istediğiniz öğeleri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TaşıTamamlananKlasörüne()
        {
            if (listBox2.SelectedItems.Count > 0)
            {
                string targetDirectory = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Mesaj Takibi\Tamamlanan";

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                try
                {
                    ListBoxItem[] selectedItems = new ListBoxItem[listBox2.SelectedItems.Count];
                    for (int i = 0; i < listBox2.SelectedItems.Count; i++)
                    {
                        selectedItems[i] = (ListBoxItem)listBox2.SelectedItems[i];
                    }

                    foreach (var selectedItem in selectedItems)
                    {
                        string selectedFile = selectedItem.Text;
                        string sourceFilePath = Path.Combine(selectedDirectory, $"{selectedFile}.txt");
                        string targetFilePath = Path.Combine(targetDirectory, $"{selectedFile}.txt");

                        targetFilePath = GetUniqueFileName(targetFilePath);

                        File.Move(sourceFilePath, targetFilePath);
                    }

                    RefreshListBox(selectedDirectory);
                    richTextBox2.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Taşıma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen taşımak istediğiniz öğeleri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TaşıOktayBey()
        {
            if (listBox2.SelectedItems.Count > 0)
            {
                string targetDirectory = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Mesaj Takibi\Oktay";

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                try
                {
                    ListBoxItem[] selectedItems = new ListBoxItem[listBox2.SelectedItems.Count];
                    for (int i = 0; i < listBox2.SelectedItems.Count; i++)
                    {
                        selectedItems[i] = (ListBoxItem)listBox2.SelectedItems[i];
                    }

                    foreach (var selectedItem in selectedItems)
                    {
                        string selectedFile = selectedItem.Text;
                        string sourceFilePath = Path.Combine(selectedDirectory, $"{selectedFile}.txt");
                        string targetFilePath = Path.Combine(targetDirectory, $"{selectedFile}.txt");

                        targetFilePath = GetUniqueFileName(targetFilePath);

                        File.Move(sourceFilePath, targetFilePath);
                    }

                    RefreshListBox(selectedDirectory);
                    richTextBox2.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Taşıma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen taşımak istediğiniz öğeleri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void TaşıBilalBey()
        {
            if (listBox2.SelectedItems.Count > 0)
            {
                string targetDirectory = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Mesaj Takibi\Bilal";

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                try
                {
                    ListBoxItem[] selectedItems = new ListBoxItem[listBox2.SelectedItems.Count];
                    for (int i = 0; i < listBox2.SelectedItems.Count; i++)
                    {
                        selectedItems[i] = (ListBoxItem)listBox2.SelectedItems[i];
                    }

                    foreach (var selectedItem in selectedItems)
                    {
                        string selectedFile = selectedItem.Text;
                        string sourceFilePath = Path.Combine(selectedDirectory, $"{selectedFile}.txt");
                        string targetFilePath = Path.Combine(targetDirectory, $"{selectedFile}.txt");

                        targetFilePath = GetUniqueFileName(targetFilePath);

                        File.Move(sourceFilePath, targetFilePath);
                    }

                    RefreshListBox(selectedDirectory);
                    richTextBox2.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Taşıma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen taşımak istediğiniz öğeleri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void TaşıMehmetBey()
        {
            if (listBox2.SelectedItems.Count > 0)
            {
                string targetDirectory = @"\\10.1.10.6\towel\Genel\Mehmet\Mesajlar\Mesaj Takibi\Mehmet";

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                try
                {
                    ListBoxItem[] selectedItems = new ListBoxItem[listBox2.SelectedItems.Count];
                    for (int i = 0; i < listBox2.SelectedItems.Count; i++)
                    {
                        selectedItems[i] = (ListBoxItem)listBox2.SelectedItems[i];
                    }

                    foreach (var selectedItem in selectedItems)
                    {
                        string selectedFile = selectedItem.Text;
                        string sourceFilePath = Path.Combine(selectedDirectory, $"{selectedFile}.txt");
                        string targetFilePath = Path.Combine(targetDirectory, $"{selectedFile}.txt");

                        targetFilePath = GetUniqueFileName(targetFilePath);

                        File.Move(sourceFilePath, targetFilePath);
                    }

                    RefreshListBox(selectedDirectory);
                    richTextBox2.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Taşıma hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen taşımak istediğiniz öğeleri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listBox1.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    listBox1.SelectedIndex = index;
                    ContextMenu contextMenu = new ContextMenu();
                    contextMenu.MenuItems.Add(new MenuItem("Düzenle", duzenleMenuItem_Click));
                    contextMenu.Show(listBox1, e.Location);
                }
            }
        }

        private bool IsOneDayPassed(DateTime addedDate)
        {
            DateTime now = DateTime.Now;
            DateTime yesterday = now.AddDays(-1);

            return addedDate.Date <= yesterday.Date;
        }

        private void duzenleMenuItem_Click(object sender, EventArgs e)
        {
            string seciliOge = listBox1.SelectedItem.ToString();
            Form4 form4 = new Form4();
            form4.textBox1.Text = label1.Text;
            form4.textBox2.Text = seciliOge;
            form4.richTextBox1.Text = richTextBox1.Text;
            form4.StartPosition = FormStartPosition.CenterScreen;
            form4.Show();
        }

        private Form2 form2 = null;
        
        private void button3_Click(object sender, EventArgs e)
        {
            if (form2 == null || form2.IsDisposed)
            {
                form2 = new Form2
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    TopMost = true
                };

            }

            if (!form2.Visible)
            {
                form2.Show();
            }
        }
    }
}
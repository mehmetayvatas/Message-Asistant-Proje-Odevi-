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
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Message_Asistant___Kayra_Export
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            LoadUniqueValuesFromExcel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = @"\\10.1.10.6\Towel\Genel\Mehmet\Message Asistant - Kayra Export\Message Asistant - Kayra Export\Hatalar.xlsx";

            if (string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(richTextBox1.Text) ||
                string.IsNullOrWhiteSpace(richTextBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            IWorkbook workbook;

            try
            {
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    workbook = stream.Length > 0 ? new XSSFWorkbook(stream) : new XSSFWorkbook();
                }

                ISheet sheet = workbook.GetSheet("Sayfa1") ?? workbook.CreateSheet("Sayfa1");

                int rowIndex = FindEmptyRow(sheet);

                double value4;
                if (!double.TryParse(textBox4.Text, out value4))
                {
                    MessageBox.Show("Lütfen geçerli bir sayısal değer giriniz.");
                    return;
                }

                var row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(DateTime.Now.ToString("dd/MM/yyyy"));
                row.CreateCell(1).SetCellValue(textBox5.Text);
                row.CreateCell(2).SetCellValue(textBox1.Text);
                row.CreateCell(3).SetCellValue(richTextBox1.Text);
                row.CreateCell(4).SetCellValue(richTextBox2.Text);
                row.CreateCell(5).SetCellValue(textBox2.Text);
                row.CreateCell(6).SetCellValue(textBox3.Text);
                row.CreateCell(7).SetCellValue(value4);

                using (var saveStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(saveStream);
                }

                MessageBox.Show("Veriler başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            richTextBox1.Clear();
            richTextBox2.Clear();
        }

        private int FindEmptyRow(ISheet sheet)
        {
            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null || row.Cells.All(d => d.CellType == CellType.Blank))
                {
                    return i;
                }
            }
            return sheet.LastRowNum + 1;
        }

        private void LoadUniqueValuesFromExcel()
        {
            string filePath = @"\\10.1.10.6\Towel\Genel\Mehmet\Message Asistant - Kayra Export\Message Asistant - Kayra Export\Hatalar.xlsx";
            IWorkbook workbook;
            HashSet<string> uniqueValues = new HashSet<string>();

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(stream);
                }

                ISheet sheet = workbook.GetSheetAt(0);

                if (sheet != null)
                {
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        var currentRow = sheet.GetRow(row);
                        if (currentRow != null)
                        {
                            var cell = currentRow.GetCell(5);
                            if (cell != null && !string.IsNullOrWhiteSpace(cell.ToString()))
                            {
                                uniqueValues.Add(cell.ToString());
                            }
                        }
                    }
                }

                comboBox1.Items.Clear();
                foreach (var value in uniqueValues)
                {
                    comboBox1.Items.Add(value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sourcePath = @"\\10.1.10.6\Towel\Genel\Mehmet\Message Asistant - Kayra Export\Message Asistant - Kayra Export\Hatalar.xlsx";
            string tempPath = Path.GetTempFileName() + ".xlsx"; 

            try
            {
                string selectedValue = comboBox1.SelectedItem.ToString(); 

                using (var fs = new FileStream(sourcePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook sourceWorkbook = new XSSFWorkbook(fs); 
                    ISheet sourceSheet = sourceWorkbook.GetSheetAt(0); 

                    IWorkbook newWorkbook = new XSSFWorkbook(); 
                    ISheet newSheet = newWorkbook.CreateSheet("FilteredData"); 

                    IRow headerRow = sourceSheet.GetRow(0);
                    IRow newHeaderRow = newSheet.CreateRow(0);
                    CopyRow(headerRow, newHeaderRow);


                    int newRowNum = 1;
                    for (int i = 1; i <= sourceSheet.LastRowNum; i++)
                    {
                        IRow sourceRow = sourceSheet.GetRow(i);
                        if (sourceRow != null)
                        {
                            ICell cell = sourceRow.GetCell(5); 
                            if (cell != null && cell.ToString() == selectedValue)
                            {
                                IRow newRow = newSheet.CreateRow(newRowNum++);
                                CopyRow(sourceRow, newRow); 
                            }
                        }
                    }


                    for (int col = 0; col < newHeaderRow.LastCellNum; col++)
                    {
                        newSheet.AutoSizeColumn(col);
                    }

                    using (var dst = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                    {
                        newWorkbook.Write(dst); 
                    }
                }

                System.Diagnostics.Process.Start("explorer.exe", tempPath); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
        }

        private void CopyRow(IRow sourceRow, IRow newRow)
        {
            newRow.Height = sourceRow.Height;
            for (int i = 0; i < sourceRow.LastCellNum; i++)
            {
                ICell sourceCell = sourceRow.GetCell(i);
                ICell newCell = newRow.CreateCell(i, sourceCell.CellType);
                switch (sourceCell.CellType)
                {
                    case CellType.Numeric:
                        newCell.SetCellValue(sourceCell.NumericCellValue);
                        break;
                    case CellType.String:
                        newCell.SetCellValue(sourceCell.StringCellValue);
                        break;
                    case CellType.Boolean:
                        newCell.SetCellValue(sourceCell.BooleanCellValue);
                        break;
                    case CellType.Formula:
                        newCell.SetCellFormula(sourceCell.CellFormula);
                        break;
                    case CellType.Blank:
                        newCell.SetCellType(CellType.Blank);
                        break;
                    default:
                        if (sourceCell != null)
                            newCell.SetCellValue(sourceCell.ToString());
                        break;
                }
            }
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Drawing.Imaging;

namespace IT__Inventory
{
    public partial class Scanin : Form
    {
        private PrintDocument printDoc = new PrintDocument();

        public Scanin()
        {
            InitializeComponent();
        }

        private void Scanin_Load(object sender, EventArgs e)
        {
            // Set default location
            radioButtonUSA.Checked = true;

            // Event handlers for location radio buttons
            radioButtonUSA.CheckedChanged += new EventHandler(LocationRadioButton_CheckedChanged);
            radioButtonCHINA.CheckedChanged += new EventHandler(LocationRadioButton_CheckedChanged);
        }

        private void INVID_TextChanged(object sender, EventArgs e)
        {
            try
            {
        private string connectionString = "Data Source=xxxxxxxxx;Initial Catalog=xxxxxxxxx;User ID=xxxxxxxxx;Password=xxxxxxxxx";
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    string query = "SELECT TOP 1 INVID FROM IT_INVENTORY ORDER BY INVID DESC";
                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        int lastUsedID = Convert.ToInt32(command.ExecuteScalar());
                        INVID.Text = (lastUsedID + 1).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Please try other numbers. " + ex.Message);
            }
        }

        private void Btn_Generate_Click(object sender, EventArgs e)
        {
            GenerateBarcode();
        }

        private void LocationRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUSA.Checked || radioButtonCHINA.Checked)
            {
                GenerateBarcode();
            }
        }

        private void GenerateBarcode()
        {
            try
            {
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Please select a device type.");
                    return;
                }

                string deviceTypeCode = GetDeviceTypeCode(comboBox1.SelectedItem.ToString());
                string dateCode = DateTime.Now.ToString("yyyyMMdd");
                string locationCode = radioButtonUSA.Checked ? "US" : "CN";
                int nextSequenceNumber = GetNextSequenceNumber(deviceTypeCode, dateCode, locationCode);

                string barcodeText = $"{deviceTypeCode}{dateCode}-{locationCode}{nextSequenceNumber:00}";
                txt_Barcode.Text = barcodeText;

                BarcodeWriter barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Height = 90,
                        Width = 400,
                        Margin = 0
                    }
                };

                Bitmap barcodeImage = barcodeWriter.Write(barcodeText);

                Bitmap barcodeImageWithMargin = new Bitmap(barcodeImage.Width, barcodeImage.Height + 10);
                using (Graphics g = Graphics.FromImage(barcodeImageWithMargin))
                {
                    g.Clear(Color.White);
                    g.DrawImage(barcodeImage, new Point(0, 10));
                }

                string fileName = $"{barcodeText}.png";
                SaveBarcodeImage(barcodeImageWithMargin, fileName);

                Pic_barcode.SizeMode = PictureBoxSizeMode.StretchImage;
                Pic_barcode.Image = barcodeImageWithMargin;

                BARCODENO.Text = barcodeText;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating and displaying barcode image: " + ex.Message);
            } 
        }

        private int GetNextSequenceNumber(string deviceTypeCode, string dateCode, string locationCode)
        {
        private string connectionString = "Data Source=xxxxxxxxx;Initial Catalog=xxxxxxxxx;User ID=xxxxxxxxx;Password=xxxxxxxxx";
        string query = "SELECT MAX(SUBSTRING(BARCODENO, LEN(BARCODENO) - 1, 2)) " +
                           "FROM IT_INVENTORY " +
                           "WHERE BARCODENO LIKE @BarcodePattern";

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@BarcodePattern", $"{deviceTypeCode}{dateCode}-{locationCode}%");

                    try
                    {
                        connect.Open();
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            return Convert.ToInt32(result) + 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error fetching sequence number: " + ex.Message);
                    }
                }
            }

            return 1;
        }

        private string GetDeviceTypeCode(string deviceType)
        {
            switch (deviceType.ToUpper())
            {
                case "LAPTOP": return "LT";
                case "DESKTOP": return "DT";
                case "TABLET": return "TL";
                case "SURFACEPRO": return "SP";
                case "DESKPHONE": return "DP";
                case "SWITCH": return "SW";
                case "WIRELESS HEADSET": return "HS";
                case "ROUTER": return "RT";
                case "MINI PC": return "TC";
                case "USB ADAPTOR": return "AT";
                case "FIREWALL": return "FW";
                case "MONITOR": return "MT";
                case "NETWORK MONITORING": return "NW";
                case "PRINTER": return "PT";
                case "ALLINONE": return "AO";
                case "NAS DRIVE": return "NAS";
                case "ACCESSORY": return "ACC";
                case "IPCAMERA": return "IPC";
                case "IPAD": return "IPAD";
                case "ACCESS POINT": return "AP";
                case "BRIDGE P2P": return "P2P";
                case "AI HORN": return "AH";
                default: return "XX";
            }
        }

        private void SaveBarcodeImage(Bitmap image, string fileName)
        {
            string directoryPath = @"C:\Users\palc\Desktop\IT_INVENTORY BARCODE";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, fileName);
            image.Save(filePath, ImageFormat.Png);
        }

        private void Btn_Clear_Click(object sender, EventArgs e)
        {
            Pic_barcode.Image = null;
            txt_Barcode.Clear();
            txt_Barcode.Focus();
        }

        private void Btn_Print_Click(object sender, EventArgs e)
        {
            printDoc.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
            PrintDialog printDialog = new PrintDialog
            {
                Document = printDoc
            };

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }

            printDoc.PrintPage -= new PrintPageEventHandler(PrintDocument_PrintPage);
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Image barcodeImage = Pic_barcode.Image;
            int printAreaWidth = e.MarginBounds.Width;
            int printAreaHeight = e.MarginBounds.Height;
            float aspectRatio = (float)barcodeImage.Width / barcodeImage.Height;
            int printWidth = printAreaWidth;
            int printHeight = (int)(printWidth / aspectRatio);
            if (printHeight > printAreaHeight)
            {
                printHeight = printAreaHeight;
                printWidth = (int)(printHeight * aspectRatio);
            }
            int printX = (printAreaWidth - printWidth) / 2;
            int printY = (printAreaHeight - printHeight) / 2;
            e.Graphics.DrawImage(barcodeImage, printX, printY, printWidth, printHeight);
        }

        private void STATUS_SCANIN_CheckedChanged(object sender, EventArgs e)
        {
            if (STATUS_SCANIN.Checked)
            {
                STATUS.Text = "SCAN_IN";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedDate = DateTimePicker1.Value.ToString("yyyy-MM-dd");
        private string connectionString = "Data Source=xxxxxxxxx;Initial Catalog=xxxxxxxxx;User ID=xxxxxxxxx;Password=xxxxxxxxx";
        string query = "INSERT INTO IT_INVENTORY (INVID, DEVICETYP, BRAND, MODELNO, SERIALNO, PERSON, DEVICENO, BARCODENO, MAC, PRICES, DATE, STATUS) " +
                           "VALUES (@INVID, @DEVICETYP, @BRAND, @MODELNO, @SERIALNO, @PERSON, @DEVICENO, @BARCODENO, @MAC, @PRICES, @DATE, @STATUS)";

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@INVID", INVID.Text);
                    command.Parameters.AddWithValue("@DEVICETYP", comboBox1.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@BRAND", BRAND.Text);
                    command.Parameters.AddWithValue("@MODELNO", MODELNO.Text);
                    command.Parameters.AddWithValue("@SERIALNO", SERIALNO.Text);
                    command.Parameters.AddWithValue("@PERSON", PERSON.Text);
                    command.Parameters.AddWithValue("@DEVICENO", DEVICENO.Text);
                    command.Parameters.AddWithValue("@BARCODENO", BARCODENO.Text);
                    command.Parameters.AddWithValue("@MAC", MAC.Text);
                    command.Parameters.AddWithValue("@PRICES", PRICES.Text);
                    command.Parameters.AddWithValue("@DATE", selectedDate);
                    command.Parameters.AddWithValue("@STATUS", STATUS.Text);

                    try
                    {
                        connect.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Record added successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Please check the format. " + ex.Message);
                    }
                }
            }
        }

        private void STATUS_SCANIN_CheckedChanged_1(object sender, EventArgs e)
        {
            if (STATUS_SCANIN.Checked)
            {
                STATUS.Text = "SCAN_IN";
            }
        }

        private void Pic_barcode_Click(object sender, EventArgs e)
        {
            // Handle PictureBox click event if needed
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle ListBox selection change event if needed
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle ComboBox selection change event if needed
        }
    }
}

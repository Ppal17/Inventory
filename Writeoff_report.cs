using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace IT__Inventory
{
    public partial class Writeoff_report : Form
    {
        private int rowIndex = 0; // Declare rowIndex variable at the class level
        public Writeoff_report()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=xxxxxxxxx;Initial Catalog=xxxxxxxxx;User ID=xxxxxxxxx;Password=xxxxxxxxx";
            string query = "SELECT INVID, DEVICETYP,BRAND,MODELNO,SERIALNO,BARCODENO,DATE, STATUS FROM IT_INVENTORY WHERE STATUS = 'WRITE_OFF' AND DATE BETWEEN @StartDate AND @EndDate";

            // Create SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        // Set the parameters for the date range
                        command.Parameters.AddWithValue("@StartDate", dateTimePicker1.Value.Date);
                        command.Parameters.AddWithValue("@EndDate", dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1));

                        // Open the connection
                        connection.Open();

                        // Create SqlDataAdapter to fill data in a DataTable
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        // Fill the DataTable with data from the query
                        adapter.Fill(dataTable);

                        // Bind the DataTable to DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No data to print.");
                    return;
                }

                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocument_PrintPage;

                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }

            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridView dataGridView = dataGridView1;

            // Set page settings for landscape A4
            e.PageSettings.Landscape = true;
            e.PageSettings.PaperSize = new PaperSize("A4", 827, 1200);

            // Adjust margins
            int leftMargin = 0; // Adjust the left margin as needed (reduced from 20)
            int topMargin = 10; // Adjust the top margin as needed
            int yPos = e.MarginBounds.Top + topMargin;
            Font cellFont = new Font(dataGridView.DefaultCellStyle.Font, FontStyle.Regular);
            Font headerFont = new Font(dataGridView.DefaultCellStyle.Font, FontStyle.Bold); // Font for column headers
            int columnWidth = (e.MarginBounds.Width - 2 * leftMargin) / dataGridView.ColumnCount;

            // Print column headers
            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                int xPos = e.MarginBounds.Left + leftMargin + (i * columnWidth);
                string headerText = dataGridView.Columns[i].HeaderText;
                e.Graphics.DrawString(headerText, headerFont, Brushes.Black, new RectangleF(xPos, yPos, columnWidth, dataGridView.ColumnHeadersHeight), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }

            // Print rows
            yPos += dataGridView.ColumnHeadersHeight;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (yPos + row.Height > e.MarginBounds.Bottom - topMargin)
                {
                    // Move to the next page if the row doesn't fit on the current page
                    e.HasMorePages = true;
                    return;
                }

                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    int xPos = e.MarginBounds.Left + leftMargin + (i * columnWidth);
                    object cellValue = row.Cells[i].Value;
                    string valueToPrint = (cellValue != null) ? cellValue.ToString() : string.Empty;
                    e.Graphics.DrawString(valueToPrint, cellFont, Brushes.Black, new RectangleF(xPos, yPos, columnWidth, row.Height), new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                yPos += row.Height;
            }

            // Reset rowIndex and indicate that no more pages are needed
            rowIndex = 0;
            e.HasMorePages = false;
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IT__Inventory
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=xxxxxxxxx;Initial Catalog=xxxxxxxxx;User ID=xxxxxxxxx;Password=xxxxxxxxx";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize the chart settings
            InitializeChart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Populate the chart with data when the button is clicked
            PopulateChart();
        }

        private void InitializeChart()
        {
            chart1.Titles.Add("IT Equipment Inventory by Device Type");

            ChartArea chartArea = new ChartArea();
            chart1.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                Name = "DeviceTypeQuantity",
                ChartType = SeriesChartType.Column,  // Use Column instead of Bar
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Int32
            };

            chart1.Series.Add(series);

            // Set axis titles
            chart1.ChartAreas[0].AxisX.Title = "Device Type";
            chart1.ChartAreas[0].AxisY.Title = "Quantity";

            // Customize the X-axis to avoid overlapping labels
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8F);
        }

        private void PopulateChart()
        {
            DataTable dataTable = GetData();

            chart1.Series["DeviceTypeQuantity"].Points.Clear();

            foreach (DataRow row in dataTable.Rows)
            {
                chart1.Series["DeviceTypeQuantity"].Points.AddXY(row["DEVICETYP"], row["Quantity"]);
            }
        }

        private DataTable GetData()
        {
            DataTable dataTable = new DataTable();

            string query = "SELECT DEVICETYP, COUNT(*) AS Quantity FROM IT_INVENTORY WHERE STATUS <> '' GROUP BY DEVICETYP";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        adapter.Fill(dataTable);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error fetching data: " + ex.Message);
                    }
                }
            }

            return dataTable;
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

namespace IT__Inventory
{
    public partial class Form2 : Form
    {
        private string connectionString = "Data Source=xxxxxxxxx;Initial Catalog=xxxxxxxxx;User ID=xxxxxxxxx;Password=xxxxxxxxx";

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Initialize the chart settings
            InitializeChart();
            //InitializePercentageChart();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Populate the chart and data grid with data when the button is clicked
            PopulateChart();
            PopulateDataGridView();
            //PopulatePercentageChart();
        }

        private void InitializeChart()
        {
            chart1.Titles.Add("IT Spending Amount and Percentages by Device Type");

            ChartArea chartArea = new ChartArea();
            chart1.ChartAreas.Add(chartArea);

            Series series = new Series
            {
                Name = "DeviceTypePrices",
                ChartType = SeriesChartType.Pie,
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Double
            };

            chart1.Series.Add(series);

            // Customize the X-axis to avoid overlapping labels
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = false;
            chart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8F);

            // Additional settings for the pie chart
            series["PieLabelStyle"] = "Outside";  // Set the labels to display outside
            series.BorderWidth = 1;
            series.BorderColor = System.Drawing.Color.Black;
        }

        private void PopulateChart()
        {
            DataTable dataTable = GetData();

            // Clear the previous series data
            chart1.Series["DeviceTypePrices"].Points.Clear();

            // Calculate total spending for all device types
            double totalSpending = dataTable.AsEnumerable()
                                            .Sum(row => Convert.ToDouble(row["TotalPrices"]));

            // Add a new column for percentage
            dataTable.Columns.Add("Percentage", typeof(double));

            // Calculate and set percentage for each row
            foreach (DataRow row in dataTable.Rows)
            {
                double totalPrices = Convert.ToDouble(row["TotalPrices"]);
                double percentage = (totalPrices / totalSpending) * 100;
                row["Percentage"] = Math.Round(percentage, 2); // Round to 2 decimal places
            }

            // Bind the modified DataTable to the DataGridView
            dataGridView1.DataSource = dataTable;

            // Populate the chart with data
            foreach (DataRow row in dataTable.Rows)
            {
                DataPoint point = new DataPoint();
                point.AxisLabel = row["DEVICETYP"].ToString();
                point.YValues = new double[] { Convert.ToDouble(row["TotalPrices"]) };
                point.Label = $"{row["DEVICETYP"]}: {row["TotalPrices"]:C} ({row["Percentage"]}%)";
                point["Exploded"] = "true";  // This can help to make the segments more distinguishable

                chart1.Series["DeviceTypePrices"].Points.Add(point);
            }

            // Ensure the labels are displayed outside the pie chart
            chart1.Series["DeviceTypePrices"].Label = "#PERCENT{P2}"; // Show percentages on labels
            chart1.Series["DeviceTypePrices"].LegendText = "#VALX (#PERCENT)";
            chart1.Series["DeviceTypePrices"].LabelForeColor = System.Drawing.Color.Black; // Set label color
        }

        private void PopulateDataGridView()
        {
            DataTable dataTable = GetData();
            dataGridView1.DataSource = dataTable;
        }

        private DataTable GetData()
        {
            DataTable dataTable = new DataTable();

            string query = "SELECT DEVICETYP, SUM(PRICES) AS TotalPrices FROM IT_INVENTORY " +
                           "WHERE STATUS <> 'WRITE_OFF' AND DATE BETWEEN @StartDate AND @EndDate GROUP BY DEVICETYP";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", dateTimePickerStart.Value);
                    command.Parameters.AddWithValue("@EndDate", dateTimePickerEnd.Value);

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

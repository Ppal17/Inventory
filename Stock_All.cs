using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace IT__Inventory
{
    public partial class Stock_All : Form
    {
        public Stock_All()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            {
                string searchString = txtSearch.Text.Trim();

                if (!string.IsNullOrEmpty(searchString))
                {
                    string query = @"SELECT * FROM IT_INVENTORY WHERE " +
                                   "INVID LIKE @SearchString OR " +
                                   "DEVICETYP LIKE @SearchString OR " +
                                   "BRAND LIKE @SearchString OR " +
                                   "MODELNO LIKE @SearchString OR " +
                                   "SERIALNO LIKE @SearchString OR " +
                                   "PERSON LIKE @SearchString OR " +
                                   "DEVICENO LIKE @SearchString OR " +
                                   "BARCODENO LIKE @SearchString";

                    using (SqlConnection connection = new SqlConnection("Data Source=xxxxxxxxx;Initial Catalog=xxxxxxxxx;User ID=xxxxxxxxx;Password=xxxxxxxxx"))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@SearchString", "%" + searchString + "%");

                            SqlDataAdapter adapter = new SqlDataAdapter(command);
                            DataTable dataTable = new DataTable();

                            try
                            {
                                connection.Open();
                                adapter.Fill(dataTable);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                            }

                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
                else
                {
                    // If search string is empty, display all data
                    // Perform your initial load logic here or retrieve all data from the database
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connect = new SqlConnection("Data Source=xxxxxxxxx;Initial Catalog=xxxxxxxxx;User ID=xxxxxxxxx;Password=xxxxxxxxx");
            connect.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlCommand command = new SqlCommand("Select * from IT_INVENTORY", connect);

            da.SelectCommand = command;
            DataTable dt = new DataTable();

            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            connect.Close();
        }
    }
}

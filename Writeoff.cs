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
    public partial class Writeoff : Form
    {
        public Writeoff()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox3.Text = "WRITE_OFF";
            }
        }

    

    private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=xxxxxxxxx;Initial Catalog=xxxxxxxxx;User ID=xxxxxxxxx;Password=xxxxxxxxx";
            string query = "UPDATE IT_INVENTORY SET [DATE]=@Date, STATUS='WRITE_OFF' WHERE BARCODENO=@Barcode";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Extract the value from DateTimePicker3 control
                    DateTime selectedDate = dateTimePicker1.Value;

                    command.Parameters.AddWithValue("@Date", selectedDate);
                    command.Parameters.AddWithValue("@Barcode", textBox1.Text);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Status and Date updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated : Check the barcode.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating: Please try again " + ex.Message);
                    }
                }
            }
        }
    }
}

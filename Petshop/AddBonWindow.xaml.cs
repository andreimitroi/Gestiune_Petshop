using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Petshop
{
    /// <summary>
    /// Interaction logic for AddBonWindow.xaml
    /// </summary>
    public partial class AddBonWindow : Window
    {
        public AddBonWindow()
        {
            InitializeComponent();
            fillTextBoxes();
        }

        public void fillTextBoxes()
        {
            tboxNumeClient.Text = Angajati.rowGlobal.Row.ItemArray[1].ToString();
            tboxPrenumeClient.Text = Angajati.rowGlobal.Row.ItemArray[2].ToString();
        }

        private void btnCreeazaBon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO Bon (IDClient, DataOra) VALUES (@IDClient, CURRENT_TIMESTAMP)";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@IDClient", SqlDbType.VarChar, 50).Value = Angajati.rowGlobal.Row.ItemArray[0];

                    // deschide conexiunea, executa insert, inchide conexiunea
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
                MessageBox.Show("Bon creat!");
                this.Close();
            }
            catch (Exception exce)
            {
                MessageBox.Show(exce.ToString());
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

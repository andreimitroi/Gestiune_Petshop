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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class UpdateProdusWindow : Window
    {
        public UpdateProdusWindow()
        {
            InitializeComponent();
            fillTextBoxes();
        }

        public void fillTextBoxes()
        {
            tboxDenumire.Text = Angajati.rowGlobal.Row.ItemArray[0].ToString();
            tboxBrand.Text = Angajati.rowGlobal.Row.ItemArray[1].ToString();
            tboxPret.Text = Angajati.rowGlobal.Row.ItemArray[2].ToString();
            cboxPentru.Text = Angajati.rowGlobal.Row.ItemArray[3].ToString();
            cboxCategorie.Text = Angajati.rowGlobal.Row.ItemArray[4].ToString();
            
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            float testarePret = 0;
            float.TryParse(tboxPret.Text, out testarePret);

            try
            {
                string query = "UPDATE Produse " +
                                "SET Pret = @Pret "+
                                "WHERE Denumire = @Denumire AND Brand = @Brand";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@Denumire", SqlDbType.VarChar, 50).Value = tboxDenumire.Text;
                    cmd.Parameters.Add("@Brand", SqlDbType.VarChar, 50).Value = tboxBrand.Text;
                    cmd.Parameters.Add("@Pret", SqlDbType.VarChar, 50).Value = tboxPret.Text;
                    // deschide conexiunea, executa insert, inchide conexiunea
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }

                MessageBox.Show("Updated!");
                this.Close();
            }
            catch (Exception exce)
            {
                MessageBox.Show(exce.ToString());
            }
        }
    }
}

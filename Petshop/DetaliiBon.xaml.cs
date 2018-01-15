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
    /// Interaction logic for DetaliiBon.xaml
    /// </summary>
    public partial class DetaliiBon : Window
    {
        public DetaliiBon()
        {
            InitializeComponent();
            FillDataGrid();
        }

        private void btnBackBon_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FillDataGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT P.Denumire 'Denumire produs', PA.Cantitate 'Cantitate produs', P.Pret 'Pret unitar', P.Pret*PA.Cantitate 'Subtotal' " +
                    "FROM Produse P, ProduseAchizitionate PA, Bon B " +
                    "WHERE P.IDProdus = PA.IDProdus AND B.IDBon = PA.IDBon AND B.IDBon = @IDBon";
                
                SqlCommand cmd = new SqlCommand(CmdString, con);

                //adaugare parametrii cmd
                cmd.Parameters.Add("@IDBon", SqlDbType.VarChar, 50).Value = Angajati.rowGlobal.Row.ItemArray[0].ToString();
                //con.Open();
                //cmd.ExecuteNonQuery();
                //con.Close();

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Bonuri");
                sda.Fill(dt);
                grdDetaliiBon.ItemsSource = dt.DefaultView;
                
            }
        }
    }
}

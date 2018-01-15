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
    public partial class AddProdusWindow : Window
    {
        public AddProdusWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
          
            try
            {
                //mapare categorie animal
                String CategAnimale=String.Empty;
                if (cboxPentru.Text == "Caini") CategAnimale = "1";
                if (cboxPentru.Text == "Pisici") CategAnimale = "2";
                if (cboxPentru.Text == "Pasari") CategAnimale = "3";
                if (cboxPentru.Text == "Rozatoare") CategAnimale = "4";

                //mapare categorie produs
                String CategProdus = String.Empty;
                if (cboxCategorie.Text == "Hrana") CategProdus = "1";
                if (cboxCategorie.Text == "Accesorii") CategProdus = "2";
                if (cboxCategorie.Text == "Farmaceutice") CategProdus = "3";
                

                string query = "INSERT INTO Produse (Denumire, Brand, Pret, IDCategAnimale, IDCategProduse) "+
                                "VALUES(@Denumire, @Brand, @Pret, @IDCategAnimale, @IDCategProduse) ";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@Denumire", SqlDbType.VarChar, 50).Value = tboxDenumire.Text;
                    cmd.Parameters.Add("@Brand", SqlDbType.VarChar, 50).Value = tboxBrand.Text;
                    cmd.Parameters.Add("@Pret", SqlDbType.VarChar, 50).Value = tboxPret.Text;
                    cmd.Parameters.Add("@IDCategAnimale", SqlDbType.VarChar, 50).Value = CategAnimale;
                    cmd.Parameters.Add("@IDCategProduse", SqlDbType.VarChar, 50).Value = CategProdus;

                    // deschide conexiunea, executa insert, inchide conexiunea
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }

                this.Close();
            }
            catch (Exception exce)
            {
                throw;
            }
        }
    }
}

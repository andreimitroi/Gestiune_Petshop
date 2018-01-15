using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for Produse.xaml
    /// </summary>
    public partial class Produse : Window
    {
        public Produse()
        {
            InitializeComponent();
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT P.Denumire 'Denumire Produs', P.Brand, P.Pret, CA.Denumire 'Categorie Animale', CP.Denumire 'Categorie produs' "
                            +"FROM Produse P, CategAnimale CA, CategProduse CP "+
                            "WHERE P.IDCategAnimale = CA.IDCategAnimale AND P.IDCategProduse = CP.IDCategProduse ";
                if (cboxFiltrareProduse.Text == "Pentru caini") CmdString += "AND CA.Denumire = 'Caini'";
                if (cboxFiltrareProduse.Text == "Pentru pisici") CmdString += "AND CA.Denumire = 'Pisici'";
                if (cboxFiltrareProduse.Text == "Pentru pasari") CmdString += "AND CA.Denumire = 'Pasari'";
                if (cboxFiltrareProduse.Text == "Pentru rozatoare") CmdString += "AND CA.Denumire = 'Rozatoare'";
                if (cboxSortareProduse.Text == "A-Z") CmdString += " ORDER BY P.Denumire ASC";
                if (cboxSortareProduse.Text == "Z-A") CmdString += " ORDER BY P.Denumire DESC";
                if (cboxSortareProduse.Text == "Pret crescator") CmdString += " ORDER BY P.Pret ASC";
                if (cboxSortareProduse.Text == "Pret descrescator") CmdString += " ORDER BY P.Pret DESC";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Produse");
                sda.Fill(dt);
                grdProduse.ItemsSource = dt.DefaultView;
                //MessageBox.Show(dt.Rows[1]["Nume"].ToString());
            }
        }

        private void btnBackProdus_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btnAddProdus_Click(object sender, RoutedEventArgs e)
        {
            AddProdusWindow addProdWind = new AddProdusWindow();
            addProdWind.ShowDialog();
        }

        private void btnUpdateProdus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Angajati.rowGlobal = grdProduse.SelectedItem as DataRowView;

                DataRowView row = grdProduse.SelectedItem as DataRowView;

                if (row.Row.ItemArray[0] != null)
                {
                    UpdateProdusWindow updateProdusWind = new UpdateProdusWindow();
                    updateProdusWind.ShowDialog();
                    
                }
                else
                {
                    MessageBox.Show("Selectati o inregistrare.");
                }
            }
            catch (Exception eww)
            {
                MessageBox.Show("Selectati o inregistrare.[batch]");
            }
         
        }

        private void btnRefreshProdus_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void btnDeleteProdus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = grdProduse.SelectedItem as DataRowView;
                //MessageBox.Show(row.Row.ItemArray.ToString());

                //mapare categorie animal
                String CategAnimale = String.Empty;
                if (row.Row.ItemArray[3].ToString() == "Caini") CategAnimale = "1";
                if (row.Row.ItemArray[3].ToString() == "Pisici") CategAnimale = "2";
                if (row.Row.ItemArray[3].ToString() == "Pasari") CategAnimale = "3";
                if (row.Row.ItemArray[3].ToString() == "Rozatoare") CategAnimale = "4";

                //mapare categorie produs
                String CategProdus = String.Empty;
                if (row.Row.ItemArray[4].ToString() == "Hrana") CategProdus = "1";
                if (row.Row.ItemArray[4].ToString() == "Accesorii") CategProdus = "2";
                if (row.Row.ItemArray[4].ToString() == "Farmaceutice") CategProdus = "3";

                string query = "DELETE FROM Produse "+"" +
                    "WHERE Denumire=@Denumire AND Brand=@Brand AND Pret=@Pret AND IDCategAnimale=@IDCategAnimale AND IDCategProduse=@IDCategProduse ";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@Denumire", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[0].ToString();
                    cmd.Parameters.Add("@Brand", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[1].ToString();
                    cmd.Parameters.Add("@Pret", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[2].ToString();
                    cmd.Parameters.Add("@IDCategAnimale", SqlDbType.VarChar, 50).Value = CategAnimale;
                    cmd.Parameters.Add("@IDCategProduse", SqlDbType.VarChar, 50).Value = CategProdus;
                    // deschide conexiunea, executa insert, inchide conexiunea
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
                FillDataGrid();
                MessageBox.Show("Sters!");
            }
            catch (Exception ew)
            {
                MessageBox.Show("Selectati o inregistrare.");
            }
        }

        private void grdProduse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnStatistici_Click(object sender, RoutedEventArgs e)
        {
            StatisticiProduseWindow statisticiProduseWind = new StatisticiProduseWindow();
            statisticiProduseWind.ShowDialog();
        }
    }
}

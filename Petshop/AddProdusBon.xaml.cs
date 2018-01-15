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
    /// Interaction logic for AddProdusBon.xaml
    /// </summary>
    public partial class AddProdusBon : Window
    {

        String idProd = String.Empty;
        
        public AddProdusBon()
        {
            InitializeComponent();
            fillTextBoxes();
            fillProdus();
        }

        public void fillDetaliiProdus()
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //search pentru produse dupa categorie si animal
                CmdString = "SELECT Brand, Pret, IDProdus " +
                        "FROM Produse " +
                        "WHERE Denumire = @Denumire ";

                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.Add("@Denumire", SqlDbType.VarChar, 50).Value = cboxProdus.Text;
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Detalii");
                sda.Fill(dt);

                //pune detalii in tboxes
                //DataRow[] row = dt.Select();
                //tboxBrand.Text = row[1].ItemArray[0].ToString();

                DataRow[] row = dt.Select();
                foreach (DataRow element in row)
                {
                    tboxBrand.Text = element.ItemArray[0].ToString();
                    tboxPret.Text = element.ItemArray[1].ToString();
                    idProd = element.ItemArray[2].ToString();
                }
                tboxPret.Text += " RON";

            }
        }

        public void fillProdus()
        {
            cboxProdus.Items.Clear();
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //search pentru produse dupa categorie si animal
                CmdString = "SELECT P.Denumire, P.Brand, P.Pret " +
                        "FROM Produse P, CategAnimale CA, CategProduse CP " +
                        "WHERE P.IDCategAnimale = CA.IDCategAnimale AND P.IDCategProduse = CP.IDCategProduse ";
                //adaugare filtru specie
                if (cboxPentru.Text == "Caini") CmdString += "AND CA.Denumire = 'Caini' ";
                if (cboxPentru.Text == "Pisici") CmdString += "AND CA.Denumire = 'Pisici' ";
                if (cboxPentru.Text == "Pasari") CmdString += "AND CA.Denumire = 'Pasari' ";
                if (cboxPentru.Text == "Rozatoare") CmdString += "AND CA.Denumire = 'Rozatoare' ";

                //adaugare filtru categorie produs
                if (cboxCategorie.Text == "Hrana") CmdString += "AND CP.Denumire = 'Hrana' ";
                if (cboxCategorie.Text == "Accesorii") CmdString += "AND CP.Denumire = 'Accesorii' ";
                if (cboxCategorie.Text == "Farmaceutice") CmdString += "AND CP.Denumire = 'Farmaceutice' ";
               

                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Produse");
                sda.Fill(dt);

                //pune categoriile in cbox
                DataRow[] row = dt.Select();
                foreach (DataRow element in row)
                {
                    cboxProdus.Items.Add(element.ItemArray[0]);
                }
            }
            
        }

        public void fillTextBoxes()
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //pune categoriile de produse in cbox
                CmdString = "SELECT Denumire "
                            + "FROM CategProduse ";

                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Categorii");
                sda.Fill(dt);
                
                DataRow[] row = dt.Select();
                foreach (DataRow element in row)
                {
                    cboxCategorie.Items.Add(element.ItemArray[0]);
                }

                //pune categoriile de animale in cbox
                CmdString = "SELECT Denumire "
                            + "FROM CategAnimale ";

                cmd = new SqlCommand(CmdString, con);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable("Animale");
                sda.Fill(dt);
                
                row = dt.Select();
                foreach (DataRow element in row)
                {
                    cboxPentru.Items.Add(element.ItemArray[0]);
                }

                fillProdus();
            }
        }

        private void btnAddProdusBon_Click(object sender, RoutedEventArgs e)
        {
            int testCantitate = 0;
            int.TryParse(tboxCantitate.Text, out testCantitate);
            if (testCantitate != 0)
            {
                try
                {
                    string query = "INSERT INTO ProduseAchizitionate (IDBon, IDProdus, Cantitate)" +
                                    "VALUES(@IDBon, @IDProdus, @Cantitate)";

                    string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                    using (SqlConnection cn = new SqlConnection(ConString))
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        // definirea parametrilor si a valorilor lor
                        cmd.Parameters.Add("@IDBon", SqlDbType.VarChar, 50).Value = Angajati.rowGlobal.Row.ItemArray[0];
                        cmd.Parameters.Add("@IDProdus", SqlDbType.VarChar, 50).Value = idProd;
                        cmd.Parameters.Add("@Cantitate", SqlDbType.VarChar, 50).Value = tboxCantitate.Text;


                        // deschide conexiunea, executa insert, inchide conexiunea
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();

                    }

                    MessageBox.Show("Produs adaugat!");
                }
                catch (Exception exce)
                {
                    MessageBox.Show("Eroare!");
                }
            } else
            {
                MessageBox.Show("Introduceti cantitatea!");
            }
            
        }

        private String CalculeazaTotal()
        {
            try
            {
                String total = String.Empty;

                string query = "SELECT SUM(P.Pret*PA.Cantitate) " +
                                "FROM Produse P, ProduseAchizitionate PA, Bon B " +
                                "WHERE P.IDProdus = PA.IDProdus AND PA.IDBon = B.IDBon AND B.IDBon=@IDBon ";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    
                    cmd.Parameters.Add("@IDBon", SqlDbType.VarChar, 50).Value = Angajati.rowGlobal.Row.ItemArray[0];

                   

                    // deschide conexiunea, executa insert, inchide conexiunea
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();


                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("Calculeaza");
                    sda.Fill(dt);

                    DataRow[] row = dt.Select();
                    foreach (DataRow element in row)
                    {
                        total = element.ItemArray[0].ToString();
                        
                    }
                    
                }
                return total;
            }
            catch (Exception exce)
            {
                MessageBox.Show("Eroare functie calcul!");
                return "0";
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "UPDATE Bon SET Total = @Total WHERE IDBon = @IDBon";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    String test = CalculeazaTotal();
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@Total", SqlDbType.VarChar, 50).Value = test;
                    cmd.Parameters.Add("@IDBon", SqlDbType.VarChar, 50).Value = Angajati.rowGlobal.Row.ItemArray[0];

                    

                    // deschide conexiunea, executa insert, inchide conexiunea
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();

                }
                this.Close();
            }
            catch (Exception exce)
            {
                MessageBox.Show("Eroare salvare!");
            }
        }

        private void cboxProdus_DropDownOpened(object sender, EventArgs e)
        {
            fillProdus();
        }

        private void cboxProdus_DropDownClosed(object sender, EventArgs e)
        {
            fillDetaliiProdus();
        }
    }
}

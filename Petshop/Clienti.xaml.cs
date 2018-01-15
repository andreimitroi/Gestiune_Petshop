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
    /// Interaction logic for Clienti.xaml
    /// </summary>
    public partial class Clienti : Window
    {
        public Clienti()
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
                CmdString = "SELECT IDClient, Nume 'Nume client', Prenume 'Prenume client', Adresa, Varsta, Sex, convert(varchar(10), DataInregistrarii, 120) 'Data inregistrarii' " +
                            "FROM Clienti ";
                
                if (cboxSortareClienti.Text == "A-Z") CmdString += " ORDER BY Nume ASC, Prenume ASC";
                if (cboxSortareClienti.Text == "Z-A") CmdString += " ORDER BY Nume DESC, Prenume DESC";
                if (cboxSortareClienti.Text == "Numar bonuri")
                {
                    CmdString = "SELECT IDClient, C.Nume 'Nume client', C.Prenume 'Prenume client', C.Adresa, C.Varsta, C.Sex, " +
                            "convert(varchar(10), C.DataInregistrarii, 120) 'Data inregistrarii', COUNT(B.IDClient) 'Numar bonuri' " +
                            "FROM Clienti C, Bon B " +
                            "WHERE C.IDClient = B.IDClient " +
                            "GROUP BY C.Nume, C.Prenume, C.Adresa, C.Varsta, C.Sex, C.DataInregistrarii";
                }
                if (cboxSortareClienti.Text == "Numar produse")
                {
                    CmdString = "SELECT IDClient, C.Nume 'Nume client', C.Prenume 'Prenume client', C.Adresa, C.Varsta, C.Sex, " +
                            "convert(varchar(10), C.DataInregistrarii, 120) 'Data inregistrarii', SUM(PA.Cantitate) 'Numar produse cumparate' " +
                            "FROM Clienti C, ProduseAchizitionate PA, Bon B " +
                            "WHERE C.IDClient = B.IDClient AND B.IDBon = PA.IDBon " +
                            "GROUP BY C.Nume, C.Prenume, C.Adresa, C.Varsta, C.Sex, C.DataInregistrarii";
                }
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Clienti");
                sda.Fill(dt);
                
                grdClienti.ItemsSource = dt.DefaultView;
                //MessageBox.Show(dt.Rows[1]["Nume"].ToString());
                //pune iteme in lista combobox
                //DataRow[] row = dt.Select();
                //foreach (DataRow element in row)
                //{
                //    cboxSortareClienti2.Items.Add(element.ItemArray[0]);
                //}

                //grdClienti.Columns[0].Visibility = Visibility.Hidden;
                
                
            }
        }

        

        private void btnBackClient_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addClientWind = new AddClientWindow();
            addClientWind.ShowDialog();
        }

        private void btnUpdateClient_Click(object sender, RoutedEventArgs e)
        {



            // DataRowView row = grdClienti.SelectedItem as DataRowView;
            // MessageBox.Show(grdClienti.SelectedItem.ToString());
            Angajati.rowGlobal = grdClienti.SelectedItem as DataRowView;
               
                if (grdClienti.SelectedItem != null)
                {
                    updateClientWindow updateClientWind = new updateClientWindow();
                    updateClientWind.ShowDialog();
                    
                }
                else
                {
                    MessageBox.Show("Selectati o inregistrare.");
                }
            
            
            
        }

        private void btnRefreshClient_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void btnDeleteClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = grdClienti.SelectedItem as DataRowView;
                //MessageBox.Show(row.Row.ItemArray[0].ToString());

                string query = "DELETE FROM Clienti "+
                    "WHERE Nume = @Nume AND Prenume = @Prenume AND Adresa = @Adresa AND Varsta = @Varsta";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@Nume", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[1].ToString();
                    cmd.Parameters.Add("@Prenume", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[2].ToString();
                    cmd.Parameters.Add("@Adresa", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[3].ToString();
                    cmd.Parameters.Add("@Varsta", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[4].ToString();
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
                MessageBox.Show("Eroare.");
            }
        }

        private void grdClienti_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddBon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = grdClienti.SelectedItem as DataRowView;
                Angajati.rowGlobal = grdClienti.SelectedItem as DataRowView;

                if (row.Row.ItemArray[0] != null)
                {
                    AddBonWindow addBonWind = new AddBonWindow();
                    addBonWind.ShowDialog();
                    
                }
                else
                {
                    MessageBox.Show("Selectati o inregistrare.");
                }
            }
            catch (Exception eww)
            {
                MessageBox.Show("Selectati o inregistrare.");
            }

        }
    }
}

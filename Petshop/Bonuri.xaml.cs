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
    /// Interaction logic for Bonuri.xaml
    /// </summary>
    public partial class Bonuri : Window
    {
        public Bonuri()
        {
            InitializeComponent();
            FillDataGrid();
        }


        private void btnDeleteBon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = grdBonuri.SelectedItem as DataRowView;
                //MessageBox.Show(row.Row.ItemArray[0].ToString());

                string query = "DELETE FROM Bon " +
                    "WHERE IDBon = @IDBon AND DataOra = @DataOra";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@IDBon", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[0].ToString();
                    cmd.Parameters.Add("@DataOra", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[4].ToString();

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

        private void btnRefreshBon_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void btnBackBon_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWind = new MainWindow();
            mainWind.Show();
            this.Close();
        }

        private void FillDataGrid()
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT B.IDBon, C.Nume 'Nume client', C.Prenume 'Prenume client', B.Total 'Total bon', B.DataOra 'Data si ora' " +
                    "FROM Bon B, Clienti C " +
                    "WHERE B.IDClient = C.IDClient";
                if (cboxSortareAngajati.Text == "A-Z") CmdString += " ORDER BY Nume ASC, Prenume ASC";
                if (cboxSortareAngajati.Text == "Z-A") CmdString += " ORDER BY Nume DESC, Prenume DESC";
                if (cboxSortareAngajati.Text == "Total ASC") CmdString += " ORDER BY B.Total ASC";
                if (cboxSortareAngajati.Text == "Total DESC") CmdString += " ORDER BY B.Total DESC";
                if (cboxSortareAngajati.Text == "Cele mai recente") CmdString += " ORDER BY B.DataOra DESC";
                if (cboxSortareAngajati.Text == "Cele mai vechi") CmdString += " ORDER BY B.DataOra ASC";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Bonuri");
                sda.Fill(dt);
                grdBonuri.ItemsSource = dt.DefaultView;
            }
        }

        private void btnDetaliiBon_Click(object sender, RoutedEventArgs e)
        {
            if (grdBonuri.SelectedItem != null)
            {
                Angajati.rowGlobal = grdBonuri.SelectedItem as DataRowView;
                DetaliiBon detaliiBonWind = new DetaliiBon();
                detaliiBonWind.ShowDialog();
            } else
            {
                MessageBox.Show("Selectati un bon!");
            }
            
        }

        private void btnAddProdusBon_Click(object sender, RoutedEventArgs e)
        {
            if (grdBonuri.SelectedItem != null)
            {
                Angajati.rowGlobal = grdBonuri.SelectedItem as DataRowView;
                AddProdusBon addProdusBonWind = new AddProdusBon();
                addProdusBonWind.ShowDialog();
            }
            else
            {
                MessageBox.Show("Selectati un bon!");
            }
        }

        private void btnStatisticiBon_Click(object sender, RoutedEventArgs e)
        {
            StatisticiBonWindow statisticiBonWind = new StatisticiBonWindow();
            statisticiBonWind.ShowDialog();
        }
    }
}

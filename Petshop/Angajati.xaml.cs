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
    /// Interaction logic for Angajati.xaml
    /// </summary>
    public partial class Angajati : Window
    {
        public static DataRowView rowGlobal;
        public Angajati()
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
                CmdString = "SELECT Nume 'Nume angajat', Prenume 'Prenume angajat', CNP, Varsta, Sex, Salariu 'Salariu (RON)', convert(varchar(10), DataAngajarii, 120) 'Data Angajarii' FROM Angajati";
                if (cboxSortareAngajati.Text == "A-Z") CmdString += " ORDER BY Nume ASC, Prenume ASC";
                if (cboxSortareAngajati.Text == "Z-A") CmdString += " ORDER BY Nume DESC, Prenume DESC";
                if (cboxSortareAngajati.Text == "Salariu crescator") CmdString += " ORDER BY Salariu ASC";
                if (cboxSortareAngajati.Text == "Salariu descrescator") CmdString += " ORDER BY Salariu DESC";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Angajati");
                sda.Fill(dt);
                grdAngajati.ItemsSource = dt.DefaultView;

                
            }
        }

        private void btnBackAngajat_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void btnAddAngajat_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWind = new AddWindow();
            addWind.ShowDialog();
        }

        private void btnUpdateAngajat_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                rowGlobal = grdAngajati.SelectedItem as DataRowView;

                DataRowView row = grdAngajati.SelectedItem as DataRowView;
                //if (!grdAngajati.SelectedItem.Equals(null))
                if (row.Row.ItemArray[0] != null)
                {
                    UpdateWindow updateWind = new UpdateWindow();
                    updateWind.ShowDialog();
                    
                }
                else
                {
                    MessageBox.Show("Selectati o inregistrare.");
                }
            }
            catch (Exception eww)
            {
                MessageBox.Show("Selectati o inregistrare.[catch]");
            }
         
        }

        private void btnRefreshAngajat_Click(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void btnDeleteAngajat_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Stergeti inregistrarea?", "Stergere inregistrare", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                //do no stuff
                this.Close();
            }
            else
            {
                //do yes stuff

                try
                {
                    DataRowView row = grdAngajati.SelectedItem as DataRowView;
                    //MessageBox.Show(row.Row.ItemArray[0].ToString());

                    string query = "DELETE FROM Angajati WHERE CNP = @CNP ";

                    string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                    using (SqlConnection cn = new SqlConnection(ConString))
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        // definirea parametrilor si a valorilor lor
                        cmd.Parameters.Add("@CNP", SqlDbType.VarChar, 50).Value = row.Row.ItemArray[2].ToString();
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
        }

        private void grdAngajati_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnStatisticiAngajat_Click(object sender, RoutedEventArgs e)
        {
            StatisticiAngajatWindow statisticiAngajatWind = new StatisticiAngajatWindow();
            statisticiAngajatWind.ShowDialog();
        }
    }
}

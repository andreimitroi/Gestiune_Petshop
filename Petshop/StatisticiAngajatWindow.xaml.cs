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
    /// Interaction logic for StatisticiAngajatWindow.xaml
    /// </summary>
    public partial class StatisticiAngajatWindow : Window
    {
        public StatisticiAngajatWindow()
        {
            InitializeComponent();
            FillDataGrids();
        }

        private void btnBackBon_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FillDataGrids()
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //fill grid1
                CmdString = "SELECT S.Nume, S.Salariu " +
                    "FROM (SELECT TOP 3 A.Nume 'Nume', A.Prenume 'Prenume', A.Salariu 'Salariu' " +
                    "FROM Angajati A ORDER BY A.Salariu DESC ) S";

                SqlCommand cmd = new SqlCommand(CmdString, con);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("statistici1");
                sda.Fill(dt);
                grdStatistici1.ItemsSource = dt.DefaultView;

                //fill grid2
                CmdString = "SELECT TOP 3 A.Nume, A.Prenume, A.Salariu " +
                    "FROM Angajati A GROUP BY A.Nume, A.Prenume, A.Salariu " +
                    "HAVING A.Salariu <= (SELECT AVG(Salariu) FROM Angajati) ORDER BY A.Salariu DESC";
                cmd = new SqlCommand(CmdString, con);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable("statistici2");
                sda.Fill(dt);
                grdStatistici2.ItemsSource = dt.DefaultView;
            }
        }

    }
}

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
    /// Interaction logic for StatisticiBonWindow.xaml
    /// </summary>
    public partial class StatisticiBonWindow : Window
    {
        public StatisticiBonWindow()
        {
            InitializeComponent();
            FillDataGrids();
        }

        private void FillDataGrids()
        {
            string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //fill grid1
                CmdString = "SELECT C.Nume, C.Prenume, N.NrBonuri 'Numar bonuri' " + 
                            "FROM Clienti C, (SELECT C2.IDClient, C2.Nume, C2.Prenume, COUNT(*) NrBonuri FROM Clienti C2, Bon B WHERE C2.IDClient = B.IDClient " +
                            "GROUP BY C2.IDClient, C2.Nume, C2.Prenume) AS N WHERE C.IDClient = N.IDClient ORDER BY N.NrBonuri DESC";

                SqlCommand cmd = new SqlCommand(CmdString, con);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("statistici1");
                sda.Fill(dt);
                grdStatistici1.ItemsSource = dt.DefaultView;

                //fill grid2
                CmdString = "SELECT C.Nume, C.Prenume, B.IDBon, B.Total " +
                            "FROM Bon B, Clienti C " +
                               "WHERE B.IDClient = C.IDClient " +
                               "GROUP BY C.Nume, C.Prenume, B.IDBon, B.Total " +
                               "HAVING B.Total > (SELECT AVG(Total) FROM Bon)  " +
                               "ORDER BY B.Total DESC";
                cmd = new SqlCommand(CmdString, con);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable("statistici2");
                sda.Fill(dt);
                grdStatistici2.ItemsSource = dt.DefaultView;
            }
        }

        private void btnBackBon_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

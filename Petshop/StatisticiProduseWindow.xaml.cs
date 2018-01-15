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
    /// Interaction logic for StatisticiProduseWindow.xaml
    /// </summary>
    public partial class StatisticiProduseWindow : Window
    {
        public StatisticiProduseWindow()
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
                //fill grid1
                CmdString = "SELECT TOP 3 P.Denumire, P.Brand, P.Pret, I2.NrAparitii 'Numar produse vandute'" +
                    "FROM Produse P, (SELECT PA.IDProdus, SUM(PA.Cantitate) NrAparitii " +
                    "FROM ProduseAchizitionate PA GROUP BY PA.IDProdus) I2 " +
                "WHERE P.IDProdus = I2.IDProdus AND I2.NrAparitii <= (SELECT MAX(I.NrAparitii) " +
                    "FROM (SELECT PA2.IDProdus, SUM(PA2.Cantitate) NrAparitii " +
                    "FROM ProduseAchizitionate PA2 " +
                    "GROUP BY PA2.IDProdus) I) ORDER BY I2.NrAparitii DESC";

                SqlCommand cmd = new SqlCommand(CmdString, con);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("statistici1");
                sda.Fill(dt);
                grdStatistici.ItemsSource = dt.DefaultView;
            }
        }
    }
}

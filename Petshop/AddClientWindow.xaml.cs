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
    /// Interaction logic for AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        public AddClientWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //long testareCNP = 0;
            //long.TryParse(tboxCNP.Text, out testareCNP);
            
            if (tboxNume.Text.Length == 0) MessageBox.Show("Nume invalid!");
            if (tboxPrenume.Text.Length == 0) MessageBox.Show("Preume invalid!");
            if (tboxVarsta.Text.Length == 0)
            {
                MessageBox.Show("Varsta invalida!");
            }
            
            if (tboxDataInregistrarii.Text.Length == 0) MessageBox.Show("Data inregistrarii invalida!");
            //if ((testareCNP == 0)||
            //    (tboxCNP.Text==null)||
            //    (tboxCNP.Text.Length!=13)) MessageBox.Show("CNP invalid!");
            
            try
            {
                string query = "INSERT INTO Clienti (Nume, Prenume, Adresa, Varsta, Sex, DataInregistrarii)"+
                                "VALUES(@Nume, @Prenume, @Adresa, @Varsta, @Sex, @DataInregistrarii)";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@Nume", SqlDbType.VarChar, 50).Value = tboxNume.Text;
                    cmd.Parameters.Add("@Prenume", SqlDbType.VarChar, 50).Value = tboxPrenume.Text;
                    cmd.Parameters.Add("@Adresa", SqlDbType.VarChar, 50).Value = tboxAdresa.Text;
                    cmd.Parameters.Add("@Varsta", SqlDbType.VarChar, 50).Value = tboxVarsta.Text;
                    cmd.Parameters.Add("@Sex", SqlDbType.VarChar, 50).Value = cboxSex.Text;
                    cmd.Parameters.Add("@DataInregistrarii", SqlDbType.VarChar, 50).Value = tboxDataInregistrarii.Text;
          

                    // deschide conexiunea, executa insert, inchide conexiunea
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }

                this.Close();
            }
            catch (Exception exce)
            {
                MessageBox.Show("Data inregistrarii invalida!"+"\n"+"Format data: YYYYMMDD");
            }
        }
    }
}

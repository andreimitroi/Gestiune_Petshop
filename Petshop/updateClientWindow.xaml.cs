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
    /// Interaction logic for updateClientWindow.xaml
    /// </summary>
    public partial class updateClientWindow : Window
    {
        public updateClientWindow()
        {
            InitializeComponent();
            fillTextBoxes();
        }

        public void fillTextBoxes()
        {
            tboxNume.Text = Angajati.rowGlobal.Row.ItemArray[1].ToString();
            tboxPrenume.Text = Angajati.rowGlobal.Row.ItemArray[2].ToString();
            tboxAdresa.Text = Angajati.rowGlobal.Row.ItemArray[3].ToString();
            tboxVarsta.Text = Angajati.rowGlobal.Row.ItemArray[4].ToString();
            cboxSex.Text = Angajati.rowGlobal.Row.ItemArray[5].ToString();
            tboxDataInregistrarii.Text = Angajati.rowGlobal.Row.ItemArray[6].ToString();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tboxNume.Text.Length == 0) MessageBox.Show("Nume invalid!");
            if (tboxPrenume.Text.Length == 0) MessageBox.Show("Preume invalid!");
            if (tboxVarsta.Text.Length == 0)
            {
                MessageBox.Show("Varsta invalida!");
            }

            if (tboxDataInregistrarii.Text.Length == 0) MessageBox.Show("Data Inregistrarii invalida!");
            

            try
            {
                string query = "UPDATE Clienti " +
                                "SET Nume = @Nume, Prenume = @Prenume, Adresa = @Adresa, Varsta = @Varsta, Sex = @Sex, DataInregistrarii = @DataInregistrarii " +
                                "WHERE IDClient = @IDClient";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@IDClient", SqlDbType.VarChar, 50).Value = Angajati.rowGlobal.Row.ItemArray[0].ToString();
                    cmd.Parameters.Add("@Nume", SqlDbType.VarChar, 50).Value = tboxNume.Text;
                    cmd.Parameters.Add("@Prenume", SqlDbType.VarChar, 50).Value = tboxPrenume.Text;
                    cmd.Parameters.Add("@Varsta", SqlDbType.VarChar, 50).Value = tboxVarsta.Text;
                    cmd.Parameters.Add("@Sex", SqlDbType.VarChar, 50).Value = cboxSex.Text;
                    cmd.Parameters.Add("@Adresa", SqlDbType.VarChar, 50).Value = tboxAdresa.Text;
                    cmd.Parameters.Add("@DataInregistrarii", SqlDbType.VarChar, 50).Value = tboxDataInregistrarii.Text;

                    // deschide conexiunea, executa insert, inchide conexiunea
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }

                MessageBox.Show("Updated!");
                this.Close();
            }
            catch (Exception exce)
            {
                MessageBox.Show("Data inregistrarii invalida!" + "\n" + "Format data: YYYYMMDD");
            }
        }
    

    private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

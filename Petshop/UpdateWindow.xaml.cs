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
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
            fillTextBoxes();
        }

        public void fillTextBoxes()
        {
            tboxNume.Text = Angajati.rowGlobal.Row.ItemArray[0].ToString();
            tboxPrenume.Text = Angajati.rowGlobal.Row.ItemArray[1].ToString();
            tboxCNP.Text = Angajati.rowGlobal.Row.ItemArray[2].ToString();
            tboxVarsta.Text = Angajati.rowGlobal.Row.ItemArray[3].ToString();
            cboxSex.Text = Angajati.rowGlobal.Row.ItemArray[4].ToString();
            tboxSalariu.Text = Angajati.rowGlobal.Row.ItemArray[5].ToString();
            tboxDataAngajarii.Text = Angajati.rowGlobal.Row.ItemArray[6].ToString();

        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            float testareSalariu = 0;
            float.TryParse(tboxSalariu.Text, out testareSalariu);


            if (tboxNume.Text.Length == 0) MessageBox.Show("Nume invalid!");
            if (tboxPrenume.Text.Length == 0) MessageBox.Show("Preume invalid!");
            if (tboxVarsta.Text.Length == 0)
            {
                MessageBox.Show("Varsta invalida!");
            }

            if (tboxDataAngajarii.Text.Length == 0) MessageBox.Show("Data angajarii invalida!");
            //if ((testareCNP == 0)||
            //    (tboxCNP.Text==null)||
            //    (tboxCNP.Text.Length!=13)) MessageBox.Show("CNP invalid!");
            if (testareSalariu == 0) MessageBox.Show("Salariu invalid!");

            try
            {
                string query = "UPDATE Angajati " +
                                "SET Nume = @Nume, Prenume = @Prenume, Varsta = @Varsta, Sex = @Sex, Salariu = @Salariu, DataAngajarii = @DataAngajarii "+
                                "WHERE CNP = @CNP";

                string ConString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                using (SqlConnection cn = new SqlConnection(ConString))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    // definirea parametrilor si a valorilor lor
                    cmd.Parameters.Add("@Nume", SqlDbType.VarChar, 50).Value = tboxNume.Text;
                    cmd.Parameters.Add("@Prenume", SqlDbType.VarChar, 50).Value = tboxPrenume.Text;
                    cmd.Parameters.Add("@CNP", SqlDbType.VarChar, 50).Value = tboxCNP.Text;
                    cmd.Parameters.Add("@Varsta", SqlDbType.VarChar, 50).Value = tboxVarsta.Text;
                    cmd.Parameters.Add("@Sex", SqlDbType.VarChar, 50).Value = cboxSex.Text;
                    cmd.Parameters.Add("@Salariu", SqlDbType.VarChar, 50).Value = tboxSalariu.Text;
                    cmd.Parameters.Add("@DataAngajarii", SqlDbType.VarChar, 50).Value = tboxDataAngajarii.Text;

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
                MessageBox.Show("CNP invalid sau data angajarii invalida!" + "\n" + "Format data: YYYYMMDD");
            }
        }
    }
}

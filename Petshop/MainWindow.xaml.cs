using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Petshop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btnAngajati_Click(object sender, RoutedEventArgs e)
        {
            Angajati angajatiWindow = new Angajati();
            angajatiWindow.Show();
            this.Close();
        }

        private void btnProduse_Click(object sender, RoutedEventArgs e)
        {
            Produse produseWindow = new Produse();
            produseWindow.Show();
            this.Close();
        }

        private void btnClienti_Click(object sender, RoutedEventArgs e)
        {
            Clienti clientiWindow = new Clienti();
            clientiWindow.Show();
            this.Close();
        }

        private void btnBonuri_Click(object sender, RoutedEventArgs e)
        {
            Bonuri bonuriWind = new Bonuri();
            bonuriWind.Show();
            this.Close();
        }
    }
}

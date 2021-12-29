using Real.ViewModel;
using System;
using System.Collections.Generic;
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

namespace Real
{

    /// <summary>
    /// Namechange.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Namechange : Page
    {
        public Namechange()
        {
            InitializeComponent();
            this.DataContext = new NameChangeViewModel();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate
            (
                 new Uri("VIew/menu.xaml", UriKind.Relative)
            );
        }
    }
}

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
using System.Data;
using Real.ViewModel;

namespace Real
{
    /// <summary>
    /// change.xaml에 대한 상호 작용 논리
    /// </summary>		value	1483680	int
    
    public partial class change : Page
    {
        public change()
        {
            InitializeComponent();
            this.DataContext = new changeViewModel();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate
            (
                 new Uri("/menu.xaml", UriKind.Relative)
            );
        }
    }
}

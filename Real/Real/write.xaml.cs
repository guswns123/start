using Microsoft.WindowsAPICodePack.Dialogs;
using Real.ViewModel;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Real
{
    /// <summary>
    /// write.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class write : Page
    {
        public write()
        {
            InitializeComponent();
            this.DataContext = new writeViewModel();
        }
   
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate
            (
                new Uri("/menu.xaml", UriKind.Relative)
            );
        }
    }
}

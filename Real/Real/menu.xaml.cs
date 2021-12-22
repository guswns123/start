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
using Microsoft.Win32;
using System.Management;
using System.IO;
using Real.menu;
namespace Real
{

/// <summary>
/// maue.xaml에 대한 상호 작용 논리
/// </summary>
public partial class maue : Page
    {

        public maue()
        {
            InitializeComponent();
            this.DataContext = new menuViewModel();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Outedid_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate
            (
                new Uri("/reaed.xaml", UriKind.Relative)
            );
        }
        private void Write_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate
            (
                new Uri("/write.xaml", UriKind.Relative)
            );
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (ChangeEdid.dtd > 0)
            {
                NavigationService.Navigate
                (
                    new Uri("/change.xaml", UriKind.Relative)
                );
            }
            else
                System.Windows.MessageBox.Show("DTB does not exist");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (ChangeEdid.dess != "")
            {
                NavigationService.Navigate
                (
                    new Uri("/Namechange.xaml", UriKind.Relative)
                );
            }
            else
                System.Windows.MessageBox.Show("The name specified in the descriptor does not exist");
        }
    }
}

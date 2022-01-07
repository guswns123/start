using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using EDIDParser;
using Real.ViewModel;
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
                new Uri("VIew/reaed.xaml", UriKind.Relative)
            );
        }
        private void Write_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService.Navigate
            (
                new Uri("VIew/write.xaml", UriKind.Relative)
            );
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

                NavigationService.Navigate
                (
                    new Uri("VIew/change.xaml", UriKind.Relative)
                );
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
                NavigationService.Navigate
                (
                    new Uri("VIew/Namechange.xaml", UriKind.Relative)
                );
        }
    }
}

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
using System.Collections;
using System.Management;
using System.Web.Mobile;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Real.ViewModel;

namespace Real
{

    /// <summary>
    /// reaed.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class reaed 
    {
        public reaed()
        {
            InitializeComponent();
            this.DataContext = new readViewModel();
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

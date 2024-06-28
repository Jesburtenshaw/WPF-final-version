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

namespace CDMMain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var cdmControl = new CDM.UserControls.CDMUserControl(this.Dispatcher, grdMain.ActualWidth, grdMain.ActualHeight);
            SizeChanged += cdmControl.CDMUserControl_SizeChanged;
            grdMain.Children.Add(cdmControl);
            cdmControl.Loaded += CdmControl_Loaded;

        }

        private void CdmControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                (sender as CDM.UserControls.CDMUserControl).LoadUI();
            }
            catch 
            {

            }
        }
    }
}

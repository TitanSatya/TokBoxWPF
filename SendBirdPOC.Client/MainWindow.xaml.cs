using SendBird;
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

namespace SendBirdPOC.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string APP_ID = "D44B8383-3ADD-4247-8CC7-79715EDE2F03";
        const string USER_ID = "10100";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SendBirdClient.Init(APP_ID);
            SendBirdClient.Connect(USER_ID, (User user, SendBirdException z) =>
            {
                if (z != null)
                {

                    MessageBox.Show(z.Message);
                    return;

                }
                else
                {
                    MessageBox.Show("User Connected");
                    
                }
            });

        }
    }
}

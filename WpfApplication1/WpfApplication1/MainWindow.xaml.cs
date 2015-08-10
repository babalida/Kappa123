using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Windows.Media.Animation;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ClientSocket clientSocket;
        Timer timer = new Timer();
        int interval_timer = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (tbWarning.Opacity > 0)
            {
                DoubleAnimation a = new DoubleAnimation();

                a.AutoReverse = false;
                a.RepeatBehavior = new RepeatBehavior(1);
                a.AccelerationRatio = 0.3;
                a.From = tbWarning.Opacity;
                a.To = 0.0;
                a.Duration = TimeSpan.FromSeconds(1);
                tbWarning.BeginAnimation(TextBlock.OpacityProperty, a);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Elapsed += timer_Elapsed;
            clientSocket = new ClientSocket();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (interval_timer > 10)
            {
                if (clientSocket.clientSocket.Connected)
                {
                    UserData uData = new UserData();
                    uData.Login = loginTextBox.Text;
                    uData.Password = passTextBox.Text;
                    string JsonText = JsonConvert.SerializeObject(uData);
                    clientSocket.Send(JsonText);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(loginTextBox.Text) || !string.IsNullOrWhiteSpace(passTextBox.Text))
            {
                interval_timer = 0;
                timer.Interval = 1000;
                timer.Enabled = true;
                timer.Start();
                if (!clientSocket.clientSocket.Connected)
                {
                    clientSocket.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    clientSocket.Connect("192.168.0.102", 25014);
                }
            }
            else
            {
                DoubleAnimation a = new DoubleAnimation();
                
                a.AutoReverse = false;
                a.RepeatBehavior = new RepeatBehavior(1);
                a.AccelerationRatio = 0.3;
                a.From = tbWarning.Opacity;
                a.To = 0.8;
                a.Duration = TimeSpan.FromSeconds(1);
                tbWarning.BeginAnimation(TextBlock.OpacityProperty, a);

            }

            DialogResult = true;
        }

        private void loginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void passTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void passTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (tbWarning.Opacity > 0)
            {
                DoubleAnimation a = new DoubleAnimation();

                a.AutoReverse = false;
                a.RepeatBehavior = new RepeatBehavior(1);
                a.AccelerationRatio = 0.3;
                a.From = tbWarning.Opacity;
                a.To = 0.0;
                a.Duration = TimeSpan.FromSeconds(1);
                tbWarning.BeginAnimation(TextBlock.OpacityProperty, a);
            }
        }
    }
}

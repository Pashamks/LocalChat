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
using ChatClient.ServiceChat;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        bool isConnected = false, mode = false;
        ServiceChatClient client;
        int ID;
        public MainWindow()
        {
            InitializeComponent();
        }
        void ConnectUser()
        {
            if(!isConnected)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                bConDiscon.Content = "Disconnect";
                isConnected = true;
            }
        }
        void DisConnectUser()
        {
            if (isConnected)
            {
                client.DisConnect(ID);
                client = null;
                tbUserName.IsEnabled = true;
                bConDiscon.Content = "Connect";
                isConnected = false;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisConnectUser();
            }
            else
            {
                ConnectUser();
            }
        }


        public void MsgCallBack(string msg)
        {
            lbChat.Items.Add(msg);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bColor.Background = Brushes.White;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisConnectUser();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(client!=null)
                client.SentMsg(tbMessage.Text, ID);
                tbMessage.Text = "";
            }
        }

        private void bSent_Click(object sender, RoutedEventArgs e)
        {
            if (client != null)
                client.SentMsg(tbMessage.Text, ID);
            tbMessage.Text = "";
        }

        private void bColor_Click(object sender, RoutedEventArgs e)
        {
            if(!mode)
            {
                mainForm.Background = Brushes.Black;
                lbChat.Background = Brushes.Gray;
                tbUserName.Background = Brushes.Gray;
                tbMessage.Background = Brushes.Gray;

                modeImg.Source = new BitmapImage(new Uri(@"/Resources/Black_mode.png", UriKind.RelativeOrAbsolute));
                bColor.Background = Brushes.Black;
                mode = true;
            }
            else
            {
                mainForm.Background = Brushes.White;
                lbChat.Background = Brushes.White;
                tbUserName.Background = Brushes.White;
                tbMessage.Background = Brushes.White;
                modeImg.Source = new BitmapImage(new Uri(@"/Resources/White_mode.png", UriKind.RelativeOrAbsolute));
                bColor.Background = Brushes.White;
                mode = false;
            }
            
        }
    }
}

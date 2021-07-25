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
using Newtonsoft.Json;
using System.IO;

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}//ChatHistory.json";
        List<string> history = new List<string>();
        private bool isConnected = false, mode = false, historyShowed = false;
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
                historyShowed = false;
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
        public void LoadDate()
        {
            bool FileExist = File.Exists(PATH);
            if(!FileExist)
            {
                File.Create(PATH).Dispose();
            }
            else
            {
                using (var reader = File.OpenText(PATH))
                {
                    string temp = reader.ReadToEnd();
                    history = JsonConvert.DeserializeObject<List<string>>(temp);
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bColor.Background = Brushes.White;
            try
            {
                LoadDate();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SaveDate()
        {
            List<string> temp = new List<string>();
            using (StreamWriter write = File.CreateText(PATH))
            {
                if(!historyShowed)
                {
                    foreach (var item in lbChat.Items)
                    {
                        temp.Add(item.ToString());
                    }
                }
                else
                {
                    foreach (var item in history)
                    {
                        temp.Add(item);
                    }
                    foreach (var item in lbChat.Items)
                    {
                        temp.Add(item.ToString());
                    }
                }
                history = new List<string>(temp);
                /*foreach (var item in lbChat.Items)
                {
                    history.Add(item.ToString());
                }*/
                string output = JsonConvert.SerializeObject(history);
                write.Write(output);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisConnectUser();
            SaveDate();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (client!=null)
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

        private void bHistory_Click(object sender, RoutedEventArgs e)
        {
            if (history == null)
                return;
            historyShowed = true;
            string[] arr = new string[lbChat.Items.Count];

            for(int i = 0; i < lbChat.Items.Count; ++i )
            {
                arr[i] = lbChat.Items.GetItemAt(i).ToString();
            }
            lbChat.Items.Clear();

            for(int i = 0; i < history.Count; ++i)
            {
                lbChat.Items.Add(history[i]);
            }

            foreach (var item in arr)
            {
                lbChat.Items.Add(item);
            }
  
        }
        private void SetView(string file, SolidColorBrush color1, SolidColorBrush color2,  bool mode_val)
        {
            mainForm.Background = color1;
            lbChat.Background = color2;
            tbUserName.Background = color2;
            tbMessage.Background = color2;
            bColor.Background = color1;

            modeImg.Source = new BitmapImage(new Uri(file, UriKind.RelativeOrAbsolute));
            mode = mode_val;
        }
        private void bColor_Click(object sender, RoutedEventArgs e)
        {
            if(!mode)
            {
                SetView("/Resources/Black_mode.png", Brushes.Black, Brushes.Gray, true);
            }
            else
            {
                SetView("/Resources/White_mode.png", Brushes.White, Brushes.White, false);
            }
        }
    }
}

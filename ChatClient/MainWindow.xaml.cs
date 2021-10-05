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
    public partial class MainWindow : Window, IServiceChatCallback
    {
        private bool isConnect = false;
        private ServiceChatClient client;
        private int Id;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            
        }
        void ConnectUser()
        {
            if (!isConnect)
            {
                client = new ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                Id = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                bConDiscon.Content = "Disconnect";
                isConnect = true;
            }
        }
        void DisconnectUser()
        {
            if (isConnect)
            {
                client.Disconnect(Id);
                client = null;
                tbUserName.IsEnabled = true;
                bConDiscon.Content = "Connect";
                isConnect = false;
            }
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (isConnect)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MsgCallback(string msg)
        {
            lbChat.Items.Add(msg);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void TbMessageKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client != null)
                {
                    client.SendMsg(tbMessage.Text, Id);
                    tbMessage.Text = string.Empty;
                }
            }
        }
    }
}

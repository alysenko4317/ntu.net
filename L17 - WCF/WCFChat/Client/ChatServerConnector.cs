using Client.ChatServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public class ChatServerConnector : IChatServiceCallback
    {
        private static ChatServerConnector _instance = null;
        private ChatServiceClient _client;   // тип ChatServiceClient створено автоматично
        private int _userId;

        private ChatServerConnector() {
            _client = new ChatServiceClient(new System.ServiceModel.InstanceContext(this));
        }

        static ChatServerConnector()
        {
            if (_instance == null)
                _instance = new ChatServerConnector();
        }

        public static ChatServerConnector GetInstance()
        {
            if (_instance == null)
                _instance = new ChatServerConnector();
            return _instance;
        }

        public static void Connect(string nickName)
        {
            ChatServerConnector instance = GetInstance();
            instance._userId = instance._client.Connect(nickName);
        }

        public static void SendMessageToServer(string msg)
        {
            GetInstance()._client.SendMessage(msg);
        }

        public void SendMessageToClient(string msg)
        {
            (Application.OpenForms["Form1"] as Form1).lstChatHistory.Items.Add(msg);
        }
    }
}

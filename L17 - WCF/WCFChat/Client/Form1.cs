using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lstChatHistory.Items.Clear();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ChatServerConnector.Connect(tbNickName.Text);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            ChatServerConnector.SendMessageToServer(tbMessage.Text);
            tbMessage.Text = "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;

namespace NoIPChat_Bulletin_board_client
{
    public class Plugin : IPlugin
    {
        public Client.Client? Client { get; set; }
        private Form1? form1 = null;
        public async Task Initialize()
        {
            if (Client != null)
            {
                if (form1 == null)
                {
                    form1 = new();
                    form1.MdiParent = Client.main;
                }
                ToolStripMenuItem menuitem = new()
                {
                    Text = "Bulletin board"
                };
                menuitem.Click += Bulletinboard_Click;
                Client.AddMainMenu(menuitem);
            }
        }
        private void Bulletinboard_Click(object? sender, EventArgs e)
        {
            if(form1 == null)
            {
                form1 = new();
                form1.MdiParent = Client?.main;      
            }
            form1.Show();
        }
        public async Task MessageReceived(Messages.Message message)
        {
            if (message.Extra != null && message.Extra.TryGetValue("BBS", out byte[]? bbs) && bbs != null)
            {
                MessageFormat.Message msg = await Messages.Processing.DeserializeGeneric<MessageFormat.Message>(bbs);
                if (msg.Title != null)
                {
                    if(form1 == null)
                    {
                        form1 = new();
                        form1.MdiParent = Client?.main;
                    }
                    form1.AddMessage(msg);
                }
            }
        }
    }
}

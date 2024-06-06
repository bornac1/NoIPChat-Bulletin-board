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
        private string logfile = "Bulletin.log";
        public async Task Initialize()
        {
            if (Client != null)
            {
                form1 ??= new(this)
                    {
                        MdiParent = Client.main
                    };
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
            form1 ??= new(this)
                {
                    MdiParent = Client?.main
                };
            form1.Show();
        }
        public async Task MessageReceived(Messages.Message message)
        {
            try
            {
                if (message.Extra != null && message.Extra.TryGetValue("BBS", out byte[]? bbs) && bbs != null)
                {
                    MessageFormat.Message msg = await Messages.Processing.DeserializeGeneric<MessageFormat.Message>(bbs);
                    if (msg.Title != null)
                    {
                        form1 ??= new(this)
                            {
                                MdiParent = Client?.main
                            };
                        form1.AddMessage(msg);
                    }
                }
            } catch (Exception ex)
            {
                await WriteLog(ex);
            }
        }
        public async Task WriteLog(Exception ex)
        {
            string log = DateTime.Now.ToString("d.M.yyyy. H:m:s") + " " + ex.ToString() + Environment.NewLine;
            try
            {
               await System.IO.File.AppendAllTextAsync(logfile, log);
            }
            catch (Exception ex1)
            {
                MessageBox.Show($"Plugin Bulletin board can't save log to file {logfile}.");
                MessageBox.Show(log);
                MessageBox.Show(ex1.ToString());
            }
        }
    }
}

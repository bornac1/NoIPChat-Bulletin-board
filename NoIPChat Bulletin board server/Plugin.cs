using System.Collections.Concurrent;
using Messages;
using Server_base;
namespace NoIPChat_Bulletin_board_server
{
    public class Plugin : IPlugin
    {
        public Server? Server { get; set; }
        private readonly ConcurrentList<MessageFormat.Message> messages = [];
        private readonly ConcurrentList<Client> notsend = [];
        private string logfile = "Bulletin.log";
        public void Initialize()
        {

        }
        private async Task SendToClient(Client client)
        {
            try
            {
                foreach (var message in messages)
                {
                    Dictionary<string, byte[]?> extra = [];
                    byte[] data = await Processing.SerializeGeneric<MessageFormat.Message>(message);
                    extra.Add("BBS", data);
                    await client.SendMessage(new Messages.Message() { Extra = extra });
                }
            } catch (Exception ex)
            {
                WriteLog(ex);
            }
        }
        private async Task Resend()
        {
            start:
            if (notsend.Count > 0)
            {
                foreach(Client client in notsend)
                {
                    if (client.Authenticated)
                    {
                        await SendToClient(client);
                        notsend.Remove(client);
                    }
                }
            }
            else
            {
                return;
            }
            //1 second
            await Task.Delay(1000);
            goto start;
        }
        public async Task ClientAccpetedAsync (Client client)
        {
            if (client.Authenticated)
            {
                await SendToClient(client);
            }
            else
            {
                notsend.Add(client);
                await Resend();
            }
        }
        public void WriteLog(Exception ex)
        {
            string log = DateTime.Now.ToString("d.M.yyyy. H:m:s") + " " + ex.ToString() + Environment.NewLine;
            try
            {
                System.IO.File.AppendAllText(logfile, log);
            }
            catch (Exception ex1)
            {
                Console.WriteLine($"Plugin Bulletin board can't save log to file {logfile}.");
                Console.WriteLine(log);
                Console.WriteLine(ex1.ToString());
            }
        }
    }
}

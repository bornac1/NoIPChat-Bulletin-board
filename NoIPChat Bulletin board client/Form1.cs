using Client;
using Messages;
using MessageFormat;
using System.ComponentModel;
namespace NoIPChat_Bulletin_board_client
{
    public partial class Form1 : Form
    {
        private readonly BindingSource messages;
        private int rowindex = -1;
        private Plugin plugin;
        public Form1(Plugin plugin)
        {
            this.plugin = plugin;
            messages = [];
            InitializeComponent();
            grid.DataSource = messages;
            //messages.ListChanged += Messages_ListChanged;
        }
        /// <summary>
        /// Constructor for debugging withou loading plugin.
        /// </summary>
        public Form1()
        {
            messages = [];
            InitializeComponent();
            grid.DataSource = messages;
        }
        public void AddMessage(MessageFormat.Message message)
        {
            messages.Add(message);
            messages.ResetBindings(false);
            grid.Refresh();
        }
        /*private void UpdateFiles()
        {
            messages.Clear();
            string[] filesname = Directory.GetFiles("Data");
            foreach (string file in filesname)
            {
                messages.Add(new MessageFormat.Message() { Name = Path.GetFileName(file), Path = Path.GetFullPath(file) });
            }
            messages.ResetBindings(false);
            grid.Refresh();
            rowindex = -1;
        }*/
        /*private void Messages_ListChanged(object? sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                UpdateMessages();
            }
        }*/
    }
}

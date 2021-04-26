using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace LanPlayGUI
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<ServerLists> lists;
        public MainWindow()
        {
            InitializeComponent();
            System.Net.WebClient wc = new System.Net.WebClient();
            // GitHubGistからリストを取得して反映させる
            var serverList = wc.DownloadString("https://gist.githubusercontent.com/tkgstrator/bfa35577ccd388751ab512a7ffd70a2c/raw/ServerList.json");
            lists = JsonConvert.DeserializeObject<List<ServerLists>>(serverList);

            foreach (ServerLists list in lists)
            {
                comboBox.Items.Add(list.Name);
            }

            // ファイルがなければダウンロードする
            if (!File.Exists("lanplay-x64.exe"))
            {
                // ディスコードから最新のものをダウンロード
                wc.DownloadFile("https://cdn.discordapp.com/attachments/720612694667034646/735934764409946152/lanplay-x64.exe", "lanplay-x64.exe");
                wc.Dispose();
            }
        }

        public class ServerLists
        {
            public string Host { get; set; }
            public string Name { get; set; }
            public string Location { get; set; }

        }

        private void connect(object sender, RoutedEventArgs e)
        {
            try
            {
                string host = lists[comboBox.SelectedIndex].Host;
                Process.Start("lanplay-x64.exe", string.Format(@"--fake-internet --pmtu 1000 --relay-server-addr {0}:11451", host));
            }
            catch
            {
            }
        }
    }
}

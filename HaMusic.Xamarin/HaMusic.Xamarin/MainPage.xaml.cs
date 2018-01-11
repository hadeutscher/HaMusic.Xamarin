using HaMusic.Xamarin.Droid;
using HaMusicLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HaMusic.Xamarin
{
	public partial class MainPage : ContentPage
	{

        ConnectionManager connection;
        Data data;

		public MainPage()
		{
			InitializeComponent();
            data = new Data();
            connection = new ConnectionManager();
            BindingContext = data;
		}

        private async void skipButton_Clicked(object sender, EventArgs e)
        {
            await connection.AttemptNetworkFunctionWithReconnect(async delegate
            {
                await HaProtoImpl.SendAsync(connection.currentConnectionStream, HaProtoImpl.Opcode.SKIP, new HaProtoImpl.SKIP());
                Utils.ShowMessage("Skipped");
            });
        }


        private async Task WaitForDatabaseUpdate()
        {
            await HaProtoImpl.SendAsync(connection.currentConnectionStream, HaProtoImpl.Opcode.GETDB, new HaProtoImpl.GETDB());
            while (true)
            {
                var result = await HaProtoImpl.ReceiveAsync(connection.currentConnectionStream);
                var opcode = result.Item1;
                var buffer = result.Item2;
                if (opcode != HaProtoImpl.Opcode.SETDB)
                    continue;
                var packet = HaProtoImpl.SETDB.Parse(buffer);
                data.ServerDataSource = packet.dataSource;
                data.MainPlaylist = data.ServerDataSource.Playlists.First();
                Utils.ShowMessage("Reconnected");
                break;
            }
        }


        private async void reconnectButton_Clicked(object sender, EventArgs e)
        {
            await connection.AttemptNetworkFunctionWithReconnect(async delegate
            {
                await WaitForDatabaseUpdate();
            });
        }
    }
}

using HaMusic.Xamarin.Droid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HaMusic.Xamarin
{
    public class ConnectionManager
    {
        public TcpClient currentConnection;
        public NetworkStream currentConnectionStream;

        private const string SERVER_ADDRESS = "192.168.0.101";
        //private const string SERVER_ADDRESS = "192.168.0.100";
        private const int SERVER_PORT = 5151;

        public async Task Reconnect()
        {
            if (currentConnection != null)
            {
                try
                {
                    currentConnection.Close();
                }
                catch { }
            }
            currentConnection = new TcpClient();
            await currentConnection.ConnectAsync(SERVER_ADDRESS, SERVER_PORT);
            currentConnectionStream = currentConnection.GetStream();
        }

        public async Task AttemptNetworkFunctionWithReconnect(Func<Task> action)
        {
            if (currentConnection == null || currentConnectionStream == null || !currentConnection.Connected)
            {
                await Reconnect();
            }
            int tries = 0;
            while (tries < 3)
            {
                try
                {
                    await action.Invoke();
                    return;
                }
                catch (IOException e)
                {
                    await Reconnect();
                    tries++;
                }
            }
            Utils.ShowMessage("Could not reconnect");
        }

    }
}

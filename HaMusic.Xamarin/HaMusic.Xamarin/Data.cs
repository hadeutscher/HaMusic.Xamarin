using HaMusicLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace HaMusic.Xamarin
{
    public class Data : PropertyNotifierBase
    {
        ServerDataSource _serverDataSource = null;
        public ServerDataSource ServerDataSource { get => _serverDataSource; set => SetField(ref _serverDataSource, value); }

        Playlist _mainPlaylist = null;
        public Playlist MainPlaylist { get => _mainPlaylist; set => SetField(ref _mainPlaylist, value); }
    }
}

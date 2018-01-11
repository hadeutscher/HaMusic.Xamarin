using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using HaMusicLib;
using System.Collections.Generic;

namespace HaMusic.Xamarin.Droid
{
    [Activity(Label = "HaMusic", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataMimeType = "text/plain")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            if (String.IsNullOrEmpty(Intent.GetStringExtra(Intent.ExtraText)))
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                base.OnCreate(bundle);

                global::Xamarin.Forms.Forms.Init(this, bundle);
                LoadApplication(new App());
            }
            else
            {
                base.OnCreate(bundle);

                string data = Intent.GetStringExtra(Intent.ExtraText);
                data = Utils.GetYoutubeIdentifier(data);

                var connection = new ConnectionManager();
                await connection.AttemptNetworkFunctionWithReconnect(async delegate
                {
                    await HaProtoImpl.SendAsync(connection.currentConnectionStream, HaProtoImpl.Opcode.ADD, new HaProtoImpl.ADD() { uid = -1, after = HaProtoImpl.ADD.LOCATION_LAST, paths = new List<string>() { data }, special = "youtube" });
                    Utils.ShowMessage("Adding to playlist...");
                });

                global::Xamarin.Forms.Forms.Init(this, bundle);
                LoadApplication(new App());
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HaMusic.Xamarin.Droid
{
    public static class Utils
    {
        public static void ShowMessage(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }

        public static string GetYoutubeIdentifier(string x)
        {
            if (x.Contains("youtu.be"))
            {
                return x.Substring(x.LastIndexOf("/") + "/".Length);
            }
            x = x.Substring(x.IndexOf("v=") + "v=".Length);
            int end = x.IndexOf("&");
            if (end >= 0)
            {
                x = x.Substring(0, end);
            }
            return x;
        }
    }
}
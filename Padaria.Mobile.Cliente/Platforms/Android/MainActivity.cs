using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Padaria.Mobile.Cliente;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
     protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
        {
            RequestedOrientation = ScreenOrientation.Landscape;
        }
        if (DeviceInfo.Idiom == DeviceIdiom.Phone)
        {
            RequestedOrientation = ScreenOrientation.Portrait;
        }
    }
}

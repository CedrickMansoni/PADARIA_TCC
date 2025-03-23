namespace Padaria.Mobile.View.FuncionarioViews.Views;

public partial class Start_Page : ContentPage
{
	public Start_Page()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
#if ANDROID
		if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
		{
			Platform.CurrentActivity!.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
		}
		else
		{
			Platform.CurrentActivity!.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
		}

#elif IOS
		if(DeviceInfo.Idiom == DeviceIdiom.Tablet)
		{
			UIKit.UIDevice.CurrentDevice.SetValueForKey(
			new Foundation.NSNumber((int)UIKit.UIInterfaceOrientation.Landscape), 
			new Foundation.NSString("orientation"));
		}else
		{
			UIKit.UIDevice.CurrentDevice.SetValueForKey(
			new Foundation.NSNumber((int)UIKit.UIInterfaceOrientation.Portrait), 
			new Foundation.NSString("orientation"));
		}
#endif
	}
}
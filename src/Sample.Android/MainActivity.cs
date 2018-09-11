using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Sample.Droid
{
	[Activity(
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
		WindowSoftInputMode = SoftInput.AdjustNothing | SoftInput.StateHidden
	)]
	public class MainActivity : Windows.UI.Xaml.ApplicationActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_main);
		}
	}
}

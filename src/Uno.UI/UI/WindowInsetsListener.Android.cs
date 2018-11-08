#if __ANDROID_28__
using Android.Views;

namespace Uno.UI
{
	public class WindowInsetsListener : Java.Lang.Object, View.IOnApplyWindowInsetsListener
	{
		public WindowInsets OnApplyWindowInsets(View v, WindowInsets insets)
		{
			var consumedInsets = insets.ConsumeSystemWindowInsets();

			Windows.UI.Xaml.Window.Current.LocalWindowInsets = consumedInsets;
			Windows.UI.Xaml.Window.Current.RaiseNativeSizeChanged();

			return consumedInsets;
		}
	}
}
#endif

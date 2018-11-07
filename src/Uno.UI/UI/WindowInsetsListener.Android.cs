#if __ANDROID_28__
using System;
using System.Collections.Generic;
using System.Text;
using Android.Views;

namespace Uno.UI
{
	public class WindowInsetsListener : Java.Lang.Object, View.IOnApplyWindowInsetsListener
	{
		public WindowInsets OnApplyWindowInsets(View v, WindowInsets insets)
		{
			Windows.UI.Xaml.Window.Current.LocalWindowInsets = insets;

			return insets;
		}
	}
}
#endif

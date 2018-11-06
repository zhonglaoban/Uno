
using Android.App;
using Windows.UI.Xaml;
using Fragment = Android.Support.V4.App.Fragment;

namespace Uno.UI.Controls
{
	public partial class BindableFragment : Fragment, DependencyObject
	{
		public BindableFragment(Activity owner, global::Android.Util.IAttributeSet attrs)
		{
			InitializeBinder();
		}
	}
}

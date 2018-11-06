using Android.Support.V4.App;

namespace Uno.UI.Extensions
{
	public static class FragmentManagerExtensions
	{
		public static BaseFragment GetCurrentFragment(this FragmentManager fragmentManager)
		{
			if (fragmentManager.BackStackEntryCount > 0)
			{
				var entry = fragmentManager.GetBackStackEntryAt(fragmentManager.BackStackEntryCount - 1);

				var fragment = fragmentManager.FindFragmentByTag(entry.Name) as BaseFragment;

				return fragment;
			}

			return null;
		}
	}
}

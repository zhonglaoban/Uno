#if XAMARIN_ANDROID
using System;
using Android.App;
using Android.Util;
using Android.Views;
using Uno.UI;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace Windows.UI.Xaml
{
	public sealed partial class Window
	{
		private static Window _current;
		private Grid _main;
		private Border _rootBorder;
		private Border _fullWindow;
		private UIElement _content;

		public Window()
		{
			Dispatcher = CoreDispatcher.Main;
			CoreWindow = new CoreWindow();
		}

		public WindowInsets LocalWindowInsets { get; set; }

		public Thickness WindowInsetsThickness
		{
			get
			{
				return LocalWindowInsets != null
				? new Thickness(
					ViewHelper.PhysicalToLogicalPixels(LocalWindowInsets.SystemWindowInsetLeft),
					ViewHelper.PhysicalToLogicalPixels(LocalWindowInsets.SystemWindowInsetTop),
					ViewHelper.PhysicalToLogicalPixels(LocalWindowInsets.SystemWindowInsetRight),
					ViewHelper.PhysicalToLogicalPixels(LocalWindowInsets.SystemWindowInsetBottom)
				)
				:Thickness.Empty;
			}
		}

		internal int SystemUiVisibility { get; set; }

		private bool IsNavigationBarVisible => (SystemUiVisibility & (int)SystemUiFlags.HideNavigation) == 0;

		private void InternalSetContent(UIElement value)
		{
			if (_main == null)
			{
				_rootBorder = new Border();
				_fullWindow = new Border()
				{
					VerticalAlignment = VerticalAlignment.Stretch,
					HorizontalAlignment = HorizontalAlignment.Stretch,
					Visibility = Visibility.Collapsed
				};

				_main = new Grid()
				{
					Children =
					{
						_rootBorder,
						_fullWindow
					}
				};


				ApplicationActivity.Instance?.SetContentView(_main);
			}

			_rootBorder.Child = _content = value;

#if __ANDROID_28__
			_rootBorder.Child.SetOnApplyWindowInsetsListener(new WindowInsetsListener());
#endif
		}

		private UIElement InternalGetContent()
		{
			return _content;
		}

		internal UIElement MainContent => _main;

		private static Window InternalGetCurrentWindow()
		{
			if (_current == null)
			{
				_current = new Window();
			}

			return _current;
		}

		internal void RaiseNativeSizeChanged()
		{
			var display = (ContextHelper.Current as Activity)?.WindowManager?.DefaultDisplay;
			var fullScreenMetrics = new DisplayMetrics();

			// To get the real size of the screen, we should use GetRealMetrics
			// GetMetrics or Resources.DisplayMetrics return the usable metrics, ignoring the bottom rounded space on device like LG G7 ThinQ for example
			display?.GetRealMetrics(outMetrics: fullScreenMetrics);

			var newBounds = ViewHelper.PhysicalToLogicalPixels(new Rect(0, 0, fullScreenMetrics.WidthPixels, fullScreenMetrics.HeightPixels));

			var statusBarHeight = GetLogicalStatusBarHeight();
			var navigationBarHeight = GetLogicalNavigationBarHeight();

			var leftPadding = WindowInsetsThickness.Left;
			var topPadding = Math.Max(statusBarHeight, WindowInsetsThickness.Top);
			var rightPadding = WindowInsetsThickness.Right;
			var bottomPadding = Math.Max(navigationBarHeight, WindowInsetsThickness.Bottom);

			var newVisibleBounds = new Rect(
				x: newBounds.X + leftPadding,
				y: newBounds.Y + topPadding,
				width: newBounds.Width - leftPadding - rightPadding,
				height: newBounds.Height - topPadding - bottomPadding
			);

			ApplicationView.GetForCurrentView()?.SetVisibleBounds(newVisibleBounds);

			if (Bounds != newBounds)
			{
				Bounds = newBounds;

				RaiseSizeChanged(
					new WindowSizeChangedEventArgs(
						new Windows.Foundation.Size(Bounds.Width, Bounds.Height)
					)
				);
			}
		}

		private double GetLogicalStatusBarHeight()
		{
			var logicalStatusBarHeight = 0d;
			var activity = ContextHelper.Current as Activity;
			var decorView = activity.Window.DecorView;
			var isStatusBarVisible = ((int)decorView.SystemUiVisibility & (int)SystemUiFlags.Fullscreen) == 0;

			var isStatusBarTranslucent =
				((int)activity.Window.Attributes.Flags & (int)WindowManagerFlags.TranslucentStatus) != 0
				|| ((int)activity.Window.Attributes.Flags & (int)WindowManagerFlags.LayoutNoLimits) != 0;

			if (isStatusBarVisible && isStatusBarTranslucent)
			{
				int resourceId = Android.Content.Res.Resources.System.GetIdentifier("status_bar_height", "dimen", "android");
				if (resourceId > 0)
				{
					logicalStatusBarHeight = ViewHelper.PhysicalToLogicalPixels(Android.Content.Res.Resources.System.GetDimensionPixelSize(resourceId));
				}
			}

			return logicalStatusBarHeight;
		}

		private double GetLogicalNavigationBarHeight()
		{
			var logicalNavigationBarHeight = 0d;
			var metrics = new DisplayMetrics();
			var defaultDisplay = (ContextHelper.Current as Activity)?.WindowManager?.DefaultDisplay;

			var activity = ContextHelper.Current as Activity;
			var decorView = activity.Window.DecorView;

			var isNavigationBarTranslucent =
				((int)activity.Window.Attributes.Flags & (int)WindowManagerFlags.TranslucentNavigation) != 0
				|| ((int)activity.Window.Attributes.Flags & (int)WindowManagerFlags.LayoutNoLimits) != 0;

			if (defaultDisplay != null && IsNavigationBarVisible && isNavigationBarTranslucent)
			{
				defaultDisplay.GetMetrics(metrics);
				var usableHeight = metrics.HeightPixels;

				defaultDisplay.GetRealMetrics(metrics);
				var realHeight = metrics.HeightPixels;

				logicalNavigationBarHeight = realHeight > usableHeight
					? ViewHelper.PhysicalToLogicalPixels(realHeight - usableHeight)
					: 0;
			}

			return logicalNavigationBarHeight;
		}

		internal void DisplayFullscreen(UIElement element)
		{
			if (element == null)
			{
				_fullWindow.Child = null;
				_rootBorder.Visibility = Visibility.Visible;
				_fullWindow.Visibility = Visibility.Collapsed;
			}
			else
			{
				_fullWindow.Visibility = Visibility.Visible;
				_rootBorder.Visibility = Visibility.Collapsed;
				_fullWindow.Child = element;
			}
		}
	}
}
#endif

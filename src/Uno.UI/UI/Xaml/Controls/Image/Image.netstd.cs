using System;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Uno.Diagnostics.Eventing;
using Uno.Extensions;
using Uno.Foundation;
using Uno.Logging;
using Windows.UI.Xaml.Media.Imaging;

namespace Windows.UI.Xaml.Controls
{
	partial class Image : FrameworkElement
	{
		public Image() : base("img")
		{
			ImageOpened += (snd, evt) => InvalidateMeasure();
		}

		public event RoutedEventHandler ImageOpened
		{
			add => RegisterEventHandler("load", value);
			remove => UnregisterEventHandler("load", value);
		}

		public event RoutedEventHandler ImageFailed
		{
			add => RegisterEventHandler("error", value);
			remove => UnregisterEventHandler("error", value);
		}

		#region Source DependencyProperty

		public ImageSource Source
		{
			get => (ImageSource)GetValue(SourceProperty);
			set => SetValue(SourceProperty, value);
		}

		// Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SourceProperty =
			DependencyProperty.Register("Source", typeof(ImageSource), typeof(Image), new PropertyMetadata(null, (s, e) => ((Image)s)?.OnSourceChanged(e)));


		private void OnSourceChanged(DependencyPropertyChangedEventArgs e)
		{
			UpdateHitTest();

			var source = e.NewValue as ImageSource;
			var url = source?.WebUri;

			if (url != null)
			{
				if(url.IsAbsoluteUri)
				{
					SetAttribute("src", url.AbsoluteUri);
				}
				else
				{
					SetAttribute("src", url.OriginalString);
				}
			}
		}

		#endregion

		public Stretch Stretch
		{
			get { return (Stretch)this.GetValue(StretchProperty); }
			set { this.SetValue(StretchProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty StretchProperty =
			DependencyProperty.Register("Stretch", typeof(Stretch), typeof(Image), new PropertyMetadata(Media.Stretch.Uniform, (s, e) =>
				((Image)s).OnStretchChanged((Stretch)e.NewValue, (Stretch)e.OldValue)));

		private void OnStretchChanged(Stretch newValue, Stretch oldValue)
		{
			InvalidateArrange();
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			return base.ArrangeOverride(finalSize);
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			var measuredSize = MeasureView(availableSize);
			Size ret;

			switch (Stretch)
			{
				default:
				case Stretch.None:
					return measuredSize;

				case Stretch.Fill:
					if(
						double.IsInfinity(availableSize.Width)
						|| double.IsInfinity(availableSize.Height)
					)
					{
						ret = measuredSize;
					}
					else
					{
						ret = availableSize;
					}
					break;

				case Stretch.Uniform:
					ret = measuredSize;
					break;

				case Stretch.UniformToFill:
					if (
						double.IsInfinity(availableSize.Width)
						|| double.IsInfinity(availableSize.Height)
					)
					{
						ret = measuredSize;
					}
					else
					{
						if(measuredSize.Width > measuredSize.Height)
						{
							var ratio = availableSize.Height / measuredSize.Height;

							ret = new Size(measuredSize.Width * ratio, availableSize.Height);
						}
						else
						{
							var ratio = availableSize.Width / measuredSize.Width;

							ret = new Size(availableSize.Width, measuredSize.Height * ratio);
						}
					}
					break;
			}

			Console.WriteLine($"Image({Source?.WebUri}).MeasureOverride({availableSize}) = {measuredSize} -> {ret}");

			return ret;
		}

		internal override bool IsViewHit()
		{
			return Source != null || base.IsViewHit();
		}
	}
}

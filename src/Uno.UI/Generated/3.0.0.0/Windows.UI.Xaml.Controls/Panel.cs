#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
namespace Windows.UI.Xaml.Controls
{
	#if false || false || false || false || false
	[global::Uno.NotImplemented]
	#endif
	public  partial class Panel : global::Windows.UI.Xaml.FrameworkElement
	{
		// Skipping already declared property ChildrenTransitions
		#if false || false || false || false || __MACOS__
		[global::Uno.NotImplemented]
		public  global::Windows.UI.Xaml.Media.Brush Background
		{
			get
			{
				return (global::Windows.UI.Xaml.Media.Brush)this.GetValue(BackgroundProperty);
			}
			set
			{
				this.SetValue(BackgroundProperty, value);
			}
		}
		#endif
		// Skipping already declared property Children
		// Skipping already declared property IsItemsHost
		#if false || false || false || false || __MACOS__
		[global::Uno.NotImplemented]
		public static global::Windows.UI.Xaml.DependencyProperty BackgroundProperty { get; } = 
		Windows.UI.Xaml.DependencyProperty.Register(
			"Background", typeof(global::Windows.UI.Xaml.Media.Brush), 
			typeof(global::Windows.UI.Xaml.Controls.Panel), 
			new FrameworkPropertyMetadata(default(global::Windows.UI.Xaml.Media.Brush)));
		#endif
		// Skipping already declared property ChildrenTransitionsProperty
		// Skipping already declared property IsItemsHostProperty
		// Skipping already declared method Windows.UI.Xaml.Controls.Panel.Panel()
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.Panel()
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.Children.get
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.Background.get
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.Background.set
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.IsItemsHost.get
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.ChildrenTransitions.get
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.ChildrenTransitions.set
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.BackgroundProperty.get
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.IsItemsHostProperty.get
		// Forced skipping of method Windows.UI.Xaml.Controls.Panel.ChildrenTransitionsProperty.get
	}
}

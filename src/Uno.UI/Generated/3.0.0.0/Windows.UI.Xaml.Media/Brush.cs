#pragma warning disable 108 // new keyword hiding
#pragma warning disable 114 // new keyword hiding
namespace Windows.UI.Xaml.Media
{
	#if false || false || false || false || false
	[global::Uno.NotImplemented]
	#endif
	public  partial class Brush : global::Windows.UI.Xaml.DependencyObject
	{
		#if __ANDROID__ || __IOS__ || NET46 || __WASM__ || __MACOS__
		[global::Uno.NotImplemented]
		public  global::Windows.UI.Xaml.Media.Transform Transform
		{
			get
			{
				return (global::Windows.UI.Xaml.Media.Transform)this.GetValue(TransformProperty);
			}
			set
			{
				this.SetValue(TransformProperty, value);
			}
		}
		#endif
		// Skipping already declared property RelativeTransform
		// Skipping already declared property Opacity
		// Skipping already declared property OpacityProperty
		// Skipping already declared property RelativeTransformProperty
		#if __ANDROID__ || __IOS__ || NET46 || __WASM__ || __MACOS__
		[global::Uno.NotImplemented]
		public static global::Windows.UI.Xaml.DependencyProperty TransformProperty { get; } = 
		Windows.UI.Xaml.DependencyProperty.Register(
			"Transform", typeof(global::Windows.UI.Xaml.Media.Transform), 
			typeof(global::Windows.UI.Xaml.Media.Brush), 
			new FrameworkPropertyMetadata(default(global::Windows.UI.Xaml.Media.Transform)));
		#endif
		// Skipping already declared method Windows.UI.Xaml.Media.Brush.Brush()
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.Brush()
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.Opacity.get
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.Opacity.set
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.Transform.get
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.Transform.set
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.RelativeTransform.get
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.RelativeTransform.set
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.OpacityProperty.get
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.TransformProperty.get
		// Forced skipping of method Windows.UI.Xaml.Media.Brush.RelativeTransformProperty.get
	}
}

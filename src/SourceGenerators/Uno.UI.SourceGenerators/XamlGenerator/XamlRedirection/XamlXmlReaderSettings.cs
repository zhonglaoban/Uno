#if !NETSTANDARD2_0
extern alias __ms;
#endif
extern alias __uno;

namespace Uno.UI.SourceGenerators.XamlGenerator.XamlRedirection
{
	internal class XamlXmlReaderSettings
	{
		public XamlXmlReaderSettings()
		{
			if (XamlConfig.IsUnoXaml)
			{
				UnoInner = new __uno::Uno.Xaml.XamlXmlReaderSettings();
			}
#if !NETSTANDARD2_0
			else
			{
				MsInner = new __ms::System.Xaml.XamlXmlReaderSettings();
			}
#endif
		}

		public bool ProvideLineInfo
		{
#if !NETSTANDARD2_0
			get => XamlConfig.IsUnoXaml ? UnoInner.ProvideLineInfo : MsInner.ProvideLineInfo;
			set
			{
				if (XamlConfig.IsUnoXaml)
				{
					UnoInner.ProvideLineInfo = value;
				}
				else
				{
					MsInner.ProvideLineInfo = value;
				}
			}
#else
			get => UnoInner.ProvideLineInfo;
			set => UnoInner.ProvideLineInfo = value;
#endif
		}

		public __uno::Uno.Xaml.XamlXmlReaderSettings UnoInner { get; internal set; }
#if !NETSTANDARD2_0
		public __ms::System.Xaml.XamlXmlReaderSettings MsInner { get; internal set; }
#endif
	}
}

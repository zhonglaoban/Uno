#if !NETSTANDARD2_0
extern alias __ms;
#endif
extern alias __uno;

namespace Uno.UI.SourceGenerators.XamlGenerator.XamlRedirection
{
	internal class XamlSchemaContext
	{
		public XamlSchemaContext()
		{
			if (XamlConfig.IsUnoXaml)
			{
				UnoInner = new __uno::Uno.Xaml.XamlSchemaContext();
			}
#if !NETSTANDARD2_0
			else
			{
				MsInner = new __ms::System.Xaml.XamlSchemaContext();
			}
#endif
		}

		public XamlSchemaContext(System.Collections.Generic.IEnumerable<System.Reflection.Assembly> enumerable)
		{
			if (XamlConfig.IsUnoXaml)
			{
				UnoInner = new __uno::Uno.Xaml.XamlSchemaContext(enumerable);
			}
#if !NETSTANDARD2_0
			else
			{
				MsInner = new __ms::System.Xaml.XamlSchemaContext(enumerable);
			}
#endif
		}
#if !NETSTANDARD2_0
		public __ms::System.Xaml.XamlSchemaContext MsInner { get; }
#endif
		public __uno::Uno.Xaml.XamlSchemaContext UnoInner { get; }
	}
}

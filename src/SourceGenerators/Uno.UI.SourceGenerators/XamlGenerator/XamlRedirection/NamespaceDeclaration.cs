#if !NETSTANDARD2_0
extern alias __ms;
#endif
extern alias __uno;

namespace Uno.UI.SourceGenerators.XamlGenerator.XamlRedirection
{
	public class NamespaceDeclaration
	{
		private __uno::Uno.Xaml.NamespaceDeclaration _unoNs;

#if !NETSTANDARD2_0
		private __ms::System.Xaml.NamespaceDeclaration _msNs;

		public NamespaceDeclaration(__ms::System.Xaml.NamespaceDeclaration ns)
			=> _msNs = ns;
#endif

		public NamespaceDeclaration(__uno::Uno.Xaml.NamespaceDeclaration ns)
			=> _unoNs = ns;

#if !NETSTANDARD2_0
		public string Namespace => XamlConfig.IsUnoXaml ? _unoNs.Namespace : _msNs.Namespace;
		public string Prefix => XamlConfig.IsUnoXaml ? _unoNs.Prefix : _msNs.Prefix;
#else
		public string Namespace => _unoNs.Namespace;
		public string Prefix => _unoNs.Prefix;
#endif
	}
}

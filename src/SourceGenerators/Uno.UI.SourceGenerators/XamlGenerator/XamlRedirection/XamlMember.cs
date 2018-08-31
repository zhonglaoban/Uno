#if !NETSTANDARD2_0
extern alias __ms;
#endif
extern alias __uno;

using Uno.Extensions;

namespace Uno.UI.SourceGenerators.XamlGenerator.XamlRedirection
{
	internal class XamlMember
	{
		private string _name;
		private XamlType _declaringType;
		private bool _isAttachable;

		private __uno::Uno.Xaml.XamlMember _unoMember;
		public static XamlMember FromMember(__uno::Uno.Xaml.XamlMember member) => member != null ? new XamlMember(member) : null;
#if !NETSTANDARD2_0
		private __ms::System.Xaml.XamlMember _msMember;
		public static XamlMember FromMember(__ms::System.Xaml.XamlMember member) => member != null ? new XamlMember(member) : null;
#endif

		public static XamlMember WithDeclaringType(XamlMember member, XamlType declaringType)
		{
#if !NETSTANDARD2_0
			var newMember = XamlConfig.IsUnoXaml ? FromMember(member._unoMember) : FromMember(member._msMember);
#else
			var newMember = FromMember(member._unoMember);
#endif
			newMember._declaringType = declaringType;
			return newMember;
		}

		private XamlMember(__uno::Uno.Xaml.XamlMember member) => this._unoMember = member;
#if !NETSTANDARD2_0
		private XamlMember(__ms::System.Xaml.XamlMember member) => this._msMember = member;
#endif

		public XamlMember(string name, XamlType declaringType, bool isAttachable)
		{
			this._name = name;
			this._declaringType = declaringType;
			this._isAttachable = isAttachable;
		}

#if !NETSTANDARD2_0
		public string Name 
			=> _name.HasValue() ? _name : XamlConfig.IsUnoXaml ? _unoMember.Name : _msMember.Name;

		public XamlType DeclaringType 
			=> _declaringType != null ? _declaringType : XamlConfig.IsUnoXaml ? XamlType.FromType(_unoMember.DeclaringType) : XamlType.FromType(_msMember.DeclaringType);

		public XamlType Type
			=> XamlConfig.IsUnoXaml ? XamlType.FromType(_unoMember.Type) : XamlType.FromType(_msMember.Type);

		public string PreferredXamlNamespace 
			=> XamlConfig.IsUnoXaml ? _unoMember.PreferredXamlNamespace : _msMember.PreferredXamlNamespace;

		public bool IsAttachable 
			=> _declaringType != null ? _isAttachable : XamlConfig.IsUnoXaml ? _unoMember.IsAttachable : _msMember.IsAttachable;

		public override string ToString() => XamlConfig.IsUnoXaml ? _unoMember.ToString() : _msMember.ToString();
#else
		public string Name => _name.HasValue() ? _name : _unoMember.Name;

		public XamlType DeclaringType => _declaringType != null ? _declaringType : XamlType.FromType(_unoMember.DeclaringType);

		public XamlType Type => XamlType.FromType(_unoMember.Type);

		public string PreferredXamlNamespace => _unoMember.PreferredXamlNamespace;

		public bool IsAttachable => _declaringType != null ? _isAttachable : _unoMember.IsAttachable;

		public override string ToString() => _unoMember.ToString();
#endif
	}
}

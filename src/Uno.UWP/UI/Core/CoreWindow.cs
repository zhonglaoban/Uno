using System;
using System.Runtime.InteropServices;
using Uno;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;

namespace Windows.UI.Core
{
	public partial class CoreWindow 
	{
		public global::Windows.UI.Core.CoreDispatcher Dispatcher
			=> CoreDispatcher.Main;

		[NotImplemented]
		public global::Windows.UI.Core.CoreVirtualKeyStates GetAsyncKeyState(global::Windows.System.VirtualKey virtualKey)
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "GetAsyncKeyState");

			return CoreVirtualKeyStates.None;
		}

		[NotImplemented]
		public global::Windows.UI.Core.CoreVirtualKeyStates GetKeyState(global::Windows.System.VirtualKey virtualKey)
		{
			global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "GetKeyState");

			return CoreVirtualKeyStates.None;
		}

		[NotImplemented]
		public global::Windows.Foundation.Point PointerPosition
		{
			get
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "get_PointerPosition");

				return new Point(0, 0);
			}
			set
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "set_PointerPosition");
			}
		}

		private CoreCursor _pointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);

		[NotImplemented]
		public global::Windows.UI.Core.CoreCursor PointerCursor
		{
			get
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "get_PointerCursor");

				return _pointerCursor;
			}
			set
			{
				global::Windows.Foundation.Metadata.ApiInformation.TryRaiseNotImplemented("Windows.UI.Core.CoreWindow", "set_PointerCursor");

				_pointerCursor = value;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;

namespace Windows.UI.Input
{
	public partial class PointerPoint
	{
		public Point Position { get; }

		internal PointerPoint(Point position)
		{
			Position = position;
		}

		[global::Uno.NotImplemented]
		public bool IsInContact => true;

		[global::Uno.NotImplemented]
		public global::Windows.UI.Input.PointerPointProperties Properties { get; } = new PointerPointProperties();

	}
}

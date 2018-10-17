using System;
using System.Collections.Generic;
using System.Text;
using AppKit;

namespace Windows.UI.Xaml.Controls
{
	public partial class ListView : ListViewBase, IListView
	{
		public override void ViewWillMoveToSuperview(NSView newsuper) => base.ViewWillMoveToSuperview(newsuper);
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ListViewItem;
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ListViewItem() { IsGeneratedContainer = true };
		}

		internal override ContentControl GetGroupHeaderContainer(object groupHeader)
		{
			return new ListViewHeaderItem();
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using Android.Support.V7.Widget;
using Android.Views;
using Uno.Extensions;
using Uno.UI;
using Windows.UI.Xaml.Controls.Primitives;

namespace Windows.UI.Xaml.Controls
{
	public partial class NativeListViewBase : UnoRecyclerView, ILayoutConstraints
	{
		/// <summary>
		/// Is the RecyclerView currently undergoing animated scrolling, either user-initiated or programmatic.
		/// </summary>
		private bool _isInAnimatedScroll;

		internal BufferViewCache ViewCache { get; }

		internal IEnumerable<SelectorItem> CachedItemViews => ViewCache.CachedItemViews;

		public override OverScrollMode OverScrollMode
		{
			get
			{
				// This duplicates the logic in Android.Views.View.overScrollBy(), which for some reason RecyclerView doesn't use, but
				// only checks getOverScrollMode(). This ensures edge effects aren't shown if content is too small to scroll.
				if (NativeLayout == null)
				{
					return base.OverScrollMode;
				}
				else if (NativeLayout.ScrollOrientation == Orientation.Vertical)
				{
					return ComputeVerticalScrollRange() > ComputeVerticalScrollExtent() ?
						base.OverScrollMode :
						OverScrollMode.Never;
				}
				else
				{
					return ComputeHorizontalScrollRange() > ComputeHorizontalScrollExtent() ?
						base.OverScrollMode :
						OverScrollMode.Never;
				}
			}
		}
		public NativeListViewBase() : base(ContextHelper.Current)
		{
			InitializeScrollbars();
			VerticalScrollBarEnabled = true;
			HorizontalScrollBarEnabled = true;

			ViewCache = new BufferViewCache(this);
			SetViewCacheExtension(ViewCache);

			InitializeSnapHelper();

			MotionEventSplittingEnabled = false;
		}

		partial void InitializeSnapHelper();

		private void InitializeScrollbars()
		{
			// Force scrollbars to initialize since we're not inflating from xml
			if (Android.OS.Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.Kitkat)
			{
				var styledAttributes = Context.Theme.ObtainStyledAttributes(Resource.Styleable.View);
				InitializeScrollbars(styledAttributes);
				styledAttributes.Recycle();
			}
			else
			{
				InitializeScrollbars(null);
			}
		}

		internal NativeListViewBaseAdapter CurrentAdapter
		{
			get { return GetAdapter() as NativeListViewBaseAdapter; }
			set { SetAdapter(value); }
		}
		internal VirtualizingPanelLayout NativeLayout
		{
			get { return GetLayoutManager() as VirtualizingPanelLayout; }
			set
			{
				SetLayoutManager(value);
				PropagatePadding();
			}
		}

		private Thickness _padding;
		public Thickness Padding
		{
			get
			{
				return _padding;
			}

			set
			{
				_padding = value;
				PropagatePadding();
			}
		}

		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return HorizontalScrollBarEnabled ? ScrollBarVisibility.Visible : ScrollBarVisibility.Hidden;
			}

			set
			{
				switch (value)
				{
					case ScrollBarVisibility.Disabled:
					case ScrollBarVisibility.Hidden:
						HorizontalScrollBarEnabled = false;
						break;
					case ScrollBarVisibility.Auto:
					case ScrollBarVisibility.Visible:
					default:
						HorizontalScrollBarEnabled = true;
						break;
				}
			}
		}

		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return VerticalScrollBarEnabled ? ScrollBarVisibility.Visible : ScrollBarVisibility.Hidden;
			}

			set
			{
				switch (value)
				{
					case ScrollBarVisibility.Disabled:
					case ScrollBarVisibility.Hidden:
						VerticalScrollBarEnabled = false;
						break;
					case ScrollBarVisibility.Auto:
					case ScrollBarVisibility.Visible:
					default:
						VerticalScrollBarEnabled = true;
						break;
				}
			}
		}

		public void ScrollIntoView(int displayPosition)
		{
			ScrollToPosition(displayPosition);
		}

		public void ScrollIntoView(int displayPosition, ScrollIntoViewAlignment alignment)
		{
			StopScroll();

			if (NativeLayout != null)
			{
				NativeLayout.ScrollToPosition(displayPosition, alignment);

				AwakenScrollBars();
			}
		}

		public override void ScrollTo(int x, int y)
		{
			ScrollBy(x - NativeLayout.HorizontalOffset, y - NativeLayout.VerticalOffset);
		}

		public void SmoothScrollTo(int x, int y)
		{
			SmoothScrollBy(x - NativeLayout.HorizontalOffset, y - NativeLayout.VerticalOffset);
		}

		public override void OnScrolled(int dx, int dy)
		{
			InvokeOnScroll();
		}

		private void InvokeOnScroll()
		{
			XamlParent?.ScrollViewer?.OnScrollInternal(
				ViewHelper.PhysicalToLogicalPixels(NativeLayout.HorizontalOffset),
				ViewHelper.PhysicalToLogicalPixels(NativeLayout.VerticalOffset),
				isIntermediate: _isInAnimatedScroll
				);
		}

		public override void OnScrollStateChanged(int state)
		{
			switch (state)
			{
				case ScrollStateIdle:
					_isInAnimatedScroll = false;
					InvokeOnScroll();
					break;
				case ScrollStateDragging:
				case ScrollStateSettling:
					_isInAnimatedScroll = true;
					break;
			}
		}

		// We override these two methods because the base implementation depends on ComputeScrollOfset, which is not reliable when, eg, items 
		// are inserted/removed out of view.
		public override bool CanScrollHorizontally(int direction)
		{
			return NativeLayout.CanCurrentlyScrollHorizontally(direction);
		}

		public override bool CanScrollVertically(int direction)
		{
			return NativeLayout.CanCurrentlyScrollVertically(direction);
		}

		protected override void AttachViewToParent(View child, int index, ViewGroup.LayoutParams layoutParams)
		{
			var vh = GetChildViewHolder(child);
			if (vh != null)
			{
				vh.IsDetached = false;
			}
			base.AttachViewToParent(child, index, layoutParams);
		}

		protected override void DetachViewFromParent(int index)
		{
			var view = GetChildAt(index);
			if (view != null)
			{
				var vh = GetChildViewHolder(view);
				if (vh != null)
				{
					vh.IsDetached = true;
				}
			}
			base.DetachViewFromParent(index);
		}

		protected override void RemoveDetachedView(View child, bool animate)
		{
			var vh = GetChildViewHolder(child);
			if (vh != null)
			{
				vh.IsDetached = false;
			}
#if DEBUG
			if (!vh.IsDetachedPrivate)
			{
				// Preempt the unmanaged exception 'Java.Lang.IllegalArgumentException: Called removeDetachedView with a view which is not flagged as tmp detached.' for easier debugging.
				throw new InvalidOperationException($"View {child} is not flagged tmp detached.");
			}
#endif
			base.RemoveDetachedView(child, animate);
		}

		partial void OnUnloadedPartial()
		{
			ViewCache?.OnUnloaded();
		}

		public void Refresh()
		{
			CurrentAdapter?.Refresh();

			var isScrollResetting = NativeLayout != null && NativeLayout.ContentOffset != 0;
			NativeLayout?.Refresh();

			if (isScrollResetting)
			{
				// Raise scroll events since offset has been reset to 0
				InvokeOnScroll();
			}
		}

		private void PropagatePadding()
		{
			var asVirtualizingPanelLayout = NativeLayout as VirtualizingPanelLayout;
			if (asVirtualizingPanelLayout != null)
			{
				asVirtualizingPanelLayout.Padding = this.Padding;
			}
		}

		internal new UnoViewHolder GetChildViewHolder(View view) => base.GetChildViewHolder(view) as UnoViewHolder;

		bool ILayoutConstraints.IsWidthConstrained(View requester)
		{
			if (requester != null && NativeLayout?.ScrollOrientation == Orientation.Horizontal)
			{
				//If scroll is horizontal, width change requires relayout of siblings
				return false;
			}

			//Otherwise use standard rules
			return this.IsWidthConstrainedSimple() ?? (base.Parent as ILayoutConstraints)?.IsWidthConstrained(this) ?? false;
		}

		bool ILayoutConstraints.IsHeightConstrained(View requester)
		{
			if (requester != null && NativeLayout?.ScrollOrientation == Orientation.Vertical)
			{
				//If scroll is vertical, height change requires relayout of siblings
				return false;
			}

			return this.IsHeightConstrainedSimple() ?? (base.Parent as ILayoutConstraints)?.IsHeightConstrained(this) ?? false;
		}
	}
}

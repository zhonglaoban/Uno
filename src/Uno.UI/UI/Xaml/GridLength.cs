﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Uno.Extensions;

namespace Windows.UI.Xaml
{
    public partial struct GridLength : IEquatable<GridLength>
    {
        public static GridLength Auto => GridLengthHelper.Auto;

        public GridUnitType GridUnitType { get; private set; }

        public bool IsAbsolute { get { return GridUnitType == Xaml.GridUnitType.Pixel; } }

        public bool IsAuto { get { return GridUnitType == Xaml.GridUnitType.Auto; } }

        public bool IsStar { get { return GridUnitType == Xaml.GridUnitType.Star; } }

        public double Value { get; private set; }

		public static implicit operator GridLength(string value)
			=> FromString(value);

		public GridLength(double pixels) : this(pixels, GridUnitType.Pixel)
        {
        }

        public GridLength(double value, GridUnitType gridUnitType)
		{
			Value = value;
			GridUnitType = gridUnitType;
		}

		public static GridLength FromString(string s)
		{
			var trimmed = s.Trim();

			if (trimmed == "*")
			{
				return new GridLength(1, GridUnitType.Star);
			}
			else if (trimmed.Equals("auto", StringComparison.OrdinalIgnoreCase))
			{
				return new GridLength(0, GridUnitType.Auto);
			}
			else if (trimmed.EndsWith("*"))
			{
				var stringValue = trimmed.Substring(0, trimmed.Length - 1);

				if (double.TryParse(stringValue, NumberStyles.Any & ~NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var value))
				{
					return new GridLength(value, GridUnitType.Star);
				}
				else
				{
					throw new InvalidOperationException($"The value [{trimmed}] is not a valid GridLength");
				}
			}
			else
			{
				if (double.TryParse(trimmed, NumberStyles.Any & ~NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var value))
				{
					return new GridLength(value, GridUnitType.Pixel);
				}
				else
				{
					throw new InvalidOperationException($"The value [{trimmed}] is not a valid GridLength");
				}
			}
		}

		public static GridLength[] ParseGridLength(string s)
		{
			var parts = s.Split(',');

			var result = new List<GridLength>(parts.Length);

			foreach (var part in parts)
			{
				if (string.IsNullOrEmpty(part))
				{
					result.Add(new GridLength(0, GridUnitType.Auto));
					continue;
				}

				result.Add(FromString(part));
			}

			return result.ToArray();
		}

		public bool Equals(GridLength other) => 
			other.Value == Value && other.GridUnitType == GridUnitType;
	}
}

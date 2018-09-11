using System;
using Windows.UI.Xaml;

namespace Sample.Wasm
{
	class Program
	{
		static void Main(string[] args)
		{
			Application.Start(_ => new App());
		}
	}
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Uno.UI;
using Uno.Extensions;
using Uno.UI.Extensions;
using Windows.UI.Xaml;
using Uno.UI.Web;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Windows.UI.Core;
using Uno.Logging;
using Uno.Foundation;

namespace Windows.UI.Xaml.Controls
{
	public partial class WebView
	{
		public WebView() : base("iframe")
		{
		}

		//This should be IAsyncOperation<string> instead of Task<string> but we use an extension method to enable the same signature in Win.
		//IAsyncOperation is not available in Xamarin.
		public async Task<string> InvokeScriptAsync(CancellationToken ct, string script, string[] arguments)
		{
			throw new NotSupportedException();
		}

		partial void GoBackPartial()
		{
		}

		partial void GoForwardPartial()
		{
		}

		partial void NavigatePartial(Uri uri)
		{
			WebAssemblyRuntime.InvokeJS("Uno.UI.WindowManager.current.setWebViewSource(\"" + HtmlId + "\", \"" + uri.OriginalString + "\");");
		}

		partial void NavigateToStringPartial(string text)
		{
			WebAssemblyRuntime.InvokeJS("Uno.UI.WindowManager.current.setWebViewHtmlString(\"" + HtmlId + "\", \"" + text + "\");");
		}

		partial void NavigateWithHttpRequestMessagePartial(HttpRequestMessage requestMessage)
		{
		}

		partial void StopPartial()
		{
		}
	}
}


using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Content;
using Android.App;
using PayPalXF;
using PayPalXF.Droid;

[assembly: ExportRenderer (typeof(PaymentPage), typeof(PaymentPageRenderer))]

namespace PayPalXF.Droid
{

	public class PaymentPageRenderer : PageRenderer
	{
		private Activity _activity;

		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged (e);


			if (e.OldElement == null) {
				_activity = Context as Activity;

				Intent intent = new Intent (_activity, typeof(PayPalActivity));

				_activity.StartActivityForResult (intent, 0);

			}
		}

	}
}
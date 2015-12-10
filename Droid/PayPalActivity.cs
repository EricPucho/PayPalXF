using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

using Newtonsoft.Json;

using Com.Paypal.Android.Sdk.Payments;
using Java.Math;

namespace PayPalXF.Droid
{
	[Activity (Label = "PayPalActivity")]			
	public class PayPalActivity : Activity
	{
		

		private const string CONFIG_ENVIRONMENT = PayPalConfiguration.EnvironmentNoNetwork;
		// note that these credentials will differ between live & sandbox environments.
		private const String CONFIG_CLIENT_ID = "ASpkpVUsuaE73F-3glCVNVZO31eJgWPmv0DFDUoPAozHy9qPX_HKQQ_igzvLHFe7Vz5pvr66VNItscFy";
		private const int REQUEST_CODE_PAYMENT = 1;
		private static PayPalConfiguration config = new PayPalConfiguration ()
			.Environment (CONFIG_ENVIRONMENT)
			.ClientId (CONFIG_CLIENT_ID);
		Button payPal;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your application here
			SetContentView (Resource.Layout.MainLayout);
			PayPalService t = new PayPalService ();

			Intent intent = new Intent (this, t.Class);
			intent.PutExtra (PayPalService.ExtraPaypalConfiguration, config);
			StartService (intent);
			payPal = FindViewById<Button> (Resource.Id.payPal);
			payPal.Click += (sender, e) => OnBuyPressed ();
		}

		public void OnBuyPressed ()
		{
			PayPalPayment thingToBuy = getThingToBuy (PayPalPayment.PaymentIntentSale);
			PaymentActivity tt = new PaymentActivity ();

			Intent intent = new Intent (this, tt.Class);
			intent.PutExtra (PayPalService.ExtraPaypalConfiguration, config);
			intent.PutExtra (PaymentActivity.ExtraPayment, thingToBuy);
			StartActivityForResult (intent, REQUEST_CODE_PAYMENT);
		}

		private PayPalPayment getThingToBuy (String paymentIntent)
		{
			return new PayPalPayment (new BigDecimal ("1.75"), "USD", "sample item",
				paymentIntent);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (requestCode == REQUEST_CODE_PAYMENT) {
				if (resultCode == Result.Ok) {
					PaymentConfirmation confirm =
						(PaymentConfirmation)data.GetParcelableExtra (PaymentActivity.ExtraResultConfirmation);
					if (confirm != null) {
						try {
							Console.WriteLine (confirm.ToJSONObject ().ToString (4));
							Console.WriteLine (confirm);
							/**
                         *  TODO: send 'confirm' (and possibly confirm.getPayment() to your server for verification
                         */
							Toast.MakeText (this, "PaymentConfirmation info received" + " from PayPal", ToastLength.Long).Show ();
						} catch (JsonException e) {
							Toast.MakeText (this, "an extremely unlikely failure" +
							" occurred:", ToastLength.Long).Show ();
						}
					}
				} else if (resultCode == Result.Canceled) {
					Toast.MakeText (this, "The user canceled.", ToastLength.Long).Show ();
				} else if (int.Parse (resultCode.ToString ()) == PaymentActivity.ResultExtrasInvalid) {
					Toast.MakeText (this, "An invalid Payment or PayPalConfiguration" +
					" was submitted. Please see the docs.", ToastLength.Long).Show ();
				}
			}
		}

		protected override void OnDestroy ()
		{

			base.OnDestroy ();
			// Stop service when done
			PayPalService ttt = new PayPalService ();
			StopService (new Intent (this, ttt.Class));
		}

	}
}

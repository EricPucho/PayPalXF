
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Diagnostics;
using PayPaliOSBinding;
using PayPalXF;
using PayPalXF.iOS;
using System.Drawing;
using System;

[assembly: ExportRenderer (typeof(PaymentPage), typeof(PaymentPageRenderer))]

namespace PayPalXF.iOS
{
	public class PaymentPageRenderer: PageRenderer
	{
		
		PPDelegate myDelegate;
		PayPalPaymentViewController paypalVC;

		//		public override void ViewDidAppear (bool animated)
		//		{
		//			base.ViewDidAppear (animated);
		//
		//			this.PresentViewController (paypalVC, true, null);
		//
		//		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.


			var config = new PayPalConfiguration () { 
				AcceptCreditCards = false,  //change to true, if you run on device (card io)
				LanguageOrLocale = "en",
				MerchantName = "Merchant",
				MerchantPrivacyPolicyURL = new Foundation.NSUrl ("https://www.paypal.com/webapps/mpp/ua/privacy-full"),
				MerchantUserAgreementURL = new Foundation.NSUrl ("https://www.paypal.com/webapps/mpp/ua/useragreement-full")
			};

			var item1 = new PayPalItem () {    
				Name = "Xamarin Monkey", 
				Price = new Foundation.NSDecimalNumber ("10.00"),
				Currency = "EUR",
				Quantity = 1,
				Sku = "FOO-29376"
			};

			var item2 = new PayPalItem () {    
				Name = "DoofesDing2", 
				Price = new Foundation.NSDecimalNumber ("15.00"),
				Currency = "EUR",
				Quantity = 1,
				Sku = "FOO-23476"
			};

			var items = new []{ item1, item2 };

			var payment = new PayPalPayment () { 
				Amount = new Foundation.NSDecimalNumber ("25.00"),
				CurrencyCode = "EUR",
				ShortDescription = "Stuffz",
				Items = items
			};

			myDelegate = new PPDelegate (this);

			paypalVC = new PayPalPaymentViewController (payment, config, myDelegate);
			var payBtn = new UIButton (new RectangleF (60, 100, 200, 60));
			payBtn.SetTitle ("Pay", UIControlState.Normal);
			payBtn.BackgroundColor = UIColor.Blue;
			payBtn.TouchUpInside += (object sender, EventArgs e) => this.PresentViewController (paypalVC, true, null);
			Add (payBtn);
		}


		protected class PPDelegate: PayPalPaymentDelegate
		{
			readonly UIViewController parent;

			public PPDelegate (UIViewController myParent)
			{
				parent = myParent;
			}

			public override void DidCancelPayment (PayPalPaymentViewController paymentViewController)
			{
				Debug.WriteLine ("Payment Cancelled");
				parent.DismissViewController (true, null);
			}

			public override void DidCompletePayment (PayPalPaymentViewController paymentViewController, PayPalPayment completedPayment)
			{
				Debug.WriteLine ("Payment Completed");
				parent.DismissViewController (true, null);
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}


}

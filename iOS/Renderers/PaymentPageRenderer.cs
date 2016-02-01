using System.Diagnostics;
using Foundation;
using PayPalXF;
using PayPaliOSBinding;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PayPalXF.iOS.Renderers;

[assembly: ExportRenderer (typeof(PaymentPage), typeof(PayementPageRenderer))]
namespace PayPalXF.iOS.Renderers
{
    public class PayementPageRenderer : PageRenderer
    {
        PPDelegate myDelegate;
        PayPalPaymentViewController paypalVC;
        public string description;
        public string balance;

        private const string _productionPayPalClientId = @"YOUR PRODUCTION CLIENT ID HERE";
        private const string _sandboxPayPalClientId = @"YOUR SANDBOX CLIENT ID HERE";

        protected override void OnElementChanged (VisualElementChangedEventArgs e)
        {
            base.OnElementChanged (e);

            var page = e.NewElement as PaymentPage;

            description = page.description_;
            balance = page.balance_;

            if (e.OldElement != null || Element == null) {
                return;
            }

        }


        public override void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);
            this.PresentModalViewController (paypalVC, true);
            Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync ();

        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();


            //NSDictionary dict = NSDictionary.FromObjectsAndKeys("PayPalEnvironmentProduction", _productionPayPalClientId, "PayPalEnvironmentSandbox", _sandboxPayPalClientId);
            //var dict = new NSDictionary ("PayPalEnvironmentProduction", _productionPayPalClientId, "PayPalEnvironmentSandbox", _sandboxPayPalClientId);

            //PayPalMobile.InitializeWithClientIdsForEnvironments (dict);
            //PayPalMobile.PreconnectWithEnvironment ("PayPalEnvironmentNoNetwork");

            var config = new PayPalConfiguration  { 
                AcceptCreditCards = true, 
                LanguageOrLocale = "en",
                MerchantName = "Your Merchant Name",

                MerchantPrivacyPolicyURL = new NSUrl ("https://www.paypal.com/webapps/mpp/ua/privacy-full"),
                MerchantUserAgreementURL = new NSUrl ("https://www.paypal.com/webapps/mpp/ua/useragreement-full")
            };

            NSDecimalNumber balance2 = new NSDecimalNumber (double.Parse (balance).ToString());

            var item1 = new PayPalItem () {    
                Name = description, 
                Price = balance2,
                Currency = "EUR",
                Quantity = 1

            };

            var items = new []{ item1 };

            var payment = new PayPalPayment () { 
                Amount = balance2,
                CurrencyCode = "EUR",
                ShortDescription = description,

                Items = items
            };

            myDelegate = new PPDelegate (this);

            paypalVC = new PayPalPaymentViewController (payment, config, myDelegate);
            //this.ViewController()
            //this.DidMoveToParentViewController (paypalVC);

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
                Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync ();


                parent.DismissModalViewController (true);
            }

            public override void DidCompletePayment (PayPalPaymentViewController paymentViewController, PayPalPayment completedPayment)
            {
                Debug.WriteLine ("Payment Completed");
                //Debug.WriteLine (completedPayment.Confirmation["response"]);

                var tt = completedPayment.Confirmation ["response"];

                NSString paymentKey = (NSString)tt.ValueForKey ((NSString)"id");
                Debug.WriteLine (paymentKey);

                Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync ();

                var mypage_renderer = new MyPage();
                //twopage_renderer.ini
                MessagingCenter.Send<MyPage, string> (mypage_renderer, "id_payement", paymentKey);

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

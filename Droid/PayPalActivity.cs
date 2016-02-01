using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

using Newtonsoft.Json;
using Com.Paypal.Android.Sdk.Payments;
using Java.Math;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

namespace PayPalXF.Droid
{
    [Activity (Label = "PayPal")]           
    public class PayPalActivity : Activity
    {
        public double balance;
        public string description;


        const string CONFIG_ENVIRONMENT = PayPalConfiguration.EnvironmentNoNetwork;
        // note that these credentials will differ between live & sandbox environments.
        const String CONFIG_CLIENT_ID = "YOUR SANDBOX CLIENT ID";
        const int REQUEST_CODE_PAYMENT = 1;
        const int REQUEST_CODE_FUTURE_PAYMENT = 2;
        const int REQUEST_CODE_PROFILE_SHARING = 3;
        static PayPalConfiguration config = new PayPalConfiguration ()
            .Environment (CONFIG_ENVIRONMENT)
            .ClientId (CONFIG_CLIENT_ID)
            .MerchantName("Your Merchant Name")
            .MerchantPrivacyPolicyUri(Android.Net.Uri.Parse("https://www.example.com/privacy"))
            .MerchantUserAgreementUri(Android.Net.Uri.Parse("https://www.example.com/legal"))
            .AcceptCreditCards (true);


        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            PayPalService t = new PayPalService ();

            //Retrieve data bundle passed from ListActivity
            description = Intent.GetStringExtra("description");
            balance = double.Parse(Intent.GetStringExtra ("balance"));

            Intent intent = new Intent (this, t.Class);
            intent.PutExtra (PayPalService.ExtraPaypalConfiguration, config);
            StartService (intent);

            OnBuyPressed ();
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
            return new PayPalPayment (new BigDecimal (balance), "EUR", description,
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

                            string json = confirm.ToJSONObject ().ToString (4);

                            //lecture du json
                            JObject jsonVal = JObject.Parse (json);
                            var ventes_ = jsonVal;

                            //decoupage du json
                            string response = ventes_ ["response"].ToString ();

                            //lecture de stock
                            JObject jstockVal = JObject.Parse (response);

                            var response__ = jstockVal;

                            string id_payement = response__ ["id"].ToString ();
                            //Console.WriteLine (confirm);
                            //Console.WriteLine (confirm.Payment);

                            var mypage_renderer = new MyPage();
                            MessagingCenter.Send<MyPage, string> (mypage_renderer, "id_payement", id_payement);

                            /**
                         *  TODO: send 'confirm' (and possibly confirm.getPayment() to your server for verification
                         */
                            Toast.MakeText (this, "PaymentConfirmation info received" + " from PayPal", ToastLength.Long).Show ();
                            Finish();
                        } catch (JsonException e) {
                            Toast.MakeText (this, "an extremely unlikely failure" +
                                            " occurred:", ToastLength.Long).Show ();
                            Console.WriteLine (e);
                        }
                    }
                } else if (resultCode == Result.Canceled) {
                    Toast.MakeText (this, "The user canceled.", ToastLength.Long).Show ();
                    Finish ();
                } else if (int.Parse (resultCode.ToString ()) == PaymentActivity.ResultExtrasInvalid) {
                    Toast.MakeText (this, "An invalid Payment or PayPalConfiguration" +
                                    " was submitted. Please see the docs.", ToastLength.Long).Show ();

                    Finish ();
                }
            }
        }

        protected override void OnDestroy ()
        {

            base.OnDestroy ();
            // Stop service when done
            PayPalService ttt = new PayPalService ();
            StopService (new Intent (this, ttt.Class));
            // Finish ();
        }

    }
}

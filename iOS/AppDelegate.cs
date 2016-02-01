using Foundation;
using UIKit;
using Xamarin.Forms;

namespace PayPalXF.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			LoadApplication (new App ());

            MessagingCenter.Subscribe<MyPage, string> (this, "payement", (sender, args) => {

                string [] recup =args.Split('/');

                sender.Navigation.PushModalAsync (new PaymentPage (recup [1], recup[0]));

            });

			return base.FinishedLaunching (app, options);


		}
	}
}
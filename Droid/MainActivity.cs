
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

namespace PayPalXF.Droid
{
	[Activity (Label = "PayPalXF.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbars;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

			LoadApplication (new App ());

            MessagingCenter.Subscribe<MyPage, string> (this, "payement", (sender, args) => {

                string [] recup =args.Split('/');

                Intent i = new Intent (Android.App.Application.Context, typeof(PayPalActivity));
                i.PutExtra ("description", recup[1]);
                i.PutExtra("balance", recup[0]);
                StartActivity (i);
            });
		}
	}
}


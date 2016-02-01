using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PayPalXF
{
    public class MyPage : ContentPage
    {
        Button button_pay;

        public MyPage ()
        {
            
            button_pay = new Button {
                Text = "PAYPAL EXAMPLE",
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex ("#1F497D"),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.White
            };

           
            Content = new StackLayout { 

                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Children = {
                    button_pay
                    }
                };

            button_pay.Clicked += (object sender, EventArgs e) => {
                Click_on_button ();
            };


        }

        public void Click_on_button ()
        {
            string description = "1000" + "/" + "product description";
            MessagingCenter.Send (this, "payement", description);
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();

            MessagingCenter.Subscribe<MyPage, string> (this, "id_payement", (sender, arg) => {

                DisplayAlert ("PAYPALXF", "PAYMENT ID : " + arg, "OK");
                Debug.WriteLine (arg); //display id_payment here

            });
        }

    }
}


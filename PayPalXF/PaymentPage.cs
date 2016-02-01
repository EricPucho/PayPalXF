using System;

using Xamarin.Forms;

namespace PayPalXF
{
    public class PaymentPage : ContentPage
    {
        public String description_;
        public String balance_;

        public PaymentPage (string description, string balance)
        {
            description_ = description;
            balance_ = balance;
        }
    }
}



using UIKit;
using Foundation;
using ObjCRuntime;

using System;

namespace PayPaliOSBinding
{
	[BaseType (typeof(NSObject))]
	public partial interface PayPalConfiguration
	{

		[Export ("defaultUserEmail", ArgumentSemantic.Copy)]
		string DefaultUserEmail {
			get;
			set;
		}

		[Export ("defaultUserPhoneCountryCode", ArgumentSemantic.Copy)]
		string DefaultUserPhoneCountryCode {
			get;
			set;
		}

		[Export ("defaultUserPhoneNumber", ArgumentSemantic.Copy)]
		string DefaultUserPhoneNumber {
			get;
			set;
		}

		[Export ("merchantName", ArgumentSemantic.Copy)]
		string MerchantName {
			get;
			set;
		}

		[Export ("merchantPrivacyPolicyURL", ArgumentSemantic.Copy)]
		NSUrl MerchantPrivacyPolicyURL {
			get;
			set;
		}

		[Export ("merchantUserAgreementURL", ArgumentSemantic.Copy)]
		NSUrl MerchantUserAgreementURL {
			get;
			set;
		}

		[Export ("acceptCreditCards")]
		bool AcceptCreditCards {
			get;
			set;
		}

		[Export ("rememberUser")]
		bool RememberUser {
			get;
			set;
		}

		[Export ("languageOrLocale", ArgumentSemantic.Copy)]
		string LanguageOrLocale {
			get;
			set;
		}

		[Export ("disableBlurWhenBackgrounding")]
		bool DisableBlurWhenBackgrounding {
			get;
			set;
		}

		[Export ("forceDefaultsInSandbox")]
		bool ForceDefaultsInSandbox {
			get;
			set;
		}

		[Export ("sandboxUserPassword", ArgumentSemantic.Copy)]
		string SandboxUserPassword {
			get;
			set;
		}

		[Export ("sandboxUserPin", ArgumentSemantic.Copy)]
		string SandboxUserPin {
			get;
			set;
		}
	}

	[BaseType (typeof(NSObject))]
	public partial interface PayPalPaymentDetails
	{

		[Static, Export ("paymentDetailsWithSubtotal:withShipping:withTax:")]
		PayPalPaymentDetails PaymentDetailsWithSubtotal (NSDecimalNumber subtotal, NSDecimalNumber shipping, NSDecimalNumber tax);

		[Export ("subtotal", ArgumentSemantic.Copy)]
		NSDecimalNumber Subtotal {
			get;
			set;
		}

		[Export ("shipping", ArgumentSemantic.Copy)]
		NSDecimalNumber Shipping {
			get;
			set;
		}

		[Export ("tax", ArgumentSemantic.Copy)]
		NSDecimalNumber Tax {
			get;
			set;
		}
	}

	[BaseType (typeof(NSObject))]
	public partial interface PayPalItem
	{

		[Static, Export ("itemWithName:withQuantity:withPrice:withCurrency:withSku:")]
		PayPalItem ItemWithName (string name, uint quantity, NSDecimalNumber price, string currency, string sku);

		[Static, Export ("totalPriceForItems:")]
		NSDecimalNumber TotalPriceForItems (NSObject[] items);

		[Export ("name", ArgumentSemantic.Copy)]
		string Name {
			get;
			set;
		}

		[Export ("quantity")]
		uint Quantity {
			get;
			set;
		}

		[Export ("price", ArgumentSemantic.Copy)]
		NSDecimalNumber Price {
			get;
			set;
		}

		[Export ("currency", ArgumentSemantic.Copy)]
		string Currency {
			get;
			set;
		}

		[Export ("sku", ArgumentSemantic.Copy)]
		string Sku {
			get;
			set;
		}
	}

	[BaseType (typeof(NSObject))]
	public partial interface PayPalPayment
	{

		[Static, Export ("paymentWithAmount:currencyCode:shortDescription:intent:")]
		PayPalPayment PaymentWithAmount (NSDecimalNumber amount, string currencyCode, string shortDescription, PayPalPaymentIntent intent);

		[Export ("currencyCode", ArgumentSemantic.Copy)]
		string CurrencyCode {
			get;
			set;
		}

		[Export ("amount", ArgumentSemantic.Copy)]
		NSDecimalNumber Amount {
			get;
			set;
		}

		[Export ("shortDescription", ArgumentSemantic.Copy)]
		string ShortDescription {
			get;
			set;
		}

		[Export ("intent")]
		PayPalPaymentIntent Intent {
			get;
			set;
		}

		[Export ("paymentDetails", ArgumentSemantic.Copy)]
		PayPalPaymentDetails PaymentDetails {
			get;
			set;
		}

		[Export ("items", ArgumentSemantic.Copy)]
		NSObject [] Items {
			get;
			set;
		}

		[Export ("bnCode", ArgumentSemantic.Copy)]
		string BnCode {
			get;
			set;
		}

		[Export ("processable")]
		bool Processable {
			get;
		}

		[Export ("localizedAmountForDisplay", ArgumentSemantic.Copy)]
		string LocalizedAmountForDisplay {
			get;
		}

		[Export ("confirmation", ArgumentSemantic.Copy)]
		NSDictionary Confirmation {
			get;
		}
	}

	[Model, BaseType (typeof(NSObject))]
	public partial interface PayPalPaymentDelegate
	{
		[Export ("payPalPaymentDidCancel:")]
		void DidCancelPayment (PayPalPaymentViewController paymentViewController);

		[Export ("payPalPaymentViewController:didCompletePayment:")]
		void DidCompletePayment (PayPalPaymentViewController paymentViewController, PayPalPayment completedPayment);
	}

	[BaseType (typeof(UINavigationController))]
	public partial interface PayPalPaymentViewController
	{

		[Export ("initWithPayment:configuration:delegate:")]
		IntPtr Constructor (PayPalPayment payment, PayPalConfiguration configuration, PayPalPaymentDelegate ppDelegate);

		[Export ("paymentDelegate", ArgumentSemantic.Assign)]
		PayPalPaymentDelegate PaymentDelegate {
			get;
		}

		[Export ("state")]
		PayPalPaymentViewControllerState State {
			get;
		}
	}

	[BaseType (typeof(NSObject))]
	public partial interface PayPalMobile
	{

		[Static, Export ("initializeWithClientIdsForEnvironments:")]
		void InitializeWithClientIdsForEnvironments (NSDictionary clientIdsForEnvironments);

		[Static, Export ("preconnectWithEnvironment:")]
		void PreconnectWithEnvironment (string environment);

		[Static, Export ("applicationCorrelationIDForEnvironment:")]
		string ApplicationCorrelationIDForEnvironment (string environment);

		[Static, Export ("clearAllUserData")]
		void ClearAllUserData ();

		[Static, Export ("libraryVersion")]
		string LibraryVersion {
			get;
		}
	}

	/*
    [Model, BaseType (typeof (NSObject))]
    public partial interface PayPalFuturePaymentDelegate {

        [Export ("payPalFuturePaymentDidCancel:")]
        void  (PayPalFuturePaymentViewController futurePaymentViewController);

        [Export ("payPalFuturePaymentViewController:didAuthorizeFuturePayment:")]
        void DidAuthorizeFuturePayment (PayPalFuturePaymentViewController futurePaymentViewController, NSDictionary futurePaymentAuthorization);
    }


    [BaseType (typeof (UINavigationController))]
    public partial interface PayPalFuturePaymentViewController {

        [Export ("initWithConfiguration:delegate:")]
        IntPtr Constructor (PayPalConfiguration configuration, PayPalFuturePaymentDelegate delegate);

        [Export ("futurePaymentDelegate", ArgumentSemantic.Assign)]
        PayPalFuturePaymentDelegate FuturePaymentDelegate { get; }

        [Field ("PayPalEnvironmentProduction")]
        NSString PayPalEnvironmentProduction { get; }

        [Field ("PayPalEnvironmentSandbox")]
        NSString PayPalEnvironmentSandbox { get; }

        [Field ("PayPalEnvironmentNoNetwork")]
        NSString PayPalEnvironmentNoNetwork { get; }
    }
    */
}


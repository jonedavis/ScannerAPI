using System;
using System.Drawing;
using ObjCRuntime;

using Foundation;
using UIKit;
using ScanAPI;
using System.Threading.Tasks;

namespace ScanTest
{
	public partial class ScanTestViewController : UIViewController
	{
		public ScanTestViewController (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//Architectures in the fat file: ibScanApi.a are: armv7 armv7s i386 x86_64 arm64 

			// Look in Info.plist
			// The documentation says that this line must be added
			// Supported External Accessory Protocol = com.socketmobile.chs
			// These to methods are from the Scanner API
			// Look at the documentation for additional information.
			var instance = SktClassFactory.CreateScanApiInstance ();
			var obj = SktClassFactory.CreateScanObject ();

			// These are just test
			var msg = obj.Msg ();
			var msgEvent = msg.Event ();
			var msgData = msgEvent.DataDecodedData ();
			var prop = obj.Property ();

			// No longer call the helpers. Now you're creating your own.
			Console.WriteLine ("Before Instance.Open");
			Task.Run (async () => await RunOpenAsync ());
			Console.WriteLine ("After Instance.Open");
		}
			
		private async Task RunOpenAsync ()
		{
			await ScanHelper.Instance.OpenAsync ();
		}
	}
}
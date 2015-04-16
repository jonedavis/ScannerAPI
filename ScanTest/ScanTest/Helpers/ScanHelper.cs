using System;
using Foundation;
using ScanAPI;
using ObjCRuntime;
using System.Threading.Tasks;

namespace ScanTest
{
	public class ScanHelper : NSObject
	{
		// Creating an Singleton
		static readonly ScanHelper _instance = new ScanHelper ();

		public static ScanHelper Instance { get { return _instance; } }

		// This is the start of the entire helper properties.
		NSString _noDeviceText;
		NSObject _commandContextsLock;
		NSMutableDictionary _deviceInfoList;
		bool _scanApiOpen;
		bool _scanApiTerminated;
		NSMutableArray _delegateStack;
		ISktScanObject _scanObjectReceived;
		ISktScanApi _scanApi;
		ScanApiHelperDelegate _delegate;


		public ScanHelper ()
		{
			_commandContextsLock = new NSObject ();
			_deviceInfoList = new NSMutableDictionary ();
			_scanApiOpen = false;
			_scanApiTerminated = false;
			_delegateStack = new NSMutableArray ();
		}

		// A 1:1 of ApiHelper's Objective-C initializeScanAPIThread: method
		public async Task InitializeScanAPIThread ()
		{
			Console.WriteLine ("In Instance.InitializeScanAPIThread");
			Task.Run (() => {
				_scanApi = SktClassFactory.CreateScanApiInstance ();
				var result = _scanApi.Open (null);

				if (_delegate != null) {
					_delegate.OnScanApiInitializeComplete (result);
				}

				_scanApiTerminated = false;
			});

			Console.WriteLine ("Leaving Instance.InitializeScanAPIThread");
		}

		// A 1:1 of ApiHelper's Objective-C open: method
		public async Task OpenAsync ()
		{
			Console.WriteLine ("In Instance.Open");
			_deviceInfoList.Clear ();

			if (_noDeviceText != null) {
				_deviceInfoList.Add (_noDeviceText, _noDeviceText);
			}

			if (_scanObjectReceived != null) {
				SktClassFactory.ReleaseScanObject (_scanObjectReceived);
			}

			// You don't have to use NSThread. You can use .NET Threading as well.
			await Task.Run (async () => await InitializeScanAPIThread ());

			_scanApiOpen = true;
		}
	}
}
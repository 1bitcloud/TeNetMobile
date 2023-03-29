using Android.App;
using Android.Content;
using CommunityToolkit.Mvvm.Messaging;
//using AndroidX.Core.App;
using Firebase.Messaging;
using TeNetMobile;

namespace TeNetMobile.Platforms.Android
{
  [Service( Exported = true )]
  [IntentFilter( new[] { "com.google.firebase.MESSAGING_EVENT" } )]
  public class FirebaseService : FirebaseMessagingService
  {
    public override void OnNewToken( string token )
    {
      base.OnNewToken( token );
      if( Preferences.ContainsKey( "DeviceToken" ) )
        Preferences.Remove( "DeviceToken" );
      Preferences.Set( "DeviceToken", token );
      MainPage.DeviceToken = token;
    }

    public override void OnMessageReceived( RemoteMessage message )
    {
      base.OnMessageReceived( message );

      WeakReferenceMessenger.Default.Send( new ConfirmationMessage {
        Resource = message.Data["Resource"],
        UserName = message.Data["UserName"],
        Key = message.Data["Key"],
        Secret = message.Data["Secret"]
      } );
    }
  }


  public class ConfirmationMessage
  {
    public string Resource { get; set; }
    public string UserName { get; set; }
    public string Key { get; set; }
    public string Secret { get; set; }
  }
}

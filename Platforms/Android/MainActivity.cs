using Android.App;
using Android.Content.PM;
using Android.OS;
using CommunityToolkit.Mvvm.Messaging;
using Firebase;
using Firebase.Messaging;
using TeNetMobile.Platforms.Android;

namespace TeNetMobile;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
  protected override void OnCreate( Bundle savedInstanceState )
  {
    base.OnCreate( savedInstanceState );

    FirebaseApp app = FirebaseApp.InitializeApp( this );
    if( Intent.Extras != null )
    {
      WeakReferenceMessenger.Default.Send( new ConfirmationMessage
      {
        Resource = Intent.Extras.GetString( "Resource" ),
        UserName = Intent.Extras.GetString( "UserName" ),
        Key = Intent.Extras.GetString( "Key" ),
        Secret = Intent.Extras.GetString( "Secret" )
      } );
    }


//          var t = FirebaseMessaging.Instance.GetToken().Result;
    }
  }


namespace TeNetMobile;

public partial class MainPage : ContentPage
{
  public static string DeviceToken { get; set; }
  private string _key;
  private int _secret1;

  public MainPage()
	{
		InitializeComponent();
    if( Preferences.ContainsKey( "DeviceToken" ) )
      DeviceToken = Preferences.Get( "DeviceToken", "" );

    WeakReferenceMessenger.Default.Register<ConfirmationMessage>( this, (r,m) =>
      MainThread.BeginInvokeOnMainThread( () => OnConfirmationMessage( m ) ) );
  }


  private void OnConfirmationMessage( ConfirmationMessage cm )
  {
    // Тут по секрету2 должен определяться аккаунт (и секрет1), но так как аккаунтов нет, то тут пока просто проверка
    int secret2 = Preferences.Get( "Secret2", 0 );
    if( int.Parse( cm.Secret ) != secret2 )
      return; // Прислали какой-то левый секрет
    _secret1 = Preferences.Get( "Secret1", 0 );

    _key = cm.Key;

    TitleLabel.Text = "Попытка входа";
    TextLabel.Text = $"Кто то пытается войти в {cm.Resource} как пользователь {cm.UserName}";

    TextLabel.IsVisible = true; 
    YesBtn.IsVisible = true;
    NoBtn.IsVisible = true;
  }


  private async Task SendConfirmation( bool confirmed )
  {
    TitleLabel.Text = "Ждем снова...";

    TextLabel.IsVisible = false;
    YesBtn.IsVisible = false;
    NoBtn.IsVisible = false;

    try
    {
      MobileApp.MobileAppClient client = new MobileApp.MobileAppClient( MauiProgram.Channel );
      await client.ConfirmAsync( new MobileAppConfirmRequest { Secret = _secret1, Key = _key, Confirmed = confirmed } );
    }
    catch(Exception ex)
    {
      // Чисто отладочная диагностика
      await App.Current.MainPage.DisplayAlert( "Ошибка", ex.Message, "OK" );
    }
  }

  

  private void OnAddClicked(object sender, EventArgs e)
	{
    // Парсим код и проверяем пределы
    if( int.TryParse( Secret1.Text, out int code ) && code < 1000000 )
    {
      Secret1.Text = "";
      var popup = new SimplePopup( code );
      this.ShowPopup( popup );
    }
    else
      Secret1.Text = "";
  }

  private void OnYesClicked(object sender, EventArgs e)
	{
    _ = SendConfirmation( true );
  }

  private void OnNoClicked(object sender, EventArgs e)
	{
    _ = SendConfirmation( false );
  }
}

/*
public class NotificationMessage : ValueChangedMessage<RemoteMessage>
{
  public NotificationMessage( RemoteMessage.Notification n ) : base( n )
  {
  }
}*/
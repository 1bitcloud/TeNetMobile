using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TeNetMobile
{
  public partial class SimplePopup : Popup
  {
    private int _secret1;
    private int _secret2;
    public SimplePopup( int secret1 )
    {
      _secret1 = secret1;

      // !!! Secret2 должен быть уникальным среди всех зарегистрированных в приложении аккаунтов !!!
      // Данный прототип приложения имеет всего один аккаунт, поэтому никаких проверок нет
      // По Secret2 происходит идентификация аккаунта для корректного ответа подверждения аутентификации
      // В подверждении отправляется Secret1 + GUID из нотификации
      // (такая сложная схема для того, чтобы перехвата нотификаций было недостаточно для взлома).
      _secret2 = RandomNumberGenerator.GetInt32( 0, 1000000 );

      InitializeComponent();

      Secret2.Text = $"{_secret2:D6}";
      _ = SetupAsync();
    }


    private async Task SetupAsync()
    {
      try
      {
        MobileApp.MobileAppClient client = new MobileApp.MobileAppClient( MauiProgram.Channel );
        MobileAppSetupReply reply = await client.SetupAsync( new MobileAppSetupRequest {
          Secret1 = _secret1, Secret2 = _secret2, DeviceToken = MainPage.DeviceToken } );

        if( reply.Success )
        {
          // Тут надо делать сохранение по аккаунтам
          Preferences.Set( "Secret1", _secret1 );
          Preferences.Set( "Secret2", _secret2 );
        }
        Close( reply.Success );
      }
      catch(Exception ex)
      {
        // Чисто отладочная диагностика
        await App.Current.MainPage.DisplayAlert( "Ошибка", ex.Message, "OK" );
      }

      Close( false );
    }
  }
}

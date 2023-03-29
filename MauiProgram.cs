global using Microsoft.Extensions.Logging;

global using CommunityToolkit.Maui;
global using CommunityToolkit.Maui.Views;
global using CommunityToolkit.Mvvm.Messaging;

global using Grpc.Net.Client;

global using TeNetApi.Api;
global using TeNetMobile.Platforms.Android;


namespace TeNetMobile;

public static class MauiProgram
{
  static public GrpcChannel Channel { get; private set; }

  public static MauiApp CreateMauiApp()
  {
    var builder = MauiApp.CreateBuilder();
    builder
      .UseMauiApp<App>()
      .UseMauiCommunityToolkit()
      .ConfigureFonts( fonts =>
      {
        fonts.AddFont( "OpenSans-Regular.ttf", "OpenSansRegular" );
        fonts.AddFont( "OpenSans-Semibold.ttf", "OpenSansSemibold" );
      } );

#if DEBUG
    builder.Logging.AddDebug();
#endif

    // На время разработки юзаем HTTP, чтобы не трахаться с SSL сертификатами
    // На релизе надо переключать на HTTPS
//    AppContext.SetSwitch( "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true );
//    Channel = GrpcChannel.ForAddress( "http://10.0.2.2:37372" );

    Channel = GrpcChannel.ForAddress( "https://vds321.1cbit.ru:37373" );

    return builder.Build();
  }
}

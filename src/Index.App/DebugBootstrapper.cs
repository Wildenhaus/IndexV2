using System;
using Index.App.Views;
using Index.Domain.GameProfiles;
using Index.Domain.Models;
using Prism.Ioc;

namespace Index.App
{

  internal class DebugBootstrapper : Bootstrapper
  {

    private string _gameId;
    private string _gamePath;

    public DebugBootstrapper( string gameId, string gamePath )
    {
      _gameId = gameId;
      _gamePath = gamePath;
    }

    protected override void OnInitialized()
    {
      var profileManager = Container.Resolve<IGameProfileManager>();
      var profile = profileManager.Profiles[ _gameId ];

      var editorEnvironment = Container.Resolve<IEditorEnvironment>();
      editorEnvironment.GameId = _gameId;
      editorEnvironment.GameName = profile.GameName;
      editorEnvironment.GamePath = _gamePath;
      editorEnvironment.GameProfile = profile;

      var loadingView = Container.Resolve<EditorLoadingView>();
      loadingView.ShowDialog();

      var editor = Container.Resolve<EditorView>();
      editor.Closed += ( s, e ) => Environment.Exit( 0 );
      editor.Show();
    }

  }

}

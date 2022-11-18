using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Index.App.Models;
using Index.Domain.Entities;
using Index.Domain.GameProfiles;
using Index.Domain.Repositories;
using Index.Domain.Services;
using Index.Domain.ViewModels;
using Index.UI.Windows;
using Prism.Commands;

namespace Index.App.ViewModels
{

  public class LauncherViewModel : ViewModelBase
  {

    #region Data Members

    private readonly IFileDialogService _fileDialogService;
    private readonly IGameProfileManager _profileManager;
    private readonly IGamePathRepository _gamePathRepository;

    private readonly ObservableCollection<LauncherItem> _items;

    #endregion

    #region Properties

    public ObservableCollection<LauncherItem> Items => _items;
    public LauncherItem SelectedItem { get; set; }

    public DelegateCommand ScanPathCommand { get; }
    public DelegateCommand RemoveSelectedPathCommand { get; }
    public DelegateCommand<IxDialogWindow> LaunchCommand { get; }

    #endregion

    #region Constructor

    public LauncherViewModel(
      IFileDialogService fileDialogService,
      IGameProfileManager profileManager,
      IGamePathRepository gamePathRepository )
    {
      _fileDialogService = fileDialogService;
      _profileManager = profileManager;
      _gamePathRepository = gamePathRepository;

      _items = new ObservableCollection<LauncherItem>();

      ScanPathCommand = new DelegateCommand( ScanPath );
      RemoveSelectedPathCommand = new DelegateCommand( RemoveSelectedGamePath );
      LaunchCommand = new DelegateCommand<IxDialogWindow>( LaunchEditor );

      ReloadLauncherItems();
    }

    #endregion

    #region Private Methods

    private void LaunchEditor( IxDialogWindow window )
    {
      window.Parameters.Set( nameof( LauncherItem.GameId ), SelectedItem.GameId );
      window.Parameters.Set( nameof( LauncherItem.GameName ), SelectedItem.GameName );
      window.Parameters.Set( nameof( LauncherItem.GamePath ), SelectedItem.GamePath );

      window.DialogResult = true;
      window.Close();
    }

    private void ReloadLauncherItems()
    {
      var newItems = new List<LauncherItem>();

      var gamePaths = _gamePathRepository.GetAll();
      foreach ( var gamePath in gamePaths )
      {
        if ( !_profileManager.Profiles.TryGetValue( gamePath.GameId, out var gameProfile ) )
          continue;

        newItems.Add( new LauncherItem
        {
          GameId = gameProfile.GameId,
          GameName = gameProfile.GameName,
          GameIcon = LoadGameIcon( gameProfile ),

          GamePath = gamePath.Path
        } );
      }

      _items.Clear();
      foreach ( var newItem in newItems )
        _items.Add( newItem );
    }

    private void ScanPath()
    {
      var gamePath = _fileDialogService.BrowseForDirectory( "Select a path to scan" );
      if ( string.IsNullOrWhiteSpace( gamePath ) )
        return;

      var identificationResults = _profileManager.ScanPathForSupportedGames( gamePath );
      if ( !identificationResults.Any() )
        return; // TODO: Show dialog

      var pathsAdded = 0;
      foreach ( var result in identificationResults )
      {
        if ( _gamePathRepository.CheckIfGamePathAlreadyExists( result.GamePath ) )
          continue;

        _gamePathRepository.Add( new GamePath
        {
          GameId = result.GameId,
          Path = result.GamePath
        } );
        pathsAdded++;
      }

      if ( pathsAdded > 0 )
        _gamePathRepository.SaveChanges();

      ReloadLauncherItems();
    }

    private void RemoveSelectedGamePath()
    {
      if ( SelectedItem is null )
        return;

      var gamePath = _gamePathRepository.GetByPath( SelectedItem.GamePath );
      if ( gamePath is null )
        return;

      _gamePathRepository.Delete( gamePath );
      _gamePathRepository.SaveChanges();

      ReloadLauncherItems();
    }

    private ImageSource? LoadGameIcon( IGameProfile gameProfile )
    {
      var gameIconStream = gameProfile.LoadGameIcon();
      if ( gameIconStream is null )
        return null;

      var gameIcon = new BitmapImage();
      gameIcon.BeginInit();
      {
        gameIcon.StreamSource = gameIconStream;
        gameIcon.DecodePixelWidth = 64;
      }
      gameIcon.EndInit();

      return gameIcon;
    }

    #endregion

  }

}

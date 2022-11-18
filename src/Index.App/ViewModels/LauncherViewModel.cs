using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Index.App.Models;
using Index.UI.ViewModels;
using Index.UI.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Index.App.ViewModels
{

  public class LauncherViewModel : BindableBase
  {

    #region Data Members

    private readonly ObservableCollection<LauncherItem> _items;

    #endregion

    #region Properties

    public ObservableCollection<LauncherItem> Items => _items;
    public LauncherItem SelectedItem { get; set; }

    public DelegateCommand<IxDialogWindow> LaunchCommand { get; private set; }

    #endregion

    #region Constructor

    public LauncherViewModel()
    {
      _items = new ObservableCollection<LauncherItem>();

      LaunchCommand = new DelegateCommand<IxDialogWindow>( LaunchEditor );

      _items.Add( new LauncherItem
      {
        GameName = "Test Name",
        GamePath = "Test Path"
      } );
    }

    #endregion

    private void LaunchEditor( IxDialogWindow window )
    {
      window.DialogResult = true;
      window.Parameters[ "GameId" ] = SelectedItem.GameId;
      window.Parameters[ "GamePath" ] = SelectedItem.GamePath;
      window.Close();
    }

  }

}

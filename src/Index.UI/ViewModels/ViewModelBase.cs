using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;
using Index.UI.Controls.Menus;
using Prism.Regions;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  [AddINotifyPropertyChangedInterface]
  public abstract class ViewModelBase : IViewModel
  {

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region Data Members

    private bool _isDisposed;
    private ContextMenu _contextMenu;

    #endregion

    #region Properties

    public Dispatcher Dispatcher { get; }

    public ContextMenu ContextMenu
    {
      get => _contextMenu;
      protected set => _contextMenu = value;
    }

    #endregion

    #region Constructor

    protected ViewModelBase()
    {
      Dispatcher = Dispatcher.CurrentDispatcher;
    }

    ~ViewModelBase()
    {
      Dispose( false );
    }

    #endregion

    #region Private Methods

    protected void ConfigureContextMenu()
    {
      var builder = new MenuViewModelBuilder();
      OnConfigureContextMenu( builder );

      if ( builder.Items.Count == 0 )
      {
        ContextMenu = null;
        return;
      }

      Dispatcher.Invoke( () =>
      {
        var menu = new ContextMenu();
        menu.ItemsSource = builder.Items;

        ContextMenu = menu;
      } );
    }

    protected virtual void OnConfigureContextMenu( MenuViewModelBuilder builder )
    {
    }

    #endregion

    #region INavigationAware Methods

    public virtual bool IsNavigationTarget( NavigationContext navigationContext )
      => false;

    public virtual void OnNavigatedTo( NavigationContext navigationContext )
    {
    }

    public virtual void OnNavigatedFrom( NavigationContext navigationContext )
    {
    }

    #endregion

    #region IDisposable Methods

    public void Dispose()
      => Dispose( true );

    private void Dispose( bool disposing )
    {
      if ( _isDisposed )
        return;

      if ( disposing )
        OnDisposing();

      _isDisposed = true;
    }

    protected virtual void OnDisposing()
    {
    }

    #endregion

  }

}
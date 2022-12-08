using System;
using System.ComponentModel;
using System.Windows.Threading;
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

    #endregion

    #region Properties

    public Dispatcher Dispatcher { get; }

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
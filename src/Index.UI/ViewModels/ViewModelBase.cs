using System;
using System.ComponentModel;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  [AddINotifyPropertyChangedInterface]
  public abstract class ViewModelBase : IDisposable, INotifyPropertyChanged
  {

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region Data Members

    private bool _isDisposed;

    #endregion

    #region Constructor

    protected ViewModelBase()
    {
    }

    ~ViewModelBase()
    {
      Dispose( false );
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
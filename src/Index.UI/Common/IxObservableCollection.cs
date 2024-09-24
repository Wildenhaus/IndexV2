using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Index.UI.Common
{

  public sealed class IxObservableCollection<T> : ObservableCollection<T>
  {

    #region Data Members

    private bool _notificationSuppressed;
    private bool _suppressNotifications;

    #endregion

    #region Properties

    public bool SuppressNotifications
    {
      get => _suppressNotifications;
      set
      {
        _suppressNotifications = value;
        if( _suppressNotifications == false && _notificationSuppressed )
        {
          this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
          _notificationSuppressed = false;
        }
      }
    }

    #endregion

    #region Constructor

    public IxObservableCollection()
    {
    }

    public IxObservableCollection( IEnumerable<T> collection ) 
      : base( collection )
    {
    }

    public IxObservableCollection( List<T> list ) 
      : base( list )
    {
    }

    #endregion

    #region Overrides

    protected override void OnCollectionChanged( NotifyCollectionChangedEventArgs e )
    {
      if(SuppressNotifications)
      {
        _notificationSuppressed = true;
        return;
      }

      base.OnCollectionChanged( e );
    }

    #endregion

  }

}

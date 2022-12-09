using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Index.Common
{

  public interface IProgressInfo : INotifyPropertyChanged
  {

    #region Properties

    bool IsIndeterminate { get; set; }

    string Status { get; set; }
    string SubStatus { get; set; }

    double CompletedUnits { get; set; }
    double TotalUnits { get; set; }
    string UnitName { get; set; }

    double PercentCompleted { get; }

    #endregion

  }

  public class ProgressInfo : IProgressInfo
  {

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region Data Members

    private bool _isIndeterminate;

    private string _status;
    private string _subStatus;

    private double _completedUnits;
    private double _totalUnits;
    private string _unitName;

    #endregion

    #region Properties

    public bool IsIndeterminate
    {
      get => _isIndeterminate;
      set
      {
        SetProperty( ref _isIndeterminate, value );
        RaisePropertyChanged( nameof( PercentCompleted ) );
      }
    }

    public string Status
    {
      get => _status;
      set => SetProperty( ref _status, value );
    }

    public string SubStatus
    {
      get => _subStatus;
      set => SetProperty( ref _subStatus, value );
    }

    public double CompletedUnits
    {
      get => _completedUnits;
      set
      {
        SetProperty( ref _completedUnits, value );
        RaisePropertyChanged( nameof( PercentCompleted ) );
      }
    }

    public double TotalUnits
    {
      get => _totalUnits;
      set
      {
        SetProperty( ref _totalUnits, value );
        RaisePropertyChanged( nameof( PercentCompleted ) );
      }
    }

    public string UnitName
    {
      get => _unitName;
      set => SetProperty( ref _unitName, value );
    }

    public double PercentCompleted
    {
      get
      {
        if ( _isIndeterminate )
          return 0;

        if ( _totalUnits == 0 )
          return 0;

        return _completedUnits / _totalUnits;
      }
    }

    #endregion

    #region Constructor

    public ProgressInfo()
    {
      _isIndeterminate = true;
    }

    #endregion

    #region Private Methods

    private void SetProperty<T>( ref T propertyStorage, T value, [CallerMemberName] string propertyName = null )
    {
      propertyStorage = value;
      RaisePropertyChanged( propertyName );
    }

    private void RaisePropertyChanged( [CallerMemberName] string propertyName = null )
      => PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );

    #endregion

  }

}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Index.Validation;
using PropertyChanged;

namespace Index.Domain.Assets
{

  [AddINotifyPropertyChangedInterface]
  public abstract class AssetExportOptions : ValidatableObject, INotifyPropertyChanged
  {

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region Properties

    [Required]
    [ValidatePathExists]
    public string ExportPath { get; set; }

    public bool OverwriteExisting { get; set; }

    #endregion

    #region Private Methods

    protected void OnPropertyChanged( [CallerMemberName] string? propertyName = null )
    {
      var eventArgs = new PropertyChangedEventArgs( propertyName );
      HandlePropertyChanged( eventArgs );
      PropertyChanged?.Invoke( this, eventArgs );
      Validate();
    }

    protected virtual void HandlePropertyChanged( PropertyChangedEventArgs eventArgs )
    {
    }

    #endregion

  }

}

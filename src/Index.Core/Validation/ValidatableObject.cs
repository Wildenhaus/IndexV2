using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Index.Validation
{

  public abstract class ValidatableObject : INotifyDataErrorInfo
  {

    #region Events

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    #endregion

    #region Data Members

    private readonly object _lock;
    private readonly Dictionary<string, List<string>> _validationErrors;

    #endregion

    #region Properties

    public bool IsValid => !HasErrors;
    public bool HasErrors => _validationErrors.Any( x => x.Value != null && x.Value.Count > 0 );

    #endregion

    #region Constructor

    protected ValidatableObject()
    {
      _lock = new object();
      _validationErrors = new Dictionary<string, List<string>>();

      Validate();
    }

    #endregion

    #region Public Methods

    public IEnumerable GetErrors( string? propertyName )
    {
      if ( string.IsNullOrEmpty( propertyName ) )
        return _validationErrors.SelectMany( x => x.Value.ToList() );

      if ( !_validationErrors.TryGetValue( propertyName, out var errors ) )
        return null;

      return errors.Count > 0 ? errors : null;
    }

    public void Validate()
    {
      lock ( _lock )
      {
        var validationContext = new ValidationContext( this, null, null );
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject( this, validationContext, validationResults, true );

        var propertyNames = _validationErrors.Keys.ToList();
        foreach ( var propertyName in propertyNames )
        {
          _validationErrors.Remove( propertyName );
          OnErrorsChanged( propertyName );
        }

        UpdateValidationErrors( validationResults );
      }
    }

    public void ValidateProperty( object value, [CallerMemberName] string propertyName = null )
    {
      lock ( _lock )
      {
        var validationContext = new ValidationContext( this, null, null );
        validationContext.MemberName = propertyName;
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateProperty( value, validationContext, validationResults );

        _validationErrors.Remove( propertyName );
        OnErrorsChanged( propertyName );

        UpdateValidationErrors( validationResults );
      }
    }

    #endregion

    #region Private Methods

    private void UpdateValidationErrors( List<ValidationResult> validationResults )
    {
      var groups = from validationResult in validationResults
                   from memberName in validationResult.MemberNames
                   group validationResult by memberName into resultGroup
                   select resultGroup;

      foreach ( var group in groups )
      {
        var propertyName = group.Key;
        var messages = group.Select( x => x.ErrorMessage ).ToList();

        // Clear previous errors
        _validationErrors.Remove( propertyName );
        OnErrorsChanged( propertyName );

        _validationErrors.Add( propertyName, messages );
        OnErrorsChanged( propertyName );
      }
    }

    protected void OnErrorsChanged( string propertyName )
    {
      var eventArgs = new DataErrorsChangedEventArgs( propertyName );
      HandleErrorsChanged( eventArgs );
      ErrorsChanged?.Invoke( this, eventArgs );
    }

    protected virtual void HandleErrorsChanged( DataErrorsChangedEventArgs eventArgs )
    {
    }

    #endregion

  }

}

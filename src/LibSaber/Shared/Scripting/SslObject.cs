using System.Dynamic;

namespace LibSaber.Shared.Scripting
{

  public class SslObject : DynamicObject
  {

    #region Data Members

    protected Dictionary<string, object> _members;

    #endregion

    #region Properties

    public object this[ string name ]
    {
      get
      {
        if ( !_members.TryGetValue( name, out var result ) )
          return null;

        return result;
      }
      set => _members[ name ] = value;
    }

    public IReadOnlyDictionary<string, object> Members
    {
      get => _members;
    }

    #endregion

    #region Constructor

    public SslObject()
    {
      _members = new Dictionary<string, object>();
    }

    #endregion

    #region Public Methods

    public void Merge( SslObject objectToMerge )
    {
      foreach ( var memberPair in objectToMerge.Members )
        _members[ memberPair.Key ] = memberPair.Value;
    }

    #endregion

    #region Overrides

    public override bool TryGetMember( GetMemberBinder binder, out object? result )
    {
      var name = binder.Name.ToLower();
      return _members.TryGetValue( name, out result );
    }

    public override bool TrySetMember( SetMemberBinder binder, object? value )
    {
      var name = binder.Name.ToLower();
      _members[ name ] = value;
      return true;
    }

    #endregion

  }

}

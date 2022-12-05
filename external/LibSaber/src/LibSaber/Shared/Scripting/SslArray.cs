using System.Dynamic;

namespace LibSaber.Shared.Scripting
{

  public class SslArray : DynamicObject
  {

    #region Data Members

    private List<dynamic> _items;

    #endregion

    #region Properties

    public int Count
    {
      get => _items.Count;
    }

    public dynamic this[ int index ]
    {
      get
      {
        if ( !GetItem( index, out var result ) )
          throw new IndexOutOfRangeException();

        return result;
      }
      set
      {
        if ( !SetItem( index, value ) )
          throw new IndexOutOfRangeException();
      }
    }

    #endregion

    #region Constructor

    public SslArray()
    {
      _items = new List<dynamic>();
    }

    #endregion

    #region Public Methods

    public void Push( dynamic value )
      => _items.Add( value );

    #endregion

    #region Overrides

    public override bool TryGetMember( GetMemberBinder binder, out object? result )
    {
      result = default;
      var name = binder.Name.ToLower();

      if ( name == nameof( Count ).ToLower() )
      {
        result = Count;
        return true;
      }

      if ( !int.TryParse( binder.Name, out var index ) )
        return false;

      return GetItem( index, out result );
    }

    public override bool TrySetMember( SetMemberBinder binder, object? value )
    {
      if ( !int.TryParse( binder.Name, out var index ) )
        return false;

      if ( index < 0 || index > Count )
        return false;

      if ( index == Count )
        _items.Add( value );
      else
        _items[ index ] = value;

      return true;
    }

    #endregion

    #region Private Methods

    private bool GetItem( int index, out dynamic result )
    {
      result = default;

      if ( index < 0 || index >= Count )
        return false;

      result = _items[ index ];
      return true;
    }

    private bool SetItem( int index, dynamic value )
    {
      if ( index < 0 || index > Count )
        return false;

      if ( index == Count )
        _items.Add( value );
      else
        _items[ index ] = value;

      return true;
    }

    #endregion

  }

}

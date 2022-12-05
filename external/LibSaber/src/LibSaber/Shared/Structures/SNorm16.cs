using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Shared.Structures
{

  [StructLayout( LayoutKind.Explicit, Size = sizeof( short ) )]
  public readonly struct SNorm16
  {

    #region Constants

    public const float Coefficient = 32767.0f;

    #endregion

    #region Data Members

    [FieldOffset( 0 )]
    private readonly short _value;

    #endregion

    #region Constructor

    public SNorm16( short value )
    {
      _value = value;
    }

    #endregion

    #region Casts

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public static implicit operator SNorm16( short shortValue )
      => new SNorm16( shortValue );

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public static implicit operator short( SNorm16 snormValue )
      => snormValue._value;

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public static implicit operator float( SNorm16 snormValue )
      => snormValue._value / Coefficient;

    #endregion

    #region Operators

    public static bool operator ==( SNorm16 left, SNorm16 right )
      => left._value == right._value;

    public static bool operator !=( SNorm16 left, SNorm16 right )
      => left._value != right._value;

    public static bool operator ==( SNorm16 left, float right )
      => ( left._value / Coefficient ) == right;

    public static bool operator !=( SNorm16 left, float right )
      => ( left._value / Coefficient ) == right;

    public static bool operator ==( float left, SNorm16 right )
      => ( right._value / Coefficient ) == left;

    public static bool operator !=( float left, SNorm16 right )
      => ( right._value / Coefficient ) == left;

    #endregion

    #region Overrides

    public override bool Equals( object obj )
    {
      if ( obj is SNorm16 snormValue )
        return _value == snormValue._value;
      if ( obj is float floatValue )
        return ( _value / Coefficient ) == floatValue;
      if ( obj is short shortValue )
        return _value == shortValue;

      return false;
    }

    #endregion

    #region Serialization

    public static SNorm16 Deserialize( NativeReader reader, ISerializationContext context )
      => new SNorm16( reader.ReadInt16() );

    #endregion

  }

}

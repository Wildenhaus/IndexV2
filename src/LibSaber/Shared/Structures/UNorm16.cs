using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Shared.Structures
{

  [StructLayout( LayoutKind.Explicit, Size = sizeof( ushort ) )]
  public readonly struct UNorm16
  {

    #region Constants

    public const float Coefficient = 65535.0f;

    #endregion

    #region Data Members

    [FieldOffset( 0 )]
    private readonly ushort _value;

    #endregion

    #region Constructor

    public UNorm16( ushort value )
    {
      _value = value;
    }

    #endregion

    #region Casts

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public static implicit operator ushort( UNorm16 unormValue )
      => unormValue._value;

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    public static implicit operator float( UNorm16 unormValue )
      => unormValue._value / Coefficient;

    #endregion

    #region Operators

    public static bool operator ==( UNorm16 left, UNorm16 right )
      => left._value == right._value;

    public static bool operator !=( UNorm16 left, UNorm16 right )
      => left._value != right._value;

    public static bool operator ==( UNorm16 left, float right )
      => ( left._value / Coefficient ) == right;

    public static bool operator !=( UNorm16 left, float right )
      => ( left._value / Coefficient ) == right;

    public static bool operator ==( float left, UNorm16 right )
      => ( right._value / Coefficient ) == left;

    public static bool operator !=( float left, UNorm16 right )
      => ( right._value / Coefficient ) == left;

    #endregion

    #region Overrides

    public override bool Equals( object obj )
    {
      if ( obj is UNorm16 unormValue )
        return _value == unormValue._value;
      if ( obj is float floatValue )
        return ( _value / Coefficient ) == floatValue;
      if ( obj is ushort ushortValue )
        return _value == ushortValue;

      return false;
    }

    #endregion

    #region Serialization

    public static UNorm16 Deserialize( NativeReader reader, ISerializationContext context )
      => new UNorm16( reader.ReadUInt16() );

    #endregion

  }

}

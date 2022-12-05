using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Shared.Structures
{

  public abstract class BitSet
  {

    #region Data Members

    protected int _count;
    protected BitArray _bitArray;

    #endregion

    #region Properties

    public bool this[ int index ]
    {
      get => _bitArray[ index ];
    }

    #endregion

    #region Constructor

    protected BitSet( int count, byte[] data )
    {
      _count = count;
      _bitArray = new BitArray( data );
    }

    #endregion

  }

  public class BitSet<TCount> : BitSet, IEnumerable<bool>
    where TCount : unmanaged, IConvertible
  {

    #region Constructor

    public BitSet( int count, byte[] data )
      : base( count, data )
    {
    }

    #endregion

    public bool this[ Enum value ]
    {
      get => HasFlag( value );
    }

    #region Public Methods

    [MethodImpl( MethodImplOptions.AggressiveOptimization )]
    public bool HasFlag<TEnum>( TEnum flag )
      where TEnum : Enum
      => _bitArray[ GetFlagIndex( flag ) ];

    [MethodImpl( MethodImplOptions.AggressiveOptimization )]
    public TEnum GetFlags<TEnum>()
      where TEnum : struct, Enum
    {
      ulong flags = 0;
      for ( var i = 0; i < _count; i++ )
        if ( this[ i ] )
          flags |= 1UL << i;

      return ( TEnum ) ( object ) flags;
    }

    #endregion

    #region Private Methods

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    private static int GetFlagIndex<TEnum>( TEnum value )
      where TEnum : Enum
    {
      var numericValue = Convert.ToUInt64( value );
      return BitOperations.Log2( numericValue );
    }

    #endregion

    #region Serialization

    public static BitSet<TCount> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var count = ReadCount( reader ).ToInt32( null );

      const int FLAGS_PER_BYTE = 8;
      var numBytes = ( count + 7 ) / FLAGS_PER_BYTE;

      var data = new byte[ numBytes ];
      reader.Read( data );

      return new BitSet<TCount>( count, data );
    }

    private static TCount ReadCount( NativeReader reader )
      => reader.ReadUnmanaged<TCount>();

    #endregion

    #region IEnumerable Methods

    public IEnumerator<bool> GetEnumerator()
    {
      for ( var i = 0; i < _count; i++ )
        yield return _bitArray[ i ];
    }

    IEnumerator IEnumerable.GetEnumerator()
      => GetEnumerator();

    #endregion

  }

}

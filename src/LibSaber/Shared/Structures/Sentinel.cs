using LibSaber.IO;

namespace LibSaber.Shared.Structures
{

  public readonly struct Sentinel<TOffset>
    where TOffset : unmanaged, IConvertible
  {

    #region Data Members

    public readonly short Id;
    public readonly long EndOffset;

    #endregion

    #region Constructor

    public Sentinel( short id, TOffset endOffset )
    {
      Id = id;
      EndOffset = endOffset.ToInt64( null );
    }

    public static Sentinel<TOffset> Read( NativeReader reader )
    {
      var id = reader.ReadUnmanaged<short>();
      var endOffset = reader.ReadUnmanaged<TOffset>();

      return new Sentinel<TOffset>( id, endOffset );
    }

    #endregion

  }

}

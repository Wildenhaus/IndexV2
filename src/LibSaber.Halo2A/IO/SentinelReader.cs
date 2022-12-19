using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.IO
{

  public sealed class SentinelReader : SentinelReader<int>
  {

    public SentinelReader( NativeReader reader )
      : base( reader )
    {
    }

  }

}

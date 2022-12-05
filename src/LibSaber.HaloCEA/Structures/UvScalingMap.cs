using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.ObjectUvScaling )]
  public class UvScalingMap : Dictionary<int, int>
  {

    /* This is a dictionary that represents how UV channels should be scaled.
     * The key is the UV channel index.
     * The value is the scaling multiplier.
     */

    #region Serialization

    public static UvScalingMap Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new UvScalingMap();

      var count = reader.ReadByte();
      for ( var i = 0; i < count; i++ )
      {
        var uvIndex = reader.ReadByte();
        var uvScale = reader.ReadInt32();
        data.Add( uvIndex, uvScale );
      }

      return data;
    }

    #endregion

  }

}

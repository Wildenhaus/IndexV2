using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.TextureListEntry )]
  public struct TextureListEntry
  {

    #region Data Members

    public string TextureName;

    #endregion

    #region Casts

    public static implicit operator string( TextureListEntry entry )
      => entry.TextureName;

    public static implicit operator TextureListEntry( string textureName )
      => new TextureListEntry { TextureName = textureName };

    #endregion

    #region Serialization

    public static TextureListEntry Deserialize( NativeReader reader, ISerializationContext context )
    {
      return reader.ReadNullTerminatedString();
    }

    #endregion

  }

}

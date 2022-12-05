using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.AnimationSequence_ActionFrames )]
  public struct ActionFrame
  {

    #region Data Members

    public byte Unk_00;
    public int Frame;
    public string Comment;

    #endregion

    #region Serialization

    public static ActionFrame Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new ActionFrame
      {
        Unk_00 = reader.ReadByte(),
        Frame = reader.ReadInt32(),
        Comment = reader.ReadLengthPrefixedString32(),
      };
    }

    #endregion

  }

}

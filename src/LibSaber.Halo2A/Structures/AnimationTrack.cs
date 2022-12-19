using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;

namespace LibSaber.Halo2A.Structures
{

  public class AnimationTrack
  {

    #region Properties

    public List<AnimationSequence> SeqList { get; set; }
    public List<ObjectAnimation> ObjAnimList { get; set; }
    public List<short> ObjMapList { get; set; }
    public AnimationRooted RootAnim { get; set; }

    #endregion

  }

}

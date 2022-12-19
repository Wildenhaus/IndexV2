namespace LibSaber.Halo2A.Structures
{

  public class AnimationSequence
  {

    #region Properties

    public string Name { get; set; }
    public uint LayerId { get; set; }
    public float StartFrame { get; set; }
    public float EndFrame { get; set; }
    public float OffsetFrame { get; set; }
    public float LenFrame { get; set; }
    public float TimeSec { get; set; }
    public List<ActionFrame> ActionFrames { get; set; }
    public Box BoundingBox { get; set; }

    #endregion

  }

}

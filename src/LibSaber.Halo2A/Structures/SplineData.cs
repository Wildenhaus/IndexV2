using LibSaber.Extensions;
using LibSaber.Halo2A.IO;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;
using static LibSaber.Assertions;

namespace LibSaber.Halo2A.Structures
{

  public struct SplineData
  {

    #region Data Members

    public SplineType SplineType;
    public byte CompressedDataSize;
    public byte Unk_02;
    public byte Unk_03;
    public uint Count;
    public uint SizeInBytes;
    public float[] Data;

    #endregion

  }

}
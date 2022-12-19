namespace LibSaber.Halo2A.IO
{

  public readonly struct H2AStreamChunk
  {

    #region Data Members

    public readonly long StartOffset;
    public readonly long EndOffset;
    public readonly long CompressedLength;
    public readonly long UncompressedLength;

    #endregion

    #region Constructor

    public H2AStreamChunk( long startOffset, long endOffset, long uncompressedLength )
    {
      StartOffset = startOffset;
      EndOffset = endOffset;
      CompressedLength = endOffset - startOffset;
      UncompressedLength = uncompressedLength;
    }

    #endregion

  }

}

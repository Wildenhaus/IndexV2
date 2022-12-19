namespace LibSaber.Halo2A.IO
{

  public struct H2AStreamCompressionInfo
  {

    #region Properties

    public int ChunkCount => Chunks.Length;

    #endregion

    #region Data Members

    public readonly H2AStreamChunk[] Chunks;
    public readonly bool IsCompressed;
    public readonly long UncompressedLength;

    #endregion

    #region Constructor

    public H2AStreamCompressionInfo( bool isCompressed, H2AStreamChunk[] chunks )
    {
      IsCompressed = isCompressed;
      Chunks = chunks;

      // Calculate the uncompressed length
      var length = 0l;
      foreach ( var chunk in chunks )
        length += chunk.UncompressedLength;

      UncompressedLength = length;
    }

    #endregion

  }

}

namespace LibSaber.HaloCEA.IO
{

  /// <summary>
  ///   Reusable information about a stream.
  /// </summary>
  public struct CEAStreamCompressionInfo
  {

    #region Properties

    public bool IsInitialized { get; set; }
    public int ChunkCount => Chunks.Length;

    #endregion

    #region Data Members

    /// <summary>
    ///   Info about each chunk.
    /// </summary>
    public readonly CEAStreamChunk[] Chunks;

    /// <summary>
    ///   The uncompressed length of the stream.
    /// </summary>
    public readonly long UncompressedLength;

    #endregion

    #region Constructor

    public CEAStreamCompressionInfo( CEAStreamChunk[] chunks )
    {
      Chunks = chunks;
      IsInitialized = true;

      // Calculate the uncompressed length
      var length = 0l;
      foreach ( var chunk in chunks )
        length += chunk.UncompressedLength;

      UncompressedLength = length;
    }

    #endregion

  }

}
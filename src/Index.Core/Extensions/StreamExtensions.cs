namespace Index
{

  public static class StreamExtensions
  {

    public static byte[] CopyToArray( this Stream stream )
    {
      var array = new byte[ stream.Length ];
      var memoryStream = new MemoryStream( array );

      stream.Position = 0;
      stream.CopyTo( memoryStream );

      return array;
    }

  }

}

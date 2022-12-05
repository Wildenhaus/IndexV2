using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSaber.HaloCEA.IO
{

  public readonly struct CEAStreamChunk
  {

    #region Data Members

    public readonly long StartOffset;
    public readonly long EndOffset;
    public readonly long CompressedLength;
    public readonly long UncompressedLength;

    #endregion

    #region Constructor

    public CEAStreamChunk( long startOffset, long endOffset, long uncompressedLength )
    {
      StartOffset = startOffset;
      EndOffset = endOffset;
      CompressedLength = endOffset - startOffset;
      UncompressedLength = uncompressedLength;
    }

    #endregion

  }

}

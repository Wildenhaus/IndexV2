using LibSaber.Extensions;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class SplineSerializer : H2ASerializerBase<Spline>
  {

    #region Overrides

    public override Spline Deserialize( NativeReader reader, ISerializationContext context )
    {
      var splineData = ReadSplineProperties( reader );
      ValidateSplineData( splineData );

      return CreateSpline( splineData );
    }

    #endregion

    #region Private Methods

    private SplineData ReadSplineProperties( NativeReader reader )
    {
      /* Splines use 16-bit sentinels to denote each property.
       * After the sentinel is a uint32 offset to the next sentinel,
       * relative to the 1SERtpl signature, and then the data.
       * 
       * The actual spline data is a list of floats. Sometimes those
       * floats are compressed into int16 values (see ReadSplineData).
       */
      var splineData = new SplineData();

      while ( true )
      {
        var sentinel = ( SplineDataSentinel ) reader.ReadUInt16();
        var endOffset = reader.ReadUInt32();

        if ( ( ushort ) sentinel == 1 )
        {
          ASSERT( reader.BaseStream.Position == endOffset,
            "End of Spline reached, but ending offset does not match." );
          break;
        }

        switch ( sentinel )
        {
          case SplineDataSentinel.SplineType:
            splineData.SplineType = ( SplineType ) reader.ReadByte();
            break;
          case SplineDataSentinel.CompressedDataSize:
            splineData.CompressedDataSize = reader.ReadByte();
            break;
          case SplineDataSentinel.Unk_02:
            splineData.Unk_02 = reader.ReadByte();
            break;
          case SplineDataSentinel.Unk_03:
            splineData.Unk_03 = reader.ReadByte();
            break;
          case SplineDataSentinel.Count:
            splineData.Count = reader.ReadUInt32();
            break;
          case SplineDataSentinel.SizeInBytes:
            splineData.SizeInBytes = reader.ReadUInt32();
            break;
          case SplineDataSentinel.Data:
            splineData.Data = ReadSplineData( reader, splineData );
            break;
          default:
            FAIL( "Unknown Spline Property Sentinel: {0:X}", ( ushort ) sentinel );
            break;
        }

        ASSERT( reader.BaseStream.Position == endOffset,
          "Unexpected stream position after Spline Property Read. " +
          "Expected: 0x{0:X}, Actual: 0x{1:X}",
          endOffset,
          reader.Position );
      }

      return splineData;
    }

    private float[] ReadSplineData( NativeReader reader, SplineData splineData )
    {
      // TODO: Verify this is the correct way to uncompess the data.
      // If CompressedDataSize > 0, the data has been compressed.
      // This appears to be SNorm compression (float -> int16).
      if ( splineData.CompressedDataSize == 2 )
      {
        var elementCount = splineData.SizeInBytes / sizeof( short );

        var data = new float[ elementCount ];
        for ( var i = 0; i < elementCount; i++ )
          data[ i ] = reader.ReadInt16().SNormToFloat();

        // TODO: Odd SizeInBytes values seem to be an issue for a few files.
        // I'm just skipping the last byte in these cases. Verify this is correct.
        var elementRemainder = splineData.SizeInBytes % sizeof( short );
        if ( elementRemainder > 0 )
          reader.ReadByte();

        return data;
      }
      else
      {
        ASSERT( splineData.CompressedDataSize == 0,
          $"Unknown compressed data size for Spline: {splineData.CompressedDataSize}" );

        var elementCount = splineData.SizeInBytes / sizeof( float );

        var data = new float[ elementCount ];
        for ( var i = 0; i < elementCount; i++ )
          data[ i ] = reader.ReadFloat32();

        return data;
      }
    }

    private Spline CreateSpline( SplineData splineData )
    {
      Spline spline;

      var splineType = splineData.SplineType;
      switch ( splineData.SplineType )
      {
        case SplineType.Linear1D:
          return new SplineLinear1D( splineData );
        case SplineType.Linear2D:
          return new SplineLinear2D( splineData );
        case SplineType.Linear3D:
          return new SplineLinear3D( splineData );
        case SplineType.Hermit:
          return new SplineHermit( splineData );
        case SplineType.Bezier2D:
          return new SplineBezier2D( splineData );
        case SplineType.Bezier3D:
          return new SplineBezier3D( splineData );
        case SplineType.Lagrange:
          return new SplineLagrange( splineData );
        case SplineType.Quat:
          return new SplineQuat( splineData );
        case SplineType.Color:
          return new SplineQuat( splineData );
        default:
          throw new NotImplementedException( $"Spline Type {splineType} is not implemented." );
      }
    }

    #endregion

    #region Validation Methods

    private void ValidateSplineData( SplineData splineData )
    {
      // Assert valid SplineType
      var isSplineTypeDefined = Enum.IsDefined( typeof( SplineType ), splineData.SplineType );
      ASSERT( isSplineTypeDefined, $"Invalid Spline Type: {splineData.SplineType:X}" );

      // Assert Valid Count
      ASSERT( splineData.Count > 0, $"Invalid Spline Count: {splineData.Count}" );

      // Assert Valid SizeInBytes
      var count = splineData.Count;
      var sizeInBytes = splineData.SizeInBytes;
      if ( splineData.CompressedDataSize == 0 )
        ASSERT( sizeInBytes % count == 0, "Invalid Spline SizeInBytes." );
    }

    #endregion

    #region Embedded Types

    private enum SplineDataSentinel : ushort
    {
      SplineType = 0xF0,
      CompressedDataSize = 0xF1,
      Unk_02 = 0xF2, // TODO: Appears to be a dimension of the data.
      Unk_03 = 0xF3, // TODO: Same as above.
      Count = 0xF4,
      SizeInBytes = 0xF5,
      Data = 0xF6
    }

    #endregion

  }

}

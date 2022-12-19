using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class TemplateSerializer : H2ASerializerBase<Template>
  {

    #region Constants

    private const uint SIGNATURE_1SER = 0x52455331; //1SER
    private const uint SIGNATURE_TPL = 0x006C7074; //TPL.

    private const uint SIGNATURE_TPL1 = 0x314C5054; //TPL1

    #endregion

    #region Overrides

    public override Template Deserialize( NativeReader reader, ISerializationContext context )
    {
      var template = new Template();

      ReadSerTplHeader( reader );
      ReadSignature( reader, SIGNATURE_TPL1 );

      var propertyFlags = BitSet<int>.Deserialize( reader, context );

      if ( propertyFlags.HasFlag( TemplatePropertyFlags.Name ) )
        ReadNameProperty( reader, template );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.NameClass ) )
        ReadNameClassProperty( reader, template );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.State ) )
        ReadStateProperty( reader, template );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.Affixes ) )
        ReadAffixesProperty( reader, template );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.PS ) )
        ReadPsProperty( reader, template );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.Skin ) )
        ReadSkinProperty( reader, template );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.TrackAnim ) )
        ReadTrackAnimProperty( reader, template, context );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.OnReadAnimExtra ) )
        ReadOnReadAnimExtraProperty( reader, template );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.BoundingBox ) )
        ReadBoundingBoxProperty( reader, template );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.LodDefinition ) )
        ReadLodDefinitionProperty( reader, template, context );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.TextureList ) )
        ReadTexListProperty( reader, template );
      if ( propertyFlags.HasFlag( TemplatePropertyFlags.GeometryMNG ) )
        ReadGeometryMngProperty( reader, template, context );

      return template;
    }

    #endregion

    #region Private Methods

    private void ReadSerTplHeader( NativeReader reader )
    {
      ReadSignature( reader, SIGNATURE_1SER );
      ReadSignature( reader, SIGNATURE_TPL );

      _ = reader.ReadUInt64(); // Unk count
      _ = reader.ReadUInt64(); // Unk count
      _ = reader.ReadUInt64(); // Unk size

      _ = reader.ReadInt32(); // Unk flags
      var guid = reader.ReadGuid();
      _ = reader.ReadInt32(); // Unk

      var stringCount = reader.ReadInt32(); // Unk
      _ = reader.ReadInt32(); // Unk

      for ( var i = 0; i < stringCount; i++ )
      {
        _ = reader.ReadUInt16(); // Unk
        _ = reader.ReadByte(); // Unk

        _ = reader.ReadByte(); // Delimiter?
        _ = reader.ReadInt64(); // Unk
        _ = reader.ReadInt64(); // Unk
        _ = reader.ReadLengthPrefixedString32();
      }
    }

    private void ReadNameProperty( NativeReader reader, Template template )
    {
      /* This is the name of the Template.
       * LengthPrefixed String (int32)
       */
      template.Name = reader.ReadLengthPrefixedString32();
    }

    private void ReadNameClassProperty( NativeReader reader, Template template )
    {
      /* Not sure what this is. Haven't encountered any files with it yet.
       * RTTI states that it's a LengthPrefixed String (int32)
       */
      template.NameClass = reader.ReadLengthPrefixedString32();
    }

    private void ReadStateProperty( NativeReader reader, Template template )
    {
      // TODO
      /* Not sure what this is. It is present in a majority of files.
       * 48 bits in length. Possibly bitfield flags.
       */
      var unk0 = reader.ReadUInt16(); // TODO
      var unk1 = reader.ReadUInt32(); // TODO
    }

    private void ReadAffixesProperty( NativeReader reader, Template template )
    {
      // TODO
      /* A bunch of export/attribute strings. Not sure what they're used for.
       * RTTI says this uses a special string serializer.
       * There seems to be a delimiter between each string. It might be deserialized to a list.
       * Represented as a LengthPrefixed String (int32)
       */
      template.Affixes = reader.ReadLengthPrefixedString32();
    }

    private void ReadPsProperty( NativeReader reader, Template template )
    {
      /* Some sort of scripting tied to the Template.
       * Most of the time this is just a one-line script denoting a base type.
       * RTTI says it uses a special serializer.
       * Represented as a LengthPrefixed String (int32)
       */
      template.PS = reader.ReadLengthPrefixedString32();
    }

    private void ReadSkinProperty( NativeReader reader, Template template )
    {
      // TODO
      /* Not sure what this is. Maybe skinning data, but tied to the whole template.
       * It's a list of 4x4 matrices. Sometimes it cuts off early.
       */
      var count = reader.ReadUInt32();
      var unk_0 = reader.ReadUInt16(); // TODO
      var unk_1 = reader.ReadByte(); // TODO

      // Sometimes the count is positive, but the data ends immediately after.
      var endFlag = reader.ReadUInt16();
      if ( endFlag != 0xFFFF )
      {
        var endOffset = reader.ReadUInt32();

        for ( var i = 0; i < count; i++ )
          reader.ReadMatrix4x4();

        ASSERT( endOffset == reader.BaseStream.Position, "Invalid end position for TPL1 Skin property." );

        endFlag = reader.ReadUInt16();
      }

      ASSERT( endFlag == 0xFFFF, "Invalid read of TPL1.skin property" );
      ASSERT( reader.ReadUInt32() == reader.BaseStream.Position, "Invalid end position for TPL1 Skin property." );
    }

    private void ReadTrackAnimProperty( NativeReader reader, Template template, ISerializationContext context )
    {
      /* Animation Tracks for the Template.
       */
      var serializer = new AnimationTrackSerializer();
      template.AnimTrack = serializer.Deserialize( reader, context );
    }

    private void ReadOnReadAnimExtraProperty( NativeReader reader, Template template )
    {
      /* Not sure what this is. No files seem to use it.
       * Keeping it here so we can throw an exception if it ever pops up.
       */
      throw new NotImplementedException();
    }

    private void ReadBoundingBoxProperty( NativeReader reader, Template template )
    {
      /* Bounding box for the whole Template.
       */
      template.BoundingBox = new Box( reader.ReadVector3(), reader.ReadVector3() );
    }

    private void ReadLodDefinitionProperty( NativeReader reader, Template template, ISerializationContext context )
    {
      /* Level-of-detail definitions for the Template.
       */
      var lodDefSerializer = new LodDefinitionSerializer();
      template.LodDefinitions = lodDefSerializer.Deserialize( reader, context );
    }

    private void ReadTexListProperty( NativeReader reader, Template template )
    {
      /* A list of common textures that the Template references.
       * Not sure how it's used.
       * Its a list of LengthPrefixed Strings (int16)
       */
      var count = reader.ReadUInt32();
      var unk0 = reader.ReadUInt16(); // TODO: Always 0?
      var endOffset = reader.ReadUInt32();

      template.TexList = new List<string>( ( int ) count );
      for ( var i = 0; i < count; i++ )
        template.TexList.Add( reader.ReadLengthPrefixedString16() );

      var endMarker = reader.ReadUInt16();
      _ = reader.ReadUInt32(); // EndOffset, points to next data
      ASSERT( endMarker == 0xFFFF, "Invalid EndMarker for TexList Property." );
    }

    private void ReadGeometryMngProperty( NativeReader reader, Template template, ISerializationContext context )
    {
      /* Geometry (Multi-Node Graph?) Data
       * Contains most of the model info.
       */
      var geometryGraphSerializer = new GeometryGraphSerializer();
      template.GeometryGraph = geometryGraphSerializer.Deserialize( reader, context );
    }

    #endregion

    #region Property Flags

    [Flags]
    public enum TemplatePropertyFlags : ushort
    {
      Name = 1 << 0,
      NameClass = 1 << 1,
      State = 1 << 2,
      Affixes = 1 << 3,
      PS = 1 << 4,
      Skin = 1 << 5,
      TrackAnim = 1 << 6,
      OnReadAnimExtra = 1 << 7,
      BoundingBox = 1 << 8,
      LodDefinition = 1 << 9,
      TextureList = 1 << 10,
      GeometryMNG = 1 << 11,
      ExternData = 1 << 12
    }

    #endregion

  }

}

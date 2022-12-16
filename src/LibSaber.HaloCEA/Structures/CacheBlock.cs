using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSaber.IO;

namespace LibSaber.HaloCEA.Structures
{

  public class CacheBlock
  {

    #region Data Members

    public int SectionCount;
    public int EntryCount;
    public int DataSize;
    public int Unk;
    public CacheBlockSection[] Sections;

    #endregion

    #region Serialization

    public static CacheBlock Deserialize( NativeReader reader )
    {
      var block = new CacheBlock();

      var sectionCount = block.SectionCount = reader.ReadInt32();
      block.EntryCount = reader.ReadInt32();
      block.DataSize = reader.ReadInt32();
      block.Unk = reader.ReadInt32();

      var sections = block.Sections = new CacheBlockSection[ sectionCount ];
      for ( var i = 0; i < sectionCount; i++ )
        sections[ i ] = CacheBlockSection.Deserialize( reader );

      foreach ( var section in sections )
        foreach ( var entry in section.Entries )
          entry.Data = reader.ReadFixedLengthString( entry.DataSize );

      return block;
    }

    #endregion

  }

  public class CacheBlockSection
  {

    #region Data Members

    public int DataSize;
    public int EntryCount;
    public CacheBlockEntry[] Entries;

    #endregion

    #region Serialization

    public static CacheBlockSection Deserialize( NativeReader reader )
    {
      var section = new CacheBlockSection();

      section.DataSize = reader.ReadInt32();
      var entryCount = section.EntryCount = reader.ReadInt32();

      var entries = section.Entries = new CacheBlockEntry[ entryCount ];
      for ( var i = 0; i < entryCount; i++ )
        entries[ i ] = CacheBlockEntry.Deserialize( reader );

      return section;
    }

    #endregion

  }

  public class CacheBlockEntry
  {

    #region Data Members

    public int NameCrc;
    public int DataSize;
    public string Name;

    public string Data;

    #endregion

    #region Serialization

    public static CacheBlockEntry Deserialize( NativeReader reader )
    {
      return new CacheBlockEntry
      {
        NameCrc = reader.ReadInt32(),
        DataSize = reader.ReadInt32(),
        Name = reader.ReadLengthPrefixedString32()
      };
    }

    #endregion

  }

}

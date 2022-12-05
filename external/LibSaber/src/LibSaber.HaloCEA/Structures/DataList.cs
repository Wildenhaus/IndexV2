using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.HaloCEA.Structures
{

  public class DataList<T> : List<T>
  {

    #region Serialization

    public static DataList<T> Deserialize( NativeReader reader, ISerializationContext context, Func<NativeReader, ISerializationContext, T> elemFunc )
    {
      var list = new DataList<T>();

      var count = reader.ReadInt32();
      for ( var i = 0; i < count; i++ )
        list.Add( elemFunc( reader, context ) );

      return list;
    }

    #endregion

  }
}

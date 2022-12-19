using System.IO;
using LibSaber.IO;

namespace LibSaber.Halo2A.Serialization.Scripting
{

  public interface IBinaryScriptingSerializer : IScriptingSerializer
  {

    dynamic Deserialize( NativeReader reader );

  }

  public interface IBinaryConfigurationSerializer<T> : IBinaryScriptingSerializer
    where T : class, new()
  {

    new T Deserialize( NativeReader reader );

  }

}

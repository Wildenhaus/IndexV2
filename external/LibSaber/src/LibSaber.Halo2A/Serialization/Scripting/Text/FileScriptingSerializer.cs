using System;
using System.Collections.Generic;

namespace LibSaber.Halo2A.Serialization.Scripting
{

  public class FileScriptingSerializer<T> : TextScriptingSerializer<T>
    where T : class, new()
  {

    #region Data Members

    private readonly Dictionary<Type, IScriptingSerializer> _serializerCache
      = new Dictionary<Type, IScriptingSerializer>();

    #endregion

    #region Overrides

    protected override Dictionary<Type, IScriptingSerializer> GetSerializerCache()
      => _serializerCache;

    #endregion

  }

}

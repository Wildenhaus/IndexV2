namespace LibSaber.Shared.Scripting
{

  public enum SslTokenType
  {
    Unknown,

    Assignment,
    Accessor,
    Identifier,
    Comment,
    Separator,

    Boolean,
    String,
    Integer,
    Double,
    Script,

    StartArray,
    EndArray,
    StartObject,
    EndObject,

  }

}

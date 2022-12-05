namespace LibSaber.Shared.Attributes
{

  public class SaberInternalNameAttribute : Attribute
  {

    #region Data Members

    public readonly string InternalName;

    #endregion

    #region Constructor

    public SaberInternalNameAttribute( string internalName )
    {
      InternalName = internalName;
    }

    #endregion

  }

}

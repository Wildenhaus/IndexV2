namespace LibSaber.Shared.Attributes
{

  [AttributeUsage( AttributeTargets.All, AllowMultiple = true )]
  public class SentinelAttribute : Attribute
  {

    #region Data Members

    public readonly short SentinelId;

    #endregion

    #region Constructor

    public SentinelAttribute( short sentinelId )
    {
      SentinelId = sentinelId;
    }

    #endregion

  }

}

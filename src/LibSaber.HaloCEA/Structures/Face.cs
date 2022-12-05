namespace LibSaber.HaloCEA.Structures
{

  public struct Face
  {

    #region Data Members

    public short A;
    public short B;
    public short C;

    #endregion

    #region Constructor

    public Face( short a, short b, short c )
    {
      A = a;
      B = b;
      C = c;
    }

    #endregion

  }

}

namespace LibSaber.Common
{

  public abstract class DisposableObject : IDisposable
  {

    #region Properties

    public bool IsDisposed { get; private set; }

    #endregion

    #region Destructor

    ~DisposableObject()
    {
      Dispose( false );
    }

    #endregion

    #region Methods

    public void Dispose()
    {
      Dispose( true );
    }

    private void Dispose( bool disposing )
    {
      if ( IsDisposed )
        return;

      if ( disposing )
        OnDisposing();

      IsDisposed = true;
    }

    protected virtual void OnDisposing()
    {
    }

    #endregion

  }

}

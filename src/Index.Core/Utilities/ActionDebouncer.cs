namespace Index.Utilities
{

  public class ActionDebouncer
  {

    private Timer _timer;
    private readonly int _interval;
    private Action _action;

    public ActionDebouncer( int interval, Action action )
    {
      _action = action;
      _interval = interval;
    }

    public void Invoke()
    {
      _timer?.Dispose();
      _timer = new Timer( OnDebouceTimerTick, null, _interval, _interval );
    }

    private void OnDebouceTimerTick( object? state )
    {
      _action();
      _timer?.Dispose();
    }

  }

}

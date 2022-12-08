using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index.Utilities
{

  // Based on https://gist.github.com/mstepura/7ded78c66927114aa8fa

  public class ActionThrottler : IDisposable
  {

    #region Constants

    private const int STATE_IDLE = 0;
    private const int STATE_EXECUTING = 1;
    private const int STATE_DISPOSING = 2;

    #endregion

    #region Data Members

    private readonly Action _action;
    private readonly TimeSpan _delay;
    private readonly Timer _timer;

    private int _state;
    private bool _runAgain;

    #endregion

    #region Constructor

    public ActionThrottler( Action action, TimeSpan delay )
    {
      _action = action;
      _delay = delay;
      _timer = new Timer( OnExecute, null, Timeout.Infinite, Timeout.Infinite );

      _state = STATE_IDLE;
    }

    public ActionThrottler( Action action, int delayMilliseconds )
      : this( action, TimeSpan.FromMilliseconds( delayMilliseconds ) )
    {
    }

    #endregion

    #region Public Methods

    public void Execute()
    {
      var currentState = Interlocked.CompareExchange( ref _state, STATE_EXECUTING, STATE_IDLE );
      if ( currentState == STATE_IDLE )
        _timer.Change( _delay, Timeout.InfiniteTimeSpan );
      else
        _runAgain = true;
    }

    #endregion

    #region Private Methods

    private void OnExecute( object state )
    {
      try
      {
        _runAgain = false;
        _action();
      }
      finally
      {
        Interlocked.CompareExchange( ref _state, STATE_IDLE, STATE_EXECUTING );
        if ( _runAgain )
          Execute();
      }
    }

    #endregion

    #region IDisposable Methods

    public void Dispose()
    {
      var state = Interlocked.Exchange( ref _state, STATE_DISPOSING );
      if ( state == STATE_IDLE )
      {
        _timer.Dispose();
      }
      else if ( state == STATE_EXECUTING )
      {
        using ( var waitHandle = new ManualResetEvent( false ) )
        {
          if ( _timer.Dispose( waitHandle ) )
            waitHandle.WaitOne();
        }
      }
    }

    #endregion

  }

}

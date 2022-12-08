using System.Runtime.InteropServices;
using NativePoint = System.Drawing.Point;
using Point = System.Windows.Point;

namespace Index.UI.Common
{

  public static class Win32
  {

    #region Imports

    [DllImport( "user32.dll", CharSet = CharSet.Auto, ExactSpelling = true )]
    private static extern short GetAsyncKeyState( int keyId );

    [DllImport( "user32.dll" )]
    private static extern bool GetCursorPos( out NativePoint Point );

    [DllImport( "user32.dll" )]
    private static extern int SetCursorPos( int X, int Y );

    #endregion

    #region Public Methods

    public static bool IsKeyPressed( WinKeys key )
      => ( GetAsyncKeyState( ( int ) key ) & 32768 ) != 0;

    public static bool GetCursorPosition( out Point point )
    {
      point = default;

      if ( !GetCursorPos( out var result ) )
        return false;

      point = new Point( result.X, result.Y );
      return true;
    }

    public static int SetCursorPosition( Point point )
      => SetCursorPos( ( int ) point.X, ( int ) point.Y );

    #endregion

  }

  public enum WinKeys : int
  {
    A = 65,
    B = 66,
    C = 67,
    D = 68,
    E = 69,
    F = 70,
    G = 71,
    H = 72,
    I = 73,
    J = 74,
    K = 75,
    L = 76,
    M = 77,
    N = 78,
    O = 79,
    P = 80,
    Q = 81,
    R = 82,
    S = 83,
    T = 84,
    U = 85,
    V = 86,
    W = 87,
    X = 88,
    Y = 89,
    Z = 90,
    Add = 107,
    Subtract = 109,
    Shift = 0x10,
  }

}

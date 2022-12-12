using System;
using System.Runtime.InteropServices;
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

    [DllImport( "user32" )]
    internal static extern bool GetMonitorInfo( IntPtr hMonitor, NativeMonitorInfo lpmi );

    [DllImport( "user32" )]
    internal static extern IntPtr MonitorFromWindow( IntPtr handle, int flags );

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

      point = new Point( result.x, result.y );
      return true;
    }

    public static int SetCursorPosition( Point point )
      => SetCursorPos( ( int ) point.X, ( int ) point.Y );

    public static void WmGetMinMaxInfo( IntPtr hWnd, IntPtr lParam, int minWidth, int minHeight )
    {
      var mmi = Marshal.PtrToStructure<NativeMinMaxInfo>( lParam );

      const int MONITOR_DEFAULT_TO_NEAREST = 0x00000002;
      var monitor = MonitorFromWindow( hWnd, MONITOR_DEFAULT_TO_NEAREST );

      if ( monitor != IntPtr.Zero )
      {
        var monitorInfo = new NativeMonitorInfo();
        GetMonitorInfo( monitor, monitorInfo );

        var rcWorkArea = monitorInfo.rcWork;
        var rcMonitorArea = monitorInfo.rcMonitor;

        mmi.ptMaxPosition.x = Math.Abs( rcWorkArea.left - rcMonitorArea.left );
        mmi.ptMaxPosition.y = Math.Abs( rcWorkArea.top - rcMonitorArea.top );
        mmi.ptMaxSize.x = Math.Abs( rcWorkArea.right - rcWorkArea.left );
        mmi.ptMaxSize.y = Math.Abs( rcWorkArea.bottom - rcWorkArea.top );
        mmi.ptMinTrackSize.x = minWidth;
        mmi.ptMinTrackSize.y = minHeight;
      }

      Marshal.StructureToPtr( mmi, lParam, true );
    }

    #endregion

  }

  #region Native Structs

  [StructLayout( LayoutKind.Explicit )]
  public struct NativeMinMaxInfo
  {

    #region Data Members

    [FieldOffset( 0x00 )] public NativePoint ptReserved;
    [FieldOffset( 0x08 )] public NativePoint ptMaxSize;
    [FieldOffset( 0x10 )] public NativePoint ptMaxPosition;
    [FieldOffset( 0x18 )] public NativePoint ptMinTrackSize;
    [FieldOffset( 0x20 )] public NativePoint ptMaxTrackSize;

    #endregion

  };

  [StructLayout( LayoutKind.Explicit )]
  public struct NativePoint
  {

    #region Data Members

    [FieldOffset( 0x00 )] public int x;
    [FieldOffset( 0x04 )] public int y;

    #endregion

    #region Constructor

    public NativePoint( int x, int y )
    {
      this.x = x;
      this.y = y;
    }

    #endregion

  }

  [StructLayout( LayoutKind.Explicit, Pack = 0 )]
  public struct NativeRect
  {

    #region Data Members

    public static readonly NativeRect Empty = new NativeRect();

    [FieldOffset( 0x0 )] public int left;
    [FieldOffset( 0x4 )] public int top;
    [FieldOffset( 0x8 )] public int right;
    [FieldOffset( 0xC )] public int bottom;

    #endregion

    #region Properties

    public int Width => Math.Abs( right - left );
    public int Height => bottom - top;
    public bool IsEmpty => left >= right || top >= bottom;

    #endregion

    #region Constructor

    public NativeRect( int left, int top, int right, int bottom )
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    public NativeRect( NativeRect src )
    {
      left = src.left;
      top = src.top;
      right = src.right;
      bottom = src.bottom;
    }

    #endregion

    #region Overrides

    public override string ToString()
    {
      if ( this == NativeRect.Empty )
        return "NativeRect[ Empty ]";

      return $"NativeRect[ Left: {left}, Top: {top}, Right: {right}, Bottom: {bottom} ]";
    }

    public override bool Equals( object obj )
    {
      if ( obj is not NativeRect nativeRect )
        return false;

      return ( this == nativeRect );
    }

    public override int GetHashCode()
      => HashCode.Combine( left, top, right, bottom );

    #endregion

    #region Operators

    public static bool operator ==( NativeRect a, NativeRect b )
    {
      return a.left == b.left
          && a.top == b.top
          && a.right == b.right
          && a.bottom == b.bottom;
    }

    public static bool operator !=( NativeRect a, NativeRect b )
      => !( a == b );

    #endregion

  }

  [StructLayout( LayoutKind.Explicit, CharSet = CharSet.Auto )]
  public class NativeMonitorInfo
  {

    #region Data Members

    [FieldOffset( 0x00 )] public int cbSize;
    [FieldOffset( 0x04 )] public NativeRect rcMonitor;
    [FieldOffset( 0x14 )] public NativeRect rcWork;
    [FieldOffset( 0x24 )] public int dwFlags;

    #endregion

    #region Constructor

    public NativeMonitorInfo()
    {
      cbSize = Marshal.SizeOf( typeof( NativeMonitorInfo ) );
      rcMonitor = new NativeRect();
      rcWork = new NativeRect();
      dwFlags = 0;
    }

    #endregion

  }

  #endregion

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

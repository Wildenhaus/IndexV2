using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Index.UI.Controls
{

  public class IxIcon : ContentControl
  {

    #region Properties

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
      nameof( Icon ),
      typeof( object ),
      typeof( IxIcon ),
      new PropertyMetadata( null, OnIconChanged ) );

    public object Icon
    {
      get => ( object ) GetValue( IconProperty );
      set => SetValue( IconProperty, value );
    }

    public static readonly DependencyProperty SegoeIconProperty = DependencyProperty.Register(
      nameof( SegoeIcon ),
      typeof( SegoeIcon ),
      typeof( IxIcon ) );

    public SegoeIcon SegoeIcon
    {
      get => ( SegoeIcon ) GetValue( SegoeIconProperty );
      set
      {
        SetValue( SegoeIconProperty, value );
        SetValue( PathProperty, null );
      }
    }

    public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
      nameof( Path ),
      typeof( Path ),
      typeof( IxIcon ) );

    public Path Path
    {
      get => ( Path ) GetValue( PathProperty );
      set
      {
        SetValue( PathProperty, value );
        SetValue( SegoeIconProperty, SegoeIcon.None );
      }
    }

    #endregion

    #region Constructor

    static IxIcon()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxIcon ),
        new FrameworkPropertyMetadata( typeof( IxIcon ) ) );
    }

    #endregion

    #region Event Handlers

    private static void OnIconChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
    {
      var icon = ( IxIcon ) d;

      if ( e.NewValue is SegoeIcon segoeIcon )
        icon.SegoeIcon = segoeIcon;
      else if ( e.NewValue is Path path )
        icon.Path = path;
    }

    #endregion

  }

}

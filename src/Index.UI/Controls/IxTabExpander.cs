using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Index.UI.Extensions;

namespace Index.UI.Controls
{

  public class IxTabExpander : TabControl
  {

    #region Data Members

    private Grid _grid;
    private ColumnDefinition _columnDefinition;
    private RowDefinition _rowDefinition;

    private int _storedIndex;
    private GridLength _storedLength;

    #endregion

    #region Properties

    public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(
      nameof( ExpandDirection ),
      typeof( ExpandDirection ),
      typeof( IxTabExpander ) );

    public ExpandDirection ExpandDirection
    {
      get => ( ExpandDirection ) GetValue( ExpandDirectionProperty );
      set => SetValue( ExpandDirectionProperty, value );
    }

    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
      nameof( IsExpanded ),
      typeof( bool ),
      typeof( IxTabExpander ),
      new PropertyMetadata( true, OnExpandedChanged ) );

    public bool IsExpanded
    {
      get => ( bool ) GetValue( IsExpandedProperty );
      set => SetValue( IsExpandedProperty, value );
    }

    #endregion

    #region Constructor

    static IxTabExpander()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxTabExpander ),
        new FrameworkPropertyMetadata( typeof( IxTabExpander ) ) );
    }

    public IxTabExpander()
    {
    }

    #endregion

    #region Overrides

    protected override void OnInitialized( EventArgs e )
    {
      base.OnInitialized( e );

      var grid = _grid = this.FindParent<Grid>();
      if ( grid is null )
        throw new NotSupportedException( "IxTabExpander must be inside of a Grid." );

      var columnDefinitions = grid.ColumnDefinitions;
      if ( columnDefinitions.Count > 0 )
        _columnDefinition = columnDefinitions[ Grid.GetColumn( this ) ];

      var rowDefinitions = grid.RowDefinitions;
      if ( rowDefinitions.Count > 0 )
        _rowDefinition = rowDefinitions[ Grid.GetRow( this ) ];
    }

    protected override void OnPropertyChanged( DependencyPropertyChangedEventArgs e )
    {
      base.OnPropertyChanged( e );
      if ( e.Property == SelectedIndexProperty )
      {
        var index = ( int ) e.NewValue;
        if ( index == -1 )
          return;

        _storedIndex = index;
        IsExpanded = true;
      }
    }

    #endregion

    #region Event Handlers

    private static void OnExpandedChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
    {
      var control = d as IxTabExpander;
      if ( control is null )
        return;

      var isExpanded = ( bool ) e.NewValue;
      if ( isExpanded )
        control.ExpandPanel();
      else
        control.CollapsePanel();
    }

    #endregion

    private void CollapsePanel()
    {
      switch ( ExpandDirection )
      {
        case ExpandDirection.Up:
        case ExpandDirection.Down:
          _storedLength = _rowDefinition.Height;
          _rowDefinition.Height = GridLength.Auto;
          break;
      }

      _storedIndex = SelectedIndex;
      SelectedIndex = -1;
    }

    private void ExpandPanel()
    {
      switch ( ExpandDirection )
      {
        case ExpandDirection.Up:
        case ExpandDirection.Down:
          _rowDefinition.Height = _storedLength;
          break;
      }

      SelectedIndex = _storedIndex;
    }

  }

}

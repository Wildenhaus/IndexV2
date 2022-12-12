using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Index.Modules.TextureEditor.Views
{

  public partial class TextureInformationView : UserControl
  {

    #region Properties

    public static readonly DependencyProperty TextureInformationProperty = DependencyProperty.Register(
      nameof( TextureInformation ),
      typeof( IEnumerable<(string, string)> ),
      typeof( TextureInformationView ),
      new FrameworkPropertyMetadata( null, OnTextureInformationPropertyChanged ) );

    public IEnumerable<(string, string)> TextureInformation
    {
      get => ( IEnumerable<(string, string)> ) GetValue( TextureInformationProperty );
      set => SetValue( TextureInformationProperty, value );
    }

    #endregion

    #region Constructor

    public TextureInformationView()
    {
      InitializeComponent();
      UpdateInfoPanel();
    }

    #endregion

    #region Event Handlers

    private static void OnTextureInformationPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
    {
      var control = d as TextureInformationView;
      if ( control is null )
        return;

      control.UpdateInfoPanel();
    }

    #endregion

    #region Private Methods

    private void UpdateInfoPanel()
    {
      if ( InfoGrid is null || TextureInformation is null )
        return;

      InfoGrid.Children.Clear();
      InfoGrid.RowDefinitions.Clear();

      foreach ( var info in TextureInformation )
        AddInfoEntry( info );
    }

    private void AddInfoEntry( (string, string) info )
    {
      (string key, string value) = info;

      var rowIndex = InfoGrid.RowDefinitions.Count;
      var rowDefinition = new RowDefinition();
      InfoGrid.RowDefinitions.Add( rowDefinition );

      var keyText = new TextBlock
      {
        Text = key,
        TextAlignment = TextAlignment.Left
      };
      Grid.SetRow( keyText, rowIndex );
      Grid.SetColumn( keyText, 0 );
      InfoGrid.Children.Add( keyText );

      var valueText = new TextBlock
      {
        Text = value,
        TextAlignment = TextAlignment.Right
      };
      Grid.SetRow( valueText, rowIndex );
      Grid.SetColumn( valueText, 2 );
      InfoGrid.Children.Add( valueText );
    }

    #endregion

  }

}

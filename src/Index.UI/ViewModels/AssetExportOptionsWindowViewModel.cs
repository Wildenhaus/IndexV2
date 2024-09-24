using System.Windows.Input;
using Index.Domain.Assets;
using Index.UI.Controls.Buttons;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace Index.UI.ViewModels
{

  public class AssetExportOptionsWindowViewModel<TOptions> : DialogWindowViewModel
    where TOptions : AssetExportOptions, new()
  {

    #region Data Members

    private AssetExportOptionsViewModelBase<TOptions> _base;

    #endregion

    #region Properties

    public TOptions Options => _base?.Options;
    public ICommand ExportCommand { get; private set; }

    #endregion

    #region Constructor

    public AssetExportOptionsWindowViewModel( IContainerProvider container )
      : base( container )
    {
      ExportCommand = new DelegateCommand( Export, () => Options?.IsValid ?? false );
    }

    #endregion

    #region Overrides

    protected override void OnConfigureButtons( DialogButtonBuilder builder )
    {
      builder.AddButton().Content( "Export" ).Command( ExportCommand );
    }

    public override void OnDialogOpened( IDialogParameters parameters )
    {
      base.OnDialogOpened( parameters );

      var assetReference = Parameters.GetValue<IAssetReference>( "AssetReference" );
      Title = $"Export Options | {assetReference.AssetName}";

      var assetType = assetReference.AssetType;
      _base = new AssetExportOptionsViewModelBase<TOptions>( assetType, Container );

      Options.PropertyChanged += OnOptionsPropertyChanged;
      OnOptionsPropertyChanged( null, null );
    }

    public override void OnDialogClosed()
    {
      Options.PropertyChanged -= OnOptionsPropertyChanged;
      _base.SaveOptions();
    }

    #endregion

    #region Private Methods

    private void Export()
    {
      Parameters.Add( "Options", Options );
      CloseDialog( Parameters );
    }
    #endregion

    #region Event Handlers

    private void OnOptionsPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
      => ( ExportCommand as DelegateCommand ).RaiseCanExecuteChanged();

    #endregion

  }

}

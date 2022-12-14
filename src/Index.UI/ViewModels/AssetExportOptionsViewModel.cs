using System.Windows.Input;
using Index.Domain.Assets;
using Index.UI.Controls.Buttons;
using Prism.Commands;
using Prism.Ioc;

namespace Index.UI.ViewModels
{

  public class AssetExportOptionsViewModel<TOptions> : DialogWindowViewModel
    where TOptions : AssetExportOptions, new()
  {

    #region Properties

    public TOptions Options { get; }
    public ICommand ExportCommand { get; }

    #endregion

    #region Constructor

    public AssetExportOptionsViewModel( IContainerProvider container )
      : base( container )
    {
      Options = new TOptions();
      ExportCommand = new DelegateCommand( Export, () => Options.IsValid );

      Options.PropertyChanged += OnOptionsPropertyChanged;
    }

    #endregion

    #region Overrides

    protected override void OnConfigureButtons( DialogButtonBuilder builder )
    {
      builder.AddButton().Content( "Export" ).Command( ExportCommand );
    }

    public override void OnDialogClosed()
    {
      Options.PropertyChanged -= OnOptionsPropertyChanged;
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

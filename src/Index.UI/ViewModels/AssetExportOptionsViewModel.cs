using System.Text.Json;
using System.Windows.Input;
using Index.Domain.Assets;
using Index.Domain.Database.Repositories;
using Index.UI.Controls.Buttons;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace Index.UI.ViewModels
{

  public class AssetExportOptionsViewModel<TOptions> : DialogWindowViewModel
    where TOptions : AssetExportOptions, new()
  {

    #region Data Members

    private readonly ISavedSettingsRepository _settingsRepository;

    #endregion

    #region Properties

    public TOptions Options { get; }
    public ICommand ExportCommand { get; }

    #endregion

    #region Constructor

    public AssetExportOptionsViewModel( IContainerProvider container )
      : base( container )
    {
      _settingsRepository = container.Resolve<ISavedSettingsRepository>();

      Options = GetOptions();
      ExportCommand = new DelegateCommand( Export, () => Options.IsValid );

      Options.PropertyChanged += OnOptionsPropertyChanged;
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
    }

    public override void OnDialogClosed()
    {
      Options.PropertyChanged -= OnOptionsPropertyChanged;
      SaveOptions();
    }

    #endregion

    #region Private Methods

    private void Export()
    {
      Parameters.Add( "Options", Options );
      CloseDialog( Parameters );
    }

    private TOptions GetOptions()
    {
      var key = typeof( TOptions ).Name;
      var savedOptions = _settingsRepository.GetByKey( key );
      if ( savedOptions is null )
        return new TOptions();

      var options = JsonSerializer.Deserialize<TOptions>( savedOptions.Data );
      return options;
    }

    private void SaveOptions()
    {
      var key = typeof( TOptions ).Name;
      var savedOptions = _settingsRepository.GetByKey( key );

      if ( savedOptions is null )
        savedOptions = _settingsRepository.New( key );

      savedOptions.Data = JsonSerializer.Serialize( Options );
      _settingsRepository.Update( savedOptions );
      _settingsRepository.SaveChanges();
    }

    #endregion

    #region Event Handlers

    private void OnOptionsPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
      => ( ExportCommand as DelegateCommand ).RaiseCanExecuteChanged();

    #endregion

  }

}

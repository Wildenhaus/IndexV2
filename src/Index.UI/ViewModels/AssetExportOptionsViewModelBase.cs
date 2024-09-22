using System;
using System.Text.Json;
using System.Windows.Input;
using Index.Domain.Assets;
using Index.Domain.Database.Repositories;
using Prism.Ioc;

namespace Index.UI.ViewModels
{

  public class AssetExportOptionsViewModelBase<TOptions> : ViewModelBase
    where TOptions : AssetExportOptions, new()
  {

    #region Data Members

    private readonly IAssetManager _assetManager;
    private readonly ISavedSettingsRepository _settingsRepository;

    #endregion

    #region Properties

    public Type AssetType { get; }
    public string AssetTypeName { get; }

    public TOptions Options { get; }
    public ICommand ExportCommand { get; }

    #endregion

    #region Constructor

    public AssetExportOptionsViewModelBase( Type assetType, IContainerProvider container )
    {
      _assetManager = container.Resolve<IAssetManager>();
      _settingsRepository = container.Resolve<ISavedSettingsRepository>();

      AssetType = assetType;
      AssetTypeName = _assetManager.GetAssetTypeName( assetType );

      Options = GetOptions();
    }

    #endregion

    #region Private Methods

    internal TOptions GetOptions()
    {
      var key = typeof( TOptions ).Name;
      var savedOptions = _settingsRepository.GetByKey( key );
      if ( savedOptions is null )
        return new TOptions();

      var options = JsonSerializer.Deserialize<TOptions>( savedOptions.Data );
      return options;
    }

    internal void SaveOptions()
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

  }

}

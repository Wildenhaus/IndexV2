using System;
using Index.Domain.Assets;
using Prism.Ioc;

namespace Index.UI.ViewModels
{

  public interface IAssetExportOptionsTabViewModel : ITabViewModel
  {

    public Type AssetType { get; }
    public Type AssetExportOptionsType { get; }

    public AssetExportOptions Options { get; }
    public string AssetTypeName { get; }

  }

  public class AssetExportOptionsTabViewModel<TOptions> : TabViewModelBase, IAssetExportOptionsTabViewModel
    where TOptions : AssetExportOptions, new()
  {

    #region Data Members

    private readonly AssetExportOptionsViewModelBase<TOptions> _base;

    #endregion

    #region Properties

    public Type AssetType => _base.AssetType;
    public Type AssetExportOptionsType => typeof( TOptions );

    public TOptions Options => _base.Options;
    public string AssetTypeName => _base.AssetTypeName;

    AssetExportOptions IAssetExportOptionsTabViewModel.Options => Options;

    #endregion

    #region Constructor

    public AssetExportOptionsTabViewModel( Type assetType, IContainerProvider containerProvider )
    {
      _base = new AssetExportOptionsViewModelBase<TOptions>( assetType, containerProvider );
      TabName = _base.AssetTypeName;
    }

    #endregion

  }

}
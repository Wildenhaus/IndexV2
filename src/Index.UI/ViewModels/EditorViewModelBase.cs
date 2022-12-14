using System.Threading.Tasks;
using System.Windows.Input;
using Index.Domain.Assets;
using Index.Domain.Jobs;
using Index.Jobs;
using Index.UI.Commands;
using Index.UI.Controls.Menus;
using Index.Utilities;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  public abstract class EditorViewModelBase<TAsset> : InitializableViewModel, IEditorViewModel<TAsset>
    where TAsset : class, IAsset
  {

    #region Properties

    public string TabName { get; set; }
    public ICommand CloseCommand { get; set; }

    public TAsset Asset { get; protected set; }
    public IAssetReference AssetReference { get; protected set; }

    [DoNotNotify] protected IContainerProvider Container { get; }
    [DoNotNotify] protected IAssetManager AssetManager { get; }

    #endregion

    #region Constructor

    protected EditorViewModelBase( IContainerProvider container )
      : base( container )
    {
      Container = container;
      AssetManager = container.Resolve<IAssetManager>();
      CloseCommand = new DelegateCommand( Close );
    }

    #endregion

    #region Overrides

    protected override IJob CreateInitializationJob( IContainerProvider container )
      => AssetManager.LoadAsset<TAsset>( AssetReference );

    protected sealed override void OnInitializationJobCompleted( IJob job )
    {
      var assetJob = job as IJob<TAsset>;
      Asset = assetJob.Result;
      OnAssetLoaded( Asset );
    }

    public override bool IsNavigationTarget( NavigationContext navigationContext )
    {
      var assetReference = ( IAssetReference ) navigationContext.Parameters[ "AssetReference" ];
      return assetReference == AssetReference;
    }

    public override void OnNavigatedTo( NavigationContext navigationContext )
    {
      if ( AssetReference is null )
      {
        AssetReference = ( IAssetReference ) navigationContext.Parameters[ "AssetReference" ];
        TabName = AssetReference.AssetName;
        Initialize();
      }
    }

    protected override void OnConfigureContextMenu( MenuViewModelBuilder builder )
    {
      if ( AssetReference is null )
        return;

      builder.AddItem( config =>
        config.Header( "Close" )
              .Command( EditorCommands.CloseTabCommand, AssetReference ) );

      builder.AddItem( config =>
        config.Header( "Close All Tabs" )
              .Command( EditorCommands.CloseAllTabsCommand ) );

      builder.AddItem( config =>
        config.Header( "Close All But This" )
              .Command( EditorCommands.CloseAllTabsButThisCommand, AssetReference ) );

    }

    protected override void OnDisposing()
    {
      CancelInitialization();
      Asset?.AssetLoadContext?.Dispose();
      Asset?.Dispose();

      GCHelper.ForceCollect();
    }

    #endregion

    #region Virtual Methods

    protected virtual void OnAssetLoaded( TAsset asset )
    {
    }

    #endregion

    #region Private Methods

    private void Close()
      => EditorCommands.CloseTabCommand.Execute( AssetReference );

    #endregion

  }

}

using System;
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
using Prism.Services.Dialogs;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  public abstract class EditorViewModelBase<TAsset> : InitializableViewModel, IEditorViewModel<TAsset>
    where TAsset : class, IAsset
  {

    #region Properties

    public string TabName { get; set; }

    public ICommand CloseCommand { get; set; }
    public ICommand ExportAssetCommand { get; protected set; }

    public TAsset Asset { get; protected set; }
    public IAssetReference AssetReference { get; protected set; }

    public bool IsExporting { get; protected set; }

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

      if ( typeof( TAsset ).IsAssignableTo( typeof( IExportableAsset ) ) )
        ExportAssetCommand = new DelegateCommand( ExportAsset );
    }

    #endregion

    #region Overrides

    protected override IJob CreateInitializationJob( IContainerProvider container )
      => AssetManager.LoadAsset<TAsset>( AssetReference );

    protected sealed override void OnInitializationJobCompleted( IJob job )
    {
      var assetJob = job as IJob<TAsset>;
      if ( assetJob.State == JobState.Faulted )
      {
        var parameters = new DialogParameters();
        parameters.Add( nameof( Exception ), assetJob.Exception );

        var dialogService = Container.Resolve<IDialogService>();
        Dispatcher.Invoke( () =>
        {
          dialogService.ShowDialog( "UnhandledExceptionDialog", parameters, r =>
          {
            this.Close();
          } );
        } );

        return;
      }

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

    public override void ConfirmNavigationRequest( NavigationContext navigationContext, Action<bool> continuationCallback )
    {
      continuationCallback( !IsExporting );
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

    private void ExportAsset()
    {
      ASSERT( Asset is IExportableAsset );

      var exportableAsset = Asset as IExportableAsset;
      var optionsType = exportableAsset.ExportOptionsType;
      var jobType = exportableAsset.ExportJobType;

      var parameters = new DialogParameters();
      parameters.Add( "Asset", Asset );
      parameters.Add( "AssetReference", Asset.AssetReference );
      parameters.Add( "OptionsType", optionsType );
      parameters.Add( "JobType", jobType );

      var dialogService = Container.Resolve<IDialogService>();
      dialogService.ShowDialog( optionsType.Name, parameters, result =>
      {
        if ( result is null || result.Result != ButtonResult.OK )
          return;

        var options = result.Parameters.GetValue<object>( "Options" );

        var jobParams = new ParameterCollection();
        jobParams.Set<IAsset>( "Asset", Asset );
        jobParams.Set( "AssetReference", Asset.AssetReference );
        jobParams.Set( "Options", options );

        var jobManager = Container.Resolve<IJobManager>();
        var job = jobManager.CreateJob( jobType, jobParams );

        if ( job.State == JobState.Completed )
          return;

        IsExporting = true;
        Progress = job.Progress;
        ShowProgressOverlay = true;

        jobManager.StartJob( job, ( j ) =>
        {
          IsExporting = false;
          ShowProgressOverlay = false;
          Progress = null;

          if ( j.State == JobState.Faulted )
          {
            var dialogParams = new DialogParameters();
            dialogParams.Add( nameof( Exception ), j.Exception );

            dialogService.ShowDialog( "UnhandledExceptionDialog", dialogParams, r => { } );
          }

        } );
      } );
    }

    #endregion

  }

}

using Index.Domain.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace Index.Modules.FileDialogs
{

  public class FileDialogsModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterSingleton<IFileDialogService, FileDialogService>();
    }

  }

}

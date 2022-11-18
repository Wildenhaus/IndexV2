using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Domain.Repositories;
using Index.Modules.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Prism.Ioc;
using Prism.Modularity;

namespace Index.Modules.Database
{

  public class DatabaseModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
      var context = containerProvider.Resolve<IndexDataContext>();
      context.Database.EnsureCreated();
      context.Database.Migrate();
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.Register<IndexDataContext>();

      containerRegistry.Register<IGamePathRepository, GamePathRepository>();
    }

  }

}

using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;
using Prism.Regions;

namespace Index.UI.ViewModels
{

  public interface IViewModel : IDisposable, INotifyPropertyChanged, INavigationAware
  {

    #region Properties

    Dispatcher Dispatcher { get; }

    ContextMenu ContextMenu { get; }

    #endregion

  }

}

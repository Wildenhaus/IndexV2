using System;
using System.ComponentModel;
using Prism.Regions;

namespace Index.UI.ViewModels
{

  public interface IViewModel : IDisposable, INotifyPropertyChanged, INavigationAware
  {
  }

}

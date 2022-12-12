using System;
using System.Windows;
using System.Windows.Controls;
using Index.UI.Extensions;
using Index.Utilities;
using Microsoft.Xaml.Behaviors;
using Prism.Regions;

namespace Index.UI.Commands
{

  public class CloseTabAction : TriggerAction<Button>
  {

    #region Overrides

    protected override void Invoke( object parameter )
    {
      var args = parameter as RoutedEventArgs;
      if ( args is null )
        return;

      var tabItem = ( args.OriginalSource as DependencyObject ).FindParent<TabItem>();
      if ( tabItem is null )
        return;

      var tabControl = tabItem.FindParent<TabControl>();
      if ( tabControl is null )
        return;

      var region = RegionManager.GetObservableRegion( tabControl ).Value;
      if ( region is null )
        return;

      RemoveItemFromRegion( tabItem.Content, region );
    }

    #endregion

    #region Private Methods

    private void RemoveItemFromRegion( object item, IRegion region )
    {
      var navigationContext = new NavigationContext( region.NavigationService, null );
      if ( !CanRemove( item, navigationContext ) )
        return;

      InvokeOnNavigatedFrom( item, navigationContext );
      region.Remove( item );

      var view = item as FrameworkElement;
      if ( view is null )
        return;

      if ( view.DataContext is IDisposable disposableViewModel )
        disposableViewModel?.Dispose();

      if ( item is IDisposable disposableTab )
        disposableTab?.Dispose();

      GCHelper.ForceCollect();
    }

    private bool CanRemove( object item, NavigationContext navigationContext )
    {
      var canRemove = true;

      var confirmRequestItem = item as IConfirmNavigationRequest;
      if ( confirmRequestItem is null )
      {
        var frameworkElement = item as FrameworkElement;
        if ( frameworkElement is not null )
          confirmRequestItem = frameworkElement.DataContext as IConfirmNavigationRequest;
      }

      if ( confirmRequestItem is not null )
      {
        confirmRequestItem.ConfirmNavigationRequest( navigationContext, result =>
        {
          canRemove = result;
        } );
      }

      return canRemove;
    }

    private void InvokeOnNavigatedFrom( object item, NavigationContext navigationContext )
    {
      var navigationAware = item as INavigationAware;
      if ( navigationAware is null )
      {
        var frameworkElement = item as FrameworkElement;
        navigationAware = frameworkElement.DataContext as INavigationAware;
      }

      if ( navigationAware is not null )
        navigationAware.OnNavigatedFrom( navigationContext );
    }

    #endregion

  }

}

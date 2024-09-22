using System;
using Index.Domain.Assets;
using PropertyChanged;
using static Index.Assertions;

namespace Index.Modules.DataExplorer.ViewModels
{

  public class BulkExportAssetNodeViewModel : NodeViewModelBase<BulkExportAssetNodeViewModel>
  {
    #region Data Members

    private AssetNodeViewModel _assetNodeViewModel;
    private bool? _isChecked;

    #endregion

    #region Properties

    public override string Name => _assetNodeViewModel.Name;
    private IAssetReference AssetReference => _assetNodeViewModel.AssetReference;
    public Type AssetType => _assetNodeViewModel.AssetType;

    [DoNotNotify] private bool IsUpdating { get; set; }
    [DoNotNotify] private int ChildrenChecked { get; set; }

    public bool? IsChecked
    {
      get => _isChecked;
      set
      {
        if ( _isChecked != value && !IsUpdating )
        {
          bool? previousValue = _isChecked;
          _isChecked = value;
          OnPropertyChanged( new( nameof( IsChecked ) ) );

          UpdateChildrenCheckedCount( previousValue, value );
          PropagateCheckedState( value );
        }
      }
    }

    #endregion

    #region Constructor

    public BulkExportAssetNodeViewModel( AssetNodeViewModel assetNodeViewModel )
    {
      _isChecked = false;
      _assetNodeViewModel = assetNodeViewModel;
    }

    #endregion

    #region Private Methods

    private void PropagateCheckedState( bool? isChecked )
    {
      if ( IsChild )
        PropagateParentCheckedState( isChecked );
      if ( IsParent )
        PropagateChildCheckedState( isChecked );
    }

    private void PropagateChildCheckedState( bool? isChecked )
    {
      if ( !isChecked.HasValue )
        return;

      IsUpdating = true;
      Children.SuppressNotifications = true;

      foreach ( var child in Children )
      {
        if(child.IsVisible)
          child.IsChecked = isChecked;
      }

      Children.SuppressNotifications = false;
      IsUpdating = false;
    }

    private void PropagateParentCheckedState( bool? isChecked )
    {
      if ( Parent is null )
        return;

      IsUpdating = true;

      if ( Parent.ChildrenChecked == 0 )
        Parent.IsChecked = false;
      else if ( Parent.ChildrenChecked == Parent.Children.Count )
        Parent.IsChecked = true;
      else
        Parent.IsChecked = null;

      IsUpdating = false;
    }

    private void UpdateChildrenCheckedCount( bool? previousValue, bool? newValue )
    {
      if ( Parent == null || previousValue == newValue )
        return;

      if ( previousValue == true && newValue != true )
      {
        Parent.ChildrenChecked--;
      }
      else if ( previousValue != true && newValue == true )
      {
        Parent.ChildrenChecked++;
      }

      ASSERT( Parent.ChildrenChecked >= 0, "ChildrenChecked became negative." );
      ASSERT( Parent.ChildrenChecked <= Parent.Children.Count, "ChildrenChecked exceeded number of children." );
    }

    #endregion

  }


}

using System;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Index.UI.ViewModels
{

  public abstract class InitializableViewModel : BindableBase, IInitializableViewModel
  {

    #region Data Members

    private bool _isInitialized;
    private bool _isInitializing;

    #endregion

    #region Properties

    public bool IsInitializing
    {
      get => _isInitializing;
      set => SetProperty( ref _isInitializing, value );
    }

    #endregion

    #region Public Methods

    public async Task<IResult> Initialize()
    {
      if ( _isInitialized )
        return Result.Successful();

      try
      {
        IsInitializing = true;
        await OnInitializing();
        return Result.Successful();
      }
      catch ( Exception ex )
      {
        return Result.Unsuccessful( ex );
      }
      finally
      {
        IsInitializing = false;
      }
    }

    #endregion

    #region Privte Methods

    protected abstract Task OnInitializing();

    #endregion

  }

}

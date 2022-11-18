namespace Index.UI.Windows
{

  public class IxDialogWindow : IxWindow
  {

    public IParameterCollection Parameters { get; }

    public IxDialogWindow()
    {
      Parameters = new ParameterCollection();
    }

  }

}

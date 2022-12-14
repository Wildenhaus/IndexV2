namespace Index.Domain.Assets
{

  public interface IExportableAsset : IAsset
  {

    Type ExportOptionsType { get; }
    Type ExportJobType { get; }

  }

}

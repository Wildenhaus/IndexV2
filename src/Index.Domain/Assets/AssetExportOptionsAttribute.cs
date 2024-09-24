namespace Index.Domain.Assets
{

  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class AssetExportOptionsTypeAttribute : Attribute
  {

    public Type Type { get; }

    public AssetExportOptionsTypeAttribute(Type assetExportOptionsType)
    {
      Type = assetExportOptionsType;
    }

  }

}

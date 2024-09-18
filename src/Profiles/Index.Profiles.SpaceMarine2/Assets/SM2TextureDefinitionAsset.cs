using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.SpaceMarine2.FileSystem.Files;
using LibSaber.SpaceMarine2.Structures.Resources;

namespace Index.Profiles.SpaceMarine2.Assets
{

  public class SM2TextureDefinitionAsset : SM2TextAsset
  {

    public override string TypeName => "Material";

    public SM2TextureDefinitionAsset( IAssetReference assetReference ) 
      : base( assetReference )
    {
    }

    internal override string GetAssetFilePath( IAssetReference assetReference )
    {
      var assetNode = assetReference.Node as SM2ResourceFileNode<resDESC_TD>;
      ASSERT( assetNode is not null, "TextureDefinition asset file node is not a valid SM2ResourceFileNode" );

      var tdFileName = assetNode.ResourceDescription.td;
      ASSERT( !string.IsNullOrWhiteSpace( tdFileName ), "resDESC_TD header does not specify a TextureDefinition file." );

      return tdFileName;
    }

  }

}

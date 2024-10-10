using Assimp;

namespace Index.Domain.Assets.Meshes
{

  public enum MeshExportFormat
  {
    FBX,
    DAE,
    GLTF2,
    STL,
    X3D,
    JSON,
    XML
  }

  public static class MeshExportFormatExtensions
  {

    public static string GetAssimpFormatId( this MeshExportFormat format )
    {
      switch ( format )
      {
        case MeshExportFormat.FBX:
          return "fbx";
        case MeshExportFormat.DAE:
          return "collada";
        case MeshExportFormat.STL:
          return "stl";
        case MeshExportFormat.X3D:
          return "x3d";
        case MeshExportFormat.JSON:
          return "assjson";
        case MeshExportFormat.XML:
          return "assxml";
        case MeshExportFormat.GLTF2:
          return "gltf2";

        default:
          throw new NotSupportedException( $"Assimp export format not supported: {format}" );
      }
    }

    public static string GetFileExtension( this MeshExportFormat format )
    {
      var formatId = format.GetAssimpFormatId();

      using ( var context = new AssimpContext() )
      {
        var formats = context.GetSupportedExportFormats();
        var match = formats.First( x => x.FormatId == formatId );

        return match.FileExtension;
      }
    }


  }


}

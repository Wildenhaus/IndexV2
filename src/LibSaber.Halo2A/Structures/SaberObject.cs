using System.Numerics;
using LibSaber.Shared.Scripting;

namespace LibSaber.Halo2A.Structures
{

  public class SaberObject
  {

    #region Data Members

    private string _name;
    private string _boneName;
    private string _meshName;

    #endregion

    #region Properties

    public GeometryGraph GeometryGraph { get; }

    public short Id { get; set; }
    public string ReadName { get; set; }

    public short ParentId { get; set; }
    public short NextId { get; set; }
    public short PrevId { get; set; }
    public short ChildId { get; set; }

    public short AnimNumber { get; set; }
    public string ReadAffixes { get; set; }

    public Matrix4x4 MatrixLT { get; set; }
    public Matrix4x4 MatrixModel { get; set; }

    public ObjectGeometryUnshared GeomData { get; set; }

    public string UnkName { get; set; }
    // OBB
    public string Name { get; set; }
    public string Affixes { get; set; }

    public SaberObject Parent
    {
      get
      {
        if ( ParentId < 0 )
          return null;

        return GeometryGraph.Objects[ ParentId ];
      }
    }

    public IEnumerable<GeometrySubMesh> SubMeshes
    {
      get => GeometryGraph.SubMeshes.Where( x => x.NodeId == Id );
    }

    #endregion

    #region Constructor

    public SaberObject( GeometryGraph geometryGraph )
    {
      GeometryGraph = geometryGraph;
    }

    #endregion

    #region Public Methods

    public IEnumerable<SaberObject> EnumerateChildren()
    {
      var visited = new HashSet<short>();

      var currentId = ChildId;
      while ( visited.Add( currentId ) )
      {
        if ( currentId < 0 )
          break;

        yield return GeometryGraph.Objects[ currentId ];
        currentId = GeometryGraph.Objects[ currentId ].NextId;
      }
    }

    public string GetName()
    {
      if ( _name != null )
        return _name;

      if ( !string.IsNullOrWhiteSpace( UnkName ) )
        return _name = UnkName.Split( new[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries ).Last();

      if ( !string.IsNullOrWhiteSpace( Name ) )
        return _name = Name;

      if ( !string.IsNullOrWhiteSpace( ReadName ) )
        return _name = ReadName;

      return null;
    }

    public string GetParentName()
      => Parent?.GetName();

    public string GetMeshName()
    {
      if ( _meshName != null )
        return _meshName;

      if ( string.IsNullOrWhiteSpace( UnkName ) )
        return null;

      var nameParts = UnkName.Split( new[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries )
        .Where( x => x != "h" && !x.StartsWith( "_b_" ) && !x.StartsWith( "_m_" ) );

      return _meshName = string.Join( "_", nameParts.TakeLast( 2 ) ).Trim();
    }

    public string GetParentMeshName()
    {

      if ( string.IsNullOrWhiteSpace( UnkName ) )
        return null;

      var nameParts = UnkName.Split( new[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries )
        .Where( x => x != "h" && !x.StartsWith( "_b_" ) && !x.StartsWith( "_m_" ) );

      return nameParts.FirstOrDefault();
    }

    public string GetBoneName()
    {
      if ( _boneName != null )
        return _boneName;

      if ( string.IsNullOrWhiteSpace( UnkName ) )
        return null;

      var nameParts = UnkName.Split( new[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries )
        .Where( x => x.StartsWith( "_b_" ) );

      return _boneName = nameParts.LastOrDefault();
    }

    #endregion

  }

}

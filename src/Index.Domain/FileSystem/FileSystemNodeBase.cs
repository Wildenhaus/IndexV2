﻿using System.Diagnostics;

namespace Index.Domain.FileSystem
{

  [DebuggerDisplay( "FileSystemNode[ {Name} ]" )]
  public abstract class FileSystemNodeBase : DisposableObject, IFileSystemNode
  {

    #region Data Members

    private string _path;

    #endregion

    #region Properties

    public IFileSystemDevice Device { get; }
    public IParameterCollection Metadata { get; }

    public string Name { get; }
    public virtual string DisplayName => Name;
    public virtual bool IsHidden => false;
    public bool IsDirectory
    {
      get => FirstChild is not null;
    }

    public IFileSystemNode Parent { get; }
    public IFileSystemNode FirstChild { get; set; }
    public IFileSystemNode LastChild { get; set; }
    public IFileSystemNode NextSibling { get; set; }


    #endregion

    #region Constructor

    protected FileSystemNodeBase( IFileSystemDevice device, string name, IFileSystemNode parent = null )
    {
      Device = device;
      Name = ParseFileName( name );
      Parent = parent;

      Metadata = new ParameterCollection();
    }

    #endregion

    #region Public Methods

    public void AddChild( IFileSystemNode node )
    {
      if ( FirstChild is null )
      {
        FirstChild = node;
        LastChild = node;
        return;
      }

      LastChild.AddSibling( node );
      LastChild = node;
    }

    public void AddSibling( IFileSystemNode node )
    {
      IFileSystemNode currentSibling = this;
      while ( currentSibling.NextSibling is not null )
        currentSibling = currentSibling.NextSibling;

      currentSibling.NextSibling = node;
    }

    public IEnumerable<IFileSystemNode> EnumerateChildren( bool recursive = false )
    {
      var currentChild = FirstChild;
      while ( currentChild is not null )
      {
        yield return currentChild;

        if ( recursive )
        {
          foreach ( var recursiveChild in currentChild.EnumerateChildren( recursive ) )
            yield return recursiveChild;
        }

        currentChild = currentChild.NextSibling;
      }
    }

    public IEnumerable<IFileSystemNode> EnumerateSiblings( bool includeThis = false )
    {
      if ( Parent is null )
        yield break;

      foreach ( var sibling in Parent.EnumerateChildren( recursive: false ) )
        if ( sibling != this || includeThis )
          yield return sibling;
    }

    public string GetPath( bool excludeRoot = true )
    {
      if ( _path is not null )
        return _path;

      var pathStack = new Stack<string>();
      IFileSystemNode currentNode = this;
      while ( currentNode is not null )
      {
        if ( excludeRoot && currentNode.Parent is null )
          break;

        pathStack.Push( currentNode.Name );
        currentNode = currentNode.Parent;
      }

      return _path = Path.Combine( pathStack.ToArray() );
    }

    public virtual Stream Open()
      => Device.GetStream( this );

    public T GetMetadata<T>( string key )
      => Metadata.Get<T>( key );

    public void SetMetadata<T>( string key, T value )
      => Metadata.Set<T>( key, value );

    #endregion

    #region Private Methods

    protected virtual string ParseFileName( string fileName )
    {
      return fileName;
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      Metadata?.Dispose();

      foreach ( var child in EnumerateChildren() )
        child?.Dispose();
    }

    #endregion

  }

}

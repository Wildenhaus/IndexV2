﻿using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace HelixToolkit.SharpDX.Core
{
  public class DefaultTexturePathResolver : ITexturePathResolver
  {
    static readonly ILogger logger = Logger.LogManager.Create<DefaultTexturePathResolver>();
    private const string ToUpperDictString = @"..\";

    public string Resolve( string modelPath, string texturePath )
    {
      return OnLoadTexture( modelPath, texturePath );
    }

    /// <summary>
    ///     Called when [load texture].
    /// </summary>
    /// <param name="modelPath">The model path</param>
    /// <param name="texturePath">The path.</param>
    /// <returns></returns>
    protected virtual string OnLoadTexture( string modelPath, string texturePath )
    {
      try
      {
        var dict = Path.GetDirectoryName( modelPath );
        if ( string.IsNullOrEmpty( dict ) )
        {
          dict = Directory.GetCurrentDirectory();
        }
        var p = Path.GetFullPath( Path.Combine( dict, texturePath ) );
        if ( !FileExists( p ) )
          p = HandleTexturePathNotFound( dict, texturePath );
        if ( !FileExists( p ) )
        {
          logger.LogWarning( "Load Texture Failed. Texture Path = {0}.", texturePath );
          return null;
        }
        return p;
      }
      catch ( Exception ex )
      {
        logger.LogWarning( "Load Texture Exception. Texture Path = {0}. Exception: {1}", texturePath, ex.Message );
      }
      return null;
    }

    /// <summary>
    /// Handles the texture path not found. Override to provide your own handling
    /// </summary>
    /// <param name="dir">The dir.</param>
    /// <param name="texturePath">The texture path.</param>
    /// <returns></returns>
    protected virtual string HandleTexturePathNotFound( string dir, string texturePath )
    {
      //If file not found in texture path dir, try to find the file in the same dir as the model file
      if ( texturePath.StartsWith( ToUpperDictString ) )
      {
        var t = texturePath.Remove( 0, ToUpperDictString.Length );
        var p = Path.GetFullPath( Path.Combine( dir, t ) );
        if ( FileExists( p ) )
          return p;
      }

      //If still not found, try to go one upper level and find
      var upper = Directory.GetParent( dir ).FullName;
      try
      {
        upper = Path.GetFullPath( upper + texturePath );
      }
      catch ( NotSupportedException ex )
      {
        logger.LogWarning( "Exception: {0}", ex );
      }
      if ( FileExists( upper ) )
        return upper;
      var fileName = Path.GetFileName( texturePath );
      var currentPath = Path.Combine( dir, fileName );
      if ( FileExists( currentPath ) )
      {
        return currentPath;
      }
      return string.Empty;
    }

    protected virtual bool FileExists( string path )
    {
      return File.Exists( path );
    }
  }
}
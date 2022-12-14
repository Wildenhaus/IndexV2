using System.Collections.Concurrent;
using DirectXTexNet;
using ImageMagick;

namespace Index.Textures
{

  public class ImagePostProcessor : DisposableObject
  {

    #region Data Members

    private readonly IMagickImageCollection<float> _collection;

    #endregion

    #region Properties

    public ICollection<IMagickImage<float>> Images => _collection;
    public int ImageCount => _collection.Count;

    #endregion

    #region Constructor

    private ImagePostProcessor( IMagickImageCollection<float> collection )
    {
      _collection = collection;
    }

    public static ImagePostProcessor Create( Stream[] streams )
    {
      var collection = CreateCollection( streams );
      return new ImagePostProcessor( collection );
    }

    #endregion

    #region Public Methods

    public void InvertGreenChannel()
      => Parallel.ForEach( _collection, InvertGreenChannel );

    public void RecalculateZChannel()
      => Parallel.ForEach( _collection, RecalculateZChannel );

    public Stream Save( int imageIndex, ImageFormat format )
    {
      var exportFormat = format.GetMagickFormat();
      var image = _collection[ imageIndex ];

      var stream = new MemoryStream();
      image.Write( stream, exportFormat );

      stream.Position = 0;
      return stream;
    }

    public async Task<Stream> SaveAsync( int imageIndex, ImageFormat format )
    {
      var exportFormat = format.GetMagickFormat();
      var image = _collection[ imageIndex ];

      var stream = new MemoryStream();
      await image.WriteAsync( stream, exportFormat );

      stream.Position = 0;
      return stream;
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      _collection?.Dispose();
    }

    #endregion

    #region Private Methods

    private static IMagickImageCollection<float> CreateCollection( Stream[] streams )
    {
      var imageCount = streams.Length;

      var imageDict = new ConcurrentDictionary<int, MagickImage>();
      Parallel.For( 0, imageCount, i =>
      {
        var imageStream = streams[ i ];
        var image = new MagickImage( imageStream );
        imageDict[ i ] = image;
      } );

      var collection = new MagickImageCollection();
      for ( var i = 0; i < imageCount; i++ )
        collection.Add( imageDict[ i ] );

      return collection;
    }

    private static async Task<IMagickImageCollection<float>> CreateCollectionAsync( ScratchImage dxgiImage )
    {
      using ( var dxgiStream = dxgiImage.SaveToDDSMemory( DDS_FLAGS.NONE ) )
      {
        var factory = new MagickImageCollectionFactory();
        return await factory.CreateAsync( dxgiStream );
      }

      //var imageCount = dxgiImage.GetImageCount();

      //var imageDict = new ConcurrentDictionary<int, MagickImage>();
      //Parallel.For( 0, imageCount, i =>
      //{
      //  using ( var imageStream = dxgiImage.SaveToDDSMemory( i, DDS_FLAGS.NONE ) )
      //  {
      //    var image = new MagickImage( imageStream );
      //    imageDict[ i ] = image;
      //  }
      //} );

      //var collection = new MagickImageCollection();
      //for ( var i = 0; i < imageCount; i++ )
      //  collection.Add( imageDict[ i ] );

      //return collection;
    }

    private unsafe void InvertGreenChannel( IMagickImage<float> image )
    {
      var channelCount = image.ChannelCount;
      var height = image.Height;
      var width = image.Width;

      using ( var pixels = image.GetPixelsUnsafe() )
      {
        for ( var h = 0; h < height; h++ )
        {
          var pRow = pixels.GetAreaPointer( 0, h, width, 1 );
          var row = new Span<float>( pRow.ToPointer(), width * channelCount );

          for ( var w = 0; w < row.Length; w += channelCount )
          {
            var y = row[ w + 1 ] / 65535.0f;
            y = 1.0f - y;
            row[ w + 1 ] = y * 65535.0f;
          }
          // SetArea
        }
      }
    }

    private unsafe void RecalculateZChannel( IMagickImage<float> image )
    {
      var channelCount = image.ChannelCount;
      var height = image.Height;
      var width = image.Width;

      using ( var pixels = image.GetPixelsUnsafe() )
      {
        for ( var h = 0; h < height; h++ )
        {
          var pRow = pixels.GetAreaPointer( 0, h, width, 1 );
          var row = new Span<float>( pRow.ToPointer(), width * channelCount );
          for ( var w = 0; w < row.Length; w += channelCount )
          {
            var x = row[ w + 0 ] / 65535.0f;
            var y = row[ w + 1 ] / 65535.0f;

            x = ( x * 2 ) - 1;
            y = ( y * 2 ) - 1;

            var z = ( float ) Math.Clamp( ( x * x ) + ( y * y ), 0, 1 );
            z = MathF.Sqrt( 1 - z );
            z = ( z + 1 ) / 2;

            row[ w + 2 ] = z * 65535.0f;
          }
          // SetArea
        }
      }
    }

    #endregion

  }

}

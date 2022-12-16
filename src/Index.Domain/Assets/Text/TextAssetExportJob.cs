using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Ioc;

namespace Index.Domain.Assets.Text
{

  public class TextAssetExportJob : AssetExportJobBase<TextAsset, TextAssetExportOptions>
  {

    #region Constructor

    public TextAssetExportJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    #endregion

    #region Overrides

    protected override async Task ExportAsset()
    {
      SetIndeterminate();
      SetSubStatus( "Writing File" );

      var exportStream = Asset.TextStream;
      exportStream.Position = 0;

      var exportFileName = Asset.AssetName;
      var exportFilePath = Path.Combine( Options.ExportPath, exportFileName );

      using ( var fs = File.Create( exportFilePath ) )
      {
        await exportStream.CopyToAsync( fs );
        await fs.FlushAsync();
      }
    }

    #endregion

  }

}

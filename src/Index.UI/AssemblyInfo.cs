using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,
    ResourceDictionaryLocation.SourceAssembly
)]

[assembly: XmlnsDefinition( "http://index/ui", nameof( Index.UI ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Index.UI.Controls ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Index.UI.Converters ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Index.UI.Extensions ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Index.UI.Services ) )]
[assembly: XmlnsDefinition( "http://index/ui", nameof( Index.UI.Windows ) )]

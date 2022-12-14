using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace Index.UI.Extensions
{

  public class EnumExtension : MarkupExtension
  {

    #region Data Members

    private Type _enumType;

    #endregion

    #region Properties

    public Type EnumType
    {
      get => _enumType;
      set => SetEnumType( value );
    }

    #endregion

    #region Constructor

    public EnumExtension( Type enumType )
    {
      ASSERT_NOT_NULL( enumType );
      EnumType = enumType;
    }

    #endregion

    #region Overrides

    public override object ProvideValue( IServiceProvider serviceProvider )
    {
      var values = Enum.GetValues( EnumType ).Cast<object>();
      return values.Select( x => new EnumMember
      {
        Value = x,
        Description = GetValueDescription( x )
      } ).ToArray();
    }

    #endregion

    #region Private Methods

    private string GetValueDescription( object enumValue )
    {
      var attribute = EnumType
        .GetField( enumValue.ToString() )
        .GetCustomAttribute<DescriptionAttribute>();

      if ( attribute is not null )
        return attribute.Description;

      return enumValue.ToString();
    }

    private void SetEnumType( Type enumType )
    {
      if ( enumType is null )
        return;

      if ( !enumType.IsEnum )
        FAIL( "Type is not an Enum: `{0}`", enumType.FullName );

      _enumType = enumType;
    }

    #endregion

    #region Embedded Types

    public class EnumMember
    {

      public object Value { get; set; }
      public string Description { get; set; }

    }

    #endregion

  }

}

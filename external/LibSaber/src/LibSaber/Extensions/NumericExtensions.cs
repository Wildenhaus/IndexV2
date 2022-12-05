using LibSaber.Shared.Structures;

namespace LibSaber.Extensions
{

  public static class NumericExtensions
  {

    public static float SNormToFloat( this sbyte snormValue )
      => snormValue / ( float ) sbyte.MaxValue; // TODO

    public static float SNormToFloat( this short snormValue )
      => snormValue / SNorm16.Coefficient;

    public static float UNormToFloat( this byte unormValue )
      => unormValue / ( float ) byte.MaxValue; // TODO

  }

}

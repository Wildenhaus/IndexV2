using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSaber.Halo2A.Enumerations
{

  [Flags]
  public enum GeometryBufferFlags : ulong
  {
    #pragma warning disable format
    // @formatter:off — disable formatter after this line

    _FACE = 0, // TODO: Assuming no flags means it's a face

    _VERT             = 1ul << 0x00,
    _VERT_2D          = 1ul << 0x01,
    _VERT_4D          = 1ul << 0x02,
    _COMPRESSED_VERT  = 1ul << 0x03,
    _WEIGHT1          = 1ul << 0x04,
    _WEIGHT2          = 1ul << 0x05,
    _WEIGHT3          = 1ul << 0x06,
    _WEIGHT4          = 1ul << 0x07,
    _INDEX            = 1ul << 0x08,
    _BONE             = 1ul << 0x09,
    _COMPRESSED_NORM  = 1ul << 0x0A,
    _NORM_IN_VERT4    = 1ul << 0x0B,

    _TANG0            = 1ul << 0x0C,
    _TANG1            = 1ul << 0x0D,
    _TANG2            = 1ul << 0x0E,
    _TANG3            = 1ul << 0x0F,
    _TANG4            = 1ul << 0x10,
    _COMPRESSED_TANG  = 1ul << 0x11,

    Unk_13            = 1ul << 0x12,
    Unk_14            = 1ul << 0x13,
    Unk_15            = 1ul << 0x14,
    Unk_16            = 1ul << 0x15,
    _COLOR0            = 1ul << 0x16,
    _COLOR1            = 1ul << 0x17,
    _COLOR2            = 1ul << 0x18,

    _TEX0             = 1ul << 0x19,
    _TEX1             = 1ul << 0x1A,
    _TEX2             = 1ul << 0x1B,
    _TEX3             = 1ul << 0x1C,
    _TEX4             = 1ul << 0x1D,
    _COMPRESSED_TEX_0 = 1ul << 0x1E,
    _COMPRESSED_TEX_1 = 1ul << 0x1F,
    _COMPRESSED_TEX_2 = 1ul << 0x20,
    _COMPRESSED_TEX_3 = 1ul << 0x21,
    _COMPRESSED_TEX_4 = 1ul << 0x22,

    Unk_24 = 1ul << 0x23,
    Unk_25 = 1ul << 0x24,
    Unk_26 = 1ul << 0x25,
    Unk_27 = 1ul << 0x26,
    Unk_28 = 1ul << 0x27,
    Unk_29 = 1ul << 0x28,
    Unk_2A = 1ul << 0x29,
    Unk_2B = 1ul << 0x2A,
    Unk_2C = 1ul << 0x2B,
    Unk_2D = 1ul << 0x2C,
    Unk_2E = 1ul << 0x2D,
    Unk_2F = 1ul << 0x2E,
    Unk_30 = 1ul << 0x2F,
    Unk_31 = 1ul << 0x30,
    Unk_32 = 1ul << 0x31,
    Unk_33 = 1ul << 0x32,
    Unk_34 = 1ul << 0x33,
    Unk_35 = 1ul << 0x34,
    Unk_36 = 1ul << 0x35,
    Unk_37 = 1ul << 0x36,
    Unk_38 = 1ul << 0x37,
    Unk_39 = 1ul << 0x38,
    Unk_3A = 1ul << 0x39,
    Unk_3B = 1ul << 0x3A,
    Unk_3C = 1ul << 0x3B,
    Unk_3D = 1ul << 0x3C,
    Unk_3E = 1ul << 0x3D,
    Unk_3F = 1ul << 0x3E,
    #pragma warning restore format
  }

}

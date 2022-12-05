namespace LibSaber.HaloCEA.Enumerations
{

  [Flags]
  public enum InterleavedDataFlags : ulong
  {

    UNUSED_00 = 1UL << 00,
    UNUSED_01 = 1UL << 01,
    UNUSED_02 = 1UL << 02,
    UNUSED_03 = 1UL << 03,
    UNUSED_04 = 1UL << 04,
    UNUSED_05 = 1UL << 05,
    UNUSED_06 = 1UL << 06,
    UNUSED_07 = 1UL << 07,
    UNUSED_08 = 1UL << 08,
    UNUSED_09 = 1UL << 09,

    Unk_0A = 1UL << 10,
    Unk_0B = 1UL << 11,

    _TANG0 = 1UL << 12,
    _TANG1 = 1UL << 13,
    _TANG2 = 1UL << 14,
    _TANG3 = 1UL << 15,
    UNUSED_10 = 1UL << 16, // _TANG4?

    _COMPRESSED_TANG_0 = 1UL << 17,
    _COMPRESSED_TANG_1 = 1UL << 18,
    _COMPRESSED_TANG_2 = 1UL << 19,
    _COMPRESSED_TANG_3 = 1UL << 20,
    UNUSED_15 = 1UL << 21, // _COMPRESSED_TANG_4?

    _COL0 = 1UL << 22,
    _COL1 = 1UL << 23,
    _COL2 = 1UL << 24,

    _TEX0 = 1UL << 25,
    _TEX1 = 1UL << 26,
    _TEX2 = 1UL << 27,
    _TEX3 = 1UL << 28,
    UNUSED_1D = 1UL << 29, // _TEX4?

    _COMPRESSED_TEX_0 = 1UL << 30,
    _COMPRESSED_TEX_1 = 1UL << 31,
    _COMPRESSED_TEX_2 = 1UL << 32,
    _COMPRESSED_TEX_3 = 1UL << 33,
    UNUSED_22 = 1UL << 34, // _COMPRESSED_TEX_4?

    UNUSED_23 = 1UL << 35,
    UNUSED_24 = 1UL << 36,
    UNUSED_25 = 1UL << 37,
    UNUSED_26 = 1UL << 38,
    UNUSED_27 = 1UL << 39,
    UNUSED_28 = 1UL << 40,
    UNUSED_29 = 1UL << 41,
    UNUSED_2A = 1UL << 42,
    UNUSED_2B = 1UL << 43,
    UNUSED_2C = 1UL << 44,

    Unk_2D = 1UL << 45,

    UNUSED_2E = 1UL << 46,
    UNUSED_2F = 1UL << 47,

  }

}

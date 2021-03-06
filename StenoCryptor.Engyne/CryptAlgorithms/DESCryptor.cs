﻿using StenoCryptor.Commons;
using StenoCryptor.Commons.Enums;
using StenoCryptor.Interfaces;
using System;
using System.Globalization;
using System.IO;

namespace StenoCryptor.Engyne.CryptAlgorithms
{
    public class DESCryptor : ICryptor
    {
        #region Internal classes

        public delegate ulong CryptMethod3Key(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey);

        private class Const
        {
            static public readonly byte[] IPstart =  {   
                                            57,	49,	41,	33,	25,	17,	9,	1,
                                            59,	51,	43,	35,	27,	19,	11,	3,
                                            61,	53,	45,	37,	29,	21,	13,	5,
                                            63,	55,	47,	39,	31,	23,	15,	7,
                                            56,	48,	40,	32,	24,	16,	8,	0,
                                            58,	50,	42,	34,	26,	18,	10,	2,
                                            60,	52,	44,	36,	28,	20,	12,	4,
                                            62,	54,	46,	38,	30,	22,	14,	6
                                        };
            static public readonly byte[] IPend = {
                                            39,	7,	47,	15,	55,	23,	63,	31,
                                            38,	6,	46,	14,	54,	22,	62,	30,
                                            37,	5,	45,	13,	53,	21,	61,	29,
                                            36,	4,	44,	12,	52,	20,	60,	28,
                                            35,	3,	43,	11,	51,	19,	59,	27,
                                            34,	2,	42,	10,	50,	18,	58,	26,
                                            33,	1,	41,	9,	49,	17,	57,	25,
                                            32,	0,	40,	8,	48,	16,	56,	24
                                     };
            static public readonly byte[] E = {
                                            31,	0,	1,	2,	3,	4,
                                            3,	4,	5,	6,	7,	8,
                                            7,	8,	9,	10,	11,	12,
                                            11,	12,	13,	14,	15,	16,
                                            15,	16,	17,	18,	19,	20,
                                            19,	20,	21,	22,	23,	24,
                                            23,	24,	25,	26,	27,	28,
                                            27,	28,	29,	30,	31,	0
                                 };
            static public readonly byte[] P = {
                                            15,	6,	19,	20,
                                            28,	11,	27,	16,
                                            0,	14,	22,	25,
                                            4,	17,	30,	9,
                                            1,	7,	23,	13,
                                            31,	26,	2,	8,
                                            18,	12,	29,	5,
                                            21,	10,	3,	24
                                 };
            static public readonly byte[] G = {
                                            56,	48,	40,	32,	24,	16,	8,
                                            0,	57,	49,	41,	33,	25,	17,
                                            9,	1,	58,	50,	42,	34,	26,
                                            18,	10,	2,	59,	51,	43,	35,
                                            62,	54,	46,	38,	30,	22,	14,
                                            6,	61,	53,	45,	37,	29,	21,
                                            13,	5,	60,	52,	44,	36,	28,
                                            20,	12,	4,	27,	19,	11,	3

                                  };
            static public readonly byte[] H = {
                                            13,	16,	10,	23,	0,	4,
                                            2,	27,	14,	5,	20,	9,
                                            22,	18,	11,	3,	25,	7,
                                            15,	6,	26,	19,	12,	1,
                                            40,	51,	30,	36,	46,	54,
                                            29,	39,	50,	44,	32,	47,
                                            43,	48,	38,	55,	33,	52,
                                            45,	41,	49,	35,	28,	31
                                 };
            static public readonly byte[] C = {
                                            56,	48,	40,	32,	24,	16,	8,
                                            0,	57,	49,	41,	33,	25,	17,
                                            9,	1,	58,	50,	42,	34,	26,
                                            18,	10,	2,	59,	51,	43,	35
                                 };
            static public readonly byte[] D = {
                                            62,	54,	46,	38,	30,	22,	14,
                                            6,	61,	53,	45,	37,	29,	21,
                                            13,	5,	60,	52,	44,	36,	28,
                                            20,	12,	4,	27,	19,	11,	3
                                 };

            static public readonly byte[, ,] SBox = 
            
        {
            {
                {14,	4,	13,	1,	2,	15,	11,	8,	3,	10,	6,	12,	5,	9,	0,	7},
                {0,	15,	7,	4,	14,	2,	13,	1,	10,	6,	12,	11,	9,	5,	3,	8},
                {4,	1,	14,	8,	13,	6,	2,	11,	15,	12,	9,	7,	3,	10,	5,	0},
                {15,	12,	8,	2,	4,	9,	1,	7,	5,	11,	3,	14,	10,	0,	6,	13}
            
            },
        
            {
                {15,1,	8,	14,	6,	11,	3,	4,	9,	7,	2,	13,	12,	0,	5,	10},
                {3,	13,	4,	7,	15,	2,	8,	14,	12,	0,  1,	10,	6,	9,	11,	5},
                {0,	14,	7,	11,	10,	4,	13,	1,	5,	8,	12,	6,	9,	3,	2,	15},
                {13,	8,	10,	1,	3,	15,	4,	2,	11,	6,	7,	12,	0,	5,	14,	9}  
            },
        
            {
                {10,	0,	9,	14,	6,	3,	15,	5,	1,	13,	12,	7,	11,	4,	2,	8},
                {13,	7,	0,	9,	3,	4,	6,	10,	2,	8,	5,	14,	12,	11,	15,	1},
                {13,	6,	4,	9,	8,	15,	3,	0,	11,	1,	2,	12,	5,	10,	14,	7},
                {1,	10,	13,	0,	6,	9,	8,	7,	4,	15,	14,	3,	11,	5,	2,	12}
            },
        
            {
                {7,	13,	14,	3,	0,	6,	9,	10,	1,	2,	8,	5,	11,	12,	4,	15},
                {13,	8,	11,	5,	6,	15,	0,	3,	4,	7,	2,	12,	1,	10,	14,	9},
                {10,	6,	9,	0,	12,	11,	7,	13,	15,	1,	3,	14,	5,	2,	8,	4},
                {3,	15,	0,	6,	10,	1,	13,	8,	9,	4,	5,	11,	12,	7,	2,	14}
            },
        
            {
                {2,	12,	4,	1,	7,	10,	11,	6,	8,	5,	3,	15,	13,	0,	14,	9},
                {14,	11,	2,	12,	4,	7,	13,	1,	5,	0,	15,	10,	3,	9,	8,	6},
                {4,	2,	1,	11,	10,	13,	7,	8,	15,	9,	12,	5,	6,	3,	0,	14},
                {11,	8,	12,	7,	1,	14,	2,	13,	6,	15,	0,	9,	10,	4,	5,	3}
            },
        
            {
                {12,	1,	10,	15,	9,	2,	6,	8,	0,	13,	3,	4,	14,	7,	5,	11},
                {10,	15,	4,	2,	7,	12,	9,	5,	6,	1,	13,	14,	0,	11,	3,	8},
                {9,	14,	15,	5,	2,	8,	12,	3,	7,	0,	4,	10,	1,	13,	11,	6},
                {4,	3,	2,	12,	9,	5,	15,	10,	11,	14,	1,	7,	6,	0,	8,	13}
            },
        
            {
                {4,	11,	2,	14,	15,	0,	8,	13,	3,	12,	9,	7,	5,	10,	6,	1},
                {13,	0,	11,	7,	4,	9,	1,	10,	14,	3,	5,	12,	2,	15,	8,	6},
                {1,	4,	11,	13,	12,	3,	7,	14,	10,	15,	6,	8,	0,	5,	9,	2},
                {6,	11,	13,	8,	1,	4,	10,	7,	9,	5,	0,	15,	14,	2,	3,	12}
            },
        
            {
                {13,	2,	8,	4,	6,	15,	11,	1,	10,	9,	3,	14,	5,	0,	12,	7},
                {1,	15,	13,	8,	10,	3,	7,	4,	12,	5,	6,	11,	0,	14,	9,	2},
                {7,	11,	4,	1,	9,	12,	14,	2,	0,	6,	10,	13,	15,	3,	5,	8},
                {2,	1,	14,	7,	4,	10,	8,	13,	15,	12,	9,	0,	3,	5,	6,	11}
            }
        };
            static public readonly byte[] ShiftList = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
            static public readonly ulong[] WeekKeyArr64 = {
                                                        0x0101010101010101,
                                                        0xfefefefefefefefe,
                                                        0x1f1f1f1f0e0e0e0e,
                                                        0x0e0e0ef1f1f1f1f1,
                                                    };
            static public readonly ulong[] WeekKeyArr56 = {
                                                          0x2040810204081,
                                                          0xFDFBF7EFDFBF7E,
                                                          0x3E7CF9F1C3870E,
                                                          0x1C38771E3C78F1
                                                      };
            static public readonly ulong[] PartlyWeekKeyArr64 = {
                                                              0x01fe01fe01fe01fe,
                                                              0xfe01fe01fe01fe01,
                                                              0x1fe01fe01fe01fe0,
                                                              0xe0f1e0f1e0f1e0f1,
                                                              0x01e001e001f001f0,
                                                              0xe001e001f101f101,
                                                              0x1ffe1ffe0efe0efe,
                                                              0xfe1ffe1ffe0efe0e,
                                                              0x011f011f010e010e,
                                                              0x1f011f010e010e01,
                                                              0xe0fee0fef1fef1fe,
                                                              0xfee0fee0fef1fef1
                                                          };
            static public readonly ulong[] PartlyWeekKeyArr56 = {
                                                                0x3F80FE03F80FE,
                                                                0xFC07F01FC07F01,
                                                                0x3F80FE03F80FE0,
                                                                0xC1C7071C1C7071,
                                                                0x3800E003C00F0,
                                                                0xC007001E207881,
                                                                0x3FF8FFE1DF877E,
                                                                0xFC7FF1FFC3BF0E,
                                                                0x27C09F023808E,
                                                                0x3E04F811C04701,
                                                                0xC1FB07EE3FB8FE,
                                                                0xFD83F60FDC7F71
                                                            };
            static public readonly byte Mask1Bit = 0x1;
            static public readonly byte Mask2ndBit = 0x2;
            static public readonly byte Mask4Bit = 0xF;
            static public readonly byte Mask6Bit = 0x3F;
            static public readonly byte Mask7Bit = 0x7F;
            static public readonly byte Mask8Bit = 0xFF;
            static public readonly uint Mask28Bit = 0xFFFFFFF;
            static public readonly byte MainKeyPairBitCount = 8;
            static public readonly byte BlockSizeForFile = 8;
            static public readonly byte MainKeyBitCount = 56;
            static public readonly byte ExtendedKeyBitCount = 64;
            static public readonly byte LSBCount = 7;
            static public readonly byte CandDBitCount = 28;
            static public readonly byte RandLBitCount = 32;
            static public readonly byte SBoxTablesShift = 7;
            static public readonly byte SCompressFrom = 6;
            static public readonly byte SCompressTo = 4;
            static public readonly byte SResultBitCount = 32;
            static public readonly byte KeyListCount = 16;
            static public readonly int FileBufferSize = 0x2000;
        }

        class KeyGenerator
        {
            private ulong[] _keyList = new ulong[Const.KeyListCount];
            public ulong[] KeyList
            {
                get
                {
                    return _keyList;
                }
            }

            public void GetKeyList(ulong key)
            {
                ulong[] KeyArr = new ulong[Const.KeyListCount];
                uint C, D;
                key = ExtendKey(key);
                key = Toogle(key, Const.G);
                C = (uint)(key >> Const.CandDBitCount);
                D = (uint)(key & Const.Mask28Bit);
                for (int i = 0; i < Const.KeyListCount; i++)
                {
                    C = (C << Const.ShiftList[i]) | (C >> (Const.CandDBitCount - Const.ShiftList[i])) & Const.Mask28Bit;
                    D = (D << Const.ShiftList[i]) | (D >> (Const.CandDBitCount - Const.ShiftList[i])) & Const.Mask28Bit;
                    KeyArr[i] = Toogle(((C << Const.CandDBitCount) | D), Const.H);
                }
                _keyList = KeyArr;
            }
            protected ulong Toogle(ulong value, byte[] mas)
            {
                byte length = (byte)mas.Length;
                ulong res = 0;
                for (byte i = 0; i < length; i++)
                {
                    res |= ((value >> mas[i]) & 1) << i;
                }
                return res;
            }
            public ulong ExtendKey(ulong key56)
            {
                ulong res = 0, temp = 0;
                byte ss = 0;
                byte isNotPair = 0, sample7bit = 0;
                for (byte i = 0; i < 56; i++)
                {
                    isNotPair ^= (byte)(sample7bit & Const.Mask1Bit);
                    sample7bit >>= 1;
                    ss++;
                    if (ss == 7)
                    {
                        sample7bit = (byte)(key56 & Const.Mask7Bit);
                        res |= (((key56 & Const.Mask7Bit) | (uint)((isNotPair ^ 1) << Const.LSBCount)) << Const.MainKeyBitCount);
                        key56 >>= 7;
                        temp = res;
                        res >>= 8;
                        isNotPair = 0;
                        ss = 0;
                    }

                }
                return temp;
            }
        }

        class DES : KeyGenerator
        {
            private uint S(ulong value48)
            {
                uint res32 = 0;
                byte temp;
                for (int counter = Const.SBoxTablesShift; counter >= 0; counter--)
                {
                    temp = (byte)value48;
                    res32 = ((((uint)Const.SBox[counter, (((temp >> Const.SCompressTo) & Const.Mask2ndBit) | (temp & 1)), (temp >> 1) & Const.Mask4Bit]) << Const.SResultBitCount) | res32) >> Const.SCompressTo;
                    value48 >>= Const.SCompressFrom;
                }
                return res32;
            }
            private ulong EncryptRound(ulong value, ulong Key_i)
            {
                uint L, R;
                R = (uint)(value);
                L = (uint)(value >> Const.RandLBitCount);
                L = L ^ (uint)Toogle(S((Toogle(R, Const.E)) ^ Key_i), Const.P);
                return ((ulong)R << Const.RandLBitCount) | L;
            }
            public ulong Crypt(ulong value, ulong[] KeyList, bool FlipKey)
            {
                value = Toogle(value, Const.IPstart);
                byte f = Convert.ToByte(FlipKey);
                int[,] a = new int[2, 3];
                a[0, 0] = Const.KeyListCount - 1;
                a[0, 1] = 0;
                a[0, 2] = -1;
                a[1, 0] = 0;
                a[1, 1] = Const.KeyListCount - 1;
                a[1, 2] = 1;

                for (int i = a[f, 0]; i != a[f, 1]; i = i + a[f, 2])
                {
                    value = EncryptRound(value, KeyList[i]);
                }

                return Toogle((value << Const.RandLBitCount) | (value >> Const.RandLBitCount), Const.IPend);
            }
        }

        class _3DES : DES
        {
            public ulong Crypt(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, bool FlipKey1, bool FlipKey2, bool FlipKey3)
            {
                return base.Crypt(base.Crypt(base.Crypt(value, KeyList1, FlipKey1), KeyList2, FlipKey2), KeyList3, FlipKey3);
            }
        }

        class FileProcessing
        {
            public void EnCryptFile(Stream fsRead, Stream fsWrite, CryptMethod3Key Algorythm, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                if (fsRead.Length > Const.FileBufferSize)
                {
                    ProcessSelectedPart(Const.FileBufferSize, 0, fsRead.Length - fsRead.Length % Const.FileBufferSize, fsRead, fsWrite, Algorythm, KeyList1, KeyList2, KeyList3, IV, FlipKey);
                }
               
                EnCryptLastPart(Const.BlockSizeForFile, fsRead.Length - fsRead.Length % Const.FileBufferSize, fsRead, fsWrite, Algorythm, KeyList1, KeyList2, KeyList3, IV, FlipKey);
            }
            public void DeCryptFile(Stream fsRead, Stream fsWrite, CryptMethod3Key Algorythm, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                if (fsRead.Length > Const.FileBufferSize)
                {
                    ProcessSelectedPart(Const.FileBufferSize, 0, fsRead.Length - fsRead.Length % Const.FileBufferSize, fsRead, fsWrite, Algorythm, KeyList1, KeyList2, KeyList3, IV, FlipKey);
                }
                    
                DeCryptLastPart(Const.BlockSizeForFile, 0, fsRead, fsWrite, Algorythm, KeyList1, KeyList2, KeyList3, IV, FlipKey);
            }
            private void ProcessSelectedPart(int BufferSize, long Begin, long End, Stream fsRead, Stream fsWrite, CryptMethod3Key Algorythm, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                byte[] Buffer = new byte[BufferSize];
                byte[] CurBuffer = new byte[BufferSize];
                byte[] temp;
                IAsyncResult ReadResult;
                IAsyncResult WriteResult;
                fsRead.Seek(Begin, SeekOrigin.Begin);
                ReadResult = fsRead.BeginRead(Buffer, 0, BufferSize, null, null);
                fsRead.EndRead(ReadResult);
                long i;
                for (i = Begin + BufferSize; i < End; i += BufferSize)
                {
                    fsRead.Seek(i, SeekOrigin.Begin);
                    ReadResult = fsRead.BeginRead(CurBuffer, 0, BufferSize, null, null);
                    Buffer = CryptBuffer(Buffer, Algorythm, KeyList1, KeyList2, KeyList3, ref IV, FlipKey);
                    fsWrite.Seek(i - BufferSize, SeekOrigin.Begin);
                    WriteResult = fsWrite.BeginWrite(Buffer, 0, BufferSize, null, null);
                    fsRead.EndRead(ReadResult);
                    fsWrite.EndWrite(WriteResult);
                    temp = Buffer;
                    Buffer = CurBuffer;
                    CurBuffer = temp;
                }
                fsWrite.Seek(i - BufferSize, SeekOrigin.Begin);
                WriteResult = fsWrite.BeginWrite(Buffer, 0, BufferSize, null, null);
                fsWrite.EndWrite(WriteResult);
            }
            private void EnCryptLastPart(int BufferSize, long Begin, Stream fsRead, Stream fsWrite, CryptMethod3Key Algorythm, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                ProcessSelectedPart(BufferSize, Begin, fsRead.Length - fsRead.Length % BufferSize, fsRead, fsWrite, Algorythm, KeyList1, KeyList2, KeyList3, IV, FlipKey);
                if ((fsRead.Length % BufferSize) != 0)
                {
                    byte[] Buffer = new byte[Const.BlockSizeForFile];
                    fsRead.Seek(fsRead.Length - fsRead.Length % BufferSize, SeekOrigin.Begin);
                    fsRead.Read(Buffer, 0, Const.BlockSizeForFile);
                    Buffer = CryptBuffer(Buffer, Algorythm, KeyList1, KeyList2, KeyList3, ref IV, FlipKey);
                    fsWrite.Seek(fsRead.Length - fsRead.Length % BufferSize, SeekOrigin.Begin);
                    fsWrite.Write(Buffer, 0, Const.BlockSizeForFile);
                    byte[] part = new byte[1] { (byte)(fsRead.Length % BufferSize) };
                    fsWrite.Seek(fsWrite.Length, SeekOrigin.Begin);
                    fsWrite.Write(part, 0, 1);
                }
            }
            private void DeCryptLastPart(int BufferSize, long Begin, Stream fsRead, Stream fsWrite, CryptMethod3Key Algorythm, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                if ((fsRead.Length % BufferSize) != 0)
                {
                    ProcessSelectedPart(BufferSize, Begin, fsRead.Length - fsRead.Length % BufferSize - BufferSize, fsRead, fsWrite, Algorythm, KeyList1, KeyList2, KeyList3, IV, FlipKey);
                    byte[] byt = new byte[1];
                    fsRead.Seek(-1, SeekOrigin.End);
                    fsRead.Read(byt, 0, 1);
                    byte[] lastPart = new byte[Const.BlockSizeForFile];
                    fsRead.Seek(-1 - Const.BlockSizeForFile, SeekOrigin.End);
                    fsRead.Read(lastPart, 0, Const.BlockSizeForFile);
                    lastPart = CryptBuffer(lastPart, Algorythm, KeyList1, KeyList2, KeyList3, ref IV, FlipKey);
                    try
                    {
                        fsWrite.Write(lastPart, 0, byt[0]);
                    }
                    catch
                    {
                        throw new Exception("Selected wrong file!");
                    }
                }
                else
                {
                    ProcessSelectedPart(BufferSize, Begin, fsRead.Length, fsRead, fsWrite, Algorythm, KeyList1, KeyList2, KeyList3, IV, FlipKey);
                }
            }
            private byte[] CryptBuffer(byte[] buffer, CryptMethod3Key Algorythm, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ref ulong IV, bool FlipKey)
            {
                ulong bufLong = 0;
                for (long i = 0; i < buffer.Length; i += Const.BlockSizeForFile)
                {
                    ulong temp = 0;
                    for (byte j = 0; j < Const.BlockSizeForFile; j++)
                    {
                        temp |= (ulong)buffer[i + j];
                        bufLong = temp;
                        temp <<= Const.BlockSizeForFile;
                    }
                    bufLong = Algorythm(bufLong, KeyList1, KeyList2, KeyList3, IV, FlipKey);
                    for (short j = Const.SBoxTablesShift; j >= 0; j--)
                    {
                        buffer[i + j] = (byte)(bufLong & Const.Mask8Bit);
                        bufLong >>= Const.BlockSizeForFile;
                    }
                }
                return buffer;
            }
        }

        #endregion Internal classes

        #region Contants

        static public readonly byte[] IPstart =  {   
                                            57,	49,	41,	33,	25,	17,	9,	1,
                                            59,	51,	43,	35,	27,	19,	11,	3,
                                            61,	53,	45,	37,	29,	21,	13,	5,
                                            63,	55,	47,	39,	31,	23,	15,	7,
                                            56,	48,	40,	32,	24,	16,	8,	0,
                                            58,	50,	42,	34,	26,	18,	10,	2,
                                            60,	52,	44,	36,	28,	20,	12,	4,
                                            62,	54,	46,	38,	30,	22,	14,	6
                                        };
        static public readonly byte[] IPend = {
                                            39,	7,	47,	15,	55,	23,	63,	31,
                                            38,	6,	46,	14,	54,	22,	62,	30,
                                            37,	5,	45,	13,	53,	21,	61,	29,
                                            36,	4,	44,	12,	52,	20,	60,	28,
                                            35,	3,	43,	11,	51,	19,	59,	27,
                                            34,	2,	42,	10,	50,	18,	58,	26,
                                            33,	1,	41,	9,	49,	17,	57,	25,
                                            32,	0,	40,	8,	48,	16,	56,	24
                                     };
        static public readonly byte[] E = {
                                            31,	0,	1,	2,	3,	4,
                                            3,	4,	5,	6,	7,	8,
                                            7,	8,	9,	10,	11,	12,
                                            11,	12,	13,	14,	15,	16,
                                            15,	16,	17,	18,	19,	20,
                                            19,	20,	21,	22,	23,	24,
                                            23,	24,	25,	26,	27,	28,
                                            27,	28,	29,	30,	31,	0
                                 };
        static public readonly byte[] P = {
                                            15,	6,	19,	20,
                                            28,	11,	27,	16,
                                            0,	14,	22,	25,
                                            4,	17,	30,	9,
                                            1,	7,	23,	13,
                                            31,	26,	2,	8,
                                            18,	12,	29,	5,
                                            21,	10,	3,	24
                                 };
        static public readonly byte[] G = {
                                            56,	48,	40,	32,	24,	16,	8,
                                            0,	57,	49,	41,	33,	25,	17,
                                            9,	1,	58,	50,	42,	34,	26,
                                            18,	10,	2,	59,	51,	43,	35,
                                            62,	54,	46,	38,	30,	22,	14,
                                            6,	61,	53,	45,	37,	29,	21,
                                            13,	5,	60,	52,	44,	36,	28,
                                            20,	12,	4,	27,	19,	11,	3

                                  };
        static public readonly byte[] H = {
                                            13,	16,	10,	23,	0,	4,
                                            2,	27,	14,	5,	20,	9,
                                            22,	18,	11,	3,	25,	7,
                                            15,	6,	26,	19,	12,	1,
                                            40,	51,	30,	36,	46,	54,
                                            29,	39,	50,	44,	32,	47,
                                            43,	48,	38,	55,	33,	52,
                                            45,	41,	49,	35,	28,	31
                                 };
        static public readonly byte[] C = {
                                            56,	48,	40,	32,	24,	16,	8,
                                            0,	57,	49,	41,	33,	25,	17,
                                            9,	1,	58,	50,	42,	34,	26,
                                            18,	10,	2,	59,	51,	43,	35
                                 };
        static public readonly byte[] D = {
                                            62,	54,	46,	38,	30,	22,	14,
                                            6,	61,	53,	45,	37,	29,	21,
                                            13,	5,	60,	52,	44,	36,	28,
                                            20,	12,	4,	27,	19,	11,	3
                                 };

        static public readonly byte[, ,] SBox = 
            
        {
            {
                {14,	4,	13,	1,	2,	15,	11,	8,	3,	10,	6,	12,	5,	9,	0,	7},
                {0,	15,	7,	4,	14,	2,	13,	1,	10,	6,	12,	11,	9,	5,	3,	8},
                {4,	1,	14,	8,	13,	6,	2,	11,	15,	12,	9,	7,	3,	10,	5,	0},
                {15,	12,	8,	2,	4,	9,	1,	7,	5,	11,	3,	14,	10,	0,	6,	13}
            
            },
        
            {
                {15,1,	8,	14,	6,	11,	3,	4,	9,	7,	2,	13,	12,	0,	5,	10},
                {3,	13,	4,	7,	15,	2,	8,	14,	12,	0,  1,	10,	6,	9,	11,	5},
                {0,	14,	7,	11,	10,	4,	13,	1,	5,	8,	12,	6,	9,	3,	2,	15},
                {13,	8,	10,	1,	3,	15,	4,	2,	11,	6,	7,	12,	0,	5,	14,	9}  
            },
        
            {
                {10,	0,	9,	14,	6,	3,	15,	5,	1,	13,	12,	7,	11,	4,	2,	8},
                {13,	7,	0,	9,	3,	4,	6,	10,	2,	8,	5,	14,	12,	11,	15,	1},
                {13,	6,	4,	9,	8,	15,	3,	0,	11,	1,	2,	12,	5,	10,	14,	7},
                {1,	10,	13,	0,	6,	9,	8,	7,	4,	15,	14,	3,	11,	5,	2,	12}
            },
        
            {
                {7,	13,	14,	3,	0,	6,	9,	10,	1,	2,	8,	5,	11,	12,	4,	15},
                {13,	8,	11,	5,	6,	15,	0,	3,	4,	7,	2,	12,	1,	10,	14,	9},
                {10,	6,	9,	0,	12,	11,	7,	13,	15,	1,	3,	14,	5,	2,	8,	4},
                {3,	15,	0,	6,	10,	1,	13,	8,	9,	4,	5,	11,	12,	7,	2,	14}
            },
        
            {
                {2,	12,	4,	1,	7,	10,	11,	6,	8,	5,	3,	15,	13,	0,	14,	9},
                {14,	11,	2,	12,	4,	7,	13,	1,	5,	0,	15,	10,	3,	9,	8,	6},
                {4,	2,	1,	11,	10,	13,	7,	8,	15,	9,	12,	5,	6,	3,	0,	14},
                {11,	8,	12,	7,	1,	14,	2,	13,	6,	15,	0,	9,	10,	4,	5,	3}
            },
        
            {
                {12,	1,	10,	15,	9,	2,	6,	8,	0,	13,	3,	4,	14,	7,	5,	11},
                {10,	15,	4,	2,	7,	12,	9,	5,	6,	1,	13,	14,	0,	11,	3,	8},
                {9,	14,	15,	5,	2,	8,	12,	3,	7,	0,	4,	10,	1,	13,	11,	6},
                {4,	3,	2,	12,	9,	5,	15,	10,	11,	14,	1,	7,	6,	0,	8,	13}
            },
        
            {
                {4,	11,	2,	14,	15,	0,	8,	13,	3,	12,	9,	7,	5,	10,	6,	1},
                {13,	0,	11,	7,	4,	9,	1,	10,	14,	3,	5,	12,	2,	15,	8,	6},
                {1,	4,	11,	13,	12,	3,	7,	14,	10,	15,	6,	8,	0,	5,	9,	2},
                {6,	11,	13,	8,	1,	4,	10,	7,	9,	5,	0,	15,	14,	2,	3,	12}
            },
        
            {
                {13,	2,	8,	4,	6,	15,	11,	1,	10,	9,	3,	14,	5,	0,	12,	7},
                {1,	15,	13,	8,	10,	3,	7,	4,	12,	5,	6,	11,	0,	14,	9,	2},
                {7,	11,	4,	1,	9,	12,	14,	2,	0,	6,	10,	13,	15,	3,	5,	8},
                {2,	1,	14,	7,	4,	10,	8,	13,	15,	12,	9,	0,	3,	5,	6,	11}
            }
        };
        static public readonly byte[] ShiftList = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
        static public readonly ulong[] WeekKeyArr64 = {
                                                        0x0101010101010101,
                                                        0xfefefefefefefefe,
                                                        0x1f1f1f1f0e0e0e0e,
                                                        0x0e0e0ef1f1f1f1f1,
                                                    };
        static public readonly ulong[] WeekKeyArr56 = {
                                                          0x2040810204081,
                                                          0xFDFBF7EFDFBF7E,
                                                          0x3E7CF9F1C3870E,
                                                          0x1C38771E3C78F1
                                                      };
        static public readonly ulong[] PartlyWeekKeyArr64 = {
                                                              0x01fe01fe01fe01fe,
                                                              0xfe01fe01fe01fe01,
                                                              0x1fe01fe01fe01fe0,
                                                              0xe0f1e0f1e0f1e0f1,
                                                              0x01e001e001f001f0,
                                                              0xe001e001f101f101,
                                                              0x1ffe1ffe0efe0efe,
                                                              0xfe1ffe1ffe0efe0e,
                                                              0x011f011f010e010e,
                                                              0x1f011f010e010e01,
                                                              0xe0fee0fef1fef1fe,
                                                              0xfee0fee0fef1fef1
                                                          };
        static public readonly ulong[] PartlyWeekKeyArr56 = {
                                                                0x3F80FE03F80FE,
                                                                0xFC07F01FC07F01,
                                                                0x3F80FE03F80FE0,
                                                                0xC1C7071C1C7071,
                                                                0x3800E003C00F0,
                                                                0xC007001E207881,
                                                                0x3FF8FFE1DF877E,
                                                                0xFC7FF1FFC3BF0E,
                                                                0x27C09F023808E,
                                                                0x3E04F811C04701,
                                                                0xC1FB07EE3FB8FE,
                                                                0xFD83F60FDC7F71
                                                            };
        static public readonly byte Mask1Bit = 0x1;
        static public readonly byte Mask2ndBit = 0x2;
        static public readonly byte Mask4Bit = 0xF;
        static public readonly byte Mask6Bit = 0x3F;
        static public readonly byte Mask7Bit = 0x7F;
        static public readonly byte Mask8Bit = 0xFF;
        static public readonly uint Mask28Bit = 0xFFFFFFF;
        static public readonly byte MainKeyPairBitCount = 8;
        static public readonly byte BlockSizeForFile = 8;
        static public readonly byte MainKeyBitCount = 56;
        static public readonly byte ExtendedKeyBitCount = 64;
        static public readonly byte LSBCount = 7;
        static public readonly byte CandDBitCount = 28;
        static public readonly byte RandLBitCount = 32;
        static public readonly byte SBoxTablesShift = 7;
        static public readonly byte SCompressFrom = 6;
        static public readonly byte SCompressTo = 4;
        static public readonly byte SResultBitCount = 32;
        static public readonly byte KeyListCount = 16;
        static public readonly int FileBufferSize = 0x2000;


        #endregion Constants

        #region Constructors

        public DESCryptor()
        {
            _3des = new _3DES();

            //DES
            cm1k = delegate(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                return _3des.Crypt(value, KeyList1, FlipKey);
            };
            cm1kCBCENcrupt = delegate(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                IV = _3des.Crypt(value ^ IV, KeyList1, FlipKey);
                return IV;
            };
            cm1kCBCDEcrupt = delegate(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                IV = _3des.Crypt(value, KeyList1, FlipKey) ^ IV;
                return IV;
            };

            //DESEEE3
            DESEEE3 = delegate(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                return _3des.Crypt(value, KeyList1, KeyList2, KeyList3, FlipKey, FlipKey, FlipKey);
            };
            DESEEE3CBCEncrypt = delegate(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
           {
               return _3des.Crypt(value ^ IV, KeyList1, KeyList2, KeyList3, FlipKey, FlipKey, FlipKey);
           };
            DESEEE3CBCDecrypt = delegate(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
           {
               return _3des.Crypt(value, KeyList1, KeyList2, KeyList3, FlipKey, FlipKey, FlipKey) ^ IV;
           };

            //DESEDE3
            DESEDE3 = delegate(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                return _3des.Crypt(value, KeyList1, KeyList2, KeyList3, FlipKey, !FlipKey, FlipKey);
            };
            DESEDE3CBCEncrypt = delegate(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                return _3des.Crypt(value ^ IV, KeyList1, KeyList2, KeyList3, FlipKey, !FlipKey, FlipKey);
            };
            DESEDE3CBCDecrypt = delegate(ulong value, ulong[] KeyList1, ulong[] KeyList2, ulong[] KeyList3, ulong IV, bool FlipKey)
            {
                return _3des.Crypt(value, KeyList1, KeyList2, KeyList3, FlipKey, !FlipKey, FlipKey) ^ IV;
            };
        }

        #endregion Constructors

        #region ICryptor Implementation

        public Stream Encrypt(Stream message, Key key)
        {
            DES DESkey1 = new DES();
            FileProcessing fp = new FileProcessing();
            MemoryStream ms = new MemoryStream();
            
            fp.EnCryptFile(message, ms, cm1k, DESkey1.KeyList, null, null, 0, false);

            return ms;
        }


        public Stream Decrypt(Stream message, Key key)
        {
            DES DESkey1 = new DES();
            FileProcessing fp = new FileProcessing();
            MemoryStream ms = new MemoryStream(); 

            fp.DeCryptFile(message, ms, cm1k, DESkey1.KeyList, null, null, 0, true);

            return ms;
        }

        #endregion ICryptor Implementation

        #region IKeyAware Implementation

        public bool ValidateKey(string stringKey)
        {
            byte result;

            if (string.IsNullOrEmpty(stringKey) && stringKey.Length > 7)
                return false;

            foreach (char item in stringKey)
            {
                if (!byte.TryParse(item.ToString(), NumberStyles.HexNumber, null, out result))
                    return false;
            }

            return true;
        }

        public byte[] ParseKey(string stringKey)
        {
            byte[] bytesKey = new byte[stringKey.Length];

            for (int i = 0; i < stringKey.Length; i++)
            {
                bytesKey[i] = byte.Parse(stringKey[i].ToString(), NumberStyles.HexNumber);
            }

            return bytesKey;
        }

        public bool ValidateKey(Key key, Container container)
        {
            return key.CryptType == CryptType.DES && key.MessageLength == container.InputStream.Length;
        }

        #endregion ICryptor Implementation

        #region Private Fields

        private _3DES _3des;

        private CryptMethod3Key cm1k;

        private CryptMethod3Key cm1kCBCENcrupt;

        private CryptMethod3Key cm1kCBCDEcrupt;

        private CryptMethod3Key DESEEE3;

        private CryptMethod3Key DESEEE3CBCEncrypt;

        private CryptMethod3Key DESEEE3CBCDecrypt;

        private CryptMethod3Key DESEDE3;

        private CryptMethod3Key DESEDE3CBCEncrypt;

        private CryptMethod3Key DESEDE3CBCDecrypt;

        #endregion Private Fields
    }
}

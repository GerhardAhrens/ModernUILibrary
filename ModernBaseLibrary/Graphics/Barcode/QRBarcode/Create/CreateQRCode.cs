//-----------------------------------------------------------------------
// <copyright file="CreateQRCode.cs" company="Lifeprojects.de">
//     Class: CreateQRCode
//     Copyright © Gerhard Ahrens, 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.04.2021</date>
//
// <summary>
// Klasse zum erstellen eines QR Barcode
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Barcode
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Text;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public class CreateQRCode : DisposableCoreBase
    {
        private QRModuleType[,] modules;
        private int[,] accessCount;
        private bool[,] freeMask;
        private int dim;
        private readonly Dictionary<char, int> AlphaNumericTable = null;
        private readonly int[][] AlignmentPatternLocations = null;
        private readonly QRMode[] NormalModes = null;
        private readonly QRMode[][] MicroModes = null;
        private readonly QRErrorCorrection[] NormalErrorCorrectionLevels = null;
        private readonly QRErrorCorrection[][] MicroErrorCorrectionLevels = null;
        private readonly BitArray[] NormalModeEncodings = null;
        private readonly List<Tuple<int, QRMode, BitArray>> MicroModeEncodings = null;
        private readonly Tuple<QRSymbolType, int, QRMode, int>[] CharacterWidthTable = null;
        private readonly Tuple<QRSymbolType, int, QRErrorCorrection, Tuple<int, int, int, int>[]>[] ErrorCorrectionTable = null;
        private readonly Tuple<QRSymbolType, int, QRErrorCorrection, Tuple<int, int, int, int, int>>[] DataCapacityTable = null;
        private readonly byte[][] Polynomials = null;

        #region Construction
        /// <summary>
        /// Create a QR symbol that represents the supplied `data'.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="errorCorrection"></param>
        public CreateQRCode(string data) : this(data, QRErrorCorrection.M, false)
        {
        }

        /// <summary>
        /// Create a QR symbol that represents the supplied `data' with the indicated minimum level of error correction.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <param name="errorCorrection"></param>
        public CreateQRCode(string data, QRErrorCorrection minimumErrorCorrection) : this(data, minimumErrorCorrection, false)
        {
        }

        /// <summary>
        /// Create a QR symbol that represents the supplied `data' with the indicated minimum level of error correction.
        /// </summary>
        public CreateQRCode(string data, QRErrorCorrection minimumErrorCorrection, bool allowMicroCodes)
        {
            using(QRDataTables dataTables = new QRDataTables())
            {
                this.AlphaNumericTable = dataTables.AlphaNumericTable;
                this.AlignmentPatternLocations = dataTables.AlignmentPatternLocations;
                this.NormalModes = dataTables.NormalModes;
                this.MicroModes = dataTables.MicroModes;
                this.NormalErrorCorrectionLevels = dataTables.NormalErrorCorrectionLevels;
                this.MicroErrorCorrectionLevels = dataTables.MicroErrorCorrectionLevels;
                this.NormalModeEncodings = dataTables.NormalModeEncodings;
                this.MicroModeEncodings = dataTables.MicroModeEncodings;
                this.CharacterWidthTable = dataTables.CharacterWidthTable;
                this.ErrorCorrectionTable = dataTables.ErrorCorrectionTable;
                this.DataCapacityTable = dataTables.DataCapacityTable;
                this.Polynomials = dataTables.Polynomials;
            }

            var mode = ChooseParameters(data, minimumErrorCorrection, allowMicroCodes);
            var codeWords = CreateCodeWords(data, mode);
            var bits = AddErrorCorrection(codeWords);
            this.Reserve();
            Fill(bits);
            var mask = Mask();
            this.AddFormatInformation(mask);
            this.AddVersionInformation();
        }
        #endregion

        #region External Interface
        /// <summary>
        /// Type of QR symbol (normal or micro)
        /// </summary>
        public QRSymbolType Type { get; private set; }

        /// <summary>
        /// Version of QR symbol (1-5 or 1-40, depending on type)
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Level of error correction in this symbol
        /// </summary>
        public QRErrorCorrection QRErrorCorrection { get; private set; }

        /// <summary>
        /// A textual description of a QR code's metadata
        /// </summary>
        public string Description
        {
            get
            {
                switch (Type)
                {
                    case QRSymbolType.Micro:
                        if (Version == 1)
                            return String.Format("QR M{0}", Version);
                        else
                            return String.Format("QR M{0}-{1}", Version, QRErrorCorrection);
                    case QRSymbolType.Normal:
                        return String.Format("QR {0}-{1}", Version, QRErrorCorrection);
                }

                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Save the QR code as an image at the following scale.
        /// </summary>
        /// <param name="path">Path of image file to create.</param>
        /// <param name="scale">Size of a module, in pixels.</param>
        public void Save(string imagePath, int scale)
        {
            using (Bitmap b = ToBitmap(scale))
            {
                b.Save(imagePath, ImageFormat.Png);
            }
        }

        /// <summary>
        /// Generate a bitmap of this QR code at the following scale.
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public Bitmap ToBitmap(int scale)
        {
            Bitmap b = new Bitmap(dim * scale, dim * scale);

            using (Graphics g = Graphics.FromImage(b))
            {
                Render(g, scale);
            }

            return b;
        }

        /// <summary>
        /// Render this bitmap to the supplied Graphics object at the indicated scale.
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public void Render(Graphics g, int scale)
        {
            var brush = new SolidBrush(Color.Black);

            g.Clear(Color.White);

            for (int x = 0; x < dim; x++)
            {
                for (int y = 0; y < dim; y++)
                {
                    if (Get(x, y) == QRModuleType.Dark)
                    {
                        g.FillRectangle(brush, x * scale, y * scale, scale, scale);
                    }
                }
            }
        }
        #endregion

        #region Steps
        /// <summary>
        /// Choose suitable values for Type, Version, QRErrorCorrection and QRMode.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private QRMode ChooseParameters(string data, QRErrorCorrection minimumErrorCorrection, bool allowMicroCodes)
        {
            // get list of error correction modes at least as good as the user-specified one
            var allowedErrorCorrectionModes = new QRErrorCorrection[]
            {
                QRErrorCorrection.None,
                QRErrorCorrection.L,
                QRErrorCorrection.M,
                QRErrorCorrection.Q,
                QRErrorCorrection.H,
            }.SkipWhile(e => e != minimumErrorCorrection).ToList();

            // get the tightest-fit encoding mode
            QRMode tightestMode;
            if (data.All(c => Char.IsDigit(c)))
                tightestMode = QRMode.Numeric;
            else if (data.All(c => this.AlphaNumericTable.ContainsKey(c)))
                tightestMode = QRMode.AlphaNumeric;
            else
                tightestMode = QRMode.Byte;

            // get list of allowed encoding modes
            var allowedModes = new QRMode[]
            {
                QRMode.Numeric,
                QRMode.AlphaNumeric,
                QRMode.Byte
            }.SkipWhile(m => m != tightestMode).ToList();

            // get list of possible types
            List<Tuple<QRSymbolType, byte>> possibleTypes =
                allowMicroCodes
                ? Enumerable.Concat(
                        Enumerable.Range(1, 4).Select(i => Tuple.Create(QRSymbolType.Micro, (byte)i)),
                        Enumerable.Range(1, 40).Select(i => Tuple.Create(QRSymbolType.Normal, (byte)i))).ToList()
                : Enumerable.Range(1, 40).Select(i => Tuple.Create(QRSymbolType.Normal, (byte)i)).ToList();

            // for each type in ascending order of size
            foreach (var p in possibleTypes)
            {
                // for each error correction level from most to least
                foreach (var e in allowedErrorCorrectionModes.Intersect(GetAvailableErrorCorrectionLevels(p.Item1, p.Item2)).Reverse())
                {
                    // lookup the data capacity
                    var capacityEntry = DataCapacityTable.First(f => f.Item1 == p.Item1 && f.Item2 == p.Item2 && f.Item3 == e).Item4;

                    // for each encoding mode from tightest to loosest
                    foreach (var m in allowedModes.Intersect(GetAvailableModes(p.Item1, p.Item2)))
                    {
                        int capacity = 0;

                        switch (m)
                        {
                            case QRMode.Numeric: capacity = capacityEntry.Item2; break;
                            case QRMode.AlphaNumeric: capacity = capacityEntry.Item3; break;
                            case QRMode.Byte: capacity = capacityEntry.Item4; break;
                            default: capacity = 0; break;
                        }

                        // if there is enough room, we've found our solution
                        if (capacity >= data.Length)
                        {
                            Type = p.Item1;
                            Version = p.Item2;
                            QRErrorCorrection = e;
                            return m;
                        }
                    }
                }
            }

            throw new InvalidOperationException("no suitable parameters found");
        }

        /// <summary>
        /// Encode the data in the following mode, pad, and return final code words.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <returns>The fully-encoded data.</returns>
        private byte[] CreateCodeWords(string data, QRMode mode)
        {
            #region Code word creation
            // encode data as series of bit arrays
            List<BitArray> bits = new List<BitArray>();

            // add mode indicator
            bits.Add(EncodeMode(mode));

            // add character count
            bits.Add(EncodeCharacterCount(mode, data.Length));

            // perform mode-specific data encoding
            switch (mode)
            {
                case QRMode.Byte:
                    {
                        // retrieve UTF8 encoding of data
                        bits.Add(Encoding.UTF8.GetBytes(data).ToBitArray());
                    }
                    break;

                case QRMode.Numeric:
                    {
                        int idx;

                        // for every triple of digits
                        for (idx = 0; idx < data.Length - 2; idx += 3)
                        {
                            // encode them as a 3-digit decimal number
                            int x = this.AlphaNumericTable[data[idx]] * 100 + this.AlphaNumericTable[data[idx + 1]] * 10 + this.AlphaNumericTable[data[idx + 2]];
                            bits.Add(x.ToBitArray(10));
                        }

                        // if there is a remaining pair of digits
                        if (idx < data.Length - 1)
                        {
                            // encode them as a 2-digit decimal number
                            int x = this.AlphaNumericTable[data[idx]] * 10 + this.AlphaNumericTable[data[idx + 1]];
                            idx += 2;
                            bits.Add(x.ToBitArray(7));
                        }

                        // if there is a remaining digit
                        if (idx < data.Length)
                        {
                            // encode it as a decimal number
                            int x = this.AlphaNumericTable[data[idx]];
                            idx += 1;
                            bits.Add(x.ToBitArray(4));
                        }
                    }
                    break;

                case QRMode.AlphaNumeric:
                    {
                        int idx;

                        // for every pair of characters
                        for (idx = 0; idx < data.Length - 1; idx += 2)
                        {
                            // encode them as a single number
                            int x = this.AlphaNumericTable[data[idx]] * 45 + this.AlphaNumericTable[data[idx + 1]];
                            bits.Add(x.ToBitArray(11));
                        }

                        // if there is a remaining character
                        if (idx < data.Length)
                        {
                            // encode it as a number
                            int x = this.AlphaNumericTable[data[idx]];
                            bits.Add(x.ToBitArray(6));
                        }
                    }
                    break;
            }

            // add the terminator mode marker
            bits.Add(EncodeMode(QRMode.Terminator));

            // calculate the bitstream's total length, in bits
            int bitstreamLength = bits.Sum(b => b.Length);

            // check the full capacity of symbol, in bits
            int capacity = DataCapacityTable.First(f => f.Item1 == Type && f.Item2 == Version && f.Item3 == QRErrorCorrection).Item4.Item1 * 8;

            // M1 and M3 are actually shorter by 1 nibble
            if (Type == QRSymbolType.Micro && (Version == 3 || Version == 1))
                capacity -= 4;

            // pad the bitstream to the nearest octet boundary with zeroes
            if (bitstreamLength < capacity && bitstreamLength % 8 != 0)
            {
                int paddingLength = Math.Min(8 - (bitstreamLength % 8), capacity - bitstreamLength);
                bits.Add(new BitArray(paddingLength));
                bitstreamLength += paddingLength;
            }

            // fill the bitstream with pad codewords
            byte[] padCodewords = new byte[] { 0x37, 0x88 };
            int padIndex = 0;
            while (bitstreamLength < (capacity - 4))
            {
                bits.Add(new BitArray(new byte[] { padCodewords[padIndex] }));
                bitstreamLength += 8;
                padIndex = (padIndex + 1) % 2;
            }

            // fill the last nibble with zeroes (only necessary for M1 and M3)
            if (bitstreamLength < capacity)
            {
                bits.Add(new BitArray(4));
                bitstreamLength += 4;
            }

            // flatten list of bitarrays into a single bool[]
            bool[] flattenedBits = new bool[bitstreamLength];
            int bitIndex = 0;
            foreach (var b in bits)
            {
                b.CopyTo(flattenedBits, bitIndex);
                bitIndex += b.Length;
            }

            return new BitArray(flattenedBits).ToByteArray();
        }

        /// <summary>
        /// Generate error correction words and interleave with code words.
        /// </summary>
        /// <param name="codeWords"></param>
        /// <returns></returns>
        private BitArray AddErrorCorrection(byte[] codeWords)
        {
            List<byte[]> dataBlocks = new List<byte[]>();
            List<byte[]> eccBlocks = new List<byte[]>();

            // generate error correction words
            var ecc = ErrorCorrectionTable.First(f => f.Item1 == Type && f.Item2 == Version && f.Item3 == QRErrorCorrection).Item4;
            int dataIndex = 0;

            // for each collection of blocks that are needed
            foreach (var e in ecc)
            {
                // lookup number of data words and error words in this block
                int dataWords = e.Item3;
                int errorWords = e.Item2 - e.Item3;

                // retrieve the appropriate polynomial for the desired error word count
                var poly = Polynomials[errorWords].ToArray();

                // for each needed block
                for (int b = 0; b < e.Item1; b++)
                {
                    // add the block's data to the final list
                    dataBlocks.Add(codeWords.Skip(dataIndex).Take(dataWords).ToArray());
                    dataIndex += dataWords;

                    // pad the block with zeroes
                    var temp = Enumerable.Concat(dataBlocks.Last(), Enumerable.Repeat((byte)0, errorWords)).ToArray();

                    // perform polynomial division to calculate error block
                    for (int start = 0; start < dataWords; start++)
                    {
                        byte pow = LogTable[temp[start]];
                        for (int i = 0; i < poly.Length; i++)
                            temp[i + start] ^= ExponentTable[Mul(poly[i], pow)];
                    }

                    // add error block to the final list
                    eccBlocks.Add(temp.Skip(dataWords).ToArray());
                }
            }
            #endregion

            // generate final data sequence
            byte[] sequence = new byte[dataBlocks.Sum(b => b.Length) + eccBlocks.Sum(b => b.Length)];
            int finalIndex = 0;

            // interleave the data blocks
            for (int i = 0; i < dataBlocks.Max(b => b.Length); i++)
                foreach (var b in dataBlocks.Where(b => b.Length > i))
                    sequence[finalIndex++] = b[i];

            // interleave the error blocks
            for (int i = 0; i < eccBlocks.Max(b => b.Length); i++)
                foreach (var b in eccBlocks.Where(b => b.Length > i))
                    sequence[finalIndex++] = b[i];

            return sequence.ToBitArray();
        }

        /// <summary>
        /// Perform the following steps
        /// - Draw finder patterns
        /// - Draw alignment patterns
        /// - Draw timing lines
        /// - Reserve space for version and format information
        /// - Mark remaining space as "free" for data
        /// </summary>
        private void Reserve()
        {
            dim = GetSymbolDimension();

            // initialize to a full symbol of unaccessed, light modules
            freeMask = new bool[dim, dim];
            accessCount = new int[dim, dim];
            modules = new QRModuleType[dim, dim];
            for (int x = 0; x < dim; x++)
            {
                for (int y = 0; y < dim; y++)
                {
                    modules[x, y] = QRModuleType.Light;
                    accessCount[x, y] = 0;
                    freeMask[x, y] = true;
                }
            }

            // draw alignment patterns
            foreach (var location in GetAlignmentPatternLocations())
            {
                // check for overlap with top-left finder pattern
                if (location.Item1 < 10 && location.Item2 < 10)
                    continue;

                // check for overlap with bottom-left finder pattern
                if (location.Item1 < 10 && location.Item2 > (dim - 10))
                    continue;

                // check for overlap with top-right finder pattern
                if (location.Item1 > (dim - 10) && location.Item2 < 10)
                    continue;

                DrawAlignmentPattern(location.Item1, location.Item2);
            }

            // draw top-left finder pattern
            DrawFinderPattern(3, 3);
            // and border
            DrawHLine(0, 7, 8, QRModuleType.Light);
            DrawVLine(7, 0, 7, QRModuleType.Light);

            switch (Type)
            {
                case QRSymbolType.Micro:
                    // draw top-left finder pattern's format area
                    DrawHLine(1, 8, 8, QRModuleType.Light);
                    DrawVLine(8, 1, 7, QRModuleType.Light);

                    // draw timing lines
                    DrawTimingHLine(8, 0, dim - 8);
                    DrawTimingVLine(0, 8, dim - 8);
                    break;

                case QRSymbolType.Normal:
                    // draw top-left finder pattern's format area
                    DrawHLine(0, 8, 9, QRModuleType.Light);
                    DrawVLine(8, 0, 8, QRModuleType.Light);

                    // draw top-right finder pattern
                    DrawFinderPattern(dim - 4, 3);
                    // and border
                    DrawHLine(dim - 8, 7, 8, QRModuleType.Light);
                    DrawVLine(dim - 8, 0, 7, QRModuleType.Light);
                    // and format area
                    DrawHLine(dim - 8, 8, 8, QRModuleType.Light);

                    // draw bottom-left finder pattern
                    DrawFinderPattern(3, dim - 4);
                    // and border
                    DrawHLine(0, dim - 8, 8, QRModuleType.Light);
                    DrawVLine(7, dim - 7, 7, QRModuleType.Light);
                    // and format area
                    DrawVLine(8, dim - 7, 7, QRModuleType.Light);
                    // and dark module
                    Set(8, dim - 8, QRModuleType.Dark);

                    // draw timing lines
                    DrawTimingHLine(8, 6, dim - 8 - 8);
                    DrawTimingVLine(6, 8, dim - 8 - 8);

                    if (Version >= 7)
                    {
                        // reserve version information areas
                        FillRect(0, dim - 11, 6, 3, QRModuleType.Light);
                        FillRect(dim - 11, 0, 3, 6, QRModuleType.Light);
                    }
                    break;
            }

            // mark non-accessed cells as free, accessed cells as reserved
            CreateFreeMask();
        }

        /// <summary>
        /// Populate the "free" modules with data.
        /// </summary>
        /// <param name="bits"></param>
        private void Fill(BitArray bits)
        {
            // start with bit 0, moving up
            int idx = 0;
            bool up = true;

            int minX = Type == QRSymbolType.Normal ? 0 : 1;
            int minY = Type == QRSymbolType.Normal ? 0 : 1;

            int timingX = Type == QRSymbolType.Normal ? 6 : 0;
            int timingY = Type == QRSymbolType.Normal ? 6 : 0;

            // from right-to-left
            for (int x = dim - 1; x >= minX; x -= 2)
            {
                // skip over the vertical timing line
                if (x == timingX)
                    x--;

                // in the indicated direction
                for (int y = (up ? dim - 1 : minY); y >= minY && y < dim; y += (up ? -1 : 1))
                {
                    // for each horizontal pair of modules
                    for (int dx = 0; dx > -2; dx--)
                    {
                        // if the module is free (not reserved)
                        if (IsFree(x + dx, y))
                        {
                            // if data remains to be written
                            if (idx < bits.Length)
                            {
                                // write the next bit
                                Set(x + dx, y, bits[idx] ? QRModuleType.Dark : QRModuleType.Light);
                            }
                            else
                            {
                                // pad with light cells
                                Set(x + dx, y, QRModuleType.Light);
                            }

                            // advance to the next bit
                            idx++;
                        }
                    }
                }

                // reverse directions
                up = !up;
            }
        }

        /// <summary>
        /// Identify and apply the best mask
        /// </summary>
        /// <returns></returns>
        private byte Mask()
        {
            List<Tuple<byte, byte, Func<int, int, bool>>> masks = null;

            // determine which mask types are applicable
            switch (Type)
            {
                case QRSymbolType.Micro:
                    masks = DataMaskTable.Where(m => m.Item2 != 255).ToList();
                    break;

                case QRSymbolType.Normal:
                    masks = DataMaskTable.ToList();
                    break;
            }

            // evaluate all the maks
            var results = masks.Select(m => Tuple.Create(m, EvaluateMask(m.Item3)));

            // choose a winner
            Tuple<byte, byte, Func<int, int, bool>> winner;
            if (Type == QRSymbolType.Normal)
                winner = results.OrderBy(t => t.Item2).First().Item1; // lowest penalty wins
            else
                winner = results.OrderBy(t => t.Item2).Last().Item1; // highest score wins

            // apply the winner
            Apply(winner.Item3);

            // return the winner's ID
            return Type == QRSymbolType.Normal ? winner.Item1 : winner.Item2;
        }

        /// <summary>
        /// Write the format information (version and mask id)
        /// </summary>
        /// <param name="maskID"></param>
        private void AddFormatInformation(byte maskID)
        {
            if (Type == QRSymbolType.Normal)
            {
                var bits = NormalFormatStrings.First(f => f.Item1 == QRErrorCorrection && f.Item2 == maskID).Item3;

                // add format information around top-left finder pattern
                Set(8, 0, bits[14] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 1, bits[13] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 2, bits[12] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 3, bits[11] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 4, bits[10] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 5, bits[9] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 7, bits[8] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 8, bits[7] ? QRModuleType.Dark : QRModuleType.Light);
                Set(7, 8, bits[6] ? QRModuleType.Dark : QRModuleType.Light);
                Set(5, 8, bits[5] ? QRModuleType.Dark : QRModuleType.Light);
                Set(4, 8, bits[4] ? QRModuleType.Dark : QRModuleType.Light);
                Set(3, 8, bits[3] ? QRModuleType.Dark : QRModuleType.Light);
                Set(2, 8, bits[2] ? QRModuleType.Dark : QRModuleType.Light);
                Set(1, 8, bits[1] ? QRModuleType.Dark : QRModuleType.Light);
                Set(0, 8, bits[0] ? QRModuleType.Dark : QRModuleType.Light);

                // add format information around top-right finder pattern
                Set(dim - 1, 8, bits[14] ? QRModuleType.Dark : QRModuleType.Light);
                Set(dim - 2, 8, bits[13] ? QRModuleType.Dark : QRModuleType.Light);
                Set(dim - 3, 8, bits[12] ? QRModuleType.Dark : QRModuleType.Light);
                Set(dim - 4, 8, bits[11] ? QRModuleType.Dark : QRModuleType.Light);
                Set(dim - 5, 8, bits[10] ? QRModuleType.Dark : QRModuleType.Light);
                Set(dim - 6, 8, bits[9] ? QRModuleType.Dark : QRModuleType.Light);
                Set(dim - 7, 8, bits[8] ? QRModuleType.Dark : QRModuleType.Light);
                Set(dim - 8, 8, bits[7] ? QRModuleType.Dark : QRModuleType.Light);

                // add format information around bottom-left finder pattern
                Set(8, dim - 7, bits[6] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, dim - 6, bits[5] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, dim - 5, bits[4] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, dim - 4, bits[3] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, dim - 3, bits[2] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, dim - 2, bits[1] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, dim - 1, bits[0] ? QRModuleType.Dark : QRModuleType.Light);
            }
            else
            {
                var bits = MicroFormatStrings.First(f => f.Item1 == Version && f.Item2 == QRErrorCorrection && f.Item3 == maskID).Item4;

                // add format information around top-left finder pattern
                Set(8, 1, bits[14] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 2, bits[13] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 3, bits[12] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 4, bits[11] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 5, bits[10] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 6, bits[9] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 7, bits[8] ? QRModuleType.Dark : QRModuleType.Light);
                Set(8, 8, bits[7] ? QRModuleType.Dark : QRModuleType.Light);
                Set(7, 8, bits[6] ? QRModuleType.Dark : QRModuleType.Light);
                Set(6, 8, bits[5] ? QRModuleType.Dark : QRModuleType.Light);
                Set(5, 8, bits[4] ? QRModuleType.Dark : QRModuleType.Light);
                Set(4, 8, bits[3] ? QRModuleType.Dark : QRModuleType.Light);
                Set(3, 8, bits[2] ? QRModuleType.Dark : QRModuleType.Light);
                Set(2, 8, bits[1] ? QRModuleType.Dark : QRModuleType.Light);
                Set(1, 8, bits[0] ? QRModuleType.Dark : QRModuleType.Light);
            }
        }

        /// <summary>
        /// Write the version information
        /// </summary>
        private void AddVersionInformation()
        {
            if (Type == QRSymbolType.Micro || Version < 7)
                return;

            var bits = VersionStrings[Version];

            // write top-right block
            var idx = 17;
            for (int y = 0; y < 6; y++)
                for (int x = dim - 11; x < dim - 8; x++)
                    Set(x, y, bits[idx--] ? QRModuleType.Dark : QRModuleType.Light);

            // write bottom-left block
            idx = 17;
            for (int x = 0; x < 6; x++)
                for (int y = dim - 11; y < dim - 8; y++)
                    Set(x, y, bits[idx--] ? QRModuleType.Dark : QRModuleType.Light);
        }
        #endregion

        #region Drawing Helpers
        private void DrawFinderPattern(int centerX, int centerY)
        {
            DrawRect(centerX - 3, centerY - 3, 7, 7, QRModuleType.Dark);
            DrawRect(centerX - 2, centerY - 2, 5, 5, QRModuleType.Light);
            FillRect(centerX - 1, centerY - 1, 3, 3, QRModuleType.Dark);
        }

        private void DrawAlignmentPattern(int centerX, int centerY)
        {
            DrawRect(centerX - 2, centerY - 2, 5, 5, QRModuleType.Dark);
            DrawRect(centerX - 1, centerY - 1, 3, 3, QRModuleType.Light);
            Set(centerX, centerY, QRModuleType.Dark);
        }

        private void FillRect(int left, int top, int width, int height, QRModuleType type)
        {
            for (int dx = 0; dx < width; dx++)
                for (int dy = 0; dy < height; dy++)
                    Set(left + dx, top + dy, type);
        }

        private void DrawRect(int left, int top, int width, int height, QRModuleType type)
        {
            DrawHLine(left, top, width, type);
            DrawHLine(left, top + height - 1, width, type);
            DrawVLine(left, top + 1, height - 2, type);
            DrawVLine(left + width - 1, top + 1, height - 2, type);
        }

        private void DrawHLine(int x, int y, int length, QRModuleType type)
        {
            for (int dx = 0; dx < length; dx++)
                Set(x + dx, y, type);
        }

        private void DrawVLine(int x, int y, int length, QRModuleType type)
        {
            for (int dy = 0; dy < length; dy++)
                Set(x, y + dy, type);
        }

        private void DrawTimingHLine(int x, int y, int length)
        {
            for (int dx = 0; dx < length; dx++)
                Set(x + dx, y, ((x + dx) % 2 == 0) ? QRModuleType.Dark : QRModuleType.Light);
        }

        private void DrawTimingVLine(int x, int y, int length)
        {
            for (int dy = 0; dy < length; dy++)
                Set(x, y + dy, ((y + dy) % 2 == 0) ? QRModuleType.Dark : QRModuleType.Light);
        }

        private void Set(int x, int y, QRModuleType type)
        {
            modules[x, y] = type;
            accessCount[x, y]++;
        }

        private QRModuleType Get(int x, int y)
        {
            return modules[x, y];
        }

        private void CreateFreeMask()
        {
            for (int x = 0; x < dim; x++)
                for (int y = 0; y < dim; y++)
                    freeMask[x, y] = accessCount[x, y] == 0;
        }

        private bool IsFree(int x, int y)
        {
            return freeMask[x, y];
        }
        #endregion

        #region Masking Helpers
        private int EvaluateMask(Func<int, int, bool> mask)
        {
            // apply the mask
            Apply(mask);

            try
            {
                if (Type == QRSymbolType.Normal)
                    return EvaluateNormalMask();
                else
                    return EvaluateMicroMask();
            }
            finally
            {
                // undo the mask
                Apply(mask);
            }
        }

        private void Apply(Func<int, int, bool> mask)
        {
            for (int x = 0; x < dim; x++)
            {
                for (int y = 0; y < dim; y++)
                {
                    if (IsFree(x, y) && mask(y, x))
                    {
                        Set(x, y, Get(x, y) == QRModuleType.Dark ? QRModuleType.Light : QRModuleType.Dark);
                    }
                }
            }
        }

        private int EvaluateMicroMask()
        {
            int darkCount1 = Enumerable.Range(1, dim - 2).Count(x => Get(x, dim - 1) == QRModuleType.Dark);
            int darkCount2 = Enumerable.Range(1, dim - 2).Count(y => Get(dim - 1, y) == QRModuleType.Dark);

            return Math.Min(darkCount1, darkCount2) * 16 + Math.Max(darkCount1, darkCount2);
        }

        private int EvaluateNormalMask()
        {
            int penalty = 0;

            // horizontal adjacency penalties
            for (int y = 0; y < dim; y++)
            {
                QRModuleType last = Get(0, y);
                int count = 1;

                for (int x = 1; x < dim; x++)
                {
                    var m = Get(x, y);
                    if (m == last)
                    {
                        count++;
                    }
                    else
                    {
                        if (count >= 5)
                            penalty += count - 2;

                        last = m;
                        count = 1;
                    }
                }

                if (count >= 5)
                    penalty += count - 2;
            }

            // vertical adjacency penalties
            for (int x = 0; x < dim; x++)
            {
                QRModuleType last = Get(x, 0);
                int count = 1;

                for (int y = 1; y < dim; y++)
                {
                    var m = Get(x, y);
                    if (m == last)
                    {
                        count++;
                    }
                    else
                    {
                        if (count >= 5)
                            penalty += count - 2;

                        last = m;
                        count = 1;
                    }
                }

                if (count >= 5)
                    penalty += count - 2;
            }

            // block penalties
            for (int x = 0; x < dim - 1; x++)
            {
                for (int y = 0; y < dim - 1; y++)
                {
                    var m = Get(x, y);

                    if (m == Get(x + 1, y) && m == Get(x, y + 1) && m == Get(x + 1, y + 1))
                        penalty += 3;
                }
            }

            // horizontal finder pattern penalties
            for (int y = 0; y < dim; y++)
            {
                for (int x = 0; x < dim - 11; x++)
                {
                    if (Get(x + 0, y) == QRModuleType.Dark &&
                        Get(x + 1, y) == QRModuleType.Light &&
                        Get(x + 2, y) == QRModuleType.Dark &&
                        Get(x + 3, y) == QRModuleType.Dark &&
                        Get(x + 4, y) == QRModuleType.Dark &&
                        Get(x + 5, y) == QRModuleType.Light &&
                        Get(x + 6, y) == QRModuleType.Dark &&
                        Get(x + 7, y) == QRModuleType.Light &&
                        Get(x + 8, y) == QRModuleType.Light &&
                        Get(x + 9, y) == QRModuleType.Light &&
                        Get(x + 10, y) == QRModuleType.Light)
                        penalty += 40;

                    if (Get(x + 0, y) == QRModuleType.Light &&
                        Get(x + 1, y) == QRModuleType.Light &&
                        Get(x + 2, y) == QRModuleType.Light &&
                        Get(x + 3, y) == QRModuleType.Light &&
                        Get(x + 4, y) == QRModuleType.Dark &&
                        Get(x + 5, y) == QRModuleType.Light &&
                        Get(x + 6, y) == QRModuleType.Dark &&
                        Get(x + 7, y) == QRModuleType.Dark &&
                        Get(x + 8, y) == QRModuleType.Dark &&
                        Get(x + 9, y) == QRModuleType.Light &&
                        Get(x + 10, y) == QRModuleType.Dark)
                        penalty += 40;
                }
            }

            // vertical finder pattern penalties
            for (int x = 0; x < dim; x++)
            {
                for (int y = 0; y < dim - 11; y++)
                {
                    if (Get(x, y + 0) == QRModuleType.Dark &&
                        Get(x, y + 1) == QRModuleType.Light &&
                        Get(x, y + 2) == QRModuleType.Dark &&
                        Get(x, y + 3) == QRModuleType.Dark &&
                        Get(x, y + 4) == QRModuleType.Dark &&
                        Get(x, y + 5) == QRModuleType.Light &&
                        Get(x, y + 6) == QRModuleType.Dark &&
                        Get(x, y + 7) == QRModuleType.Light &&
                        Get(x, y + 8) == QRModuleType.Light &&
                        Get(x, y + 9) == QRModuleType.Light &&
                        Get(x, y + 10) == QRModuleType.Light)
                        penalty += 40;

                    if (Get(x, y + 0) == QRModuleType.Light &&
                        Get(x, y + 1) == QRModuleType.Light &&
                        Get(x, y + 2) == QRModuleType.Light &&
                        Get(x, y + 3) == QRModuleType.Light &&
                        Get(x, y + 4) == QRModuleType.Dark &&
                        Get(x, y + 5) == QRModuleType.Light &&
                        Get(x, y + 6) == QRModuleType.Dark &&
                        Get(x, y + 7) == QRModuleType.Dark &&
                        Get(x, y + 8) == QRModuleType.Dark &&
                        Get(x, y + 9) == QRModuleType.Light &&
                        Get(x, y + 10) == QRModuleType.Dark)
                        penalty += 40;
                }
            }

            // ratio penalties
            int total = dim * dim;
            int darkCount = 0;

            for (int x = 0; x < dim; x++)
            {
                for (int y = 0; y < dim; y++)
                {
                    if (Get(x, y) == QRModuleType.Dark)
                        darkCount++;
                }
            }

            int percentDark = darkCount * 100 / total;
            int up = (percentDark % 5 == 0) ? percentDark : percentDark + (5 - (percentDark % 5));
            int down = (percentDark % 5 == 0) ? percentDark : percentDark - (percentDark % 5);
            up = Math.Abs(up - 50);
            down = Math.Abs(down - 50);
            up /= 5;
            down /= 5;
            penalty += Math.Min(up, down) * 10;

            return penalty;
        }
        #endregion

        #region Calculation Helpers
        private int GetSymbolDimension()
        {
            switch (Type)
            {
                case QRSymbolType.Micro:
                    return 9 + (2 * Version);
                case QRSymbolType.Normal:
                    return 17 + (4 * Version);
            }

            throw new InvalidOperationException();
        }

        private IEnumerable<Tuple<int, int>> GetAlignmentPatternLocations()
        {
            switch (Type)
            {
                case QRSymbolType.Micro:
                    break;

                case QRSymbolType.Normal:
                    var locations = this.AlignmentPatternLocations[Version];
                    for (int i = 0; i < locations.Length; i++)
                    {
                        for (int j = i; j < locations.Length; j++)
                        {
                            yield return Tuple.Create(locations[i], locations[j]);
                            if (i != j)
                                yield return Tuple.Create(locations[j], locations[i]);
                        }
                    }
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private IEnumerable<QRMode> GetAvailableModes(QRSymbolType type, int version)
        {
            switch (type)
            {
                case QRSymbolType.Normal:
                    return this.NormalModes;

                case QRSymbolType.Micro:
                    return this.MicroModes[version];

                default:
                    throw new InvalidOperationException();
            }
        }

        private IEnumerable<QRErrorCorrection> GetAvailableErrorCorrectionLevels(QRSymbolType type, int version)
        {
            switch (type)
            {
                case QRSymbolType.Normal:
                    return this.NormalErrorCorrectionLevels;

                case QRSymbolType.Micro:
                    return this.MicroErrorCorrectionLevels[version];

                default:
                    throw new InvalidOperationException();
            }
        }

        public BitArray EncodeMode(QRMode mode)
        {
            switch (Type)
            {
                case QRSymbolType.Normal:
                    return this.NormalModeEncodings[(int)mode];

                case QRSymbolType.Micro:
                    return this.MicroModeEncodings.First(t => t.Item1 == Version && t.Item2 == mode).Item3;
            }

            throw new InvalidOperationException();
        }

        private BitArray EncodeCharacterCount(QRMode mode, int count)
        {
            int bits = GetCharacterCountBits(mode);

            int min = 1;
            int max = GetMaxCharacters(mode);

            if (count < min || count > max)
                throw new ArgumentOutOfRangeException("count", String.Format("QR {0} character counts must be in the range {1} <= n <= {2}", Description, min, max));

            return count.ToBitArray(bits);
        }

        private int GetCharacterCountBits(QRMode mode)
        {
            return this.CharacterWidthTable.First(f => f.Item1 == Type && f.Item2 == Version && f.Item3 == mode).Item4;
        }

        private int GetMaxCharacters(QRMode mode)
        {
            return (1 << GetCharacterCountBits(mode)) - 1;
        }

        private static byte Mul(byte a1, byte a2)
        {
            return (byte)((a1 + a2) % 255);
        }

        private static byte Add(byte a1, byte a2)
        {
            return LogTable[ExponentTable[a1] ^ ExponentTable[a2]];
        }
        #endregion

        #region Data Tables

        private static readonly byte[] ErrorCorrectionEncodings = new byte[] { 0, 1, 0, 3, 2 };

        private static readonly Tuple<byte, byte, Func<int, int, bool>>[] DataMaskTable =
            new Tuple<byte, byte, Func<int, int, bool>>[]
            {
                Tuple.Create<byte, byte, Func<int, int, bool>>(0, 255, (i, j) => (i + j) % 2 == 0),
                Tuple.Create<byte, byte, Func<int, int, bool>>(1,   0, (i, j) => i % 2 == 0),
                Tuple.Create<byte, byte, Func<int, int, bool>>(2, 255, (i, j) => j % 3 == 0),
                Tuple.Create<byte, byte, Func<int, int, bool>>(3, 255, (i, j) => (i + j) % 3 == 0),
                Tuple.Create<byte, byte, Func<int, int, bool>>(4,   1, (i, j) => ((i / 2) + (j / 3)) % 2 == 0),
                Tuple.Create<byte, byte, Func<int, int, bool>>(5, 255, (i, j) => ((i * j) % 2) + ((i * j) % 3) == 0),
                Tuple.Create<byte, byte, Func<int, int, bool>>(6,   2, (i, j) => (((i * j) % 2) + ((i * j) % 3)) % 2 == 0),
                Tuple.Create<byte, byte, Func<int, int, bool>>(7,   3, (i, j) => (((i + j) % 2) + ((i * j) % 3)) % 2 == 0),
            };

        private static readonly Tuple<QRErrorCorrection, byte, BitArray>[] NormalFormatStrings =
            new Tuple<QRErrorCorrection, byte, BitArray>[]
            {
                Tuple.Create(QRErrorCorrection.L, (byte)0, new BitArray(new bool[] {  true,  true,  true, false,  true,  true,  true,  true,  true, false, false, false,  true, false, false })),
                Tuple.Create(QRErrorCorrection.L, (byte)1, new BitArray(new bool[] {  true,  true,  true, false, false,  true, false,  true,  true,  true,  true, false, false,  true,  true })),
                Tuple.Create(QRErrorCorrection.L, (byte)2, new BitArray(new bool[] {  true,  true,  true,  true,  true, false,  true,  true, false,  true, false,  true, false,  true, false })),
                Tuple.Create(QRErrorCorrection.L, (byte)3, new BitArray(new bool[] {  true,  true,  true,  true, false, false, false,  true, false, false,  true,  true,  true, false,  true })),
                Tuple.Create(QRErrorCorrection.L, (byte)4, new BitArray(new bool[] {  true,  true, false, false,  true,  true, false, false, false,  true, false,  true,  true,  true,  true })),
                Tuple.Create(QRErrorCorrection.L, (byte)5, new BitArray(new bool[] {  true,  true, false, false, false,  true,  true, false, false, false,  true,  true, false, false, false })),
                Tuple.Create(QRErrorCorrection.L, (byte)6, new BitArray(new bool[] {  true,  true, false,  true,  true, false, false, false,  true, false, false, false, false, false,  true })),
                Tuple.Create(QRErrorCorrection.L, (byte)7, new BitArray(new bool[] {  true,  true, false,  true, false, false,  true, false,  true,  true,  true, false,  true,  true, false })),
                Tuple.Create(QRErrorCorrection.M, (byte)0, new BitArray(new bool[] {  true, false,  true, false,  true, false, false, false, false, false,  true, false, false,  true, false })),
                Tuple.Create(QRErrorCorrection.M, (byte)1, new BitArray(new bool[] {  true, false,  true, false, false, false,  true, false, false,  true, false, false,  true, false,  true })),
                Tuple.Create(QRErrorCorrection.M, (byte)2, new BitArray(new bool[] {  true, false,  true,  true,  true,  true, false, false,  true,  true,  true,  true,  true, false, false })),
                Tuple.Create(QRErrorCorrection.M, (byte)3, new BitArray(new bool[] {  true, false,  true,  true, false,  true,  true, false,  true, false, false,  true, false,  true,  true })),
                Tuple.Create(QRErrorCorrection.M, (byte)4, new BitArray(new bool[] {  true, false, false, false,  true, false,  true,  true,  true,  true,  true,  true, false, false,  true })),
                Tuple.Create(QRErrorCorrection.M, (byte)5, new BitArray(new bool[] {  true, false, false, false, false, false, false,  true,  true, false, false,  true,  true,  true, false })),
                Tuple.Create(QRErrorCorrection.M, (byte)6, new BitArray(new bool[] {  true, false, false,  true,  true,  true,  true,  true, false, false,  true, false,  true,  true,  true })),
                Tuple.Create(QRErrorCorrection.M, (byte)7, new BitArray(new bool[] {  true, false, false,  true, false,  true, false,  true, false,  true, false, false, false, false, false })),
                Tuple.Create(QRErrorCorrection.Q, (byte)0, new BitArray(new bool[] { false,  true,  true, false,  true, false,  true, false,  true, false,  true,  true,  true,  true,  true })),
                Tuple.Create(QRErrorCorrection.Q, (byte)1, new BitArray(new bool[] { false,  true,  true, false, false, false, false, false,  true,  true, false,  true, false, false, false })),
                Tuple.Create(QRErrorCorrection.Q, (byte)2, new BitArray(new bool[] { false,  true,  true,  true,  true,  true,  true, false, false,  true,  true, false, false, false,  true })),
                Tuple.Create(QRErrorCorrection.Q, (byte)3, new BitArray(new bool[] { false,  true,  true,  true, false,  true, false, false, false, false, false, false,  true,  true, false })),
                Tuple.Create(QRErrorCorrection.Q, (byte)4, new BitArray(new bool[] { false,  true, false, false,  true, false, false,  true, false,  true,  true, false,  true, false, false })),
                Tuple.Create(QRErrorCorrection.Q, (byte)5, new BitArray(new bool[] { false,  true, false, false, false, false,  true,  true, false, false, false, false, false,  true,  true })),
                Tuple.Create(QRErrorCorrection.Q, (byte)6, new BitArray(new bool[] { false,  true, false,  true,  true,  true, false,  true,  true, false,  true,  true, false,  true, false })),
                Tuple.Create(QRErrorCorrection.Q, (byte)7, new BitArray(new bool[] { false,  true, false,  true, false,  true,  true,  true,  true,  true, false,  true,  true, false,  true })),
                Tuple.Create(QRErrorCorrection.H, (byte)0, new BitArray(new bool[] { false, false,  true, false,  true,  true, false,  true, false, false, false,  true, false, false,  true })),
                Tuple.Create(QRErrorCorrection.H, (byte)1, new BitArray(new bool[] { false, false,  true, false, false,  true,  true,  true, false,  true,  true,  true,  true,  true, false })),
                Tuple.Create(QRErrorCorrection.H, (byte)2, new BitArray(new bool[] { false, false,  true,  true,  true, false, false,  true,  true,  true, false, false,  true,  true,  true })),
                Tuple.Create(QRErrorCorrection.H, (byte)3, new BitArray(new bool[] { false, false,  true,  true, false, false,  true,  true,  true, false,  true, false, false, false, false })),
                Tuple.Create(QRErrorCorrection.H, (byte)4, new BitArray(new bool[] { false, false, false, false,  true,  true,  true, false,  true,  true, false, false, false,  true, false })),
                Tuple.Create(QRErrorCorrection.H, (byte)5, new BitArray(new bool[] { false, false, false, false, false,  true, false, false,  true, false,  true, false,  true, false,  true })),
                Tuple.Create(QRErrorCorrection.H, (byte)6, new BitArray(new bool[] { false, false, false,  true,  true, false,  true, false, false, false, false,  true,  true, false, false })),
                Tuple.Create(QRErrorCorrection.H, (byte)7, new BitArray(new bool[] { false, false, false,  true, false, false, false, false, false,  true,  true,  true, false,  true,  true }))
            };

        private static readonly Tuple<int, QRErrorCorrection, byte, BitArray>[] MicroFormatStrings =
            new Tuple<int, QRErrorCorrection, byte, BitArray>[]
            {
                Tuple.Create(2, QRErrorCorrection.M,    (byte)0, new BitArray(new bool[] { true,  true, false, false,  true,  true,  true,  true, false, false,  true, false, false,  true,  true })),
                Tuple.Create(2, QRErrorCorrection.M,    (byte)1, new BitArray(new bool[] { true,  true, false, false, false,  true, false,  true, false,  true, false, false,  true, false, false })),
                Tuple.Create(2, QRErrorCorrection.M,    (byte)2, new BitArray(new bool[] { true,  true, false,  true,  true, false,  true,  true,  true,  true,  true,  true,  true, false,  true })),
                Tuple.Create(2, QRErrorCorrection.M,    (byte)3, new BitArray(new bool[] { true,  true, false,  true, false, false, false,  true,  true, false, false,  true, false,  true, false })),
                Tuple.Create(2, QRErrorCorrection.L,    (byte)0, new BitArray(new bool[] { true, false,  true, false,  true, false,  true,  true, false,  true, false,  true,  true,  true, false })),
                Tuple.Create(2, QRErrorCorrection.L,    (byte)1, new BitArray(new bool[] { true, false,  true, false, false, false, false,  true, false, false,  true,  true, false, false,  true })),
                Tuple.Create(2, QRErrorCorrection.L,    (byte)2, new BitArray(new bool[] { true, false,  true,  true,  true,  true,  true,  true,  true, false, false, false, false, false, false })),
                Tuple.Create(2, QRErrorCorrection.L,    (byte)3, new BitArray(new bool[] { true, false,  true,  true, false,  true, false,  true,  true,  true,  true, false,  true,  true,  true })),
                Tuple.Create(3, QRErrorCorrection.L,    (byte)0, new BitArray(new bool[] { true,  true,  true, false,  true,  true, false, false,  true,  true,  true,  true, false, false, false })),
                Tuple.Create(3, QRErrorCorrection.L,    (byte)1, new BitArray(new bool[] { true,  true,  true, false, false,  true,  true, false,  true, false, false,  true,  true,  true,  true })),
                Tuple.Create(3, QRErrorCorrection.L,    (byte)2, new BitArray(new bool[] { true,  true,  true,  true,  true, false, false, false, false, false,  true, false,  true,  true, false })),
                Tuple.Create(3, QRErrorCorrection.L,    (byte)3, new BitArray(new bool[] { true,  true,  true,  true, false, false,  true, false, false,  true, false, false, false, false,  true })),
                Tuple.Create(3, QRErrorCorrection.M,    (byte)0, new BitArray(new bool[] {false, false, false, false,  true,  true, false,  true,  true, false,  true,  true,  true,  true, false })),
                Tuple.Create(3, QRErrorCorrection.M,    (byte)1, new BitArray(new bool[] {false, false, false, false, false,  true,  true,  true,  true,  true, false,  true, false, false,  true })),
                Tuple.Create(3, QRErrorCorrection.M,    (byte)2, new BitArray(new bool[] {false, false, false,  true,  true, false, false,  true, false,  true,  true, false, false, false, false })),
                Tuple.Create(3, QRErrorCorrection.M,    (byte)3, new BitArray(new bool[] {false, false, false,  true, false, false,  true,  true, false, false, false, false,  true,  true,  true })),
                Tuple.Create(1, QRErrorCorrection.None, (byte)0, new BitArray(new bool[] { true, false, false, false,  true, false, false, false,  true, false, false, false,true, false,  true })),
                Tuple.Create(1, QRErrorCorrection.None, (byte)1, new BitArray(new bool[] { true, false, false, false, false, false,  true, false,  true,  true,  true, false, false,  true, false })),
                Tuple.Create(1, QRErrorCorrection.None, (byte)2, new BitArray(new bool[] { true, false, false,  true,  true,  true, false, false, false,  true, false,  true, false,  true,  true })),
                Tuple.Create(1, QRErrorCorrection.None, (byte)3, new BitArray(new bool[] { true, false, false,  true, false,  true,  true, false, false, false,  true,  true,true, false, false })),
                Tuple.Create(4, QRErrorCorrection.M,    (byte)0, new BitArray(new bool[] {false,  true, false, false,  true, false,  true, false, false, false, false,  true, false, false, false })),
                Tuple.Create(4, QRErrorCorrection.M,    (byte)1, new BitArray(new bool[] {false,  true, false, false, false, false, false, false, false,  true,  true,  true,  true,  true,  true })),
                Tuple.Create(4, QRErrorCorrection.M,    (byte)2, new BitArray(new bool[] {false,  true, false,  true,  true,  true,  true, false,  true,  true, false, false,  true,  true, false })),
                Tuple.Create(4, QRErrorCorrection.M,    (byte)3, new BitArray(new bool[] {false,  true, false,  true, false,  true, false, false,  true, false,  true, false, false, false,  true })),
                Tuple.Create(4, QRErrorCorrection.Q,    (byte)0, new BitArray(new bool[] {false,  true,  true, false,  true, false, false,  true,  true,  true, false, false, false,  true,  true })),
                Tuple.Create(4, QRErrorCorrection.Q,    (byte)1, new BitArray(new bool[] {false,  true,  true, false, false, false,  true,  true,  true, false,  true, false,  true, false, false })),
                Tuple.Create(4, QRErrorCorrection.Q,    (byte)2, new BitArray(new bool[] {false,  true,  true,  true,  true,  true, false,  true, false, false, false,  true,  true, false,  true })),
                Tuple.Create(4, QRErrorCorrection.Q,    (byte)3, new BitArray(new bool[] {false,  true,  true,  true, false,  true,  true,  true, false,  true,  true,  true, false,  true, false })),
                Tuple.Create(4, QRErrorCorrection.L,    (byte)0, new BitArray(new bool[] {false, false,  true, false,  true,  true,  true, false, false,  true,  true, false,  true, false,  true })),
                Tuple.Create(4, QRErrorCorrection.L,    (byte)1, new BitArray(new bool[] {false, false,  true, false, false,  true, false, false, false, false, false, false, false,  true, false })),
                Tuple.Create(4, QRErrorCorrection.L,    (byte)2, new BitArray(new bool[] {false, false,  true,  true,  true, false,  true, false,  true, false,  true,  true, false,  true,  true })),
                Tuple.Create(4, QRErrorCorrection.L,    (byte)3, new BitArray(new bool[] {false, false,  true,  true, false, false, false, false,  true,  true, false,  true,  true, false, false })),
            };

        private static readonly BitArray[] VersionStrings =
            new BitArray[]
            {
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                new BitArray(new bool[] { false, false, false,  true,  true,  true,  true,  true, false, false,  true, false, false,  true, false,  true, false, false }),
                new BitArray(new bool[] { false, false,  true, false, false, false, false,  true, false,  true,  true, false,  true,  true,  true,  true, false, false }),
                new BitArray(new bool[] { false, false,  true, false, false,  true,  true, false,  true, false,  true, false, false,  true,  true, false, false,  true }),
                new BitArray(new bool[] { false, false,  true, false,  true, false, false,  true, false, false,  true,  true, false,  true, false, false,  true,  true }),
                new BitArray(new bool[] { false, false,  true, false,  true,  true,  true, false,  true,  true,  true,  true,  true,  true, false,  true,  true, false }),
                new BitArray(new bool[] { false, false,  true,  true, false, false, false,  true,  true,  true, false,  true,  true, false, false, false,  true, false }),
                new BitArray(new bool[] { false, false,  true,  true, false,  true,  true, false, false, false, false,  true, false, false, false,  true,  true,  true }),
                new BitArray(new bool[] { false, false,  true,  true,  true, false, false,  true,  true, false, false, false, false, false,  true,  true, false,  true }),
                new BitArray(new bool[] { false, false,  true,  true,  true,  true,  true, false, false,  true, false, false,  true, false,  true, false, false, false }),
                new BitArray(new bool[] { false,  true, false, false, false, false,  true, false,  true,  true, false,  true,  true,  true,  true, false, false, false }),
                new BitArray(new bool[] { false,  true, false, false, false,  true, false,  true, false, false, false,  true, false,  true,  true,  true, false,  true }),
                new BitArray(new bool[] { false,  true, false, false,  true, false,  true, false,  true, false, false, false, false,  true, false,  true,  true,  true }),
                new BitArray(new bool[] { false,  true, false, false,  true,  true, false,  true, false,  true, false, false,  true,  true, false, false,  true, false }),
                new BitArray(new bool[] { false,  true, false,  true, false, false,  true, false, false,  true,  true, false,  true, false, false,  true,  true, false }),
                new BitArray(new bool[] { false,  true, false,  true, false,  true, false,  true,  true, false,  true, false, false, false, false, false,  true,  true }),
                new BitArray(new bool[] { false,  true, false,  true,  true, false,  true, false, false, false,  true,  true, false, false,  true, false, false,  true }),
                new BitArray(new bool[] { false,  true, false,  true,  true,  true, false,  true,  true,  true,  true,  true,  true, false,  true,  true, false, false }),
                new BitArray(new bool[] { false,  true,  true, false, false, false,  true,  true,  true, false,  true,  true, false, false, false,  true, false, false }),
                new BitArray(new bool[] { false,  true,  true, false, false,  true, false, false, false,  true,  true,  true,  true, false, false, false, false,  true }),
                new BitArray(new bool[] { false,  true,  true, false,  true, false,  true,  true,  true,  true,  true, false,  true, false,  true, false,  true,  true }),
                new BitArray(new bool[] { false,  true,  true, false,  true,  true, false, false, false, false,  true, false, false, false,  true,  true,  true, false }),
                new BitArray(new bool[] { false,  true,  true,  true, false, false,  true,  true, false, false, false, false, false,  true,  true, false,  true, false }),
                new BitArray(new bool[] { false,  true,  true,  true, false,  true, false, false,  true,  true, false, false,  true,  true,  true,  true,  true,  true }),
                new BitArray(new bool[] { false,  true,  true,  true,  true, false,  true,  true, false,  true, false,  true,  true,  true, false,  true, false,  true }),
                new BitArray(new bool[] { false,  true,  true,  true,  true,  true, false, false,  true, false, false,  true, false,  true, false, false, false, false }),
                new BitArray(new bool[] {  true, false, false, false, false, false,  true, false, false,  true,  true,  true, false,  true, false,  true, false,  true }),
                new BitArray(new bool[] {  true, false, false, false, false,  true, false,  true,  true, false,  true,  true,  true,  true, false, false, false, false }),
                new BitArray(new bool[] {  true, false, false, false,  true, false,  true, false, false, false,  true, false,  true,  true,  true, false,  true, false }),
                new BitArray(new bool[] {  true, false, false, false,  true,  true, false,  true,  true,  true,  true, false, false,  true,  true,  true,  true,  true }),
                new BitArray(new bool[] {  true, false, false,  true, false, false,  true, false,  true,  true, false, false, false, false,  true, false,  true,  true }),
                new BitArray(new bool[] {  true, false, false,  true, false,  true, false,  true, false, false, false, false,  true, false,  true,  true,  true, false }),
                new BitArray(new bool[] {  true, false, false,  true,  true, false,  true, false,  true, false, false,  true,  true, false, false,  true, false, false }),
                new BitArray(new bool[] {  true, false, false,  true,  true,  true, false,  true, false,  true, false,  true, false, false, false, false, false,  true }),
                new BitArray(new bool[] {  true, false,  true, false, false, false,  true,  true, false, false, false,  true,  true, false,  true, false, false,  true }),
            };

        private static readonly byte[] ExponentTable =
            new byte[]
            {
                1,   2,   4,   8,   16,  32,  64,  128, 29,  58,  116, 232, 205, 135, 19,  38,
                76,  152, 45,  90,  180, 117, 234, 201, 143, 3,   6,   12,  24,  48,  96,  192,
                157, 39,  78,  156, 37,  74,  148, 53,  106, 212, 181, 119, 238, 193, 159, 35,
                70,  140, 5,   10,  20,  40,  80,  160, 93,  186, 105, 210, 185, 111, 222, 161,
                95,  190, 97,  194, 153, 47,  94,  188, 101, 202, 137, 15,  30,  60,  120, 240,
                253, 231, 211, 187, 107, 214, 177, 127, 254, 225, 223, 163, 91,  182, 113, 226,
                217, 175, 67,  134, 17,  34,  68,  136, 13,  26,  52,  104, 208, 189, 103, 206,
                129, 31,  62,  124, 248, 237, 199, 147, 59,  118, 236, 197, 151, 51,  102, 204,
                133, 23,  46,  92,  184, 109, 218, 169, 79,  158, 33,  66,  132, 21,  42,  84,
                168, 77,  154, 41,  82,  164, 85,  170, 73,  146, 57,  114, 228, 213, 183, 115,
                230, 209, 191, 99,  198, 145, 63,  126, 252, 229, 215, 179, 123, 246, 241, 255,
                227, 219, 171, 75,  150, 49,  98,  196, 149, 55,  110, 220, 165, 87,  174, 65,
                130, 25,  50,  100, 200, 141, 7,   14,  28,  56,  112, 224, 221, 167, 83,  166,
                81,  162, 89,  178, 121, 242, 249, 239, 195, 155, 43,  86,  172, 69,  138, 9,
                18,  36,  72,  144, 61,  122, 244, 245, 247, 243, 251, 235, 203, 139, 11,  22,
                44,  88,  176, 125, 250, 233, 207, 131, 27,  54,  108, 216, 173, 71,  142, 1
            };

        private static readonly byte[] LogTable =
            new byte[]
            {
                0,   0,   1,   25,  2,   50,  26,  198, 3,   223, 51,  238, 27,  104, 199, 75,
                4,   100, 224, 14,  52,  141, 239, 129, 28,  193, 105, 248, 200, 8,   76,  113,
                5,   138, 101, 47,  225, 36,  15,  33,  53,  147, 142, 218, 240, 18,  130, 69,
                29,  181, 194, 125, 106, 39,  249, 185, 201, 154, 9,   120, 77,  228, 114, 166,
                6,   191, 139, 98,  102, 221, 48,  253, 226, 152, 37,  179, 16,  145, 34,  136,
                54,  208, 148, 206, 143, 150, 219, 189, 241, 210, 19,  92,  131, 56,  70,  64,
                30,  66,  182, 163, 195, 72,  126, 110, 107, 58,  40,  84,  250, 133, 186, 61,
                202, 94,  155, 159, 10,  21,  121, 43,  78,  212, 229, 172, 115, 243, 167, 87,
                7,   112, 192, 247, 140, 128, 99,  13,  103, 74,  222, 237, 49,  197, 254, 24,
                227, 165, 153, 119, 38,  184, 180, 124, 17,  68,  146, 217, 35,  32,  137, 46,
                55,  63,  209, 91,  149, 188, 207, 205, 144, 135, 151, 178, 220, 252, 190, 97,
                242, 86,  211, 171, 20,  42,  93,  158, 132, 60,  57,  83,  71,  109, 65,  162,
                31,  45,  67,  216, 183, 123, 164, 118, 196, 23,  73,  236, 127, 12,  111, 246,
                108, 161, 59,  82,  41,  157, 85,  170, 251, 96,  134, 177, 187, 204, 62,  90,
                203, 89,  95,  176, 156, 169, 160, 81,  11,  245, 22,  235, 122, 117, 44,  215,
                79,  174, 213, 233, 230, 231, 173, 232, 116, 214, 244, 234, 168, 80,  88,  175
            };
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SCEditor.Compression;
using System.Linq;
using SCEditor.Helpers;
using Google.FlatBuffers;

namespace SCEditor.ScOld
{
    using ScMatrix = Matrix;
    using ScMatrixList = List<Matrix>;
    using ScColorTransform = Tuple<Color, byte, Color>;
    using ScColorTransformList = List<Tuple<Color, byte, Color>>;

    public class ScFile
    {
        const ushort SC_MAGIC = 17235;
        const uint LZHAM_MAGIC = 0x5A4C4353;

        //

        #region Constructors

        public ScFile(string filepath)
        {
            _textures = new List<ScObject>();
            _shapes = new List<ScObject>();
            _exports = new List<ScObject>();
            _movieClips = new List<ScObject>();
            _movieClipsModifier = new List<ScObject>();
            _pendingChanges = new List<ScObject>();
            _pendingMatrixs = new Dictionary<int, ScMatrixList>();
            _Filepath = filepath;
            _pendingColors = new Dictionary<int, ScColorTransformList>();
            _fontNames = new List<string>();
            _textFields = new List<ScObject>();
            _currentRenderingMovieClips = new List<ScObject>();
        }

        #endregion

        #region Fields & Properties

        private ushort _exportCount;
        private ushort _shapeCount;
        private ushort _movieClipCount;
        private ushort _textureCount;
        private int _textFieldCount; 
        private int _matrixCount;
        private int _colorsCount;

        private string _highres_suffix = "_highres";
        private string _lowres_suffix = "_lowres";
        private bool _use_multi_resolution;
        private bool _use_low_resolution;


        private List<ScObject> _textures;
        private List<ScObject> _shapes;
        private List<ScObject> _exports;
        private List<ScObject> _movieClips;
        private List<ScObject> _movieClipsModifier;
        private List<ScObject> _textFields;
        private List<(ScMatrixList, ScColorTransformList)> _transformStorage;

        private List<ScObject> _pendingChanges;
        private Dictionary<int, ScMatrixList> _pendingMatrixs;
        private Dictionary<int, ScColorTransformList>  _pendingColors;

        private List<string> _fontNames;
        private List<ScObject> _currentRenderingMovieClips;
        private Dictionary<int, long> _transformStorageOffsets;

        private readonly string _Filepath;
        private string _textureFilepath;
        private long _eofOffset;
        private long _eofTextFieldOffset;
        private long _eofMovieClipOffset;
        private long _eofShapeOffset;
        private long _eofTexOffset;
        private long _exportStartOffset;
        private long _sofTagsOffset;
        private int _transformStorageID;

        private MemoryStream _buffer;
        public BinaryReader reader;

        #endregion

        public void SetReader(byte[] data)
        {
            _buffer = new MemoryStream(data);
            reader = new BinaryReader(_buffer);
        }

        public void ResetReader()
        {
            reader = null;
            _buffer = null;
        }

        public void AddChange(ScObject change)
        {
            if (_pendingChanges.IndexOf(change) == -1)
                _pendingChanges.Add(change);
        }

        public void AddExport(Export export)
        {
            _exports.Add(export);
        }

        public void AddShape(Shape shape)
        {
            _shapes.Add(shape);
        }

        public List<(ScMatrixList, ScColorTransformList)> GetTransformStorage()
        {
            return _transformStorage;
        }

        public void AddTexture(Texture texture)
        {
            _textures.Add(texture);
        }

        public void AddMovieClip(MovieClip movieClip)
        {
            _movieClips.Add(movieClip);
        }

        public long GetEofOffset()
        {
            return _eofOffset;
        }

        public long GetEofTexOffset()
        {
            return _eofTexOffset;
        }
        public long GetSofTagsOffset()
        {
            return _sofTagsOffset;
        }

        public List<ScObject> GetExports()
        {
            return _exports;
        }

        public string GetInfoFileName()
        {
            return _Filepath;
        }

        public ScColorTransformList getColors(int storageID)
        {
            return _transformStorage[storageID].Item2;
        }

        public string GetTextureFileName()
        {
            return _textureFilepath;
        }

        public List<ScObject> GetMovieClips()
        {
            return _movieClips;
        }

        public ScMatrixList GetMatrixs(int storageID)
        {
            return _transformStorage[storageID].Item1;
        }

        public void AddMatrix(Matrix matrix, int TransformStorageID)
        {
            if (_transformStorage[TransformStorageID].Item1.Count >= ushort.MaxValue)
            {
                MessageBox.Show($"StorageBox {TransformStorageID} matrix has already exceeded max array size. Skipping.");
                return;
            }

            _transformStorage[TransformStorageID].Item1.Add(matrix);
        }
        public List<ScObject> GetShapes()
        {
            return _shapes;
        }
        public List<ScObject> GetPendingChanges()
        {
            return _pendingChanges;
        }

        public Dictionary<int, ScMatrixList> GetPendingMatrixChanges()
        {
            return _pendingMatrixs;
        }
        public ushort getShapesChunksCount()
        {
            ushort total = 0;
            foreach (ScObject sco in _shapes)
            {
                total = (ushort) (total + sco.Children.Count);
            }

            return total;
        }

        public long GetStartExportsOffset()
        {
            return _exportStartOffset;
        }

        public List<ScObject> GetTextures()
        {
            return _textures;
        }

        public void SetEofOffset(long offset)
        {
            _eofOffset = offset;
        }


        public void SetEofTexOffset(long offset)
        {
            _eofTexOffset = offset;
        }

        public void AddPendingMatrix(Matrix data, int TransformStorageID)
        {
            if (_pendingMatrixs.ContainsKey(TransformStorageID))
            {
                _pendingMatrixs[TransformStorageID].Add(data);
            }
            else
            {
                _pendingMatrixs.Add(TransformStorageID, new ScMatrixList() { data });
            }
        }

        public void SetSofTagsOffset(long offset)
        {
            _sofTagsOffset = offset;
        }

        public void SetStartExportsOffset(long offset)
        {
            _exportStartOffset = offset;
        }

        private static void ExpandFile(FileStream stream, long offset, int extraBytes)
        {
            // http://stackoverflow.com/questions/3033771/file-io-with-streams-best-memory-buffer-size
            const int SIZE = 4096;
            var buffer = new byte[SIZE];
            var length = stream.Length;
            // Expand file
            stream.SetLength(length + extraBytes);
            var pos = length;
            int to_read;
            while (pos > offset)
            {
                to_read = pos - SIZE >= offset ? SIZE : (int)(pos - offset);
                pos -= to_read;
                stream.Position = pos;
                stream.Read(buffer, 0, to_read);
                stream.Position = pos + extraBytes;
                stream.Write(buffer, 0, to_read);
            }
        }

        public void Save(FileStream input, FileStream texinput)
        {
            int exportAdd = 0;
            int textureAdd = 0;
            int movieClipAdd = 0;
            int movieClipEdits = 0;
            int shapeAdd = 0;
            int shapeChunkAdd = 0;
            int matrixAdd = 0;
            int colorsAdd = 0;
            int textFieldsAdd = 0;

            input.Seek(0, SeekOrigin.Begin);

            // Flushing depending edits.
            List<ScObject> exports = new List<ScObject>();
            _pendingChanges = _pendingChanges.OrderBy(obj => obj.objectType).ToList();

            /**
            foreach (ScObject scobjex in _pendingChanges)
            {
                if (scobjex.GetDataType() == 7)
                {
                    Console.Write(((Export)scobjex).GetName() + ';');
                }
            }
            **/

            for (int i = 0; i < _pendingChanges.Count; i++)
            {
                ScObject data = _pendingChanges[i];

                switch (data.GetDataType())
                {
                    case 7: // Exports
                        exports.Add(data);
                        exportAdd += 1;
                        break;

                    case 2: // Texture
                        data.Write(texinput);
                        textureAdd += 1;
                        if (data.customAdded == true)
                        {
                            _textureCount += 1;
                            input.Seek(4, SeekOrigin.Begin);
                            input.Write(BitConverter.GetBytes(this._textureCount), 0, 2);
                        }
                        break;

                    case 5: // TextFields
                        data.Write(input);
                        textFieldsAdd += 1;

                        if (data.customAdded == true)
                        {
                            this._textFieldCount += 1;
                            input.Seek(6, SeekOrigin.Begin);
                            input.Write(BitConverter.GetBytes(this._textFieldCount), 0, 2);
                            goto case -256;
                        }
                        else
                        {
                            Reset(data);
                        }

                        break;

                    case 1: // MovieClip
                        data.Write(input);

                        if (data.customAdded == true)
                        {
                            movieClipAdd += 1;
                            this._movieClipCount += 1;
                            input.Seek(2, SeekOrigin.Begin);
                            input.Write(BitConverter.GetBytes(this._movieClipCount), 0, 2);
                            goto case -256;
                        }
                        else
                        {
                            Reset(data);
                            movieClipEdits += 1;
                        }

                        break;

                    case 0: // Shape
                        data.Write(input);
                        shapeAdd += 1;

                        if (data.customAdded == true)
                        {
                            this._shapeCount += 1;
                            input.Seek(0, SeekOrigin.Begin);
                            input.Write(BitConverter.GetBytes(this._shapeCount), 0, 2);
                            goto case -256;
                        }
                        else
                        {
                            Reset(data);
                        }

                        break;

                    case 99: // ShapeChunk
                        throw new Exception("check");
                    //data.Write(input);
                    //shapeChunkAdd += 1;
                    //break;

                    case -256:
                        if (data.GetDataType() == -256)
                            throw new Exception("Datatype equals -256 Save()");

                        this.SetEofOffset(input.Length);
                        input.Seek(0, SeekOrigin.End);
                        input.Write(new byte[] { 0, 0, 0, 0, 0 });
                        break;

                    default:
                        data.Write(input);
                        break;
                }
            }
            _pendingChanges.Clear();

            input.Close();
            reloadFile();
            input = new FileStream(_Filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

            if (exports.Count > 0)
            {
                foreach (ScObject data in exports)
                {
                    data.Write(input);
                }
            }

            input.Close();
            reloadFile();
            input = new FileStream(_Filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

            if (_pendingMatrixs.Count > 0)
            {
                input.Seek(this._eofOffset, SeekOrigin.Begin);

                foreach (var matrixesPerStorage in _pendingMatrixs)
                {
                    int currentMatrixAdd = 0;

                    int transformStorageID = matrixesPerStorage.Key;
                    bool isEndofOffset = false;

                    long postition = _eofOffset;

                    if (transformStorageID != (this._transformStorage.Count - 1))
                    {
                        postition = _transformStorageOffsets[transformStorageID + 1];
                    }
                    else
                    {
                        isEndofOffset = true;
                    }

                    if (!isEndofOffset)
                    {
                        ExpandFile(input, postition, (matrixesPerStorage.Value.Count * 29));
                    }

                    input.Position = postition;
                    foreach (Matrix matrix in matrixesPerStorage.Value)
                    {
                        input.WriteByte(8);
                        input.Write(BitConverter.GetBytes(24), 0, 4);

                        Matrix newMatrix = new Matrix(matrix.Elements[0] / 0.00097656f, matrix.Elements[1] / 0.00097656f, matrix.Elements[2] / 0.00097656f,
                            matrix.Elements[3] / 0.00097656f, matrix.Elements[4] * 20f, matrix.Elements[5] * 20f);

                        for (int i = 0; i < 6; i++)
                        {
                            input.Write(BitConverter.GetBytes((int)newMatrix.Elements[i]), 0, 4);
                        }

                        if (isEndofOffset)
                        {
                            _eofOffset = input.Position;
                        }

                        currentMatrixAdd += 1;
                    }

                    if (isEndofOffset)
                    {
                        input.Write(new byte[] { 0, 0, 0, 0, 0 });
                    }

                    if (this._transformStorage.Count != 0 && transformStorageID != 0)
                    {
                        input.Position = _transformStorageOffsets[transformStorageID] + 5;
                        var currentValueBytes = new byte[2];
                        input.Read(currentValueBytes, 0, 2);
                        input.Seek(-2, SeekOrigin.Current);
                        ushort newValue = (ushort)(BitConverter.ToUInt16(currentValueBytes, 0) + _pendingMatrixs[transformStorageID].Count);
                        input.Write(BitConverter.GetBytes(newValue), 0, 2);
                    }

                    if (isEndofOffset && transformStorageID == 0)
                    {
                        input.Seek(8, SeekOrigin.Begin);
                        this._matrixCount += currentMatrixAdd;
                        input.Write(BitConverter.GetBytes((ushort)this._matrixCount), 0, 2);
                    }

                    matrixAdd += currentMatrixAdd;
                }

                _pendingMatrixs.Clear();
            }

            input.Flush();

            if (_pendingColors.Count > 0)
            {
                input.Seek(this._eofOffset, SeekOrigin.Begin);

                foreach (var colorsPerStorage in _pendingColors)
                {
                    int currentColorsAdd = 0;

                    int transformStorageID = colorsPerStorage.Key;
                    bool isEndofOffset = false;

                    long postition = _eofOffset;

                    if (transformStorageID != (this._transformStorage.Count - 1))
                    {
                        postition = _transformStorageOffsets[transformStorageID + 1];
                    }
                    else
                    {
                        isEndofOffset = true;
                    }

                    if (!isEndofOffset)
                    {
                        ExpandFile(input, postition, (colorsPerStorage.Value.Count * 12));
                    }

                    input.Position = postition;

                    foreach (ScColorTransform color in colorsPerStorage.Value)
                    {
                        input.WriteByte(9);
                        input.Write(BitConverter.GetBytes(7), 0, 4);

                        input.WriteByte(color.Item1.R);
                        input.WriteByte(color.Item1.G);
                        input.WriteByte(color.Item1.B);
                        input.WriteByte(color.Item2);
                        input.WriteByte(color.Item3.R);
                        input.WriteByte(color.Item3.G);
                        input.WriteByte(color.Item3.B);

                        if (isEndofOffset)
                        {
                            _eofOffset = input.Position;
                        }

                        currentColorsAdd += 1;
                    }

                    if (isEndofOffset)
                    {
                        input.Write(new byte[] { 0, 0, 0, 0, 0 });
                    }

                    if (this._transformStorage.Count != 0 && transformStorageID != 0)
                    {
                        input.Position = _transformStorageOffsets[transformStorageID] + 7;
                        var currentValueBytes = new byte[2];
                        input.Read(currentValueBytes, 0, 2);
                        input.Seek(-2, SeekOrigin.Current);
                        ushort newValue = (ushort)(BitConverter.ToUInt16(currentValueBytes, 0) + _pendingColors[transformStorageID].Count);
                        input.Write(BitConverter.GetBytes(newValue), 0, 2);
                    }

                    if (isEndofOffset && transformStorageID == 0)
                    {
                        input.Seek(10, SeekOrigin.Begin);
                        this._colorsCount += currentColorsAdd;
                        input.Write(BitConverter.GetBytes((ushort)this._colorsCount), 0, 2);
                    }

                    colorsAdd += currentColorsAdd;
                }

                _pendingColors.Clear();  
            }

            input.Close();
            reloadFile();
            

            Console.WriteLine($"SaveSC: Done saving (Add/Edit) Exports: {exportAdd} | MovieClips: {movieClipAdd}/{movieClipEdits} | Shapes: {shapeAdd} | Shape Chunks: {shapeChunkAdd} | Textures: {textureAdd} | Matrixs {matrixAdd} | Colors {colorsAdd} | TextFields {textFieldsAdd}");
        }

        private void Reset(ScObject data)
        {
            List<ushort> idsGreater = new List<ushort>();

            long currentOffset = data._offset;
            long nextOffset = long.MaxValue;

            foreach (ScObject shape in this.GetShapes())
            {
                if (shape.Id == data.Id)
                    continue;

                if (shape.offset > currentOffset)
                {
                    idsGreater.Add(shape.Id);
                }

                if (shape.offset < nextOffset && shape.offset > currentOffset)
                {
                    nextOffset = shape.offset;
                }
            }

            foreach (ScObject mv in this.GetMovieClips())
            {
                if (mv.Id == data.Id)
                    continue;

                if (mv.offset > currentOffset)
                {
                    idsGreater.Add(mv.Id);
                }

                if (mv.offset < nextOffset && mv.offset > currentOffset)
                {
                    nextOffset = mv.offset;
                }
            }

            foreach (ScObject tf in this.getTextFields())
            {
                if (tf.Id == data.Id)
                    continue;

                if (tf.offset > currentOffset)
                {
                    idsGreater.Add(tf.Id);
                }

                if (tf.offset < nextOffset && tf.offset > currentOffset)
                {
                    nextOffset = tf.offset;
                }
            }

            long dataEndOffset = currentOffset + data.length + 5;

            if (dataEndOffset == nextOffset)
                return;

            long dataDifference = dataEndOffset - nextOffset;

            foreach (ushort id in idsGreater)
            {
                int shapeIndex = this.GetShapes().FindIndex(s => s.Id == id);
                if (shapeIndex != -1)
                {
                    ScObject item = this.GetShapes()[shapeIndex];
                    long newOffset = item.offset + dataDifference;
                    item.SetOffset(newOffset);
                }
                else
                {
                    int mvIndex = this.GetMovieClips().FindIndex(mv => mv.Id == id);
                    if (mvIndex != -1)
                    {
                        ScObject item = this.GetMovieClips()[mvIndex];
                        long newOffset = item.offset + dataDifference;
                        item.SetOffset(newOffset);
                    }
                    else
                    {
                        int tfIndex = this.getTextFields().FindIndex(tf => tf.Id == id);
                        if (tfIndex != -1)
                        {
                            ScObject item = this.getTextFields()[tfIndex];
                            long newOffset = item.offset + dataDifference;
                            item.SetOffset(newOffset);
                        }
                        else
                        {
                            throw new Exception("Invalid Type");
                        }
                    }
                }
            }
        }

        public void Load()
        {
            var sw = Stopwatch.StartNew();

            bool has_texture = LoadFile(_Filepath);

            if (has_texture)
            {
                string directory = Path.GetDirectoryName(_Filepath);
                string base_name = Path.GetFileNameWithoutExtension(_Filepath);

                if (_use_multi_resolution)
                {
                    _textureFilepath = Path.Combine(directory, base_name + _highres_suffix + "_tex.sc");
                } else
                {
                    _textureFilepath = Path.Combine(directory, base_name + _highres_suffix + "_tex.sc");
                }

                if (!Path.Exists(_textureFilepath))
                {
                    throw new Exception($"Failed to load texture: {_textureFilepath}");
                }

                LoadFile(_textureFilepath);
            }

            PostprocessObjects();

            sw.Stop();
            Program.Interface.Text = $@"SC Editor :  {Path.GetFileNameWithoutExtension(_Filepath)}";
            Program.Interface.Update();
            Console.WriteLine(@"SC File loading finished in {0}ms", sw.Elapsed.TotalMilliseconds);
        }

        public bool LoadFile(string filepath)
        {
            SetReader(File.ReadAllBytes(filepath));

            ushort magic = reader.ReadUInt16();
            uint version = reader.ReadUInt32();

            if (magic == SC_MAGIC && version == 5)
            {
                Console.WriteLine("Loading SC2...");
                LoadSC2();
                return false;
            }

            if (magic == SC_MAGIC)
            {
                DecompressSC1File();
            }

            return LoadSC1();
        }

        public void DecompressSC2()
        {
           reader.BaseStream.Position = 0;

            ushort magic = reader.ReadUInt16();
            if (magic != SC_MAGIC)
            {
                throw new Exception("Corrupted file");
            }

            uint version = reader.ReadUInt32();
            if (version != 5)
            {
                throw new Exception("Unknown SC2 version");
            }

            uint metadata_length = reader.ReadUInt32();
            reader.BaseStream.Position += metadata_length;

            SetReader(zstandard.Decompress(reader));
        }

        public int LoadSC2Chunk(SC2.DataStorage storage, Func<SC2.DataStorage, ByteBuffer, int> load_function)
        {
            ByteBuffer buffer;
            {
                uint chunk_length = reader.ReadUInt32();
                byte[] chunk_data = reader.ReadBytes((int)chunk_length);
                buffer = new(chunk_data);
            }

            return load_function(storage, buffer);
        }

        public void LoadSC2()
        {
            DecompressSC2();

            SC2.DataStorage storage;
            {
                uint storage_length = reader.ReadUInt32();
                byte[] storage_data = reader.ReadBytes((int)storage_length);
                ByteBuffer buffer = new(storage_data);
                storage = SC2.DataStorage.GetRootAsDataStorage(buffer);
            }

            LoadSC2_MatrixBank(storage);

            LoadSC2Chunk(storage, LoadSC2_ExportNames);
            LoadSC2Chunk(storage, LoadSC2_TextFields);
        }

        public void LoadSC2_MatrixBank(SC2.DataStorage storage)
        {
            _transformStorage.Capacity = storage.MatrixBanksLength;
            for (int i = 0; storage.MatrixBanksLength > i; i++)
            {
                if (!storage.MatrixBanks(i).HasValue) continue;
                SC2.MatrixBank bank = storage.MatrixBanks(i).Value;

                ScMatrixList matrices = new();
                matrices.Capacity = bank.MatricesLength;

                ScColorTransformList colors = new();
                colors.Capacity = bank.ColorsLength;

                for (int t = 0; t < bank.MatricesLength; t++)
                {
                    if (!bank.Matrices(t).HasValue) continue;

                    SC2.Typing.Matrix2x3 matrix_value = bank.Matrices(t).Value;

                    ScMatrix matrix = new(
                        matrix_value.A, matrix_value.B, matrix_value.C, matrix_value.D,
                        matrix_value.Tx, matrix_value.Ty
                    );

                    matrices.Add(matrix);
                }

                for (int t = 0; bank.ColorsLength > t; i++)
                {
                    if (!bank.Colors(t).HasValue) continue;

                    SC2.Typing.ColorTransform color_value = bank.Colors(t).Value;

                    ScColorTransform color = new(
                        Color.FromArgb(color_value.RAdd, color_value.GAdd, color_value.BAdd),
                        color_value.Alpha,
                        Color.FromArgb(color_value.RMul, color_value.GMul, color_value.BMul)
                    );

                    colors.Add(color);
                }

                _transformStorage.Add((matrices, colors));
            }
        }

        public int LoadSC2_ExportNames(SC2.DataStorage storage, ByteBuffer data)
        {
            SC2.ExportNames exports = SC2.ExportNames.GetRootAsExportNames(data);
            if (exports.NameRefIdsLength != exports.ObjectIdsLength)
            {
                throw new Exception("String references and id count does not match");
            }

            _exports.Capacity = exports.ObjectIdsLength;
            for (int i = 0; exports.NameRefIdsLength > i; i++)
            {
                ushort export_id = exports.ObjectIds(i);
                uint export_name_ref = exports.NameRefIds(i);

                Export export_name = new(this);
                export_name.SetId(export_id);
                export_name.Rename(storage.Strings((int)export_name_ref));
                _exports.Add(export_name);
            }

            return 0;
        }

        public int LoadSC2_TextFields(SC2.DataStorage storage, ByteBuffer data)
        {
            SC2.TextFields textfields = SC2.TextFields.GetRootAsTextFields(data);
            _textFields.Capacity = textfields.TextfieldsLength;

            for (int i = 0; textfields.TextfieldsLength > i; i++)
            {
                if (!textfields.Textfields(i).HasValue) continue;

                SC2.TextField textfield_value = textfields.Textfields(i).Value;
                TextField textfield = new(this);

                textfield.customAdded = true;
                textfield.setId(textfield_value.Id);
                


            }

            return 0;
        }

        public void DecompressSC1File()
        {
            reader.BaseStream.Position = 0;

            ushort magic = reader.ReadUInt16();
            if (magic != SC_MAGIC)
            {
                throw new Exception("File is corrupted");
            }

            uint version = reader.ReadUInt32BE();

            // Metadata
            if (version == 4)
            {
                version = reader.ReadUInt32BE();
            }

            uint hashLength = reader.ReadUInt32BE();
            reader.BaseStream.Seek(hashLength, SeekOrigin.Current);

            byte[] data;

            switch (version)
            {
                case 5:
                    throw new Exception("Wrong compilation parameters for SC2");
                case 3:
                    Console.WriteLine("Zstandard Compression for " + Path.GetFileName(_Filepath));
                    data = zstandard.Decompress(reader);
                    break;
                case 1:
                    {
                        uint compress_magic = reader.ReadUInt32();

                        if (compress_magic == LZHAM_MAGIC)
                        {
                            Console.WriteLine("LZHAM Compression for " + Path.GetFileName(_Filepath));
                            byte dict_size = reader.ReadByte();
                            uint unpacked_size = reader.ReadUInt32();

                            data = Lzma.DecompressLZHAM(reader, dict_size, unpacked_size);

                        } else
                        {
                            reader.BaseStream.Seek(-4, SeekOrigin.Current);
                            Console.WriteLine("LZMA Compression for " + Path.GetFileName(_Filepath));

                            data = Lzma.DecompressLZMA(reader);
                        }
                    }
                    break;
                default:
                    throw new Exception("Unknown file version");
            }

            SetReader(data);
        }

        public bool LoadTags()
        {
            int shapeIndex = 0;
            int movieClipIndex = 0;
            int movieClipModifierIndex = 0;
            int textFieldIndex = 0;
            int matrixIndex = 0;
            int colorIndex = 0;
            int textureIndex = 0;

            bool has_external_texture = false;

            while (true)
            {
                long tag_offset = reader.BaseStream.Position;

                var tag = reader.ReadByte();
                var tag_size = reader.ReadUInt32();

                if (tag_size < 0)
                    throw new Exception("Negative tag length. Tag " + tag);

                if (tag == 0)
                {
                    if (_shapeCount != shapeIndex ||
                        _movieClipCount != movieClipIndex ||
                        _textFieldCount != textFieldIndex)
                    {
                        throw new Exception("Didn't load whole .sc properly.");
                    }

                    if (_matrixCount != matrixIndex ||
                    _colorsCount != colorIndex)
                    {
                        MessageBox.Show("Didn't load whole .sc properly. Martix count or colors count do not match to how many were loaded.\n\nWill proceed to open the file but proceed at your own risk.");
                    }

                    return has_external_texture;
                }

                ScObject.SCObjectType lastSCType = ScObject.SCObjectType.None;
                ushort lastSCId = 0;

                switch (tag)
                {
                    case 26:
                        has_external_texture = true; 
                        break;

                    case 32:
                        _highres_suffix = reader.ReadStringSC();
                        _lowres_suffix = reader.ReadStringSC();
                        break;

                    case 1:
                    case 16:
                    case 19:
                    case 27:
                    case 28:
                    case 29:
                    case 34:
                    case 45:
                        if (textureIndex >= _textureCount)
                            throw new Exception($"Trying to load too many shapes.\n Index: {textureIndex} | Count: {_textureCount}");

                        var texture = new Texture(this);
                        texture.SetOffset(tag_offset);
                        texture.setLength(tag_size);
                        texture.Read(this, tag, !has_external_texture);
                        _eofTexOffset = reader.BaseStream.Position;
                        this._textures.Add(texture);

                        textureIndex ++;
                        break;

                    case 2:
                    case 18:
                        if (shapeIndex >= _shapeCount)
                            throw new Exception($"Trying to load too many shapes.\n Index: {shapeIndex} | Count: {_shapeCount}");

                        var shape = new Shape(this);
                        shape.SetOffset(tag_offset);
                        shape.setLength(tag_size);
                        shape.Read(this, tag);
                        _shapes[shapeIndex] = shape;

                        _eofShapeOffset = reader.BaseStream.Position;

                        lastSCId = shape.Id;
                        lastSCType = ScObject.SCObjectType.Shape;
                        shapeIndex += 1;
                        break;

                    case 3:
                    case 10:
                    case 14:
                    case 35:
                    case 12:
                    case 49:
                        if (movieClipIndex >= _movieClipCount)
                            throw new Exception($"Trying to load too many movieclips.\n Index: {movieClipIndex} | Count: {_movieClipCount}");

                        var movieClip = new MovieClip(this, tag);
                        movieClip.SetOffset(tag_offset);
                        movieClip.setLength(tag_size);
                        ushort clipId = movieClip.ReadMV(reader, tag, tag_size);
                        _movieClips[movieClipIndex] = movieClip;

                        _eofMovieClipOffset = reader.BaseStream.Position;

                        lastSCId = movieClip.Id;
                        lastSCType = ScObject.SCObjectType.MovieClip;
                        movieClipIndex += 1;
                        break;

                    case 7:
                    case 15:
                    case 20:
                    case 21:
                    case 25:
                    case 33:
                    case 43:
                    case 44:
                        if (textFieldIndex >= _textFieldCount)
                            throw new Exception($"Trying to load too many TextFields. \n Index: {textFieldIndex} | Count: {_textFieldCount}");

                        TextField textField = new TextField(this);
                        textField.Read(this, tag);
                        textField.setLength(tag_size);
                        _textFields[textFieldIndex] = textField;

                        _eofTextFieldOffset = reader.BaseStream.Position;

                        lastSCId = textField.Id;
                        lastSCType = ScObject.SCObjectType.TextField;
                        textFieldIndex += 1;
                        break;

                    case 8:
                        float[] Points = new float[6];
                        for (int Index = 0; Index < 6; Index++)
                        {
                            Points[Index] = reader.ReadInt32();
                        }
                        Matrix _Matrix = new Matrix(Points[0] * 0.00097656f, Points[1] * 0.00097656f, Points[2] * 0.00097656f,
                            Points[3] * 0.00097656f, Points[4] / 20f, Points[5] / 20f);
                        this._transformStorage[_transformStorageID].Item1.Add(_Matrix);

                        matrixIndex++;
                        break;

                    case 9:
                        var ra = reader.ReadByte();
                        var ga = reader.ReadByte();
                        var ba = reader.ReadByte();
                        var am = reader.ReadByte();
                        var rm = reader.ReadByte();
                        var gm = reader.ReadByte();
                        var bm = reader.ReadByte();
                        this._transformStorage[_transformStorageID].Item2.Add(new ScColorTransform(Color.FromArgb(ra, ga, ba), am, Color.FromArgb(rm, gm, bm)));

                        colorIndex += 1;
                        break;

                    case 13:
                        reader.ReadInt32();
                        throw new Exception("TAG_TIMELINE_INDEXES no longer in use");

                    case 23:
                        _use_low_resolution = true;
                        break;

                    case 30:
                        _use_multi_resolution = true;
                        break;

                    case 36:
                        float[] Points2 = new float[6];
                        for (int Index = 0; Index < 6; Index++)
                        {
                            Points2[Index] = reader.ReadInt32();
                        }
                        Matrix _Matrix2 = new Matrix(Points2[0] / 65535f, Points2[1] / 65535f, Points2[2] / 65535f,
                            Points2[3] / 65535f, Points2[4] / 20f, Points2[5] / 20f);
                        this._transformStorage[_transformStorageID].Item1.Add(_Matrix2);

                        matrixIndex++;
                        break;

                    case 37:
                        ushort movieClipModifierCount = reader.ReadUInt16();

                        for (int i = 0; i < movieClipModifierCount; i++)
                        {
                            this._movieClipsModifier.Add(new MovieClipModifier(this));
                        }
                        break;

                    case 38:
                    case 39:
                    case 40:
                        MovieClipModifier mcmo = new MovieClipModifier(this);
                        mcmo.setLength(tag_size);
                        mcmo._offset = reader.BaseStream.Position;
                        mcmo.Read(this, tag);

                        this._movieClipsModifier[movieClipModifierIndex++] = mcmo;
                        break;

                    case 42: //42
                        _transformStorageID += 1;

                        _transformStorageOffsets.Add(_transformStorageID, tag_offset);

                        ushort mCount = reader.ReadUInt16();
                        ushort cCount = reader.ReadUInt16();

                        _transformStorage.Add((new ScMatrixList(), new ScColorTransformList()));

                        _matrixCount += mCount;
                        _colorsCount += cCount;
                        break;

                    default:
                        Console.WriteLine($"LoadingSC: Unknown Tag {tag} with size {tag_size}");
                        reader.ReadBytes(Convert.ToInt32(tag_size));
                        break;
                }

                //if ((offset + tagSize + 5) != reader.BaseStream.Position)
                //{
                //    string lastsctypeid = $"ID: {lastSCId}";
                //    Console.WriteLine($"Started with offset {offset} trying to load data of size {tagSize} but current position is {reader.BaseStream.Position}.\n DataTag {tag}; Hex: {tag} {(lastSCType != ScObject.SCObjectType.None ? lastsctypeid : "")}");
                //}
            }
        }

        public bool LoadSC1()
        {
            var sw = Stopwatch.StartNew();

            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            _shapeCount = reader.ReadUInt16();
            _movieClipCount = reader.ReadUInt16();
            _textureCount = reader.ReadUInt16();
            _textFieldCount = reader.ReadUInt16();
            _matrixCount = reader.ReadUInt16();
            _colorsCount = reader.ReadUInt16();

            _transformStorageID = 0;
            _transformStorage = [(new ScMatrixList(), new ScColorTransformList())];
            _transformStorageOffsets = new Dictionary<int, long>();

            for (int i = 0; i < _shapeCount; i++)
            {
                _shapes.Add(null);
            }

            for (int i = 0; i < _movieClipCount; i++)
            {
                _movieClips.Add(null);
            }

            for (int i = 0; i < _textFieldCount; i++)
            {
                _textFields.Add(null);
            }

            // 5 useless bytes
            reader.BaseStream.Seek(5, SeekOrigin.Current);

            _exportStartOffset = reader.BaseStream.Position;
            _exportCount = reader.ReadUInt16();

#if DEBUG
            Console.WriteLine(@"ExportCount: " + _exportCount);
            Console.WriteLine(@"ShapeCount: " + _shapeCount);
            Console.WriteLine(@"MovieClipCount: " + _movieClipCount);
            Console.WriteLine(@"TextureCount: " + _textureCount);
            Console.WriteLine(@"TextFieldCount: " + _textFieldCount);
            Console.WriteLine(@"Matrix2x3Count: " + _matrixCount);
            Console.WriteLine(@"ColorTransformationCount: " + _colorsCount);
#endif

            // Reads the Export IDs.
            for (int i = 0; i < _exportCount; i++)
            {
                var export = new Export(this);
                export.SetId(reader.ReadUInt16());
                _exports.Add(export);
            }

            // Reads the Export names.
            for (int i = 0; i < _exportCount; i++)
            {
                var nameLength = reader.ReadByte();
                if (nameLength != 255)
                {
                    var name = Encoding.UTF8.GetString(reader.ReadBytes(nameLength));
                    var export = (Export)_exports[i];
                    export.SetExportName(name);
                }
            }

            return LoadTags();
        }

        private void PostprocessObjects()
        {
            List<(string, int)> childrenNamesPresent = new List<(string, int)>();
            foreach (MovieClip mv in this._movieClips)
            {
                /**
                foreach (string cName in mv.timelineChildrenNames)
                {
                    if (!string.IsNullOrEmpty(cName))
                    {
                        int idxName = childrenNamesPresent.FindIndex(n => n.Item1 == cName);
                        if (idxName != -1)
                        {
                            childrenNamesPresent[idxName] = (cName, childrenNamesPresent[idxName].Item2 + 1);
                        }
                        else
                        {
                            childrenNamesPresent.Add((cName, 1));
                        }
                    }
                }
                **/

                foreach (ushort tcId in mv.timelineChildrenId)
                {
                    /**if (mv.getChildrens().FindIndex(obj => obj.Id == tcId) != -1)
                        continue;**/

                    int findIndex = this.GetShapes().FindIndex(shape => shape.Id == tcId);
                    if (findIndex != -1)
                    {
                        mv.addChildren(this.GetShapes()[findIndex]);
                    }
                    else
                    {
                        findIndex = this.GetMovieClips().FindIndex(mv => mv.Id == tcId);
                        if (findIndex != -1)
                        {
                            mv.addChildren(this.GetMovieClips()[findIndex]);
                        }
                        else
                        {
                            findIndex = this.getTextFields().FindIndex(tf => tf.Id == tcId);
                            if (findIndex != -1)
                            {
                                mv.addChildren(this.getTextFields()[findIndex]);
                            }
                            // else
                            // {
                            //     string err = $"MovieClip with ID {mv.Id} has children id {tcId} of invalid type.";
                            //     MessageBox.Show(err, "Invalid Children Type");
                            //     Console.WriteLine(err);
                            // }
                        }
                    }
                }

                foreach (Export ex in _exports)
                {
                    if (mv.Id == ex.Id)
                    {
                        ex.SetDataObject(mv);
                    }
                }
            }

            for (int i = 0; i < childrenNamesPresent.Count; i++)
            {
                Console.WriteLine($"{childrenNamesPresent[i].Item1} childrenName with count {childrenNamesPresent[i].Item2}");
            }
        }

        private static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue> (Dictionary<TKey, TValue> original) where TValue : ScMatrixList
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)((Matrix[])entry.Value.ToArray().Clone()).ToList());
            }
            return ret;
        }

        private static Dictionary<TKey, TValue> CloneDictionaryCloningValuesColor<TKey, TValue>(Dictionary<TKey, TValue> original) where TValue : ScColorTransformList
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)((ScColorTransform[])entry.Value.ToArray().Clone()).ToList());
            }
            return ret;
        }

        public void reloadFile()
        {
            ScObject[] previousPendingChanges = (ScObject[])_pendingChanges.ToArray().Clone();
            ScObject[] previousTextureData = (ScObject[])_textures.ToArray().Clone();

            Dictionary<int, ScMatrixList> previousPendingMatrix = CloneDictionaryCloningValues(_pendingMatrixs);
            Dictionary<int, ScColorTransformList> previousPendingColor = CloneDictionaryCloningValuesColor(_pendingColors);

            _shapes = new List<ScObject>();
            _exports = new List<ScObject>();
            _movieClips = new List<ScObject>();
            _textFields = new List<ScObject>();
            _movieClipsModifier = new List<ScObject>();
            _pendingChanges = new List<ScObject>();
            _pendingMatrixs = new Dictionary<int, ScMatrixList>();
            _pendingColors = new Dictionary<int, ScColorTransformList>();
            _exportCount = 0;
            _shapeCount = 0;
            _movieClipCount = 0;
            _textFieldCount = 0;
            _matrixCount = 0;
            _colorsCount = 0;
            _eofOffset = 0;
            _exportStartOffset = 0;
            _sofTagsOffset = 0;
            _eofTextFieldOffset = 0;

            LoadFile(_Filepath);

            this._textures = new List<ScObject>(previousTextureData.ToList());

            foreach (ScObject pendingData in previousPendingChanges)
            {
                int pIndex = -1;
                int type = pendingData._offset < 0 ? -1 : 1;

                switch (pendingData.GetDataType())
                {
                    case 1:
                        pIndex = _movieClips.FindIndex(mv => mv.Id == pendingData.Id);
                        if (pIndex != -1)
                        {
                            long newOffset = _movieClips[pIndex]._offset;
                            pendingData._offset = newOffset * type;
                        }
                        
                        if (pendingData.customAdded == true)
                        {
                            pendingData._offset = this._eofMovieClipOffset;
                        }
                        break;

                    case 7:
                        pIndex = _exports.FindIndex(mv => mv.Id == pendingData.Id);
                        if (pIndex != -1)
                        {
                            long newOffset = _exports[pIndex]._offset;
                            pendingData._offset = newOffset * type;
                        }
                        break;

                    case 0:
                        pIndex = _shapes.FindIndex(mv => mv.Id == pendingData.Id);
                        if (pIndex != -1)
                        {
                            long newOffset = _shapes[pIndex]._offset;
                            pendingData._offset = newOffset * type;
                        }

                        if (pendingData.customAdded == true)
                        {
                            pendingData._offset = this._eofShapeOffset;
                        }
                        break;
                } 

                _pendingChanges.Add(pendingData);
            }

            foreach (var mdata in previousPendingMatrix)
            {
                _pendingMatrixs.Add(mdata.Key, mdata.Value);
            }

            foreach (var cdata in previousPendingColor)
            {
                _pendingColors.Add(cdata.Key, cdata.Value);
            }
        }

        public int exportExists(string exportName)
        {
            int value = this._exports.FindIndex(exp => exp.GetName() == exportName);
            return value;
        }

        public void addFontName(string name)
        {
            _fontNames.Add(name);
        }

        public List<string> getFontNames()
        {
            return _fontNames;
        }

        public int shapeExists(int id)
        {
            int value = this._shapes.FindIndex(shp => shp.Id == id);
            return value;
        }

        public List<ScObject> getTextFields()
        {
            return _textFields;
        }

        public void addTextField(ScObject data)
        {
            _textFields.Add(data);
        }

        public void addColor(ScColorTransform color, int TransformStorageID)
        {
            if (_transformStorage[TransformStorageID].Item2.Count >= ushort.MaxValue)
            {
                MessageBox.Show($"StorageBox {TransformStorageID} colors has already exceeded max array size. Skipping.");
                return;
            }

            _transformStorage[TransformStorageID].Item2.Add(color);
        }

        public void addRenderingItem(ScObject item)
        {
            _currentRenderingMovieClips.Add(item);
        }

        public List<ScObject> CurrentRenderingMovieClips => _currentRenderingMovieClips;

        public void removeRenderingItem(int index)
        {
            _currentRenderingMovieClips.RemoveAt(index);
        }

        public void setRenderingItems(List<ScObject> data)
        {
            _currentRenderingMovieClips = data;
        }

        public void addPendingColor(ScColorTransform color, int TransformStorageID)
        {
            if (_pendingColors.ContainsKey(TransformStorageID))
            {
                _pendingColors[TransformStorageID].Add(color);
            }
            else 
            {
                _pendingColors.Add(TransformStorageID, new ScColorTransformList() { color });
            } 
        }

        public ushort getMaxId()
        {
            ushort maxId = 0;

            foreach (Export ex in _exports)
            {
                if (ex.Id > maxId)
                {
                    maxId = ex.Id;
                }
            }

            foreach (Shape s in _shapes)
            {
                if (s.Id > maxId)
                {
                    maxId = s.Id;
                }
            }

            foreach (MovieClip mv in _movieClips)
            {
                if (mv.Id > maxId)
                {
                    maxId = mv.Id;
                }
            }

            foreach (TextField tx in _textFields)
            {
                if (tx.Id > maxId)
                {
                    maxId = tx.Id;
                }
            }

            foreach (ScObject obj in _pendingChanges)
            {
                if (obj.Id > maxId)
                {
                    maxId = obj.Id;
                }
            }

            ushort incVal = 1;
            while (idExists((ushort)(maxId + incVal)))
            {
                incVal = (ushort)(incVal * 2);
            }

            return (ushort)(maxId + incVal);
        }

        private bool idExists(ushort id)
        {
            foreach (Export ex in _exports)
            {
                if (ex.Id == id)
                {
                    return true;
                }
            }

            foreach (Shape s in _shapes)
            {
                if (s.Id == id)
                {
                    return true;
                }
            }

            foreach (MovieClip mv in _movieClips)
            {
                if (mv.Id == id)
                {
                    return true;
                }
            }

            foreach (TextField tx in _textFields)
            {
                if (tx.Id == id)
                {
                    return true;
                }
            }

            foreach (ScObject obj in _pendingChanges)
            {
                if (obj.Id == id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

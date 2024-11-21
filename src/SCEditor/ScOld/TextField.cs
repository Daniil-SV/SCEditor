using SCEditor.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCEditor.ScOld
{
    class TextField : ScObject
    {
        public string _fontName;
        private ushort _leftCorner; // textFieldBounds 1 > float
        private ushort _topCorner; // textFieldBounds 2 > float
        private ushort _rightCorner; // textFieldBounds 3 > float
        private ushort _bottomCorner; // textFieldBounds 4 > float
        public Color _fontColor; // numbertext > color transform related
        public Color _fontOutlineColor; // numberValue
        public string _textData; // stringObject - text
        private byte _flag;
        private byte _fontWidth; // align > matrix related
        public byte _fontSize; // characterScale - fontSize  > color transform related
        private ushort _transform1;
        private ushort _unk46;
        private ScFile _scFile;
        private bool _wideCodes;
        private bool _modifier5;
        private bool _italic;
        private bool _ansi;
        private bool _shiftJIS; // multiline
        private bool _flag64;
        private bool _modifier4;
        private ushort _transform2;
        private ushort _unk46tmp;
        // 124/248 interastiveResursive bool
        // 228 and 232 highlightRange
        // this + 84 == matrix

        public override SCObjectType objectType => SCObjectType.TextField;
        public string fontName => _fontName;

        public TextField(ScFile scFile)
        {
            _scFile = scFile;
        }

        public TextField(ScFile scFile, TextField data, ushort id)
        {
            Id = id;
            _scFile = scFile;
            _fontName = data._fontName;
            _leftCorner = data._leftCorner;
            _topCorner = data._topCorner;
            _rightCorner = data._rightCorner;
            _bottomCorner = data._bottomCorner;
            _fontColor = data._fontColor;
            _fontOutlineColor = data._fontOutlineColor;
            _textData = data._textData;
            _flag = data._flag;
            _fontWidth = data._fontWidth;
            _fontSize = data._fontSize;
            _transform1 = data._transform1;
            _unk46 = data._unk46;
            _scFile = scFile;
            _wideCodes = data._wideCodes;
            _modifier5 = data._modifier5;
            _italic = data._italic;
            _ansi = data._ansi;
            _shiftJIS = data._shiftJIS;
            _flag64 = data._flag64;
            _modifier4 = data._modifier4;
            _transform2 = data._transform2;
            _unk46tmp = data._unk46tmp;
        }

        public override int GetDataType()
        {
            return 5;
        }

        public override string GetDataTypeName()
        {
            return "TextFields";
        }

        public override void Read(ScFile swf, byte tag)
        {
            Id = swf.reader.ReadUInt16();

            byte stringLength = swf.reader.ReadByte();
            if (stringLength < 255)
            {
                _fontName = Encoding.ASCII.GetString(swf.reader.ReadBytes(stringLength));
            }

            if (!string.IsNullOrEmpty(_fontName))
            {
                if (_scFile.getFontNames().FindIndex(n => n == _fontName) == -1)
                    _scFile.addFontName(_fontName);
            }

            _fontColor = swf.reader.ReadColor();

            if (swf.reader.ReadBoolean())
            {
                _flag |= 4; // 153
                _italic = true;
            } 
            if (swf.reader.ReadBoolean())
            {
                _flag |= 8; // 154
                _ansi = true;
            } 
            if (swf.reader.ReadBoolean())
            {
                _flag |= 16; // 155
                _shiftJIS = true;
            }

            _modifier4 = swf.reader.ReadBoolean(); // 156
            _fontWidth = swf.reader.ReadByte(); // 168 42 > related to matrix
            _fontSize = swf.reader.ReadByte(); // 172 43
            _leftCorner = swf.reader.ReadUInt16(); // 176 44 
            _topCorner = swf.reader.ReadUInt16(); // 180 45
            _rightCorner = swf.reader.ReadUInt16(); // 184 46
            _bottomCorner = swf.reader.ReadUInt16(); // 188 47

            if (swf.reader.ReadBoolean())
            {
                _flag |= 2; // 152
                _modifier5 = true;
            }
                
            byte stringLength2 = swf.reader.ReadByte();
            if (stringLength2 < 255)
            {
                _textData = Encoding.ASCII.GetString(swf.reader.ReadBytes(stringLength2)); // 196
            }

            if (tag == 7)
                return;

            if (swf.reader.ReadBoolean())
            {
                _flag |= 1;
                _wideCodes = true;
            }
                
            switch (tag)
            {
                case 20:
                    _flag |= 32;
                    break;
                case 21:
                    _flag |= 32;
                    _fontOutlineColor = swf.reader.ReadColor();
                    break;
                case 25:
                    _fontOutlineColor = swf.reader.ReadColor();
                    break;

                case 33:
                case 43:
                case 44:
                    _fontOutlineColor = swf.reader.ReadColor();
                    _transform1 = swf.reader.ReadUInt16();
                    _transform2 = swf.reader.ReadUInt16();
                    _flag |= 32;

                    if (tag == 43 || tag == 44)
                    {
                        _unk46tmp = swf.reader.ReadUInt16();
                        _unk46 = (ushort)(((uint)(0x7FFF * _unk46tmp + ((-40656255836343L * _unk46tmp) >> 32)) >> 31) + ((uint)(0x7FFF * _unk46tmp + ((-40656255836343L * _unk46tmp) >> 32)) >> 8));
                    }

                    if (tag == 44 && swf.reader.ReadBoolean())
                    {
                        _flag |= 64;
                        _flag64 = true;
                    }    

                    break;
            }
        }

        public override void Write(FileStream input)
        {
            if (customAdded)
            {
                input.Seek(_scFile.GetEofOffset(), SeekOrigin.Begin);
                byte tag = GetTag();

                // DataType and Length
                input.WriteByte(tag);
                input.Write(BitConverter.GetBytes(0), 0, 4);

                int dataLength = 0;

                // ID
                input.Write(BitConverter.GetBytes(Id), 0, 2);
                dataLength += 2;

                // Font Name
                if (string.IsNullOrEmpty(_fontName))
                {
                    input.WriteByte((byte)0xFF);
                }
                else
                {
                    byte[] stringData = Encoding.ASCII.GetBytes(_fontName);
                    input.WriteByte((byte)stringData.Length);
                    input.Write(stringData, 0, stringData.Length);

                    dataLength += stringData.Length;
                }
                dataLength += 1;

                // REST OF DATA
                input.WriteColor(_fontColor);
                dataLength += 4;

                input.Write(BitConverter.GetBytes(_italic), 0, 1);
                input.Write(BitConverter.GetBytes(_ansi), 0, 1);
                input.Write(BitConverter.GetBytes(_shiftJIS), 0, 1);
                dataLength += 3;

                input.Write(BitConverter.GetBytes(_modifier4), 0, 1);
                input.WriteByte(_fontWidth);
                input.WriteByte(_fontSize);
                input.Write(BitConverter.GetBytes(_leftCorner), 0, 2);
                input.Write(BitConverter.GetBytes(_topCorner), 0, 2);
                input.Write(BitConverter.GetBytes(_rightCorner), 0, 2);
                input.Write(BitConverter.GetBytes(_bottomCorner), 0, 2);
                dataLength += 11;

                input.Write(BitConverter.GetBytes(_modifier5), 0, 1);
                dataLength += 1;

                // unk32
                if (string.IsNullOrEmpty(_textData))
                {
                    input.WriteByte((byte)0xFF);
                }
                else
                {
                    byte[] stringData = Encoding.ASCII.GetBytes(_textData);
                    input.WriteByte((byte)stringData.Length);
                    input.Write(stringData, 0, stringData.Length);

                    dataLength += stringData.Length;
                }
                dataLength += 1;

                if (tag != 7)
                {
                    input.Write(BitConverter.GetBytes(_wideCodes), 0, 1);
                    dataLength += 1;

                    switch(tag)
                    {
                        case 20:
                            // flag32
                            break;

                        case 21:
                        case 25:
                            input.WriteColor(_fontOutlineColor);
                            dataLength += 4;
                            break;

                        case 33:
                        case 43:
                        case 44:
                            input.WriteColor(_fontOutlineColor);
                            input.Write(BitConverter.GetBytes(_transform1), 0, 2);
                            input.Write(BitConverter.GetBytes(_transform2), 0, 2);
                            dataLength += 8;

                            if (tag == 43 || tag == 44)
                            {
                                input.Write(BitConverter.GetBytes(_unk46tmp), 0, 2);
                                dataLength += 2;
                            }

                            if (tag == 44)
                            {
                                input.Write(BitConverter.GetBytes(_flag64), 0, 1);
                                dataLength += 1;
                            }
                            break;

                    }
                }

                input.Seek(-(dataLength + 4), SeekOrigin.Current);
                input.Write(BitConverter.GetBytes(dataLength), 0, 4);
                input.Seek(dataLength, SeekOrigin.Current);
            }
        }

        public override Bitmap Render(RenderingOptions options)
        {
            // Matrix matrixInput = new Matrix(); // multiply = this + 20, input matrix a2
            // Color colorTransform = new Color(); // multipy = _unk16, colortransform, _unk28 | this + 27 | input int a4

            // (this + 40) = (this + 40) + input float a5 % 1.0

            // shape = testfield.shapeRender(this, matrixInput, _unk24, colorTransform)

            // if shape == 1 > this + 20 = colorTransform & 0xFFFFFF00

            return shapeRender(); // shape
        }

        private Bitmap shapeRender()
        {
            Bitmap finalShape = new Bitmap((this._fontSize + 100), (this._fontSize + 100));

            using (Graphics g = Graphics.FromImage(finalShape))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Far;

                InstalledFontCollection fonts = new InstalledFontCollection();
                FontFamily textFontFamily = fonts.Families.Where(f => f.Name == this._fontName).FirstOrDefault();
                if (textFontFamily == null)
                {
                    MessageBox.Show($"{this.Id} textfield font {this._fontName} not installed");
                    textFontFamily = SystemFonts.DefaultFont.FontFamily;
                }

                var p = new Pen(this._fontOutlineColor, 0);
                p.LineJoin = LineJoin.Round;
                if (this._fontOutlineColor != (new Color()))
                {
                    p.Width = 5;
                }

                string textRender = (string.IsNullOrEmpty(this._textData) == true ? "ABC1" : this._textData);

                GraphicsPath gp = new GraphicsPath();
                Rectangle r = new Rectangle(0, 0, finalShape.Width, finalShape.Height);
                gp.AddString(textRender, textFontFamily, (int)FontStyle.Regular, this._fontSize, r, sf);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawPath(p, gp);
                g.DrawString(textRender, (new Font(textFontFamily, this._fontSize, FontStyle.Regular, GraphicsUnit.Pixel)), (new SolidBrush(this._fontColor)), r, sf);

                gp.Dispose();
                g.Flush();
            }

            return finalShape;

            //return null;

            //Bitmap image = new Bitmap(120, 120);
            //RectangleF rectf = new RectangleF(0, 0, image.Width, image.Height);

            //Graphics g = Graphics.FromImage(image);
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            //StringFormat format = new StringFormat()
            //{
            //    Alignment = StringAlignment.Center,
            //    LineAlignment = StringAlignment.Center
            //};

            //g.DrawString("yourText", new Font("Tahoma", 8), Brushes.Black, rectf, format);
            //g.Flush();

            //return image;
        }

        public void setId(ushort id)
        {
            Id = id;
        }

        public byte GetTag()
        {
            return 0;
        }
    }
}

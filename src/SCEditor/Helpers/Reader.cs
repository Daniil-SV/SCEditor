using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCEditor.Helpers
{

    // https://stackoverflow.com/a/15274591
    public static class ReadHelper
    {
        public static byte[] Reverse(this byte[] b)
        {
            Array.Reverse(b);
            return b;
        }

        public static UInt16 ReadUInt16BE(this BinaryReader binRdr)
        {
            return BitConverter.ToUInt16(binRdr.ReadBytesRequired(sizeof(UInt16)).Reverse(), 0);
        }

        public static Int16 ReadInt16BE(this BinaryReader binRdr)
        {
            return BitConverter.ToInt16(binRdr.ReadBytesRequired(sizeof(Int16)).Reverse(), 0);
        }

        public static UInt32 ReadUInt32BE(this BinaryReader binRdr)
        {
            return BitConverter.ToUInt32(binRdr.ReadBytesRequired(sizeof(UInt32)).Reverse(), 0);
        }

        public static Int32 ReadInt32BE(this BinaryReader binRdr)
        {
            return BitConverter.ToInt32(binRdr.ReadBytesRequired(sizeof(Int32)).Reverse(), 0);
        }

        public static byte[] ReadBytesRequired(this BinaryReader binRdr, int byteCount)
        {
            var result = binRdr.ReadBytes(byteCount);

            if (result.Length != byteCount)
                throw new EndOfStreamException(string.Format("{0} bytes required from stream, but only {1} returned.", byteCount, result.Length));

            return result;
        }

        public static string ReadStringSC(this BinaryReader reader)
        {
            byte length = reader.ReadByte();
            if (length != 0xFF)
            {
                return Encoding.ASCII.GetString(reader.ReadBytes(length));
            }

            return "";
        }

        public static Color ReadColor(this BinaryReader br)
        {
            byte cB = br.ReadByte();
            byte cG = br.ReadByte();
            byte cR = br.ReadByte();
            byte cA = br.ReadByte();
            return Color.FromArgb(cA, cR, cG, cB);
        }

        public static void WriteColor(this Stream input, Color c)
        {
            input.WriteByte(c.B);
            input.WriteByte(c.G);
            input.WriteByte(c.R);
            input.WriteByte(c.A);
        }
    }
}

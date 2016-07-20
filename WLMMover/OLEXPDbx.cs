using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WLMMailRulesMover.OLEXPDbx {
    // http://oedbx.aroh.de/

    // @15:16 2012/12/13

    public class DbxFH {
        BinaryReader br;
        long off;

        public DbxFH(BinaryReader br, long off) {
            this.br = br;
            this.off = off;
        }

        public bool IsMessageDatabase { get { return SUt.ToUInt32(br, off + 4) == 0x6F74FDC5U; } }
        public bool IsFolderDatabase { get { return SUt.ToUInt32(br, off + 4) == 0x6F74FDC6U; } }

        public DbxTre RootNodeAll { get { return new DbxTre(br, SUt.ToInt32(br, off + 0xE4)); } }

        public override string ToString() {
            return "FileHeader: " + (IsMessageDatabase ? "CLSID_MessageDatabase" : (IsFolderDatabase ? "CLSID_FolderDatabase" : "?"));
        }
    }

    public class DbxTre {
        BinaryReader br;
        long off;

        public DbxTre(BinaryReader br, long off) {
            this.br = br;
            this.off = off;
        }

        public int ObjectMarker { get { return SUt.ToInt32(br, off + 0); } }

        public int PtrChildNode { get { return SUt.ToInt32(br, off + 8); } }
        public int PtrParentNode { get { return SUt.ToInt32(br, off + 12); } }
        public byte NodeId { get { return SUt.ToByte(br, off + 16); } }
        public byte CountEntries { get { return SUt.ToByte(br, off + 17); } }

        public int StoredValues { get { return SUt.ToInt32(br, off + 20); } }

        public DbxTreent[] Entries {
            get {
                int cx = CountEntries;
                DbxTreent[] al = new DbxTreent[cx];
                for (int x = 0; x < cx; x++) {
                    al[x] = new DbxTreent(br, off + 24 + 12 * x);
                }
                return al;
            }
        }

        public DbxTre ChildNode {
            get {
                int ptr = PtrChildNode;
                return (ptr != 0) ? new DbxTre(br, ptr) : null;
            }
        }
        public DbxTre ParentNode {
            get {
                int ptr = PtrParentNode;
                return (ptr != 0) ? new DbxTre(br, ptr) : null;
            }
        }

        public override string ToString() {
            return String.Format("Tree: ObjectMarker={2}, NodeId={0}, CountEntries={1}", NodeId, CountEntries, ObjectMarker);
        }
    }

    public class DbxTreent {
        BinaryReader br;
        long off;

        public DbxTreent(BinaryReader br, long off) {
            this.br = br;
            this.off = off;
        }

        public int Value { get { return SUt.ToInt32(br, off + 0); } }
        public int PtrChildNode { get { return SUt.ToInt32(br, off + 4); } }
        public int Stored { get { return SUt.ToInt32(br, off + 8); } }

        public DbxII IndexedInfo { get { return new DbxII(br, Value); } }

        public DbxTre ChildNode {
            get {
                int ptr = PtrChildNode;
                return (ptr != 0) ? new DbxTre(br, ptr) : null;
            }
        }

        public override string ToString() {
            return String.Format("FolderInfo: Value={0}, ChildNode={1}, Stored={2}", Value, PtrChildNode, Stored);
        }
    }

    public class DbxII { // indexed info
        BinaryReader br;
        long off;

        public DbxII(BinaryReader br, long off) {
            this.br = br;
            this.off = off;
        }

        public int ObjectMarker { get { return SUt.ToInt32(br, off + 0); } }
        public int Len4 { get { return SUt.ToInt32(br, off + 4); } }
        public ushort Len8 { get { return SUt.ToUInt16(br, off + 8); } }
        public byte CountEntries { get { return SUt.ToByte(br, off + 10); } }
        public byte CountChanges { get { return SUt.ToByte(br, off + 11); } }

        public DbxIF[] Fields {
            get {
                int cx = CountEntries;
                long dataoff = off + 12 + 4 * cx;
                DbxIF[] al = new DbxIF[cx];
                for (int x = 0; x < cx; x++) {
                    al[x] = new DbxIF(br, off + 12 + 4 * x, dataoff);
                }
                return al;
            }
        }

        public override string ToString() {
            return String.Format("IndexedInfo: ObjectMarker={0}, Len4={1}, CountEntries={2}", ObjectMarker, Len4, CountEntries);
        }
    }

    public class DbxIF {
        BinaryReader br;
        long off, dataoff;

        public DbxIF(BinaryReader br, long off, long dataoff) {
            this.br = br;
            this.off = off;
            this.dataoff = dataoff;
        }

        public byte Index { get { return (byte)(SUt.ToByte(br, off) & 0x7F); } }
        public bool Direct { get { return 0 != (SUt.ToByte(br, off) & 0x80); } }
        public int Value { get { return (int)(SUt.ToUInt32(br, off) >> 8); } }

        public String StringValue {
            get {
                if (Direct) return null;
                List<byte> al = new List<byte>();
                br.BaseStream.Position = dataoff + Value;
                byte b;
                while (0 != (b = br.ReadByte())) {
                    al.Add(b);
                }
                return Encoding.Default.GetString(al.ToArray());
            }
        }

        public DateTime? FileTimeValue {
            get {
                if (Direct) return null;
                br.BaseStream.Position = dataoff + Value;
                return DateTime.FromFileTime(br.ReadInt64());
            }
        }

        public Int32 Int32Value {
            get {
                if (Direct) return Value;
                return SUt.ToInt32(br, dataoff + Value);
            }
        }

        public override string ToString() {
            String keyName = "Unk";
            Object keyVal = "";
            if (Index == 2) { keyName = "time message created/send"; keyVal = FileTimeValue; }
            if (Index == 5) { keyName = "original subject"; keyVal = StringValue; }
            if (Index == 8) { keyName = "subject of the message"; keyVal = StringValue; }
            if (Index == 13) { keyName = "sender name"; keyVal = StringValue; }
            if (Index == 14) { keyName = "sender mail address"; keyVal = StringValue; }
            if (Index == 19) { keyName = "receiver name"; keyVal = StringValue; }
            if (Index == 20) { keyName = "receiver mail address"; keyVal = StringValue; }
            if (Index == 28) { keyName = "message text structure"; keyVal = Int32Value.ToString("X8"); }
            return String.Format("Field: Index={0}, Disp={4}, Direct={1}, Value={2}, Contents={3}", Index, Direct, Value, keyVal, keyName);
        }
    }

    class SUt {
        internal static int ToInt32(BinaryReader br, long p) {
            br.BaseStream.Position = p;
            return br.ReadInt32();
        }

        internal static uint ToUInt32(BinaryReader br, long p) {
            br.BaseStream.Position = p;
            return br.ReadUInt32();
        }

        internal static byte ToByte(BinaryReader br, long p) {
            br.BaseStream.Position = p;
            return br.ReadByte();
        }

        internal static ushort ToUInt16(BinaryReader br, long p) {
            br.BaseStream.Position = p;
            return br.ReadUInt16();
        }
    }
}

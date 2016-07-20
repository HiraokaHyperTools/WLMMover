using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WLMMover {
    public class UtExplodeDbx {
        // 多大な参考・感謝
        // http://www.geocities.co.jp/SiliconValley-Oakland/3664/

        public static void ExplodeDbx(String fpdbx, String dirTo) {
            using (FileStream fs = File.OpenRead(fpdbx)) {
                BinaryReader br = new BinaryReader(fs);

                fs.Position = 0;
                if (br.ReadUInt64() != 0x6F74FDC5FE12ADCF) throw new InvalidDataException();

                fs.Position = 0x1C;

                uint infoSize = br.ReadUInt32();
                fs.Position = 0x24BC + infoSize;

                fs.Position = (fs.Position + 3) & (~3);

                SortedDictionary<uint, uint> dictRead = new SortedDictionary<uint, uint>(); // cur -> next

                byte[] bin = new byte[4];
                byte[] body = new byte[512];
                int t = 1;
                Int64 nextpos = fs.Position;
                while (fs.Position + 16 < fs.Length) {
                    Int64 pos = fs.Position;
                    fs.Position = nextpos;
                    nextpos += 4;
                    uint marker = br.ReadUInt32();
                    if (pos != marker) continue;
                    uint bodySize = br.ReadUInt32();
                    uint usedSize = br.ReadUInt32();
                    uint nextAddr = br.ReadUInt32();
                    if (bodySize != 0x200) continue;
                    if (usedSize > bodySize) continue;

                    if (dictRead.ContainsKey(marker)) continue;
                    dictRead[marker] = nextAddr;

                    using (FileStream os = File.Create(Path.Combine(dirTo, String.Format("{0:000000}.eml", t)))) {
                        while (true) {
                            if (pos != marker) break;
                            if (bodySize != 0x200) break;
                            if (usedSize > bodySize) break;

                            dictRead[marker] = nextAddr;

                            fs.Read(body, 0, (int)bodySize);
                            os.Write(body, 0, (int)usedSize);

                            fs.Position = nextAddr;

                            pos = fs.Position;
                            marker = br.ReadUInt32();
                            bodySize = br.ReadUInt32();
                            usedSize = br.ReadUInt32();
                            nextAddr = br.ReadUInt32();
                        }

                        if (os.Length == 0) continue;
                    }
                    t++;
                }
            }
        }

        public static void ExplodeDbx2(String fpdbx, String dirTo) {
            using (FileStream fs = File.OpenRead(fpdbx)) {
                BinaryReader br = new BinaryReader(fs);

                fs.Position = 0;
                if (br.ReadUInt64() != 0x6F74FDC5FE12ADCF) throw new InvalidDataException();

                fs.Position = 0x1C;

                uint infoSize = br.ReadUInt32();
                fs.Position = 0x24BC + infoSize;

                fs.Position = (fs.Position + 3) & (~3);

                SortedDictionary<uint, uint> dictWalk = new SortedDictionary<uint, uint>(); // cur -> next
                SortedDictionary<uint, uint> dictRev = new SortedDictionary<uint, uint>(); // next -> prev

                byte[] bin = new byte[4];
                byte[] body = new byte[512];
                Int64 nextpos = fs.Position;
                Int64 maxpos = fs.Length;
                while (nextpos + 16 < maxpos) {
                    Int64 pos = nextpos;
                    fs.Position = nextpos;
                    nextpos += 4;
                    uint marker = br.ReadUInt32();
                    if (pos != marker) continue;
                    uint bodySize = br.ReadUInt32();
                    uint usedSize = br.ReadUInt32();
                    uint nextAddr = br.ReadUInt32();
                    if (bodySize != 0x200) continue;
                    if (usedSize > bodySize) continue;

                    //if (dictWalk.ContainsKey(marker)) continue;
                    dictWalk[marker] = nextAddr;
                    if (nextAddr != 0) {
                        //Debug.Assert(!dictRev.ContainsKey(nextAddr));
                        dictRev[nextAddr] = marker;
                    }

                    nextpos += 0x200 + 16 - 4;
                }

                int t = 1;
                foreach (KeyValuePair<uint, uint> kv1 in dictWalk) {
                    if (dictRev.ContainsKey(kv1.Key)) continue;

                    using (FileStream os = File.Create(Path.Combine(dirTo, String.Format("{0:000000}.eml", t)))) {
                        uint pos = kv1.Key;
                        while (true) {
                            fs.Position = pos;
                            uint marker = br.ReadUInt32();
                            //Debug.Assert(marker == pos);
                            br.ReadUInt32();
                            int usedSize = br.ReadInt32();
                            br.ReadUInt32();

                            if (fs.Read(body, 0, usedSize) != usedSize) break;
                            os.Write(body, 0, usedSize);

                            uint npos;
                            if (!dictWalk.TryGetValue(pos, out npos) || npos == 0) break;
                            pos = npos;
                        }
                        t++;
                    }
                }
            }
        }

        public class Stat3 {
            public int x;
            public int z, emailcnt;
        }

        public delegate Stream GetOutputStream();

        public static void ExplodeDbx3(String fpdbx, Action<Stat3> reporter, GetOutputStream accepter) {
            using (FileStream fs = File.OpenRead(fpdbx)) {
                BinaryReader br = new BinaryReader(fs);

                fs.Position = 0;
                if (br.ReadUInt64() != 0x6F74FDC5FE12ADCF) throw new InvalidDataException();

                fs.Position = 0x1C;

                uint infoSize = br.ReadUInt32();
                fs.Position = 0x24BC + infoSize;

                fs.Position = (fs.Position + 3) & (~3);

                SortedDictionary<uint, uint> dictWalk = new SortedDictionary<uint, uint>(); // cur -> next
                SortedDictionary<uint, uint> dictRev = new SortedDictionary<uint, uint>(); // next -> prev

                byte[] bin = new byte[4];
                byte[] body = new byte[512];
                Int64 nextpos = fs.Position;
                Int64 maxpos = fs.Length;
                Stat3 s3 = new Stat3();
                while (nextpos + 16 < maxpos) {
                    Int64 pos = nextpos;
                    fs.Position = nextpos;
                    nextpos += 4;
                    uint marker = br.ReadUInt32();
                    if (pos != marker) continue;
                    uint bodySize = br.ReadUInt32();
                    uint usedSize = br.ReadUInt32();
                    uint nextAddr = br.ReadUInt32();
                    if (bodySize != 0x200) continue;
                    if (usedSize > bodySize) continue;

                    dictWalk[marker] = nextAddr;
                    if (nextAddr != 0) {
                        dictRev[nextAddr] = marker;
                    }

                    nextpos += 0x200 + 16 - 4;

                    if (reporter != null) {
                        int newx = (int)(100 * nextpos / maxpos);
                        if (newx != s3.x) {
                            s3.x = newx;
                            reporter(s3);
                        }
                    }
                }

                s3.x = 100;

                int t = 0, ct = dictWalk.Count;
                foreach (KeyValuePair<uint, uint> kv1 in dictWalk) {
                    t++;
                    if (dictRev.ContainsKey(kv1.Key)) continue;

                    if (reporter != null) {
                        int newz = (int)(100 * t / ct);
                        if (newz != s3.z) {
                            s3.z = newz;
                            reporter(s3);
                        }
                    }

                    using (Stream os = accepter()) {
                        uint pos = kv1.Key;
                        while (true) {
                            fs.Position = pos;
                            uint marker = br.ReadUInt32();
                            //Debug.Assert(marker == pos);
                            br.ReadUInt32();
                            int usedSize = br.ReadInt32();
                            br.ReadUInt32();

                            if (fs.Read(body, 0, usedSize) != usedSize) break;
                            os.Write(body, 0, usedSize);

                            uint npos;
                            if (!dictWalk.TryGetValue(pos, out npos) || npos == 0) break;
                            pos = npos;
                        }
                        s3.emailcnt++;
                    }
                }

                s3.z = 100;

                if (reporter != null) {
                    reporter(s3);
                }
            }
        }
    }

}

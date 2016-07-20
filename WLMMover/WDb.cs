using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Isam.Esent.Interop;
using System.IO;
using System.Diagnostics;

namespace WLMMover {
    public class WDb : IDisposable {
        class CCol {
            internal JET_COLUMNID FLDCOL_ID, FLDCOL_PARENT, FLDCOL_NAME, FLDCOL_FLAGS, FLDCOL_PATH, FLDCOL_TYPE;

            internal JET_COLUMNID Data, Table; // UserDataTable
        }

        CCol C = new CCol();

        public byte[] StoreIDW;

        public List<FolderW> ReadFolders() {
            List<FolderW> al = new List<FolderW>();


            using (Table UserDataTable = new Table(ses.JetSesid, dbid, "UserDataTable", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                C.Data = Api.GetTableColumnid(ses.JetSesid, UserDataTable.JetTableid, "Data");
                C.Table = Api.GetTableColumnid(ses.JetSesid, UserDataTable.JetTableid, "Table");

                Trace.Assert(Api.TryMoveFirst(ses.JetSesid, UserDataTable.JetTableid));
                for (int y = 0; ; y++) {
                    byte[] Data = Api.RetrieveColumn(ses.JetSesid, UserDataTable.JetTableid, C.Data);
                    String Table = Api.RetrieveColumnAsString(ses.JetSesid, UserDataTable.JetTableid, C.Table, Encoding.Unicode).TrimEnd('\0');
                    if (Table == "Folders") StoreIDW = Data;

                    if (Api.TryMoveNext(ses.JetSesid, UserDataTable.JetTableid))
                        continue;
                    break;
                }
            }
            using (Table Folders = new Table(ses.JetSesid, dbid, "Folders", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                C.FLDCOL_ID = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_ID");
                C.FLDCOL_PARENT = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_PARENT");
                C.FLDCOL_NAME = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_NAME");
                C.FLDCOL_FLAGS = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_FLAGS");
                C.FLDCOL_PATH = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_PATH");
                C.FLDCOL_TYPE = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_TYPE");

                Trace.Assert(Api.TryMoveFirst(ses.JetSesid, Folders.JetTableid));

                for (int y = 0; ; y++) {
                    FolderW w = new FolderW();
                    w.FLDCOL_ID = Api.RetrieveColumnAsInt64(ses.JetSesid, Folders.JetTableid, C.FLDCOL_ID).Value;
                    w.FLDCOL_PARENT = Api.RetrieveColumnAsInt64(ses.JetSesid, Folders.JetTableid, C.FLDCOL_PARENT).Value;
                    w.FLDCOL_NAME = Api.RetrieveColumnAsString(ses.JetSesid, Folders.JetTableid, C.FLDCOL_NAME, Encoding.GetEncoding(932)).TrimEnd('\0');
                    w.FLDCOL_FLAGS = Api.RetrieveColumnAsInt32(ses.JetSesid, Folders.JetTableid, C.FLDCOL_FLAGS).Value;
                    w.FLDCOL_PATH = Api.RetrieveColumnAsString(ses.JetSesid, Folders.JetTableid, C.FLDCOL_PATH, Encoding.Unicode);
                    if (w.FLDCOL_PATH != null) w.FLDCOL_PATH = w.FLDCOL_PATH.TrimEnd('\0');
                    w.FLDCOL_TYPE = Api.RetrieveColumnAsByte(ses.JetSesid, Folders.JetTableid, C.FLDCOL_TYPE).Value;
                    al.Add(w);

                    if (Api.TryMoveNext(ses.JetSesid, Folders.JetTableid))
                        continue;
                    break;
                }
            }

            return al;
        }

        class JLTable {
            public Table tableid;
            public Session ses;

            public JLTable(Session ses, Table t) {
                this.ses = ses;
                this.tableid = t;
            }
            public bool TryMoveFirst() {
                return Api.TryMoveFirst(ses.JetSesid, tableid);
            }
            public bool TryMoveNext() {
                return Api.TryMoveNext(ses.JetSesid, tableid);
            }
        }

        bool isReadOnly;
        Instance inst = null;
        Session ses = null;
        JET_DBID dbid = JET_DBID.Nil;
        String dirWlm;

        public void Open(String dirWlm, bool isReadOnly) {
            this.isReadOnly = isReadOnly;
            this.dirWlm = dirWlm;

            Dispose();

            String fp = Path.Combine(dirWlm, "Mail.MSMessageStore");

            inst = new Instance("WLMMover", "WLMMover");
            inst.Parameters.LogFileDirectory = dirWlm;
            inst.Parameters.TempDirectory = dirWlm;
            inst.Parameters.SystemDirectory = dirWlm;
            inst.Init();
            ses = new Session(inst);
            EUt.Check(Api.JetAttachDatabase(ses.JetSesid, fp, isReadOnly ? AttachDatabaseGrbit.ReadOnly : AttachDatabaseGrbit.None), "JetAttachDatabase");
            EUt.Check(Api.JetOpenDatabase(ses.JetSesid, fp, null, out dbid, isReadOnly ? OpenDatabaseGrbit.ReadOnly : OpenDatabaseGrbit.None), "JetOpenDatabase");
        }

        public bool IsOpened { get { return dbid != JET_DBID.Nil; } }

        public long MakeDir(String[] tree) {
            if (!IsOpened) throw new ApplicationException("!IsOpened");

            using (Table Folders = new Table(ses.JetSesid, dbid, "Folders", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                Api.JetSetCurrentIndex(ses.JetSesid, Folders.JetTableid, "1");
                Int64 p = -1;
                String FLDCOL_PATH = null;
                foreach (String key in tree) {
                    Api.MakeKey(ses.JetSesid, Folders.JetTableid, p, MakeKeyGrbit.NewKey);
                    Api.MakeKey(ses.JetSesid, Folders.JetTableid, key, Encoding.Unicode, MakeKeyGrbit.None);
                    if (Api.TrySeek(ses.JetSesid, Folders.JetTableid, SeekGrbit.SeekEQ)) {
                        FLDCOL_PATH = (Api.RetrieveColumnAsString(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_PATH"), Encoding.Unicode) ?? String.Empty).TrimEnd('\0');
                        p = Api.RetrieveColumnAsInt64(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_ID")).Value;
                    }
                    else {
                        if (p < 1) {
                            throw new ApplicationException(key + "が無く、続行できません。");
                        }
                        if (FLDCOL_PATH == null) {
                            throw new ApplicationException("FLDCOL_PATHが未設定です。");
                        }
                        String relnew = Path.Combine(FLDCOL_PATH, key), relnew2 = relnew;
                        for (int x = 1; ; x++) {
                            String dir = Path.Combine(dirWlm, relnew2);
                            if (Directory.Exists(dir)) {
                                relnew2 = relnew + "~" + x;
                            }
                            else {
                                Directory.CreateDirectory(dir);
                                break;
                            }
                        }
                        p = NewFolder(key, p, relnew2);
                    }
                }
                return p;
            }
        }

        public uint NewId(String table, Int64 folderParentId) {
            uint folderId;
            using (Table UserDataTable = new Table(ses.JetSesid, dbid, "UserDataTable", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                List<IndexInfo> ii = new List<IndexInfo>(Api.GetTableIndexes(ses.JetSesid, UserDataTable.JetTableid));
                Api.JetSetCurrentIndex(ses.JetSesid, UserDataTable.JetTableid, "Primary");
                Api.MakeKey(ses.JetSesid, UserDataTable.JetTableid, table, Encoding.Unicode, MakeKeyGrbit.NewKey);
                Api.MakeKey(ses.JetSesid, UserDataTable.JetTableid, folderParentId, MakeKeyGrbit.None);
                JET_wrn wrn;
                Trace.Assert((wrn = Api.JetSeek(ses.JetSesid, UserDataTable.JetTableid, SeekGrbit.SeekEQ)) == JET_wrn.Success);
                JET_COLUMNID jc = Api.GetTableColumnid(ses.JetSesid, UserDataTable.JetTableid, "Id");
                folderId = Api.RetrieveColumnAsUInt32(ses.JetSesid, UserDataTable.JetTableid, jc).Value;
                Api.JetPrepareUpdate(ses.JetSesid, UserDataTable.JetTableid, JET_prep.Replace);
                Api.SetColumn(ses.JetSesid, UserDataTable.JetTableid, jc, folderId + 1);
                Api.JetUpdate(ses.JetSesid, UserDataTable.JetTableid);
            }
            return folderId;
        }

        public Int64 NewFolder(String folderName, Int64 folderParent, String folderPath) {
            if (!IsOpened) throw new ApplicationException("!IsOpened");

            List<FolderW> al = new List<FolderW>();

            uint folderId;

            Api.JetBeginTransaction(ses.JetSesid);
            try {
                using (Table UserDataTable = new Table(ses.JetSesid, dbid, "UserDataTable", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                    List<IndexInfo> ii = new List<IndexInfo>(Api.GetTableIndexes(ses.JetSesid, UserDataTable.JetTableid));
                    Api.JetSetCurrentIndex(ses.JetSesid, UserDataTable.JetTableid, "Primary");
                    Api.MakeKey(ses.JetSesid, UserDataTable.JetTableid, "Folders", Encoding.Unicode, MakeKeyGrbit.NewKey);
                    Api.MakeKey(ses.JetSesid, UserDataTable.JetTableid, -1L, MakeKeyGrbit.None);
                    JET_wrn wrn;
                    Trace.Assert((wrn = Api.JetSeek(ses.JetSesid, UserDataTable.JetTableid, SeekGrbit.SeekEQ)) == JET_wrn.Success);
                    JET_COLUMNID jc = Api.GetTableColumnid(ses.JetSesid, UserDataTable.JetTableid, "Id");
                    folderId = Api.RetrieveColumnAsUInt32(ses.JetSesid, UserDataTable.JetTableid, jc).Value;
                    Api.JetPrepareUpdate(ses.JetSesid, UserDataTable.JetTableid, JET_prep.Replace);
                    Api.SetColumn(ses.JetSesid, UserDataTable.JetTableid, jc, folderId + 1);
                    Api.JetUpdate(ses.JetSesid, UserDataTable.JetTableid);
                }
                using (Table Folders = new Table(ses.JetSesid, dbid, "Folders", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                    Api.JetPrepareUpdate(ses.JetSesid, Folders.JetTableid, JET_prep.Insert);

                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_CLIENTHIGH"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_CLIENTLOW"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_COLOR"), (uint)0);


                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_FLAGS"), (uint)(0x40021));
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_HIERARCHY"), (byte)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_ID"), (UInt64)folderId);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_IGNORED"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_LISTSTAMP"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_MANAGED"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_MESSAGES"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_NAME"), Encoding.GetEncoding(932).GetBytes(folderName + "\0"));
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_NAMEHASH"), (uint)WLMHash.WNameHash.Compute(folderName));
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_NAMEW"), folderName + "\0", Encoding.Unicode);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_NOTDOWNLOADED"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_PARENT"), (UInt64)folderParent);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_PATH"), folderPath + "\0", Encoding.Unicode);


                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_SERVERCOUNT"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_SERVERHIGH"), (uint)0);

                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_SERVERLOW"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_SORTCOLUMN"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_SPECIAL"), (byte)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_STATUSMSGDELTA"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_STATUSUNREADDELTA"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_SUBSCRIBED"), (byte)1);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_THREADUNREAD"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_TYPE"), (byte)3);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_UNREAD"), (uint)0);

                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_VIEWUNREAD"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_WATCHED"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_WATCHEDHIGH"), (uint)0);
                    Api.SetColumn(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_WATCHEDUNREAD"), (uint)0);


                    Api.JetUpdate(ses.JetSesid, Folders.JetTableid);
                }

                Api.JetCommitTransaction(ses.JetSesid, CommitTransactionGrbit.None);
            }
            catch (Exception err) {
                Api.JetRollback(ses.JetSesid, RollbackTransactionGrbit.None);
                throw new WlmDbException(err);
            }

            return folderId;
        }

        class EUt {
            internal static void Check(JET_wrn wrn, String message) {
                if (wrn == JET_wrn.Success) return;
                throw new ApplicationException(message + " " + wrn);
            }
        }

        public class CORep {
            public int cFolders = 0;
            public int cMails = 0;
        }

        public CORep CollectOrphan(String dirWlm, Int64 newParent) {
            if (!IsOpened) throw new ApplicationException("!IsOpened");
            try {
                CORep res = new CORep();
                using (Table Folders = new Table(ses.JetSesid, dbid, "Folders", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                    JET_COLUMNID FLDCOL_ID = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_ID");
                    JET_COLUMNID FLDCOL_PARENT = Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_PARENT");

                    Api.JetSetCurrentIndex(ses.JetSesid, Folders.JetTableid, "0");

                    using (Table FoldersWalk = new Table(ses.JetSesid, dbid, "Folders", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                        if (Api.TryMoveFirst(ses.JetSesid, FoldersWalk.JetTableid)) {
                            do {
                                Int64? mParent = Api.RetrieveColumnAsInt64(ses.JetSesid, FoldersWalk.JetTableid, FLDCOL_PARENT);
                                bool fReloc = true;
                                if (mParent.HasValue) {
                                    if (mParent.Value < 1) {
                                        fReloc = false;
                                    }
                                    else {
                                        Api.MakeKey(ses.JetSesid, Folders.JetTableid, mParent.Value, MakeKeyGrbit.NewKey);
                                        if (Api.TrySeek(ses.JetSesid, Folders.JetTableid, SeekGrbit.SeekEQ)) {
                                            fReloc = false;
                                        }
                                    }
                                }
                                if (fReloc) {
                                    Api.JetBeginTransaction(ses.JetSesid);
                                    Api.JetPrepareUpdate(ses.JetSesid, FoldersWalk.JetTableid, JET_prep.Replace);
                                    Api.SetColumn(ses.JetSesid, FoldersWalk.JetTableid, FLDCOL_PARENT, newParent);
                                    Api.JetUpdate(ses.JetSesid, FoldersWalk.JetTableid);
                                    Api.JetCommitTransaction(ses.JetSesid, CommitTransactionGrbit.LazyFlush);
                                    res.cFolders++;
                                }
                            } while (Api.TryMoveNext(ses.JetSesid, FoldersWalk.JetTableid));
                        }
                    }
                    using (Table Messages = new Table(ses.JetSesid, dbid, "Messages", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                        JET_COLUMNID MSGCOL_FOLDERID = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_FOLDERID");
                        JET_COLUMNID MSGCOL_ID = Api.GetTableColumnid(ses.JetSesid, Messages.JetTableid, "MSGCOL_ID");
                        if (Api.TryMoveFirst(ses.JetSesid, Messages.JetTableid)) {
                            do {
                                Int64? mParent = Api.RetrieveColumnAsInt64(ses.JetSesid, Messages.JetTableid, MSGCOL_FOLDERID);
                                if (mParent.HasValue && mParent.Value > 0) {
                                    Api.MakeKey(ses.JetSesid, Folders.JetTableid, mParent.Value, MakeKeyGrbit.NewKey);
                                    if (!Api.TrySeek(ses.JetSesid, Folders.JetTableid, SeekGrbit.SeekEQ)) {
                                        Api.JetBeginTransaction(ses.JetSesid);
                                        Api.JetPrepareUpdate(ses.JetSesid, Messages.JetTableid, JET_prep.InsertCopyDeleteOriginal);
                                        Api.SetColumn(ses.JetSesid, Messages.JetTableid, MSGCOL_FOLDERID, newParent);
                                        Api.SetColumn(ses.JetSesid, Messages.JetTableid, MSGCOL_ID, (Int64)NewId("Messages", newParent));
                                        Api.JetUpdate(ses.JetSesid, Messages.JetTableid);
                                        Api.JetCommitTransaction(ses.JetSesid, CommitTransactionGrbit.LazyFlush);
                                        res.cMails++;
                                    }
                                }
                            } while (Api.TryMoveNext(ses.JetSesid, Messages.JetTableid));
                        }
                    }
                }
                return res;
            }
            catch (WlmDbException err) {
                throw new WlmDbException(err);
            }
        }

        #region IDisposable メンバ

        public void Dispose() {
            if (dbid != JET_DBID.Nil && ses != null) {
                Api.JetCloseDatabase(ses.JetSesid, dbid, CloseDatabaseGrbit.None);
                dbid = JET_DBID.Nil;
            }
            if (ses != null) {
                ses.Dispose();
                ses = null;
            }
            if (inst != null) {
                inst.Dispose();
                inst = null;
            }
        }

        #endregion

        public string GetFolderPath(long parent, bool fullpath) {
            using (Table Folders = new Table(ses.JetSesid, dbid, "Folders", isReadOnly ? OpenTableGrbit.ReadOnly : OpenTableGrbit.None)) {
                Api.JetSetCurrentIndex(ses.JetSesid, Folders.JetTableid, "0");
                String FLDCOL_PATH = null;
                Api.MakeKey(ses.JetSesid, Folders.JetTableid, parent, MakeKeyGrbit.NewKey);
                if (Api.TrySeek(ses.JetSesid, Folders.JetTableid, SeekGrbit.SeekEQ)) {
                    FLDCOL_PATH = (Api.RetrieveColumnAsString(ses.JetSesid, Folders.JetTableid, Api.GetTableColumnid(ses.JetSesid, Folders.JetTableid, "FLDCOL_PATH"), Encoding.Unicode) ?? String.Empty).TrimEnd('\0');
                    if (fullpath) FLDCOL_PATH = Path.Combine(dirWlm, FLDCOL_PATH);
                }
                return FLDCOL_PATH;
            }
        }
    }

    public class FolderW {
        public Int64 FLDCOL_ID, FLDCOL_PARENT;
        public int FLDCOL_FLAGS;
        public String FLDCOL_NAME, FLDCOL_PATH;
        public byte FLDCOL_TYPE;

        public override string ToString() {
            return FLDCOL_NAME;//FLDCOL_ID + " " + 
        }
    }

    public class WlmDbException : ApplicationException {
        public WlmDbException(Exception inner)
            : base("失敗しました", inner) {

        }
    }
}

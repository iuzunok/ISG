using ArgemUtil;
using Microsoft.Win32;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace Argem.DataServices
{
    public class DBUtil2 : IDisposable
    {
        #region Private Variables

        private static Dictionary<DataBaseTipi, string> saryConnectionString = new Dictionary<DataBaseTipi, string>();
        private static DataTable dtLoglanackTablolar = new DataTable();
        private static string ProjeRegistryAd = ConfigurationManager.AppSettings["ProjeRegistryAd"];

        private bool gLoglansinMi = false;
        private long gSure = -1;

        private string sConn = string.Empty;
        private List<NpgsqlParameter[]> arrLogSQL = null;
        private NpgsqlConnection cnNpgsql = null;
        private string sDataBaseTipi = string.Empty;
        private NpgsqlCommand cmd = null;

        StringBuilder gsbMSKolonAdlari = new StringBuilder();
        StringBuilder gsbMSInsertCommandText = new StringBuilder();
        StringBuilder gsbMSDeleteCommandText = new StringBuilder();

        #endregion

        #region constructor

        static DBUtil2()
        {
            Array enaryDataBaseTipi = System.Enum.GetValues(typeof(DataBaseTipi));

            if (saryConnectionString.Count == 0)
            {
                foreach (DataBaseTipi enDataBaseTipi in enaryDataBaseTipi)
                    saryConnectionString[enDataBaseTipi] = GetConnectionString(System.Enum.GetName(typeof(DataBaseTipi), enDataBaseTipi));
            }

            LoglanackTablolariTazele();
        }

        public DBUtil2(DataBaseTipi enDataBaseTipi)
        {
            sConn = saryConnectionString[enDataBaseTipi];
            sDataBaseTipi = enDataBaseTipi.ToString();
        }

        public string SQLQuery
        {
            get
            {
                string sDeger = "";
                string sQuery = cmd.CommandText;
                int i = 0;
                foreach (NpgsqlParameter oP in cmd.Parameters)
                {
                    if (oP.DbType == DbType.String)
                        sDeger = "'" + oP.Value.ToString() + "'";
                    else if (oP.DbType == DbType.Int32)
                        sDeger = oP.Value.ToString();
                    else if (oP.DbType == DbType.DateTime)
                    {
                        if (Convert.ToDateTime(oP.Value).Hour == 0)
                            sDeger = "to_date('" + Convert.ToDateTime(oP.Value).ToShortDateString() + "','dd.MM.yyyy')";
                        else
                            sDeger = "to_date('" + oP.Value.ToString() + "','dd.MM.yyyy HH:MI:SS')";
                    }
                    else if (oP.DbType == DbType.Decimal)
                        sDeger = oP.Value.ToString();

                    sQuery = sQuery.Replace(":" + i.ToString() + " ", sDeger + " ");
                    i++;
                }
                return sQuery;
            }
        }

        #endregion

        #region connection

        private void OpenConnection()
        {
            cnNpgsql = new NpgsqlConnection(sConn);
            cnNpgsql.Open();
        }

        private void CloseConnection()
        {
            if (cnNpgsql.State == ConnectionState.Open)
                cnNpgsql.Close();
        }

        #endregion

        #region EXEC SQL

        public object SorguCalistir(string sSQL)
        {
            return SorguCalistir(sSQL, false, 0);
        }

        public object SorguCalistir(string sSQL, bool LoglansinMi, decimal KullaniciKey)
        {
            Stopwatch oStopwatch = new Stopwatch();
            oStopwatch.Start();

            try
            {
                OpenConnection();
                cmd = new NpgsqlCommand(sSQL, cnNpgsql);

                if (LoglansinMi)
                {
                    object Sonuc = cmd.ExecuteNonQuery();
                    oStopwatch.Stop();
                    gSure = oStopwatch.ElapsedMilliseconds;

                    VeriKaydetLogYaz(sSQL, KullaniciKey);

                    return Sonuc;
                }
                else
                    return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                oStopwatch.Stop();
                VeriKaydetHataYaz(ex, sSQL);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public object SorguCalistir(ArgemSQL oSQL)
        {
            return SorguCalistir(oSQL, false, 0);
        }

        public object SorguCalistir(ArgemSQL oSQL, bool LoglansinMi, decimal KullaniciKey)
        {
            Stopwatch oStopwatch = new Stopwatch();
            oStopwatch.Start();

            cmd = new NpgsqlCommand();
            try
            {
                oSQL.CommandText = oSQL.CommandText + oSQL.SetText.ToString();
                NpgsqlCommandDoldur(ref cmd, ref oSQL);

                if (LoglansinMi)
                {
                    object Sonuc = cmd.ExecuteNonQuery();
                    oStopwatch.Stop();
                    gSure = oStopwatch.ElapsedMilliseconds;

                    VeriKaydetLogYaz(SQLQuery, KullaniciKey);

                    return Sonuc;
                }
                else
                {
                    oStopwatch.Stop();
                    // return cmd.ExecuteNonQuery();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                oStopwatch.Stop();
                VeriKaydetHataYaz(ex, oSQL.CommandText);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// NpgsqlCommand out parametre değeri şu şekilde alınabilir; cmd.Parameters[KolonAd].Value
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public void ProcedureCalistir(ref NpgsqlCommand cmd)
        {
            try
            {
                OpenConnection();
                cmd.Connection = cnNpgsql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                VeriKaydetHataYaz(ex, SQLQuery);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable ProcedureCalistir(NpgsqlCommand cmd)
        {
            try
            {
                OpenConnection();
                cmd.Connection = cnNpgsql;
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                VeriKaydetHataYaz(ex, cmd.CommandText);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion

        #region VERI GETIR

        public void DataGetir<T>(ref T ds, string sSQL) where T : DataSet
        {
            DataTable dt = ds.Tables[0];
            DataGetir(ref dt, sSQL, false, 0);
        }

        public void DataGetir(ref DataTable dt, string sSQL)
        {
            DataGetir(ref dt, sSQL, false, 0);
        }

        public void DataGetir(ref DataTable dt, string sSQL, bool bLoglansinMi, decimal KullaniciKey)
        {
            Stopwatch oStopwatch = new Stopwatch();
            oStopwatch.Start();

            try
            {
                OpenConnection();
                using (NpgsqlDataAdapter daORC = new NpgsqlDataAdapter(sSQL, cnNpgsql))
                {
                    daORC.Fill(dt);
                    oStopwatch.Stop();
                    gSure = oStopwatch.ElapsedMilliseconds;
                }

                if (bLoglansinMi)
                    VeriKaydetLogYaz(sSQL, KullaniciKey);
            }
            catch (Exception ex)
            {
                oStopwatch.Stop();
                VeriKaydetHataYaz(ex, sSQL);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public void DataGetir<T>(ref T ds, ArgemSQL oSQL) where T : DataSet
        {
            DataTable dt = ds.Tables[0];
            DataGetir(ref dt, oSQL, false, 0);
        }

        public void DataGetir<T>(ref T ds, ArgemSQL oSQL, bool bLoglansinMi, decimal KullaniciKey) where T : DataSet
        {
            DataTable dt = ds.Tables[0];
            DataGetir(ref dt, oSQL, bLoglansinMi, KullaniciKey);
        }

        public void DataGetir(ref DataTable dt, ArgemSQL oSQL)
        {
            DataGetir(ref dt, oSQL, false, 0);
        }

        public void DataGetir(ref DataTable dt, ArgemSQL oSQL, bool bLoglansinMi, decimal KullaniciKey)
        {
            Stopwatch oStopwatch = new Stopwatch();
            oStopwatch.Start();

            cmd = new NpgsqlCommand();
            try
            {
                NpgsqlCommandDoldur(ref cmd, ref oSQL);

                using (NpgsqlDataAdapter daORC = new NpgsqlDataAdapter(cmd))
                {
                    daORC.Fill(dt);
                    oStopwatch.Stop();
                    gSure = oStopwatch.ElapsedMilliseconds;
                }

                if (bLoglansinMi)
                    VeriKaydetLogYaz(SQLQuery, KullaniciKey);
            }
            catch (Exception ex)
            {
                oStopwatch.Stop();
                VeriKaydetHataYaz(ex, SQLQuery);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public object TekDegerOku(string sSQL)
        {
            return TekDegerOku(sSQL, false, 0);
        }

        /*public object TekDegerOku(string sSQL, bool LoglansinMi, decimal KullaniciKey)
        {
            return TekDegerOku(sSQL, LoglansinMi, KullaniciKey, 0);
        }*/

        public object TekDegerOku(string sSQL, bool LoglansinMi, decimal KullaniciKey)
        {
            Stopwatch oStopwatch = new Stopwatch();
            oStopwatch.Start();

            try
            {
                OpenConnection();
                cmd = new NpgsqlCommand(sSQL, cnNpgsql);

                if (LoglansinMi)
                {
                    object oSonuc = cmd.ExecuteScalar();
                    oStopwatch.Stop();
                    gSure = oStopwatch.ElapsedMilliseconds;

                    VeriKaydetLogYaz(SQLQuery, KullaniciKey);

                    return oSonuc;
                }
                else
                    return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                oStopwatch.Stop();
                VeriKaydetHataYaz(ex, SQLQuery);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public object TekDegerOku(ArgemSQL oSQL)
        {
            return TekDegerOku(oSQL, false, 0);
        }

        /*public object TekDegerOku(ArgemSQL oSQL, bool LoglansinMi, decimal KullaniciKey)
        {
            return TekDegerOku(oSQL, LoglansinMi, KullaniciKey, 0);
        }*/

        public object TekDegerOku(ArgemSQL oSQL, bool LoglansinMi, decimal KullaniciKey)
        {
            Stopwatch oStopwatch = new Stopwatch();
            oStopwatch.Start();

            cmd = new NpgsqlCommand();
            try
            {
                NpgsqlCommandDoldur(ref cmd, ref oSQL);

                if (LoglansinMi)
                {
                    object oSonuc = cmd.ExecuteScalar();
                    oStopwatch.Stop();
                    gSure = oStopwatch.ElapsedMilliseconds;

                    VeriKaydetLogYaz(SQLQuery, KullaniciKey);

                    return oSonuc;
                }
                else
                    return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                oStopwatch.Stop();
                VeriKaydetHataYaz(ex, SQLQuery);
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        private void NpgsqlCommandDoldur(ref NpgsqlCommand cmd, ref ArgemSQL oSQL)
        {
            cmd.CommandText = oSQL.CommandTextFull();
            cmd.CommandType = CommandType.Text;
            // cmd.BindByName = true;

            NpgsqlParameter P;
            foreach (SQLDetay oDetay in oSQL)
            {
                if (oDetay.KolonTipNo != KolonTipi.ArgSQL)
                {
                    // P = new NpgsqlParameter(oDetay.KolonAd, DBType(oDetay.KolonTipNo.GetHashCode()), ParameterDirection.Input);
                    P = new NpgsqlParameter(oDetay.KolonAd, DBType(oDetay.KolonTipNo.GetHashCode()));
                    P.Value = oDetay.Deger;

                    if (P.DbType == DbType.String)
                        P.Size = P.Value.ToString().Length;

                    cmd.Parameters.Add(P);
                }
            }

            cnNpgsql = new NpgsqlConnection(sConn);
            cnNpgsql.Open();
            cmd.Connection = cnNpgsql;
        }

        private NpgsqlDbType DBType(int KolonTipNo)
        {
            if (KolonTipNo == KolonTipi.Int.GetHashCode())
                return NpgsqlDbType.Integer;
            else if (KolonTipNo == KolonTipi.String.GetHashCode())
                return NpgsqlDbType.Varchar;
            else if (KolonTipNo == KolonTipi.Date.GetHashCode())
                return NpgsqlDbType.Timestamp;
            else if (KolonTipNo == KolonTipi.ByteArray.GetHashCode())
                return NpgsqlDbType.Bytea;
            else if (KolonTipNo == KolonTipi.Blob.GetHashCode())
                return NpgsqlDbType.Bytea;
            else
                return NpgsqlDbType.Numeric;
        }

        public static void LoglanackTablolariTazele()
        {
            dtLoglanackTablolar.Clear();

            using (NpgsqlConnection cn = new NpgsqlConnection(saryConnectionString[DataBaseTipi.Yetki]))
            {
                cn.Open();
                using (NpgsqlDataAdapter daORC = new NpgsqlDataAdapter("SELECT * from LOG_TABLO", cn))
                    daORC.Fill(dtLoglanackTablolar);
            }
        }

        #endregion

        #region VERI KAYDET

        public DataSet VeriKaydet(DataSet ds)
        {
            return VeriKaydet(ds, 0);
        }

        public DataSet VeriKaydet(DataSet ds, decimal UKullaniciKey)
        {
            return VeriKaydet(ds, LogAlinanTabloMu(ds.Tables[0].TableName), UKullaniciKey);
        }

        public DataSet VeriKaydet(DataSet ds, bool LoglansinMi, decimal UKullaniciKey)
        {
            return VeriKaydet(ds.Tables[0], LoglansinMi, UKullaniciKey);
        }

        private DataSet VeriKaydet(DataTable dt, bool LoglansinMi, decimal UKullaniciKey)
        {
            if (dt.PrimaryKey.Length == 0)
                throw new Exception(dt.TableName + " tablosuna ait bir \"PrimaryKey\" bulunmamaktadır.");

            // Eğer Değişiklik yapılan kayıtlar var ise kayıt işlemi yapılır.
            if (dt.GetChanges() != null)
            {
                gLoglansinMi = LoglansinMi;
                arrLogSQL = new List<NpgsqlParameter[]>();

                Stopwatch oStopwatch = new Stopwatch();
                oStopwatch.Start();

                try
                {
                    OpenConnection();
                    NpgsqlDataAdapter daORC = new NpgsqlDataAdapter("SELECT * FROM " + dt.TableName, cnNpgsql);
                    NpgsqlCommandBuilder cbNpgsql = new NpgsqlCommandBuilder(daORC);

                    daORC.InsertCommand = cbNpgsql.GetInsertCommand();
                    // daORC.InsertCommand.CommandText += " returning \"" + dt.PrimaryKey[0].ColumnName + "\" into :TabloID";
                    daORC.InsertCommand.CommandText += " returning \"" + dt.PrimaryKey[0].ColumnName + "\" ";
                    // daORC.InsertCommand.Parameters.Add(new NpgsqlParameter(":TabloID", NpgsqlDbType.Integer, ParameterDirection.Output));
                    daORC.InsertCommand.Parameters[0].Direction = ParameterDirection.Output;
                    daORC.InsertCommand.Parameters[0].SourceColumn = dt.PrimaryKey[0].ColumnName;
                    daORC.InsertCommand.UpdatedRowSource = UpdateRowSource.Both;

                    daORC.UpdateCommand = cbNpgsql.GetUpdateCommand();
                    daORC.DeleteCommand = cbNpgsql.GetDeleteCommand();

                    daORC.RowUpdated += new NpgsqlRowUpdatedEventHandler(RowUpdated);

                    daORC.Update(dt);
                    oStopwatch.Stop();
                    gSure = oStopwatch.ElapsedMilliseconds;

                    if (dt.TableName == "LOG_TABLO")
                        LoglanackTablolariTazele();

                    if (LoglansinMi)
                        VeriKaydetLogYaz(UKullaniciKey);
                }
                catch (System.Data.DBConcurrencyException ex)
                {
                    oStopwatch.Stop();
                    if (dt.Columns.Contains("UKullaniciKey"))
                        VeriKaydetHataYaz(ex, "Tablo:" + dt.TableName + " TK:" + dt.Rows[0][dt.PrimaryKey[0].ColumnName].ToString() + " KK:" + dt.Rows[0]["UKullaniciKey"].ToString());
                    else
                        VeriKaydetHataYaz(ex, "Tablo:" + dt.TableName + " Key:" + dt.Rows[0][dt.PrimaryKey[0].ColumnName].ToString());
                    throw ex;
                }
                catch (Exception ex)
                {
                    oStopwatch.Stop();
                    VeriKaydetHataYaz(ex, "Tablo:" + dt.TableName);
                    throw ex;
                }
                finally
                {
                    CloseConnection();
                }
            }
            return dt.DataSet;
        }

        public void VeriKaydetBatch(DataSet ds, int BatchSize, decimal UKullaniciKey)
        {
            VeriKaydetBatch(ds, LogAlinanTabloMu(ds.Tables[0].TableName), BatchSize, 0);
        }

        public void VeriKaydetBatch(DataSet ds, bool LoglansinMi, int BatchSize, decimal UKullaniciKey)
        {
            VeriKaydetBatch(ds.Tables[0], LoglansinMi, BatchSize, UKullaniciKey);
        }

        public void VeriKaydetBatch(DataTable dt, bool LoglansinMi, int BatchSize, decimal UKullaniciKey)
        {
            if (dt.PrimaryKey.Length == 0)
                throw new Exception(dt.TableName + " tablosuna ait bir \"PrimaryKey\" bulunmamaktadır.");

            // Eğer Değişiklik yapılan kayıtlar var ise kayıt işlemi yapılır.
            if (dt.GetChanges() != null)
            {
                Stopwatch oStopwatch = new Stopwatch();
                oStopwatch.Start();

                gLoglansinMi = LoglansinMi;
                arrLogSQL = new List<NpgsqlParameter[]>();

                using (NpgsqlConnection cnMSNpgsql = new NpgsqlConnection(sConn))
                {
                    cnMSNpgsql.Open();
                    using (NpgsqlDataAdapter daORC = new NpgsqlDataAdapter("SELECT * FROM " + dt.TableName, cnMSNpgsql))
                    {
                        using (NpgsqlCommandBuilder cbMSNpgsql = new NpgsqlCommandBuilder(daORC))
                        {
                            try
                            {
                                daORC.InsertCommand = cbMSNpgsql.GetInsertCommand();
                                daORC.InsertCommand.UpdatedRowSource = System.Data.UpdateRowSource.None;

                                string strKolonAdlari = "";
                                string strParametreler = "";

                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    strParametreler += ",:p" + (i + 1);
                                    strKolonAdlari += ",\"" + dt.Columns[i].ColumnName + "\"";
                                }

                                strParametreler = strParametreler.Remove(0, 1);
                                strKolonAdlari = strKolonAdlari.Remove(0, 1);
                                gsbMSInsertCommandText.Append("INSERT INTO " + dt.TableName + "(" + strKolonAdlari + ") VALUES (" + strParametreler + ")");

                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    if (i < dt.Columns.Count - 1)
                                        gsbMSKolonAdlari.Append(dt.Columns[i].ColumnName + ",");
                                    else
                                        gsbMSKolonAdlari.Append(dt.Columns[i].ColumnName);
                                }

                                daORC.DeleteCommand = cbMSNpgsql.GetDeleteCommand();
                                daORC.DeleteCommand.UpdatedRowSource = System.Data.UpdateRowSource.None;

                                gsbMSDeleteCommandText.Append("DELETE FROM " + dt.TableName + " WHERE (");

                                for (int i = 0; i < dt.Columns.Count; i++)
                                {
                                    if (i == 0)
                                        gsbMSDeleteCommandText.Append("(\"" + dt.Columns[i].ColumnName + "\"=:p" + (i + 1) + ")");
                                    else
                                        gsbMSDeleteCommandText.Append(" AND (\"" + dt.Columns[i].ColumnName + "\"=:p" + (i + 1) + ")");
                                }

                                gsbMSDeleteCommandText.Append(")");

                                daORC.UpdateBatchSize = BatchSize;
                                daORC.RowUpdating += new NpgsqlRowUpdatingEventHandler(RowUpdating);

                                daORC.Update(dt);
                                oStopwatch.Stop();
                                gSure = oStopwatch.ElapsedMilliseconds;

                                if (dt.TableName == "LOG_TABLO")
                                    LoglanackTablolariTazele();

                                if (LoglansinMi)
                                    VeriKaydetLogYaz(UKullaniciKey);
                            }
                            catch (Exception ex)
                            {
                                oStopwatch.Stop();
                                VeriKaydetHataYaz(ex, "Tablo:" + dt.TableName);
                                throw ex;
                            }
                        }
                    }
                }
            }
        }

        private void VeriKaydetHataYaz(Exception ex, string sSQL)
        {
            VeriKaydetHataYaz(HataTablosuInsertCumlesiOlustur(ex, sSQL));
        }

        public int VeriKaydetHataYaz(int KullaniciKey, string Kaynak, string Mesaj, string Form, string QueryString,
            string HedefSite, string StackTrace, string Referer)
        {
            NpgsqlParameter[] spc = new NpgsqlParameter[8];

            if (Kaynak.Length < 2000)
                spc[0] = new NpgsqlParameter(":Kaynak", Kaynak);
            else
                spc[0] = new NpgsqlParameter(":Kaynak", Kaynak.Substring(0, 1999));

            if (Kaynak.Length < 4000)
                spc[1] = new NpgsqlParameter(":Mesaj", Mesaj);
            else
                spc[1] = new NpgsqlParameter(":Mesaj", Mesaj.Substring(0, 3999));

            if (Form.Length < 4000)
                spc[2] = new NpgsqlParameter(":Form", Form);
            else
                spc[2] = new NpgsqlParameter(":Form", Form.Substring(0, 3999));

            if (QueryString.Length < 2000)
                spc[3] = new NpgsqlParameter(":QueryString", QueryString);
            else
                spc[3] = new NpgsqlParameter(":QueryString", QueryString.Substring(0, 1999));

            if (HedefSite.Length < 500)
                spc[4] = new NpgsqlParameter(":HedefSite", HedefSite);
            else
                spc[4] = new NpgsqlParameter(":HedefSite", HedefSite.Substring(0, 499));

            if (StackTrace.Length < 4000)
                spc[5] = new NpgsqlParameter(":StackTrace", StackTrace);
            else
                spc[5] = new NpgsqlParameter(":StackTrace", StackTrace.Substring(0, 3999));

            if (Referer.Length < 4000)
                spc[6] = new NpgsqlParameter(":Referer", Referer);
            else
                spc[6] = new NpgsqlParameter(":Referer", Referer.Substring(0, 3999));

            spc[7] = new NpgsqlParameter(":UKullaniciKey", KullaniciKey);

            return VeriKaydetHataYaz(spc);
        }

        private void RowUpdating(object sender, NpgsqlRowUpdatingEventArgs e)
        {
            if (e.Command != null)
            {
                if (e.StatementType == System.Data.StatementType.Insert)
                    e.Command.CommandText = gsbMSInsertCommandText.ToString();
                else if (e.StatementType == System.Data.StatementType.Update)
                {
                    foreach (string Kolon in gsbMSKolonAdlari.ToString().Split(','))
                        e.Command.CommandText = e.Command.CommandText.Replace(" " + Kolon + " ", " \"" + Kolon + "\" ").Replace("(" + Kolon + " ", "(\"" + Kolon + "\" ");

                    if (gLoglansinMi)
                        LogSQLOlustur(e.Row, "U");
                }
                else if (e.StatementType == System.Data.StatementType.Delete)
                    e.Command.CommandText = gsbMSDeleteCommandText.ToString();
            }
        }

        private void RowUpdated(object sender, NpgsqlRowUpdatedEventArgs e)
        {
            if (e.Errors == null)
            {
                if (e.StatementType == System.Data.StatementType.Insert)
                {
                    e.Row[0] = Convert.ToInt32(e.Command.Parameters[":TabloID"].ToString());

                    if (gLoglansinMi)
                        LogSQLOlustur(e.Row, "I");
                }
                else if (e.StatementType == System.Data.StatementType.Update)
                {
                    if (gLoglansinMi)
                        LogSQLOlustur(e.Row, "U");
                }
                else if (e.StatementType == System.Data.StatementType.Delete)
                {
                    if (gLoglansinMi)
                        LogSQLOlustur(e.Row, "D");
                }
                else if (e.StatementType == System.Data.StatementType.Batch)
                {
                    if (gLoglansinMi)
                    {
                        if (e.Row != null)
                            LogSQLOlustur(e.Row, "B");
                    }
                }
            }
        }

        /// <summary>
        /// Hatayı HATA tablosuna kaydeden şekilde PLSQL parametreleri Oluşturur.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private NpgsqlParameter[] HataTablosuInsertCumlesiOlustur(Exception ex, string sSQL)
        {
            NpgsqlParameter[] spc = new NpgsqlParameter[8];
            if (ex.Source.Length < 2000)
                spc[0] = new NpgsqlParameter(":Kaynak", ex.Source);
            else
                spc[0] = new NpgsqlParameter(":Kaynak", ex.Source.Substring(0, 1999));

            if (ex.Message.Length < 4000)
                spc[1] = new NpgsqlParameter(":Mesaj", ex.Message);
            else
                spc[1] = new NpgsqlParameter(":Mesaj", ex.Message.Substring(0, 3999));

            string From = Convert.ToString(ex.InnerException);
            if (From.Length > 4000)
                From = From.Substring(0, 3999);

            spc[2] = new NpgsqlParameter(":Form", From);

            spc[3] = new NpgsqlParameter(":QueryString", DBNull.Value);
            spc[4] = new NpgsqlParameter(":HedefSite", DBNull.Value);

            if (ex.StackTrace.Length < 4000)
                spc[5] = new NpgsqlParameter(":StackTrace", ex.StackTrace);
            else
                spc[5] = new NpgsqlParameter(":StackTrace", ex.StackTrace.Substring(0, 3999));

            if (sSQL.Length < 4000)
                spc[6] = new NpgsqlParameter(":Referer", sSQL);
            else
                spc[6] = new NpgsqlParameter(":Referer", sSQL.Substring(0, 3999));

            spc[7] = new NpgsqlParameter(":UKullaniciKey", "0");

            return spc;
        }

        #endregion

        #region LOGLAMA

        public void VeriKaydetLogYaz(string Veri, decimal KullaniciKey)
        {
            VeriKaydetLogYaz(Veri, 0, KullaniciKey);
        }

        public void VeriKaydetLogYaz(string Veri, decimal TabloKey, decimal KullaniciKey)
        {
            string VTAd = ConnectionDanVTAdBul();

            NpgsqlParameter[] oNpgsqlParameter = new NpgsqlParameter[7];
            oNpgsqlParameter[0] = new NpgsqlParameter(":TabloAd", "[SORGU]");
            oNpgsqlParameter[1] = new NpgsqlParameter(":UIDTipNo", "S");
            oNpgsqlParameter[2] = new NpgsqlParameter(":TabloKey", TabloKey.ToString());
            oNpgsqlParameter[3] = new NpgsqlParameter(":VeriTabaniAd", VTAd);
            oNpgsqlParameter[4] = new NpgsqlParameter(":Veri", Veri);
            oNpgsqlParameter[5] = new NpgsqlParameter(":Sure", gSure);
            oNpgsqlParameter[6] = new NpgsqlParameter(":UKullaniciKey", KullaniciKey.ToString());

            VeriKaydetLogYaz(oNpgsqlParameter, KullaniciKey);
        }

        private void VeriKaydetLogYaz(NpgsqlParameter[] opc, decimal KullaniciKey)
        {
            arrLogSQL = new List<NpgsqlParameter[]>();
            arrLogSQL.Add(opc);
            VeriKaydetLogYaz(KullaniciKey);
        }

        private void VeriKaydetLogYaz(decimal UKullaniciKey)
        {
            using (NpgsqlConnection cnnLog = new NpgsqlConnection(saryConnectionString[DataBaseTipi.Yetki]))
            {
                cnnLog.Open();
                using (NpgsqlCommand cmdLog = cnnLog.CreateCommand())
                {
                    cmdLog.CommandText =
                        "insert into LOG_ISLEM " +
                        "       (\"TabloAd\", \"UIDTipNo\", \"TabloKey\", \"VeriTabaniAd\", \"Veri\", \"Sure\", \"UKullaniciKey\", \"UTar\") " +
                        "values (:TabloAd, :UIDTipNo, :TabloKey, :VeriTabaniAd, :Veri, :Sure, :UKullaniciKey, current_timestamp)";

                    foreach (NpgsqlParameter[] opc in arrLogSQL)
                    {
                        cmdLog.Parameters.Clear();
                        foreach (NpgsqlParameter op in opc)
                        {
                            if (op.ParameterName == ":Sure")
                                op.Value = gSure;
                            else if (UKullaniciKey != 0 && op.ParameterName == ":UKullaniciKey")
                                op.Value = UKullaniciKey;
                            cmdLog.Parameters.Add(op);
                        }
                        cmdLog.ExecuteNonQuery();
                    }
                }
            }
        }

        private int VeriKaydetHataYaz(NpgsqlParameter[] spc)
        {
            using (NpgsqlConnection cnnHata = new NpgsqlConnection(saryConnectionString[DataBaseTipi.Yetki]))
            {
                cnnHata.Open();
                using (NpgsqlCommand cmdHata = cnnHata.CreateCommand())
                {
                    cmdHata.CommandText =
                        "insert into public.\"HATA\" " +
                        "       (\"Kaynak\", \"Mesaj\", \"Form\", \"QueryString\", \"HedefSite\", \"StackTrace\", \"Referer\", \"UKullaniciKey\", \"UTar\") " +
                        "values (:Kaynak, :Mesaj, :Form, :QueryString, :HedefSite, :StackTrace, :Referer, :UKullaniciKey, current_timestamp) " +
                        "returning \"HataKey\" ";

                    foreach (NpgsqlParameter op in spc)
                        cmdHata.Parameters.Add(op);
                    int HataKey = Convert.ToInt32(cmdHata.ExecuteScalar());
                    cnnHata.Close();
                    return HataKey;
                }
            }
        }

        private string ConnectionDanVTAdBul()
        {
            string VTAd = "";
            if (cnNpgsql != null && cnNpgsql.State == ConnectionState.Open)
            {
                VTAd = cnNpgsql.ConnectionString.ToUpper(new System.Globalization.CultureInfo("en-US", false));
                string[] aryVTAd = VTAd.Split(';');
                if (aryVTAd.Length > 0)
                {
                    foreach (string s in aryVTAd)
                    {
                        if (s.StartsWith("Database="))
                        {
                            VTAd = s.Replace("Database=", "");
                            VTAd = VTAd.Trim();
                            break;
                        }
                    }
                }

                if (VTAd.Length > 20)
                    VTAd = VTAd.Substring(0, 19);
            }
            return VTAd;
        }

        private void LogSQLOlustur(DataRow dr, string Islem)
        {
            if (arrLogSQL == null)
                arrLogSQL = new List<NpgsqlParameter[]>();

            string SonrakiOnEk = "";
            decimal UKullaniciKey = 0;
            decimal TabloKey = 0;
            string VTAd = ConnectionDanVTAdBul();
            StringBuilder sbVeri = new StringBuilder();

            if (Islem == "B") // batch
            {
                foreach (DataRow dtR in dr.Table.Rows)
                {
                    sbVeri = new StringBuilder();
                    foreach (DataColumn dtC in dr.Table.Columns)
                    {
                        // değişmiş ise öncekinide al
                        if (!object.Equals(dtR[dtC, DataRowVersion.Original], dtR[dtC, DataRowVersion.Current]))
                        {
                            if (!(dtR[dtC, DataRowVersion.Original] == DBNull.Value && dtR[dtC, DataRowVersion.Current].ToString() == ""))
                            {
                                if (dtR[dtC, DataRowVersion.Current] == DBNull.Value)
                                    sbVeri.Append("Ö[" + dtC.ColumnName + "]:[DBNull]\n");
                                else
                                    sbVeri.Append("Ö[" + dtC.ColumnName + "]:" + dtR[dtC, DataRowVersion.Original].ToString() + "\n");
                                SonrakiOnEk = "S";
                            }
                        }

                        if (dtR[dtC, DataRowVersion.Current] == DBNull.Value)
                            sbVeri.Append(SonrakiOnEk + "[" + dtC.ColumnName + "]:[DBNull]\n");
                        else
                        {
                            if (dtC.ColumnName.Equals(dtR.Table.PrimaryKey[0].ColumnName, StringComparison.CurrentCultureIgnoreCase))
                                TabloKey = Convert.ToDecimal(dtR[dtC, DataRowVersion.Current]);
                            else if (dtC.ColumnName.Equals("UKullaniciKey", StringComparison.CurrentCultureIgnoreCase))
                                UKullaniciKey = Convert.ToDecimal(dtR[dtC, DataRowVersion.Current]);
                            sbVeri.Append(SonrakiOnEk + "[" + dtC.ColumnName + "]:" + dtR[dtC, DataRowVersion.Current].ToString() + "\n");
                        }
                        SonrakiOnEk = "";
                    }

                    NpgsqlParameter[] oNpgsqlParameter = new NpgsqlParameter[7];
                    oNpgsqlParameter[0] = new NpgsqlParameter(":TabloAd", dtR.Table.TableName);
                    oNpgsqlParameter[1] = new NpgsqlParameter(":UIDTipNo", Islem);
                    oNpgsqlParameter[2] = new NpgsqlParameter(":TabloKey", TabloKey.ToString());
                    oNpgsqlParameter[3] = new NpgsqlParameter(":VeriTabaniAd", VTAd);
                    oNpgsqlParameter[4] = new NpgsqlParameter(":Veri", sbVeri.ToString());
                    oNpgsqlParameter[5] = new NpgsqlParameter(":Sure", gSure);
                    oNpgsqlParameter[6] = new NpgsqlParameter(":UKullaniciKey", UKullaniciKey.ToString());
                    arrLogSQL.Add(oNpgsqlParameter);
                }
            }
            else
            {
                if (Islem == "D")
                {
                    foreach (DataColumn dtC in dr.Table.Columns)
                    {
                        if (dr[dtC, DataRowVersion.Original] == DBNull.Value)
                            sbVeri.Append("[" + dtC.ColumnName + "]:[DBNull]\n");
                        else
                        {
                            if (dtC.ColumnName.Equals(dr.Table.PrimaryKey[0].ColumnName, StringComparison.CurrentCultureIgnoreCase))
                                TabloKey = Convert.ToDecimal(dr[dtC, DataRowVersion.Original]);
                            else if (dtC.ColumnName.Equals("UKullaniciKey", StringComparison.CurrentCultureIgnoreCase))
                                UKullaniciKey = Convert.ToDecimal(dr[dtC, DataRowVersion.Original]);
                            sbVeri.Append("[" + dtC.ColumnName + "]:" + dr[dtC, DataRowVersion.Original].ToString() + "\n");
                        }
                    }
                }
                else
                {
                    foreach (DataColumn dtC in dr.Table.Columns)
                    {
                        // değişmiş ise öncekinide al
                        if (!object.Equals(dr[dtC, DataRowVersion.Original], dr[dtC, DataRowVersion.Current]))
                        {
                            if (!(dr[dtC, DataRowVersion.Original] == DBNull.Value && dr[dtC, DataRowVersion.Current].ToString() == ""))
                            {
                                if (dr[dtC, DataRowVersion.Current] == DBNull.Value)
                                    sbVeri.Append("Ö[" + dtC.ColumnName + "]:[DBNull]\n");
                                else
                                    sbVeri.Append("Ö[" + dtC.ColumnName + "]:" + dr[dtC, DataRowVersion.Original].ToString() + "\n");
                                SonrakiOnEk = "S";
                            }
                        }

                        if (dr[dtC, DataRowVersion.Current] == DBNull.Value)
                            sbVeri.Append(SonrakiOnEk + "[" + dtC.ColumnName + "]:[DBNull]\n");
                        else
                        {
                            if (dtC.ColumnName.Equals(dr.Table.PrimaryKey[0].ColumnName, StringComparison.CurrentCultureIgnoreCase))
                                TabloKey = Convert.ToDecimal(dr[dtC, DataRowVersion.Current]);
                            else if (dtC.ColumnName.Equals("UKullaniciKey", StringComparison.CurrentCultureIgnoreCase))
                                UKullaniciKey = Convert.ToDecimal(dr[dtC, DataRowVersion.Current]);
                            sbVeri.Append(SonrakiOnEk + "[" + dtC.ColumnName + "]:" + dr[dtC, DataRowVersion.Current].ToString() + "\n");
                        }
                        SonrakiOnEk = "";
                    }
                }

                NpgsqlParameter[] oNpgsqlParameter = new NpgsqlParameter[7];
                oNpgsqlParameter[0] = new NpgsqlParameter(":TabloAd", dr.Table.TableName);
                oNpgsqlParameter[1] = new NpgsqlParameter(":UIDTipNo", Islem);
                oNpgsqlParameter[2] = new NpgsqlParameter(":TabloKey", TabloKey.ToString());
                oNpgsqlParameter[3] = new NpgsqlParameter(":VeriTabaniAd", VTAd);
                oNpgsqlParameter[4] = new NpgsqlParameter(":Veri", sbVeri.ToString());
                oNpgsqlParameter[5] = new NpgsqlParameter(":Sure", gSure);
                oNpgsqlParameter[6] = new NpgsqlParameter(":UKullaniciKey", UKullaniciKey.ToString());
                arrLogSQL.Add(oNpgsqlParameter);
            }
        }

        private void LogSQLOlustur(Exception ex)
        {
            arrLogSQL.Add(HataTablosuInsertCumlesiOlustur(ex, ""));
        }

        private bool LogAlinanTabloMu(string TabloAd)
        {
            return dtLoglanackTablolar.Select("TabloAd='" + TabloAd + "'").Length > 0;
        }

        public void SayfaZiyaretYaz(int KullaniciKey, string Controller, string Action, long Sure)
        {
            using (NpgsqlConnection cnnLog = new NpgsqlConnection(saryConnectionString[DataBaseTipi.Yetki]))
            {
                cnnLog.Open();
                using (NpgsqlCommand cmdLog = cnnLog.CreateCommand())
                {
                    cmdLog.CommandText =
                        "insert into public.\"ZIYARET\" " +
                        "       (\"KullaniciKey\", \"Controller\", \"Action\", \"Sure\", \"ZiyaretTar\") " +
                        "values (:KullaniciKey, :Controller, :Action, :Sure, current_timestamp)";

                    cmdLog.Parameters.Add(new NpgsqlParameter(":KullaniciKey", KullaniciKey));
                    cmdLog.Parameters.Add(new NpgsqlParameter(":Controller", Controller));
                    cmdLog.Parameters.Add(new NpgsqlParameter(":Action", Action));
                    cmdLog.Parameters.Add(new NpgsqlParameter(":Sure", Sure));

                    cmdLog.ExecuteNonQuery();
                }
                cnnLog.Close();
            }
        }


        #endregion

        private static string GetConnectionString(string sDataBaseTipi)
        {
            //if (ReadFromRegistry32(sDataBaseTipi) == null)
            //    return string.Empty;
            //return ASCIIEncoding.ASCII.GetString(ProtectedData.Unprotect(((byte[])ReadFromRegistry32(sDataBaseTipi)), new byte[] { 214, 122, 72, 35, 44, 6, 0, 122, 236, 198, 40, 7, 18, 34, 52, 229, 245 }, DataProtectionScope.LocalMachine));

            if (ReadFromRegistry64(sDataBaseTipi) == null)
            {
                if (ReadFromRegistry32(sDataBaseTipi) == null)
                    return string.Empty;
                else
                    return ASCIIEncoding.ASCII.GetString(ProtectedData.Unprotect(((byte[])ReadFromRegistry32(sDataBaseTipi)), new byte[] { 214, 122, 72, 35, 44, 6, 0, 122, 236, 198, 40, 7, 18, 34, 52, 229, 245 }, DataProtectionScope.LocalMachine));
            }
            else
                return ASCIIEncoding.ASCII.GetString(ProtectedData.Unprotect(((byte[])ReadFromRegistry64(sDataBaseTipi)), new byte[] { 214, 122, 72, 35, 44, 6, 0, 122, 236, 198, 40, 7, 18, 34, 52, 229, 245 }, DataProtectionScope.LocalMachine));
        }

        private static object ReadFromRegistry64(string sDataBaseTipi)
        {
            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var reg = hklm.OpenSubKey(@"SOFTWARE\Argem\" + ProjeRegistryAd))
            {
                if (reg != null)
                    return reg.GetValue(sDataBaseTipi);
            }
            return null;
        }

        private static object ReadFromRegistry32(string sDataBaseTipi)
        {
            using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"Software\Argem\" + ProjeRegistryAd, false))
            {
                if (reg != null)
                    return reg.GetValue(sDataBaseTipi);
            }
            return null;
        }

        public void Dispose()
        {
            if (cmd != null)
                cmd.Dispose();
            if (cnNpgsql != null)
                cnNpgsql.Dispose();
            arrLogSQL = null;
            sConn = null;
            sDataBaseTipi = null;
        }

        //public object SorguCalistirMSSQL(string sSQL, string ConnectionString)
        //{
        //    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();

        //    try
        //    {
        //        object sonuc = null;

        //        conn = new System.Data.SqlClient.SqlConnection(ConnectionString);
        //        conn.Open();

        //        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sSQL, conn);
        //        sonuc = cmd.ExecuteNonQuery();

        //        return sonuc;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Util.EventViewerMesajYaz((string.IsNullOrEmpty(ex.StackTrace) ? "" : ex.StackTrace) + Environment.NewLine + (string.IsNullOrEmpty(ex.Message) ? "" : ex.Message), System.Diagnostics.EventLogEntryType.Error, 0);
        //        VeriKaydetHataYaz(ex, sSQL);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn != null)
        //            conn.Dispose();
        //    }
        //}

        //public void DataGetirMSSQL(ref DataTable dt, string sSQL, string ConnectionString)
        //{
        //    System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
        //    try
        //    {
        //        conn = new System.Data.SqlClient.SqlConnection(ConnectionString);
        //        conn.Open();
        //        System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(sSQL, conn);
        //        sqlCommand.CommandTimeout = 0;
        //        using (System.Data.SqlClient.SqlDataAdapter daSQL = new System.Data.SqlClient.SqlDataAdapter(sqlCommand))
        //            daSQL.Fill(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn != null)
        //            conn.Dispose();
        //    }
        //}
    }
}
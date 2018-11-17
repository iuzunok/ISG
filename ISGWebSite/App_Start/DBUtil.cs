using Npgsql;
using System;
using System.Data;

namespace VeriTabani
{
    public class DBUtil
    {
        static string connstrYetki = "Server=localhost;Port=5432;User Id=denemeUser1;Password=123ecr45;Database=ARGEM_YETKI;";

        public static DataTable VeriGetirDT(string sSQL)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(connstrYetki);
                conn.Open();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sSQL, conn);
                DataSet ds = new DataSet();
                ds.Reset();
                da.Fill(ds);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                conn.Close();
                return dt;
            }
            catch (Exception msg)
            {
                throw new Exception(msg.ToString());
            }
        }

        public static DataSet VeriGetirDS(string sSQL)
        {
            try
            {
                // Making connection with Npgsql provider
                NpgsqlConnection conn = new NpgsqlConnection(connstrYetki);
                conn.Open();
                // quite complex sql statement
                // string sql = "SELECT * FROM public.\"PERSONEL\"";
                // data adapter making request from our connection
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sSQL, conn);
                // i always reset DataSet before i do
                // something with it.... i don't know why :-)
                DataSet ds = new DataSet();
                ds.Reset();
                // filling DataSet with result from NpgsqlDataAdapter
                da.Fill(ds);
                // since it C# DataSet can handle multiple tables, we will select first
                // connect grid to DataTable
                // since we only showing the result we don't need connection anymore
                conn.Close();
                return ds;
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why                
                // throw new Exception(msg.ToString());
                return null;
            }
        }


        public static string SorguCalistir(string sSQL)
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(connstrYetki);
                conn.Open();

                NpgsqlCommand query = new NpgsqlCommand(sSQL, conn);
                // query.Parameters.Add(new NpgsqlParameter("nombre", NpgsqlDbType.Varchar));
                // query.Parameters.Add(new NpgsqlParameter("capital", NpgsqlDbType.Varchar));
                query.Prepare();
                // query.Parameters[0].Value = this.textBox1.Text;
                // query.Parameters[1].Value = this.textBox2.Text;
                Object res = query.ExecuteScalar();
                conn.Close();

                if (res != null)
                    return res.ToString();
                else
                    return "0";
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why                
                throw new Exception(msg.ToString());
            }
        }


    }
}
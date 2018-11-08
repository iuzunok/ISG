using Npgsql;
using System;
using System.Data;

namespace VeriTabani
{
    public class DBUtil
    {
        static string connstrPersonel = "Server=localhost;Port=5432;User Id=denemeUser1;Password=123ecr45;Database=PERSONEL;";
        static string connstrYetki = "Server=localhost;Port=5432;User Id=denemeUser1;Password=123ecr45;Database=ARGEM_YETKI;";

        public static DataTable VeriGetirDT(string sSQL)
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
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                // connect grid to DataTable
                // since we only showing the result we don't need connection anymore
                conn.Close();
                return dt;
            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why                
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
                throw new Exception(msg.ToString());
            }
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Configuration;

using DatabaseAPIs.Models;
using DatabaseAPIs.Interfaces;

namespace DatabaseAPIs.Services
{
    public class PromotedService : IPromoted
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CodexDB"].ConnectionString;
        private static PromotedService Instance = null;
        public static PromotedService instantiateDB()
        {
            if (Instance == null)
            {
                Instance = new PromotedService();
            }
            return Instance;
        }

        public List<Promoted> GetPromoted()
        {
            List<Promoted> promotedList = new List<Promoted>();
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("select * from Promoted"
                    );
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        promotedList.Add(
                            new Promoted()
                            {
                                id = Int32.Parse(dr["id"].ToString()),
                                bookID = dr["bookID"].ToString().Trim(' '),
                                expiresOn = dr["expiresOn"].ToString()
                            }
                        );
                    }
                }catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            
            return promotedList;
        }
        public bool PostPromoted(Promoted p)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("insert into Promoted values ('{0}', '{1}')",
                    p.bookID, p.expiresOn
                    );
                con.Open();
                
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }
        public bool PutPromoted(Promoted p)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("update Promoted set BookID = '{0}', expiresOn='{1}' where id = {2}",
                    p.bookID, p.expiresOn, p.id
                    );
                con.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }
        public bool DeletePromoted(Promoted p)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = String.Format("delete Promoted where id = {0}",
                    p.id
                    );
                con.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
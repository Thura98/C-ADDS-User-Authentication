using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.DirectoryServices;
using TestAPI.Models;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TestAPI.Controllers
{
    public class ValuesController : ApiController
    {
        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

        public string UserData { get; }
        private string _path;
        private string _filterAttribute;

        public ValuesController() { }

        [HttpGet]
        [HttpPost]
        public bool IsAuthenticated(string username, string pwd)
        {
            string domainAndUsername = "16ELM" + @"\" + username;
            //_path = "LDAP://172.20.129.73/dc=16elm,dc=local";
            _path = "LDAP://192.168.64.128/dc=16elm,dc=local";
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
            try
            {
                // Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                // Update the new path to the user in the directory
                _path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];

            }
            catch (Exception ex)
            {
                throw new Exception("Error authentication user. " + ex.Message);
            }
            return true;
        }

        [HttpGet]
        [HttpPost]
        public Projector GetProjector(string RoomNum)
        {
            conn.Open();
            Projector p = new Projector();
            string query = "Select * from SITRooms where RoomNum = '" + RoomNum + "'";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "SITRoomsTbl");
            int rec_count = ds.Tables["SITRoomsTbl"].Rows.Count;
            if (rec_count > 0)
            {
                DataRow row = ds.Tables["SITRoomsTbl"].Rows[0];
                p.RoomNum = row["RoomNum"].ToString();
                p.RoomID = row["RoomID"].ToString();
                p.SymbolID = row["SymbolID"].ToString();
            }
            else
            {
                p = null;
            }
            conn.Close();

            return p;
        }
    }
}

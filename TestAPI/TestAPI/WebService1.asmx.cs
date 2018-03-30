using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using TestAPI.Models;

namespace TestAPI
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
        [WebMethod]
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

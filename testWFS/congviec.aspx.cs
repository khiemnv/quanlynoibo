using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class congviec : System.Web.UI.Page
{
    static bool s_bMenuInitialized = false;
    static SqlConnection s_cnn = null;

    const string zALL = "all";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!s_bMenuInitialized)
        {
            s_bMenuInitialized = initMenu();
        }
    }
    bool initMenu()
    {
        MenuItem child;
        child = new MenuItem("All");
        child.Value = zALL;
        child.Selectable = true;
        MenuBan.Items.Add(child);

        SqlConnection cnn = getCnn();
        DataTable tbl = GetData("select * from dbo.tbl_ban", cnn);
        foreach (DataRow row  in tbl.Rows) {
            string name;
            name = row["ten_ban"].ToString();
            child = new MenuItem(name);
            child.Selectable = true;
            child.Value = row["id"].ToString();
            MenuBan.Items.Add(child);
        }
        return true;
    }
    SqlConnection getCnn()
    {
        if (s_cnn == null)
        {
            string cnnstr;
            cnnstr = ConfigurationManager.ConnectionStrings["MyDbConn"].ToString();
            SqlConnection cnn = new SqlConnection(cnnstr);

            s_cnn = cnn;
        }
        return s_cnn;
    }
    bool closeCnn(){
        if (s_cnn != null)
        {
            s_cnn.Close();
            s_cnn.Dispose();
            s_cnn = null;
        }

        return true;
    }
    DataTable GetData(string qry, SqlConnection cnn)
    {
        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        dataAdapter.SelectCommand = new SqlCommand(qry, cnn);
        // Populate a new data table and bind it to the BindingSource.
        DataTable table = new DataTable();
        table.Locale = System.Globalization.CultureInfo.InvariantCulture;
        dataAdapter.Fill(table);
        return table;
    }
    DataTable GetData(SqlCommand cmd)
    {
        SqlDataAdapter dataAdapter = new SqlDataAdapter();
        dataAdapter.SelectCommand = cmd;
        // Populate a new data table and bind it to the BindingSource.
        DataTable table = new DataTable();
        table.Locale = System.Globalization.CultureInfo.InvariantCulture;
        dataAdapter.Fill(table);
        return table;
    }
    protected void MenuBan_MenuItemClick(object sender, MenuEventArgs e)
    {
        string ban;
        ban = e.Item.Value;
        const string f_STARTDATE = "ngay_bat_dau";
        const string k_STARTDATE = "@startDate";
        const string f_GROUP = "ban";
        const string f_STATUS = "tinh_trang";
        //const string k_BANLST = "@banLst";

        SqlConnection cnn = getCnn();
        SqlCommand cmd = null;
        const string tbl_DAILYTASK = "dbo.tbl_congviec";
        const string tbl_PLAN = "dbo.tbl_kehoach";
        SqlCommand selectPlanCmd = null;

        if (MenuBan.Items[0].Selected)
        {
            cmd = new SqlCommand(string.Format("select * from {0} where {1} = {2}", tbl_DAILYTASK, f_STARTDATE, k_STARTDATE));
            cmd.Parameters.AddWithValue(k_STARTDATE, DateTime.Now.Date);

            selectPlanCmd = new SqlCommand(string.Format("select * from {0} where {1} = {2}", tbl_PLAN, f_STARTDATE, k_STARTDATE));
            selectPlanCmd.Parameters.AddWithValue(k_STARTDATE, DateTime.Now.Date);
        }
        else
        {
            string groupLst = "";
            foreach(MenuItem mi in MenuBan.Items)
            {
                if (mi.Selected)
                {
                    groupLst = groupLst + "'" + mi.Value.TrimEnd() + "',";
                }
            }
            if (groupLst.Length != 0)
            {
                groupLst = groupLst.TrimEnd(',');
                cmd = new SqlCommand(string.Format("select * from {0} where {1} = {2} and {3} in ({4})",
                    tbl_DAILYTASK, f_STARTDATE, k_STARTDATE, f_GROUP, groupLst));
                cmd.Parameters.AddWithValue(k_STARTDATE, DateTime.Now.Date);
                //sqlCmd.Parameters.AddWithValue(k_BANLST, banLst);

                selectPlanCmd = new SqlCommand(string.Format("select * from {0} where DATEDIFF(DAY,{1},'{2}') <= 0 and {3} in ({4}) and {5} in (1,2,3) ",
                    tbl_PLAN, f_STARTDATE, DateTime.Now.Date.ToString("yyyy-MM-dd"), f_GROUP, groupLst, f_STATUS));
                //cmd.Parameters.AddWithValue(k_STARTDATE, DateTime.Now.Date.ToString());
            }
        }

        if (cmd!=null)
        {
            cmd.Connection = cnn;
            //DataTable tbl = GetData(cmd);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            congviecGV.DataSource = ds;
            congviecGV.DataBind();

            DataSet planDs = new DataSet();
            selectPlanCmd.Connection = cnn;
            SqlDataAdapter planDa = new SqlDataAdapter(selectPlanCmd);
            planDa.Fill(planDs);
            kehoachGV.DataSource = planDs;
            kehoachGV.DataBind();
        }
    }
}
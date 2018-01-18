using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{

    [DataContract]
    class postCmd
    {
        [DataMember]
        public string cmd;
        [DataMember]
        public string arg1;
        [DataMember]
        public string arg2;
        [DataMember]
        public string arg3;
        [DataMember]
        public string arg4;
    }

    [DataContract]
    class cmdRet
    {
        [DataMember]
        public int errorCode;
        [DataMember]
        public string dataObj;
    }

    public class myJson<T>
    {
        //serialize 
        public static string WriteFromObject(T obj)
        {
            //Create a stream to serialize the object to.  
            MemoryStream ms = new MemoryStream();

            //Serializer object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(ms, obj);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
        //Deserialize 
        public static T ReadToObject(string json)
        {
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            var obj = ser.ReadObject(ms);
            ms.Close();
            return (T)obj;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string zHdr = Request.Headers["Content-type"];
        if (zHdr == null) return;
        if (zHdr != "application/x-www-form-urlencoded") return;

        string zCmd = Request.Form["cmdObj"];
        postCmd cmd = myJson<postCmd>.ReadToObject(zCmd);
        cmdRet ret = new cmdRet();
        ret.errorCode = -1;
        ret.dataObj = "";
        string zRet = myJson<cmdRet>.WriteFromObject(ret);

        Response.Write(zRet);
    }
}
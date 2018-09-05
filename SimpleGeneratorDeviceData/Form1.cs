using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SimpleGeneratorDeviceData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_sendData_Click(object sender, EventArgs e)
        {
            this.label_resultmsg.Text = "";
            this.label_resultmsg.ForeColor = Color.Green;
            try
            {
                if (checkBox_TempDev.Checked == true)
                {
                    var fileName = int.Parse(GetLastFTPName()) + 1;
                    var ftpUrl = ConfigurationManager.AppSettings["ftpUrl"];
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + "/" + fileName.ToString("00000") + ".xml");

                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential("qwerty@measure-device-test.kl.com.ua", "Qwerty1");

                    var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("data"));

                    Random rnd = new Random();
                    doc.Root.Add(new XElement("date", DateTime.Now.ToString()));
                    doc.Root.Add(new XElement("value", rnd.Next(0, 50).ToString()));

                    Stream requestStream = request.GetRequestStream();
                    doc.Save(requestStream);
                    requestStream.Close();

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    
                    response.Close();
                    response.Dispose();
                    this.label_resultmsg.Text = "Data is added ";
                }

                if (checkBox_PresDev.Checked == true)
                {
                    var WebSeviceUrl = ConfigurationManager.AppSettings["webServiceUrl"];
                    using (var client = new HttpClient())
                    {
                        Random rnd = new Random();
                        DeviceData p = new DeviceData { Date = DateTime.Now, Value = rnd.Next(600, 900) };
                        client.BaseAddress = new Uri(WebSeviceUrl);
                        var respons = client.PostAsJsonAsync("api/DeviceValue/AddData", p).Result;
                        if (respons.StatusCode == HttpStatusCode.OK)
                            this.label_resultmsg.Text = "Data is added ";
                       
                           
                    }

                };
            }
            catch (Exception ex)
            {
                this.label_resultmsg.ForeColor = Color.Red;
                this.label_resultmsg.Text +=$"There were errors";
            }

        }

        private string GetLastFTPName()
        {
            var ftpUrl = ConfigurationManager.AppSettings["ftpUrl"];
            var ftpUser = ConfigurationManager.AppSettings["ftpUser"];
            var ftpPassword = ConfigurationManager.AppSettings["ftpPassword"];

            List<string> entries = null;
            var ftpFileNameList = new List<string>();

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            //Enter FTP Server credentials.
            request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
            request.UsePassive = true;
            request.UseBinary = true;
            request.EnableSsl = false;

            //Fetch the Response and read it using StreamReader.
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();


            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                entries = reader.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            //Getting list of only file names in FTP
            foreach (string entry in entries)
            {
                string[] splits = entry.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries);

                if (splits[0].Substring(0, 1) != "d")
                    ftpFileNameList.Add(splits[8].Split('.')[0].Trim());

            }
            response.Close();
            return ftpFileNameList.Last();
        }
    }
}

using MeasuremenrsWinService.Models;
using MeasurementsLibrary.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MeasuremenrsWinService
{
    class XmlFTPDevice : IDevice
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private int _deviceId;
        private string configKey = "XmlFTPDevice_lastfileName"; //Key for LastFileNameToConfig value to download to config
        private string LastFileNameToConfig = null;
        public XmlFTPDevice(int deviceIdArg)
        {
            _deviceId = deviceIdArg;
        }
        public int DeviceId
        {
            get { return _deviceId; }
        }

        //Get Data from FTP Server
        public IEnumerable<DeviceData> GetData()
        {
            logger.Info("start getting data");
            List<string> entries = null;
            var ftpFileNameList = new List<string>();
            var devData = new List<DeviceData>();

            try
            {
                var deviceConfig = ConfigurationManager.GetSection("DevicesGroup/XmlFTPDeviceSetting") as NameValueCollection;
                //FTP Server URL.
                var ftpUrl = deviceConfig["ftpUrl"];
                var ftpUser = deviceConfig["ftpUser"];
                var ftpPassword = deviceConfig["ftpPassword"];
                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential(deviceConfig["ftpUser"], deviceConfig["ftpPassword"]);//login and password for ftp connection
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
                        ftpFileNameList.Add(splits[8].Trim());

                }
                response.Close();

                //Get last downloded file name if it exist in config
                var lastFileNameFromConfig = ConfigurationManager.AppSettings[configKey] ?? "";

                ftpFileNameList.Sort();
                var nameIndex = ftpFileNameList.IndexOf(lastFileNameFromConfig);
                var fileNameToDownLoadList = ftpFileNameList.Skip(nameIndex + 1).ToList();
                LastFileNameToConfig = fileNameToDownLoadList.Count != 0 ? fileNameToDownLoadList.Last() : "";


                //Download needed files
                foreach (var fileName in fileNameToDownLoadList)
                {
                    FtpWebRequest downloadrequest = (FtpWebRequest)WebRequest.Create(ftpUrl + "/" + fileName);
                  
                    downloadrequest.Method = WebRequestMethods.Ftp.DownloadFile;

                    downloadrequest.Credentials = new NetworkCredential(ftpUser, ftpPassword); 
                   
                    FtpWebResponse downloadresponse = (FtpWebResponse)downloadrequest.GetResponse();

                    Stream responseStream = downloadresponse.GetResponseStream();

                    //Parse XML file
                    var doc = XDocument.Load(responseStream, LoadOptions.None).Root;
                    var v = doc.Element("value")?.Value.Trim().Replace('.', ',');
                    var value = decimal.Parse(v);
                    var date = DateTime.Parse(doc.Element("date")?.Value.Trim());

                    devData.Add(new DeviceData() { Value = value, Date = date });
                    downloadresponse.Dispose();
                    responseStream.Dispose();
                }
                if (devData.Count > 0)
                    logger.Info("the data has been gotten");
                else
                    logger.Info("there's no new data ");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            return devData;
        }

        public void SaveLastValueToConfig()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (!config.AppSettings.Settings.AllKeys.Contains(configKey))
                    config.AppSettings.Settings.Add(configKey, LastFileNameToConfig);
                else
                    config.AppSettings.Settings[configKey].Value = LastFileNameToConfig;

                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
                logger.Info("last value has been saved to config");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}

using MeasuremenrsWinService.Models;
using MeasurementsLibrary.Models;
using Newtonsoft.Json;
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

namespace MeasuremenrsWinService
{
    class JsonDevice : IDevice
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private int _deviceId;
        private string configKey = "JsonDevice_lastDate";
        private string lastDateToConfig;

        public JsonDevice(int deviceIdArg)
        {
            _deviceId = deviceIdArg;
        }

        public int DeviceId
        {
            get { return _deviceId; }

        }

        public IEnumerable<DeviceData> GetData()
        {
            List<DeviceData> devData = null;
            try
            {
                logger.Info("start getting data");
                string lastDateStr = ConfigurationManager.AppSettings[configKey] ?? "";
                DateTime? lastDate = null;
                if (!String.IsNullOrEmpty(lastDateStr))
                {
                    lastDate = DateTime.Parse(lastDateStr).AddSeconds(1);
                }

                //Download data from web service
                using (WebClient wc = new WebClient())
                {
                    var deviceConfig = ConfigurationManager.GetSection("DevicesGroup/JsonDeviceSettings") as NameValueCollection;
                    var url = deviceConfig["url"];
                    var method = lastDate == null ? "/api/DeviceValue/GetAll" : "/api/DeviceValue/GetFromDate?date=" + lastDate;
                    var json = wc.DownloadString(url + method);

                    devData = JsonConvert.DeserializeObject<List<DeviceData>>(json);
                }
                if (devData.Count > 0)
                {
                    logger.Info("the data has been gotten");
                    lastDateToConfig = devData.OrderBy(i => i.Date).Last().Date.ToString();
                }
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
                    config.AppSettings.Settings.Add(configKey, lastDateToConfig);
                else
                    config.AppSettings.Settings[configKey].Value = lastDateToConfig;

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

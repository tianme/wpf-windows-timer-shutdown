using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerShutdown
{
    class ConfigHelper
    {
        public Configuration configObject;

        /// <summary>
        /// 根据路径获取配置文件
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public ConfigHelper(string configPath)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configPath;
            this.configObject = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        public string GetConfig(string key)
        {
            string val = string.Empty;

            if (this.configObject.AppSettings.Settings.AllKeys.Contains(key))
                val = this.configObject.AppSettings.Settings[key].Value;

            return val;
        }

        /// <summary>
        /// 获取所有配置文件
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetConfig()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string key in this.configObject.AppSettings.Settings.AllKeys)
                dict.Add(key, this.configObject.AppSettings.Settings[key].Value);

            return dict;
        }

        /// <summary>
        /// 根据键值获取配置文件
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public string GetConfig(string key, string defaultValue)
        {
            string val = defaultValue;
            if (this.configObject.AppSettings.Settings.AllKeys.Contains(key))
                val = this.configObject.AppSettings.Settings[key].Value;

            if (val == null)
                val = defaultValue;

            return val;
        }

        /// <summary>
        /// 写配置文件,如果节点不存在则自动创建
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetConfig(string key, string value)
        {
            try
            {
                //Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (!this.configObject.AppSettings.Settings.AllKeys.Contains(key))
                    this.configObject.AppSettings.Settings.Add(key, value);
                else
                    this.configObject.AppSettings.Settings[key].Value = value;
                this.configObject.Save(ConfigurationSaveMode.Modified);
                //ConfigurationManager.RefreshSection("appSettings");

                return true;
            }

            catch { return false; }
        }

        /// <summary>
        /// 写配置文件(用键值创建),如果节点不存在则自动创建
        /// </summary>
        /// <param name="dict">键值集合</param>
        /// <returns></returns>
        public bool SetConfig(Dictionary<string, string> dict)
        {
            try
            {
                if (dict == null || dict.Count == 0)
                    return false;

                //Configuration conf = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                foreach (string key in dict.Keys)
                {
                    if (!this.configObject.AppSettings.Settings.AllKeys.Contains(key))
                        this.configObject.AppSettings.Settings.Add(key, dict[key]);
                    else
                        this.configObject.AppSettings.Settings[key].Value = dict[key];
                }

                this.configObject.Save(ConfigurationSaveMode.Modified);

                return true;
            }
            catch { return false; }
        }
    }
}
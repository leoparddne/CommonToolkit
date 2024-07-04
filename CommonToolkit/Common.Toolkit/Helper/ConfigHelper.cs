using System.Configuration;
using System.Xml;

namespace Common.Toolkit.Helper
{
    public class ConfigHelper
    {
        ///<summary>
        ///返回＊.exe.config文件中appSettings配置节的value项
        ///</summary>
        ///<param name="strKey"></param>
        ///<returns></returns>
        public static string GetAppConfig(string strKey)
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取自定义配置文件内容
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="key">获取的Key名称</param>
        /// <returns></returns>
        public static string GetCustomConfig(string path, string key)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径
            string strFileName = path;
            doc.Load(strFileName);
            //找出名称为“add”的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            foreach (XmlNode node in nodes)
            {
                for (int i = 0; i < node.Attributes.Count; i++)
                {
                    if (node.Attributes[i].Value == key)
                        return node.Attributes[i + 1].Value;
                }
            }
            return string.Empty;
        }

        ///<summary>
        ///在＊.exe.config文件中appSettings配置节增加一对键、值对
        ///</summary>
        ///<param name="newKey"></param>
        ///<param name="newValue"></param>
        public static void UpdateAppConfig(string newKey, string newValue)
        {
            bool isModified = false;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == newKey)
                {
                    isModified = true;
                }
            }

            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (isModified)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}

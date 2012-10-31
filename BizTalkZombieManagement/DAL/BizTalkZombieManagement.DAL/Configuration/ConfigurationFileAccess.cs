using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BizTalkZombieManagement.Entities;
using BizTalkZombieManagement.Entities.CustomEnum;

namespace BizTalkZombieManagement.Dal.Configuration
{
    public class ConfigurationFileAccess
    {
        /// <summary>
        /// getting the id of each endpoint in the configuration file
        /// </summary>
        private Dictionary<WcfType, Int32> WrapperEndpoint;

        /// <summary>
        /// retrieve the configuration file
        /// </summary>
        private System.Configuration.Configuration configuration;

        /// <summary>
        /// Initialize the configuration file
        /// </summary>
        /// <param name="path"></param>
        public ConfigurationFileAccess(String path)
        {
            configuration = ConfigurationManager.OpenExeConfiguration(path);
            RetrieveEndpointID();
        }

        #region Public method

        #region Set method
        /// <summary>
        /// update one key
        /// </summary>
        /// <param name="appKeyName"></param>
        /// <param name="value"></param>
        public void UpdateAppSetting(String appKeyName, String value)
        {
            configuration.AppSettings.Settings.Remove(appKeyName);
            configuration.AppSettings.Settings.Add(appKeyName, value);
        }



        /// <summary>
        /// update the endpoint uri in the configuration file only for WCF
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="BindingType"></param>
        public void UpdateUri(Uri uri, WcfType BindingType)
        {
            System.ServiceModel.Configuration.ServiceModelSectionGroup serviceModelSection = System.ServiceModel.Configuration.ServiceModelSectionGroup.GetSectionGroup(configuration);

            serviceModelSection.Client.Endpoints[WrapperEndpoint[BindingType]].Address = uri;
        }

        /// <summary>
        /// save the new configuration
        /// </summary>
        public void Save()
        {
            configuration.Save(ConfigurationSaveMode.Modified, true);
        }
        #endregion

        #region Get Method

        /// <summary>
        /// getting app conf
        /// </summary>
        /// <param name="appKeyName"></param>
        /// <returns></returns>
        public String GetAppSetting(String appKeyName)
        {
            return configuration.AppSettings.Settings[appKeyName].Value;
        }
        /// <summary>
        /// return the current URI
        /// </summary>
        /// <param name="bdg"></param>
        /// <returns></returns>
        public Uri GetWcfUri(WcfType bdg)
        {
            System.ServiceModel.Configuration.ServiceModelSectionGroup serviceModelSection = System.ServiceModel.Configuration.ServiceModelSectionGroup.GetSectionGroup(configuration);

            return serviceModelSection.Client.Endpoints[WrapperEndpoint[bdg]].Address;
        }
        #endregion
        #endregion


        #region private method
        private void RetrieveEndpointID()
        {
            WrapperEndpoint = new Dictionary<WcfType, int>();
            System.ServiceModel.Configuration.ServiceModelSectionGroup serviceModelSection = System.ServiceModel.Configuration.ServiceModelSectionGroup.GetSectionGroup(configuration);


            for (int i = 0; i < serviceModelSection.Client.Endpoints.Count; i++)
            {
                WcfType val;

                //test because when you've got BizTalk Adapter Pack you have got some default endpoint more like SAP or Oracle
                if (Enum.TryParse<WcfType>(serviceModelSection.Client.Endpoints[i].Name, out val))
                {
                    WrapperEndpoint.Add(val, i);
                }
            }

        }

        #endregion
    }
}

using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cambridge105.SmsApi.DataStore
{
    public class NameMap
    {
        private const string c_GoogleSheetsAppName = "Cambridge105.SmsApi.DataStore";
        private static readonly TimeSpan c_CacheRefreshInterval = new TimeSpan(1, 0, 0);

        private readonly string m_ApiKey;
        private readonly string m_SpreadsheetId;

        private DateTime m_LastPopulated = DateTime.MinValue; 
        private Dictionary<string, string> m_NameDictionary = new Dictionary<string, string>();
        private bool m_SuccessfullyPopulated = false;

        private ILog Log = LogManager.GetLogger("Cambridge105.SmsApi.DataStore.NameMap");

        public NameMap(string googleSheetsApiKey, string nameMapSpreadsheetId)
        {
            m_ApiKey = googleSheetsApiKey;
            m_SpreadsheetId = nameMapSpreadsheetId;
        }

        public string LookupNumber(string number)
        {
            RefreshCacheIfNecessary();

            string name;
            if (m_NameDictionary.TryGetValue(number, out name))
            {
                name = string.Format("{0} - {1}", name, number);
            }
            else
            {
                name = number;
            }
            return name;
        }

        private void RefreshCacheIfNecessary()
        {
            if (!m_SuccessfullyPopulated || m_LastPopulated < DateTime.UtcNow.Subtract(c_CacheRefreshInterval))
            {
                try
                {
                    m_NameDictionary = GetLiveNameNumberMap();

                    m_SuccessfullyPopulated = true;
                    m_LastPopulated = DateTime.UtcNow;
                }
                catch (Exception e)
                {
                    Log.Error("Exception refreshing name map", e);
                }
            }
        }

        private Dictionary<string, string> GetLiveNameNumberMap()
        {
            using (SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = c_GoogleSheetsAppName,
                ApiKey = m_ApiKey,
            }))
            {
                var request = service.Spreadsheets.Values.Get(m_SpreadsheetId, "Numbers!A1:B");
                var response = request.Execute();

                Dictionary<string, string> map = new Dictionary<string, string>();
                foreach (var row in response.Values)
                {
                    map[row[0].ToString()] = row[1].ToString();
                }
                return map;
            }
        }
    }
}

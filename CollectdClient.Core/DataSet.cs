using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CollectdClient.Core
{
    public class DataSet
    {
        private List<DataSource> dataSources = new List<DataSource>();

        public string Type { get; private set; }
        public IEnumerable<DataSource> DataSources { get { return dataSources.AsEnumerable(); } }
        
        private static Regex lineRegex = new Regex(@"^\s*([^\s]+)\s*(.*)$");

        public static DataSet Parse(string line)
        {
            var ds = new DataSet();

            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            var match = lineRegex.Match(line);
            if (!match.Success)
                return null;


            ds.Type = match.Groups[1].Value;
            var dsLine = match.Groups[2].Value;
            foreach (var field in dsLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var dataSource = new DataSource(field);
                if (dataSource == null)
                    continue;

                ds.AddDatasource(dataSource);
            }
            
            return ds;
        }

        public void AddDatasource(DataSource dataSource)
        {
            this.dataSources.Add(dataSource);
        }
    }
}

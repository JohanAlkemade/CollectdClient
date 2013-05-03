using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CollectdClient.Core
{
    public class TypesDB
    {
        private static Lazy<TypesDB> instance = new Lazy<TypesDB>(() => {
            var types = new TypesDB();
            types.Load();
            return types;
        });

        protected void Load()
        {
            var stream = this.GetType().Assembly.GetManifestResourceStream("CollectdClient.Core.types.db");
            Load(stream);
        }

        private void Load(System.IO.Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var ds = DataSet.Parse(line);
                    if (ds != null)
                    {
                        //add to types
                        types[ds.Type] = ds.DataSources.ToList();
                    }
                }
            }
        }

        private TypesDB()
        {
        }

        public static TypesDB Instance
        {
            get { return instance.Value; }
        }

        public List<DataSource> GetType(string type)
        {
            return types[type];
        }

        private IDictionary<string, List<DataSource>> types = new Dictionary<string, List<DataSource>>();
    }
}

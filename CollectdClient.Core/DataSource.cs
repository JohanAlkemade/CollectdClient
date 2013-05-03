using System;

namespace CollectdClient.Core
{
    public class DataSource
    {
        public const string Gauge = "GAUGE";
        public const string Counter = "COUNTER";
        public const int TypeGauge = 1;
        public const int TypeCounter = 0;

        public DataSource(string name, int type, double min, double max)
        {
            this.Name = name;
            this.Type = type == TypeCounter ? TypeCounter : TypeGauge;
            this.Min = min;
            this.Max = max;
        }

        public DataSource(string value)
        {
            ParseFromString(value);
        }

        public string ToString()
        {
            return string.Format("{0}:{1}:{2}:{3}", Name, Type == TypeCounter ? Counter : Gauge, AsString(Min), AsString(Max));
        }

        private string AsString(double value)
        {
            if (Double.IsNaN(value))
                return "U";

            return value.ToString();
        }

        private void ParseFromString(string value)
        {
            value = value.Trim();

            var fields = value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length != 4)
                throw new ArgumentException();

            Name = fields[0];
            Type = fields[1].ToUpper() == "COUNTER" ? 0 : 1;
            Min = ToDouble(fields[2]);
            Max = ToDouble(fields[3]);
        }

        private double ToDouble(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentOutOfRangeException();

            value = value.Trim();

            if (value == "U")
                return double.NaN;
            else
                return double.Parse(value);
        }

        public string Name { get; set; }
        public int Type { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
}

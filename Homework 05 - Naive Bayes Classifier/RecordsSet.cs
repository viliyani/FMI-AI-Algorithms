using System.Collections.Generic;

namespace Hw05
{
    public class RecordsSet
    {
        private ICollection<Record> records;

        public RecordsSet()
        {
            records = new List<Record>();
        }

        public IReadOnlyCollection<Record> Records
        {
            get
            {
                return (IReadOnlyCollection<Record>)records;
            }
        }

        public void AddRecord(Record record)
        {
            records.Add(record);
        }
    }
}

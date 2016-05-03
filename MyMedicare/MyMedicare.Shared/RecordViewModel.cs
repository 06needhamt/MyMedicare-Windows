using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyMedicare
{
    public class RecordViewModel
    {
        private RecordList records;

        public RecordViewModel(RecordList r)
        {
            records = r;
        }
        public ObservableCollection<Record> PopulateData()
        {
            ObservableCollection<Record> collection = new ObservableCollection<Record>();
            foreach (Record r in records.Records)
            {
                collection.Add(r);
            }
            return collection;
        }
    }
}

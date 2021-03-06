﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyMedicare
{
    [DataContract()]
    public class RecordList
    {
        [DataMember()]
        public List<Record> Records { get; set; }

        private static RecordList instance;

        private RecordList()
        {
            Records = new List<Record>();
        }

        public static RecordList GetInstance()
        {
            if (instance == null)
                instance = new RecordList();
            return instance;
        }

        public Record AddRecord(Record u)
        {
            Records.Add(u);
            return u;
        }

        public void RemoveRecord(Record u)
        {
            Records.Remove(u);
        }
    }
}

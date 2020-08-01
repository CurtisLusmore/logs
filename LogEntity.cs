using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace logs
{
    public class LogEntity : TableEntity
    {
        public string Referrer { get; set; }
        public DateTime DateTime { get; set; }
    }
}
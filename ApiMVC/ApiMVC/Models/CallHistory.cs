using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiMVC.Models
{
    public class ApiResult
    {
        public ApiData Result { get; set; }
    }
    public class ApiData
    {
        public List<CallHistory> Data { get; set; }
    }
    public class CallHistory
    {
        public int Id { get; set; }
        public DateTime CallDate { get; set; }
        public string CallId { get; set; }
        public string Recording { get; set; }
        public string Play { get; set; }
        public string Eplay { get; set; }
        public string Download { get; set; }
        public string Did { get; set; }
        public string Src { get; set; }
        public string Dst { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Disposition { get; set; }
        public string LastApp { get; set; }
        public int BillSec { get; set; }
        public int Duration { get; set; }
        public string Type { get; set; }
        public int Duration_Minutes { get; set; }
        public int Duration_Seconds { get; set; }
    }
}
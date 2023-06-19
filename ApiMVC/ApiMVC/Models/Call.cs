using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiMVC.Models
{
    public class Call
    {
        [Key]
        public int Id { get; set; }
        public string ActionId { get; set; }
        public string Type { get; set; }
        public string Extend { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string CallId { get; set; }
        public string Channel { get; set; }
        public string ChannelStateDesc { get; set; }
        public string CallerIdNum { get; set; }
        public string CallerIdName { get; set; }
        public string ConnectedLineNum { get; set; }
        public string ConnectedLineName { get; set; }
        public string Exten { get; set; }
        public string UniqueId { get; set; }
        public string LinkedId { get; set; }
    }
}
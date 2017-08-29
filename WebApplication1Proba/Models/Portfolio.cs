using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1Proba.Models
{
    public class Portfolio
    {
        public long     Id          { get; set; }
        public String   Title       { get; set; }
        public String   Client      { get; set; }
        public String   Service     { get; set; }
        [DataType(DataType.Date)]
        public String DateTime    { get; set; }
        public String   Description { get; set; }
        public String   ImageName   { get; set; }
    }
}
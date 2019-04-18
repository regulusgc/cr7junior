using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ocr7siu.Models
{
    public class OcrAzureResults
    {
        public region[] regions { get; set; }
        public string language { get; set; }
        public decimal textAngle { get; set; }
        public string orientation { get; set; }
    }
    public class region
    {
        public string boundingBox { get; set; }
        public line[] lines { get; set; }
    }
    public class line
    {
        public string boundingBox { get; set; }
        public word[] words { get; set; }
    }
    public class word
    {
        public string boundingBox { get; set; }
        public string text { get; set; }

    }
}
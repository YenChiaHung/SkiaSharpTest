using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaSharpTest.Models
{
    public class PumpInfo
    {
        public string Maker { get; set; }
        public string PartNum { get; set; }
        public List<XY_node> PSList { get; set; }
    }
    public class XY_node
    {
        public float ValueX { get; set; }
        public float ValueY { get; set; }
    }
    public class PSTs_node
    {
        public float Pressur { get; set; }
        public float Spee { get; set; }
        public float Tim { get; set; }
        public float Secon { get; set; }
    }
}

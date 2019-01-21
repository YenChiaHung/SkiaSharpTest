using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaSharpTest.Models
{
    public class PumpInfo
    {
        public string Maker { get; set; }
        public string PartNum { get; set; }
        public List<PressureSpeed> PSList { get; set; }
    }
    public class PressureSpeed
    {
        public float ValueX { get; set; }
        public float ValueY { get; set; }
    }

    public class CanvasInfo
    {
        public SkiaSharp.SKBitmap SaveBitMap { get; set; } 
        public string Title { get; set; }
        public bool XisLog { get; set; }
        public bool YisLog { get; set; }
        public float PaddingL { get; set; }
        public float PaddingR { get; set; }
        public float PaddingU { get; set; }
        public float PaddingD { get; set; }
        public float CanvasWidth { get; set; }
        public float CanvasHeight { get; set; }
        public float XMax { get; set; }
        public float XMin { get; set; }
        public float XMultiplier { get; set; }
        public float YMax { get; set; }
        public float YMin { get; set; }
        public float YMultiplier { get; set; }
        public float XScale { get; set; }
        public float YScale { get; set; }
    }
}

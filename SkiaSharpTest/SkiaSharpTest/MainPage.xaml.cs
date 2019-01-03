using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpTest.Models;
using System.Collections.ObjectModel;


namespace SkiaSharpTest
{
    public partial class MainPage : ContentPage
    {
        List<XY_node> Curve = new List<XY_node>();
        List<PSTs_node> VacCalc = new List<PSTs_node>(); 
        
        float startPressure = 760f,
              endPressure = 0.001f;
        int incrementNum = 100;
        float incrementX, incrementY;
        bool XisLog, 
             YisLog;
        float paddingL = 0, paddingR = 0, paddingU = 0, paddingD = 0;
        
        ObservableCollection<PumpInfo> PumpListData;

        public MainPage()
        {        
            
            InitializeComponent();

            //設定字元點數,標題欄1.5倍
            if (App.ScreenWidth < App.ScreenHeight)
            {
                textPaint.TextSize = textPaint.TextSize * App.ScreenWidth / 160f;
            }
            else
            {
                textPaint.TextSize = textPaint.TextSize * App.ScreenHeight / 160f;
            }
            textPaint3X2.TextSize = textPaint.TextSize * 1.5f;

            XisLog = true; YisLog = true;
            Curve.Clear();
            Curve.Add(new XY_node() { ValueX = (float)Math.Log10(1000f), ValueY = (float)Math.Log10(0.01f) });
            Curve.Add(new XY_node() { ValueX = (float)Math.Log10(0.01f), ValueY = (float)Math.Log10(1000f) });

            PumpListData = new ObservableCollection<PumpInfo>
            {
                new PumpInfo (){Maker ="HanBell" , PartNum ="PS902" ,  PSList = new List<XY_node> {new XY_node(){ValueX=760,ValueY=33} , new XY_node(){ValueX=6.01f,ValueY=33}, new XY_node(){ValueX=6,ValueY=257} , new XY_node(){ValueX=1,ValueY=260} , new XY_node(){ValueX=0.1f,ValueY=220}, new XY_node(){ValueX=0.01f,ValueY=164}, new XY_node(){ValueX=0.001f,ValueY=92} , new XY_node(){ValueX=0.0001f,ValueY=20} } },
                new PumpInfo (){Maker ="LeyBold" , PartNum ="WSU2001", PSList = new List<XY_node> {new XY_node(){ValueX=760,ValueY=181}, new XY_node(){ValueX=100,ValueY=222} , new XY_node(){ValueX=15,ValueY=417}, new XY_node(){ValueX=10,ValueY=472}, new XY_node(){ValueX=1,ValueY=500}   , new XY_node(){ValueX=0.1f,ValueY=417} , new XY_node(){ValueX=0.01f,ValueY=200} } },
                new PumpInfo (){Maker ="ULVAC" ,   PartNum ="PMB600" , PSList = new List<XY_node> {new XY_node(){ValueX=760,ValueY=24} , new XY_node(){ValueX=100,ValueY=8}   , new XY_node(){ValueX=10,ValueY=131}, new XY_node(){ValueX=1,ValueY=167} , new XY_node(){ValueX=0.1f,ValueY=161}, new XY_node(){ValueX=0.01f,ValueY=128} } },
                new PumpInfo (){Maker ="HanBell" , PartNum ="PS904" ,  PSList = new List<XY_node> {new XY_node(){ValueX=760,ValueY=33} , new XY_node(){ValueX=6.01f,ValueY=33}, new XY_node(){ValueX=6,ValueY=257} , new XY_node(){ValueX=1,ValueY=260} , new XY_node(){ValueX=0.1f,ValueY=220}, new XY_node(){ValueX=0.01f,ValueY=164}, new XY_node(){ValueX=0.001f,ValueY=92} , new XY_node(){ValueX=0.0001f,ValueY=20} } },
                new PumpInfo (){Maker ="LeyBold" , PartNum ="WSU2003", PSList = new List<XY_node> {new XY_node(){ValueX=760,ValueY=181}, new XY_node(){ValueX=100,ValueY=222} , new XY_node(){ValueX=15,ValueY=417}, new XY_node(){ValueX=10,ValueY=472}, new XY_node(){ValueX=1,ValueY=500}   , new XY_node(){ValueX=0.1f,ValueY=417} , new XY_node(){ValueX=0.01f,ValueY=200} } },
                new PumpInfo (){Maker ="ULVAC" ,   PartNum ="PMB602" , PSList = new List<XY_node> {new XY_node(){ValueX=760,ValueY=24} , new XY_node(){ValueX=100,ValueY=8}   , new XY_node(){ValueX=10,ValueY=131}, new XY_node(){ValueX=1,ValueY=167} , new XY_node(){ValueX=0.1f,ValueY=161}, new XY_node(){ValueX=0.01f,ValueY=128} } }
            };

            PumpListView.ItemsSource = PumpListData;
        }


        public void CanvasView_PressureSpeed(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.White );
            double tempDouble;
            float tempFloat;
            string tempString;

            //最大值最小值
            float xValueMax, xValueMin, yValueMax, yValueMin, xMultiplier, yMultiplier;
            xValueMax = xValueMin = Curve[0].ValueX;
            yValueMax = yValueMin = Curve[0].ValueY;
            xMultiplier = yMultiplier = 1f;
            for (int i = 1; i < Curve.Count; i++)
            {
                if (xValueMax < Curve[i].ValueX) { xValueMax = Curve[i].ValueX; }
                if (xValueMin > Curve[i].ValueX) { xValueMin = Curve[i].ValueX; }
                if (yValueMax < Curve[i].ValueY) { yValueMax = Curve[i].ValueY; }
                if (yValueMin > Curve[i].ValueY) { yValueMin = Curve[i].ValueY; }
            }

            //刻度起始值,若數據是指數,最小值退位為整數
            //刻度結束值,若數據是指數,最大值進位為整數
            //刻度起始值,若數據是自然數,設為零
            //刻度結束值,若數據是自然數,最大值=尾數*乘數;尾數進位為整數
            if (XisLog)
            {
                xValueMax = (float)Math.Ceiling(xValueMax);
                xValueMin = (float)Math.Floor(xValueMin);
            }
            else
            {
                while (xValueMax > 10f ^ xValueMax < 1f)
                {
                    if (xValueMax > 10)
                    {
                        xValueMax = xValueMax / 10;
                        xMultiplier = xMultiplier * 10;
                    }
                    if (xValueMax < 1)
                    {
                        xValueMax = xValueMax * 10;
                        xMultiplier = xMultiplier / 10;
                    }
                }      
                xValueMax = (float)Math.Ceiling(xValueMax) * xMultiplier;
                xValueMin = 0;
            }

            if (YisLog)
            {
                yValueMax = (float)Math.Ceiling(yValueMax);
                yValueMin = (float)Math.Floor(yValueMin);
            }
            else
            {
                while (yValueMax > 10f ^ yValueMax < 1f)
                {
                    if (yValueMax > 10)
                    {
                        yValueMax = yValueMax / 10;
                        yMultiplier = yMultiplier * 10;
                    }
                    if (yValueMax < 1)
                    {
                        yValueMax = yValueMax * 10;
                        yMultiplier = yMultiplier / 10;
                    }
                }   
                yValueMax = (float)Math.Ceiling(yValueMax) * yMultiplier;
                yValueMin = 0;
            }

            //計算左刻度空間
            if (YisLog)
            {
                tempDouble = Math.Pow(10, yValueMax );
            }
            else {
                tempDouble = yValueMax;
            }
            tempString = tempDouble.ToString();            
            paddingL = textPaint.MeasureText(tempString);
            if (YisLog)
            {
                tempDouble = Math.Pow(10, yValueMin);
            }
            else
            {
                tempDouble = yValueMin;
            }
            tempString = tempDouble.ToString();
            tempFloat = textPaint.MeasureText(tempString);
            if (tempFloat > paddingL) { paddingL = tempFloat; }
            paddingL = paddingL + textPaint.TextSize / 2f;

            //計算上標題列高
            paddingU = textPaint.TextSize * 2f;

            //計算右刻度欄寬
            if (XisLog)
            {
                tempDouble = Math.Pow(10, xValueMax);
            }
            else
            {
                tempDouble = xValueMax;
            }
            tempString = tempDouble.ToString();
            paddingR = (textPaint.MeasureText(tempString)+ textPaint.TextSize ) / 4f;

            //計算下刻度列高
            paddingD = textPaint.TextSize * 1.5f;

            float CanvasWidth = (float)e.Info.Width - paddingL - paddingR;
            float CanvasHeight = (float)e.Info.Height - paddingU - paddingD;
            float scaleX = CanvasWidth / (xValueMax - xValueMin);
            float scaleY = CanvasHeight / (yValueMax - yValueMin);

            //畫垂直格線
            for (float i = 0; i < xValueMax - xValueMin; i = i + xMultiplier)
            {
                SKColor colorSave = blackStrokePaint.Color ; 
                blackStrokePaint.Color = SKColors.Red;
                for (float j = xMultiplier; j < 10 * xMultiplier; j = j + xMultiplier)
                {   //畫小刻度線
                    if (XisLog)
                    {
                        canvas.DrawLine((i + (float)Math.Log10(j)) * scaleX + paddingL, paddingU, (i + (float)Math.Log10(j)) * scaleX + paddingL, CanvasHeight + paddingU, redStrokePaint);
                    }
                    else
                    {
                        canvas.DrawLine((i + j / 10) * scaleX + paddingL, paddingU, (i + j / 10) * scaleX + paddingL, CanvasHeight + paddingU, redStrokePaint);
                    }
                }
                blackStrokePaint.Color = colorSave;
                
                canvas.DrawLine(i * scaleX + paddingL, paddingU, i * scaleX + paddingL, CanvasHeight + paddingU, blackStrokePaint);

                //刻度值轉文字
                tempDouble = i + xValueMin;
                if (XisLog)
                { tempDouble = Math.Pow(10, tempDouble); }
                tempString = tempDouble.ToString();
                float textWidth = textPaint.MeasureText(tempString);
                canvas.DrawText(tempString, i * scaleX + paddingL- textWidth /2, CanvasHeight + paddingU + textPaint.TextSize *1.25f , textPaint);
            }
            canvas.DrawLine((xValueMax - xValueMin) * scaleX + paddingL, paddingU, (xValueMax - xValueMin) * scaleX + paddingL, CanvasHeight + paddingU, blackStrokePaint);

            //畫水平格線
            for (float i = 0; i < yValueMax - yValueMin; i = i + yMultiplier)
            {
                SKColor colorSave = blackStrokePaint.Color;
                blackStrokePaint.Color = SKColors.Red;
                for (float j = yMultiplier; j < 10 * yMultiplier; j = j + yMultiplier)
                {
                    if (YisLog)
                    {
                        canvas.DrawLine(paddingL, CanvasHeight + paddingU - (i + (float)Math.Log10(j)) * scaleY, CanvasWidth + paddingL, CanvasHeight + paddingU - (i + (float)Math.Log10(j)) * scaleY, redStrokePaint);
                    }
                    else
                    {
                        canvas.DrawLine(paddingL, CanvasHeight + paddingU - (i + j / 10) * scaleY, CanvasWidth + paddingL, CanvasHeight + paddingU - (i + j / 10) * scaleY, redStrokePaint);
                    }
                }
                blackStrokePaint.Color = colorSave;

                canvas.DrawLine(paddingL, CanvasHeight + paddingU - i * scaleY, CanvasWidth + paddingL, CanvasHeight + paddingU - i * scaleY, blackStrokePaint);

                tempDouble = i + yValueMin;
                if (YisLog)
                {
                    tempDouble = Math.Pow(10, tempDouble);
                }

                tempString = tempDouble.ToString();
                float textWidth = textPaint.MeasureText(tempString);
                canvas.DrawText(tempString, paddingL - textWidth-textPaint.TextSize/4, paddingU + CanvasHeight - i * scaleY, textPaint);
            }
            canvas.DrawLine(paddingL, CanvasHeight + paddingU - (yValueMax - yValueMin) * scaleY, CanvasWidth + paddingL, CanvasHeight + paddingU - (yValueMax - yValueMin) * scaleY, blackStrokePaint);

            //畫標題欄
            tempString = "L/sec VS Torr";
            canvas.DrawText(tempString, (e.Info.Width - textPaint3X2.MeasureText(tempString)) / 2, textPaint.TextSize * 1.75f, textPaint3X2);
            
            //畫曲線
            for (int i = 0; i < Curve.Count - 1; i++)
            {
                canvas.DrawLine((Curve[i].ValueX - xValueMin) * scaleX + paddingL, CanvasHeight + paddingU - (Curve[i].ValueY - yValueMin) * scaleY, (Curve[i + 1].ValueX - xValueMin) * scaleX + paddingL, CanvasHeight + paddingU - (Curve[i + 1].ValueY - yValueMin) * scaleY, blueStrokePaint);
            }
            //textPaint.Color = SKColors.Black ;
            //textPaint.StrokeWidth = 5f;
            //textPaint.TextSize = 48;
            //tempString = App.ScreenDensity.ToString();
            //canvas.DrawText(tempString, 100, 50, textPaint);
            //tempString = App.ScreenWidth .ToString ();
            //canvas.DrawText(tempString, 100, 100, textPaint);
            //tempString = App.ScreenHeight.ToString();
            //canvas.DrawText(tempString, 100, 150, textPaint);
        }

        SKPaint redStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red ,
            StrokeWidth = 1f,
            StrokeCap = SKStrokeCap.Butt
            //PathEffect = SKPathEffect.CreateDash(new int[]{1, 2}, 20);
        };
        SKPaint blackStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 3f,
            StrokeCap = SKStrokeCap.Butt
            //PathEffect = SKPathEffect.CreateDash(new int[]{1, 2}, 20);
        };

        private void PumpListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var SelectedPumpInfo = PumpListView.SelectedItem  as PumpInfo;
            var TempList = SelectedPumpInfo.PSList as List<XY_node> ;
            var ListViewID = sender as ViewCell ;

            VacCalc.Clear();
            for (int i= 0; i < TempList.Count; i++)
            {
                VacCalc.Add(new PSTs_node(){ Pressur= (float) Math.Log10( TempList[i].ValueX), Spee = TempList[i].ValueY, Tim =0, Secon= 0 });
            }
            PumpListView.IsVisible = false ;
            Btn2.IsEnabled = true; 
        }

        SKPaint blueStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 3f,
            StrokeCap = SKStrokeCap.Butt
        };
        SKPaint textPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 6
        };
        SKPaint textPaint3X2 = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 18
        };

        public void DrawPS_ButtenClicked(object sender, EventArgs e)
        {
            XisLog = true; YisLog = false;
            
            startPressure = (float)Math.Log10(760);
            endPressure = (float)Math.Log10(0.01f);

            while (VacCalc[1].Pressur >= startPressure)
            {
                VacCalc.RemoveAt(0);
            }
            while (VacCalc[VacCalc.Count - 2].Pressur <= endPressure)
            {
                VacCalc.RemoveAt(VacCalc.Count - 1);
            }
                                             
            int Idx = VacCalc.Count - 1;
            VacCalc[0].Spee = midPoint(VacCalc[0].Pressur, VacCalc[0].Spee, VacCalc[1].Pressur, VacCalc[1].Spee, startPressure);
            VacCalc[0].Pressur = startPressure;
            VacCalc[Idx].Spee = midPoint(VacCalc[Idx - 1].Pressur, VacCalc[Idx - 1].Spee, VacCalc[Idx].Pressur, VacCalc[Idx].Spee, endPressure);
            VacCalc[Idx].Pressur = endPressure;

            incrementX = (VacCalc[0].Pressur - VacCalc[Idx].Pressur) / incrementNum;
            incrementY = 0;
            for (int i = 0; i < VacCalc.Count; i++)
            {
                if (incrementY < VacCalc[i].Spee) { incrementY = VacCalc[i].Spee; }
            }
            incrementY = incrementY / incrementNum;

            float insX, insY;
            Idx = 1;
            while (Idx < VacCalc.Count)
            {
                if ((VacCalc[Idx - 1].Pressur - VacCalc[Idx].Pressur) > incrementX ^ Math.Abs(VacCalc[Idx - 1].Spee - VacCalc[Idx].Spee) > incrementY)
                {
                    if ((VacCalc[Idx - 1].Pressur - VacCalc[Idx].Pressur) / incrementX > Math.Abs(VacCalc[Idx - 1].Spee - VacCalc[Idx].Spee) / incrementY)
                    {
                        insX = VacCalc[Idx -1].Pressur -incrementX ;
                        insY = midPoint(VacCalc[Idx - 1].Pressur, VacCalc[Idx - 1].Spee, VacCalc[Idx].Pressur, VacCalc[Idx].Spee, insX);
                        VacCalc.Insert(Idx , new PSTs_node () { Pressur = insX , Spee = insY});
                    }
                    else
                    {
                        if (VacCalc[Idx - 1].Spee > VacCalc[Idx].Spee)
                        {
                            insY = VacCalc[Idx - 1].Spee - incrementY;
                        }
                        else
                        {
                            insY = VacCalc[Idx - 1].Spee + incrementY;
                        }
                        insX = midPoint(VacCalc[Idx - 1].Spee, VacCalc[Idx - 1].Pressur, VacCalc[Idx].Spee, VacCalc[Idx].Pressur, insY);
                        VacCalc.Insert(Idx, new PSTs_node() { Pressur = insX, Spee = insY });
                    }
                }
                Idx++;
            }

            Curve.Clear();
            for (int i = 0; i < VacCalc.Count; i++)
            {
                Curve.Add(new XY_node(){ValueX =  VacCalc[i].Pressur ,ValueY =VacCalc[i].Spee });
            }
            XisLog = true;YisLog = false;
            canvasViewPS.InvalidateSurface();

        }

        public void DrawTP_ButtenClicked(object sender, EventArgs e)
        {
                       
            for (int i = 0; i < Curve.Count-1; i++)
            {
                VacCalc[i+1].Tim = 2f * 100f / (VacCalc[i].Spee + VacCalc[i + 1].Spee) * (float) Math.Log(( Math.Pow(10, VacCalc[i].Pressur) / Math.Pow (10 , VacCalc[i + 1].Pressur)));
            }

            VacCalc[0].Tim = 0f;
            VacCalc[0].Secon = 0f;
            for (int i = 1; i < Curve.Count; i++)
            {
                VacCalc[i].Secon = VacCalc[i].Tim + VacCalc[i - 1].Secon;
            }
            Curve.Clear();
            for (int i = 0; i < VacCalc.Count; i++)
            {
                Curve.Add(new XY_node() { ValueX = VacCalc[i].Secon, ValueY = VacCalc[i].Pressur });
            }
            XisLog = false; YisLog = true;
            canvasViewPS.InvalidateSurface();            
        }
        
        public float midPoint(float X0, float Y0, float X1, float Y1, float Mid)
        {
            return ((Y1 - Y0) / (X1 - X0) * (Mid - X0) + Y0);
        }

    }
}
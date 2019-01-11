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
        SKBitmap saveBitmap;

        float startPressure,endPressure;
        int   incrementNum = 100;
        float incrementX, incrementY;
        float paddingL = 0, paddingR = 0, paddingU = 0, paddingD = 0;
        float canvasWidth;
        float canvasHeight;

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

            VacCalc.Clear();
            VacCalc.Add(new PSTs_node() { Pressur = (float)Math.Log10(1000f), Spee = 0.01f, Tim = 0, Secon = 0 });
            VacCalc.Add(new PSTs_node() { Pressur = (float)Math.Log10(0.01f), Spee = 1000f, Tim = 0, Secon = 0 });

            PumpListData = new ObservableCollection<PumpInfo>
{
                new PumpInfo ()
                {
                    Maker ="HanBell" , PartNum ="PS902" ,  PSList = new List<XY_node>
                    {
                        new XY_node(){ValueX= 760,     ValueY= 33  },
                        new XY_node(){ValueX= 6.01f,   ValueY= 33  },
                        new XY_node(){ValueX= 6,       ValueY= 257 },
                        new XY_node(){ValueX= 1,       ValueY= 260 },
                        new XY_node(){ValueX= 0.1f,    ValueY= 220 },
                        new XY_node(){ValueX= 0.01f,   ValueY= 164 },
                        new XY_node(){ValueX= 0.001f,  ValueY= 92  },
                        new XY_node(){ValueX= 0.0001f, ValueY= 20  }
                    }
                },
                new PumpInfo ()
                {
                    Maker ="LeyBold" , PartNum ="WSU2001+SV630", PSList = new List<XY_node>
                    {
                        new XY_node(){ValueX= 760,     ValueY= 189.6712f },
                        new XY_node(){ValueX= 608,     ValueY= 189.6712f },
                        new XY_node(){ValueX= 456,     ValueY= 191.5858f },
                        new XY_node(){ValueX= 304,     ValueY= 197.4464f },
                        new XY_node(){ValueX= 152,     ValueY= 218.3074f },
                        new XY_node(){ValueX= 76,      ValueY= 257.656f  },
                        new XY_node(){ValueX= 60.8f,   ValueY= 279.213f  },
                        new XY_node(){ValueX= 45.6f,   ValueY= 310.2672f },
                        new XY_node(){ValueX= 30.4f,   ValueY= 392.8629f },
                        new XY_node(){ValueX= 15.2f,   ValueY= 461.3514f },
                        new XY_node(){ValueX= 7.6f,    ValueY= 480.2635f },
                        new XY_node(){ValueX= 6.08f,   ValueY= 487.5537f },
                        new XY_node(){ValueX= 4.56f,   ValueY= 492.4752f },
                        new XY_node(){ValueX= 3.04f,   ValueY= 499.9508f },
                        new XY_node(){ValueX= 1.52f,   ValueY= 510.0951f },
                        new XY_node(){ValueX= 0.76f,   ValueY= 510.0951f },
                        new XY_node(){ValueX= 0.608f,  ValueY= 510.0951f },
                        new XY_node(){ValueX= 0.456f,  ValueY= 507.5399f },
                        new XY_node(){ValueX= 0.304f,  ValueY= 499.9508f },
                        new XY_node(){ValueX= 0.152f,  ValueY= 482.6814f },
                        new XY_node(){ValueX= 0.076f,  ValueY= 432.1946f },
                        new XY_node(){ValueX= 0.0608f, ValueY= 413.0957f },
                        new XY_node(){ValueX= 0.0456f, ValueY= 383.1213f },
                        new XY_node(){ValueX= 0.0304f, ValueY= 305.6279f }
                    }
                },
                new PumpInfo (){Maker ="ULVAC" ,   PartNum ="PMB600" , PSList = new List<XY_node> { new XY_node(){ValueX=760,ValueY=24} , new XY_node(){ValueX=100,ValueY=8} , new XY_node(){ValueX=10,ValueY=131} , new XY_node(){ValueX=1,ValueY=167}  , new XY_node(){ValueX=0.1f,ValueY=161} , new XY_node(){ValueX=0.01f,ValueY=128} } },
                new PumpInfo (){Maker ="HanBell" , PartNum ="PS904" ,  PSList = new List<XY_node> {new XY_node(){ValueX=760,ValueY=33} , new XY_node(){ValueX=6.01f,ValueY=33} , new XY_node(){ValueX=6,ValueY=257} , new XY_node(){ValueX=1,ValueY=260} , new XY_node(){ValueX=0.1f,ValueY=220} , new XY_node(){ValueX=0.01f,ValueY=164} , new XY_node(){ValueX=0.001f,ValueY=92} , new XY_node(){ValueX=0.0001f,ValueY=20} } },
                new PumpInfo (){Maker ="LeyBold" , PartNum ="WSU2003", PSList = new List<XY_node> { new XY_node(){ValueX=760,ValueY=181} , new XY_node(){ValueX=100,ValueY=222} , new XY_node(){ValueX=15,ValueY=417} , new XY_node(){ValueX=10,ValueY=472}  , new XY_node(){ValueX=1,ValueY=500} , new XY_node(){ValueX=0.1f,ValueY=417} , new XY_node(){ValueX=0.01f,ValueY=200} } },
                new PumpInfo (){Maker ="ULVAC" ,   PartNum ="PMB602" , PSList = new List<XY_node> { new XY_node(){ValueX=760,ValueY=24} , new XY_node(){ValueX=100,ValueY=8} , new XY_node(){ValueX=10,ValueY=131} , new XY_node(){ValueX=1,ValueY=167}  , new XY_node(){ValueX=0.1f,ValueY=161} , new XY_node(){ValueX=0.01f,ValueY=128} } }

            };

            PumpListView.ItemsSource = PumpListData;
        }

      
        public void CanvasView_PressureSpeed(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvasWidth = (float)e.Info.Width ;
            canvasHeight = (float)e.Info.Height;

          
            if (saveBitmap == null)
            {
                saveBitmap = new SKBitmap(info.Width, info.Height);
            }
            canvas.Clear(SKColors.White);
            canvas.DrawBitmap(saveBitmap, 0, 0);

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
            startPressure = (float)Math.Log10(760);
            endPressure = (float)Math.Log10(0.05f);

            //高於startPressure的座標只留最接近那個
            while (VacCalc[1].Pressur >= startPressure)
            {
                VacCalc.RemoveAt(0);
            }

            //低於endPressure的座標只留最接近那個
            while (VacCalc[VacCalc.Count - 2].Pressur <= endPressure)
            {
                VacCalc.RemoveAt(VacCalc.Count - 1);
            }

            //高於startPressure的座標Pressur=startPressure;Spee=線性內插
            //低於endPressure的座標Pressur=endPressure;Spee=線性內插                              
            int Idx = VacCalc.Count - 1;
            VacCalc[0].Spee = midPoint(VacCalc[0].Pressur, VacCalc[0].Spee, VacCalc[1].Pressur, VacCalc[1].Spee, startPressure);
            VacCalc[0].Pressur = startPressure;
            VacCalc[Idx].Spee = midPoint(VacCalc[Idx - 1].Pressur, VacCalc[Idx - 1].Spee, VacCalc[Idx].Pressur, VacCalc[Idx].Spee, endPressure);
            VacCalc[Idx].Pressur = endPressure;

            //刻度起始值結束值,數據是指數,最小值退位為整數,最大值進位為整數
            float xValueMax = (float)Math.Ceiling(VacCalc[0].Pressur);
            float xValueMin = (float)Math.Floor(VacCalc[VacCalc.Count - 1].Pressur);
            float yValueMax = VacCalc[0].Spee;
            float yMultiplier = 1f;

            //刻度結束值,將最大值拆成兩數相乘;尾數*乘數;尾數進位為整數
            for (int i = 1; i < VacCalc.Count; i++)
            {
                if (yValueMax < VacCalc[i].Spee)
                {
                    yValueMax = VacCalc[i].Spee;
                }
            }
            while (yValueMax >= 10f ^ yValueMax < 1f)
            {
                if (yValueMax >= 10)
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


            //計算左邊的留多少文字空間
            float tempFloat = yValueMax;
            string tempString = tempFloat.ToString();

            paddingL = textPaint.MeasureText(tempString) + textPaint.TextSize / 2f;

            //計算上標題列高
            paddingU = textPaint.TextSize * 2f;

            //計算右邊的留多少文字空間             
            tempFloat = (float)Math.Pow(10, xValueMax);
            tempString = tempFloat.ToString();
            paddingR = (textPaint.MeasureText(tempString) + textPaint.TextSize) / 4f;

            //計算下刻度列高
            paddingD = textPaint.TextSize * 1.5f;

            float scaleX = (canvasWidth - paddingL - paddingR) / (xValueMax - xValueMin);
            float scaleY = (canvasHeight - paddingU - paddingD) / yValueMax;

            //用一次性畫布作圖存成saveBitmap
            using (SKCanvas newcanvas = new SKCanvas(saveBitmap))
            {
                newcanvas.Clear();
                //畫垂直格線
                for (float i = 0; i < xValueMax - xValueMin; i++)
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = 1; j < 10 ; j++)
                    {
                        //畫小刻度線
                        newcanvas.DrawLine((i + (float)Math.Log10(j)) * scaleX + paddingL, paddingU, (i + (float)Math.Log10(j)) * scaleX + paddingL, canvasHeight - paddingD, redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(i * scaleX + paddingL, paddingU, i * scaleX + paddingL, canvasHeight - paddingD, blackStrokePaint);

                    //刻度值轉文字
                    tempFloat = (float) Math.Pow(10, i + xValueMin); 
                    tempString = tempFloat.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, i * scaleX + paddingL - textWidth / 2, canvasHeight - paddingD + textPaint.TextSize * 1.25f, textPaint);
                }
                newcanvas.DrawLine((xValueMax - xValueMin) * scaleX + paddingL, paddingU, (xValueMax - xValueMin) * scaleX + paddingL, canvasHeight - paddingD, blackStrokePaint);

                //畫水平格線
                for (float i = 0; i < yValueMax ; i = i + yMultiplier)
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = yMultiplier/10; j < yMultiplier; j = j + yMultiplier/10)
                    {
                        newcanvas.DrawLine(paddingL, canvasHeight - paddingD - (i + j) * scaleY, canvasWidth - paddingR, canvasHeight - paddingD - (i + j) * scaleY, redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(paddingL, canvasHeight - paddingD - i * scaleY, canvasWidth - paddingR, canvasHeight - paddingD - i * scaleY, blackStrokePaint);

                    tempString = i.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, paddingL - textWidth - textPaint.TextSize / 4, canvasHeight- paddingD - i * scaleY, textPaint);
                }
                newcanvas.DrawLine(paddingL, canvasHeight - paddingD - yValueMax * scaleY, canvasWidth - paddingR, canvasHeight - paddingD - yValueMax  * scaleY, blackStrokePaint);

                //畫標題欄
                tempString = "L/sec VS Torr";
                newcanvas.DrawText(tempString, (canvasWidth - textPaint3X2.MeasureText(tempString)) / 2, textPaint.TextSize * 1.75f, textPaint3X2);

                //畫曲線
                for (int i = 0; i < VacCalc.Count - 1; i++)
                {
                    newcanvas.DrawLine((VacCalc[i].Pressur - xValueMin) * scaleX + paddingL, canvasHeight - paddingD - VacCalc[i].Spee * scaleY, (VacCalc[i + 1].Pressur - xValueMin) * scaleX + paddingL, canvasHeight - paddingD - VacCalc[i + 1].Spee * scaleY, blueStrokePaint);
                }                
            }
            canvasViewPS.InvalidateSurface();
        }

        public void DrawTP_ButtenClicked(object sender, EventArgs e)
        {
            //兩個座標的容許最大距離
            int Idx = VacCalc.Count - 1;
            incrementX = (VacCalc[0].Pressur - VacCalc[Idx].Pressur) / incrementNum;
            incrementY = 0;
            for (int i = 0; i < VacCalc.Count; i++)
            {
                if (incrementY < VacCalc[i].Spee)
                {
                    incrementY = VacCalc[i].Spee;
                }
            }
            incrementY = incrementY / incrementNum;

            float insX, insY;
            Idx = 1;
            while (Idx < VacCalc.Count)
            {
                //大於1代表兩點距離太寬;
                insX = (VacCalc[Idx - 1].Pressur - VacCalc[Idx].Pressur) / incrementX;
                insY = Math.Abs(VacCalc[Idx - 1].Spee - VacCalc[Idx].Spee) / incrementY;
                if (insX > 1f ^ insY > 1f)
                {
                    //兩點之間是X比較寬還是Y比較寬
                    if (insX > insY)
                    {
                        insX = VacCalc[Idx - 1].Pressur - incrementX;
                        insY = midPoint(VacCalc[Idx - 1].Pressur, VacCalc[Idx - 1].Spee, VacCalc[Idx].Pressur, VacCalc[Idx].Spee, insX);
                        VacCalc.Insert(Idx, new PSTs_node() { Pressur = insX, Spee = insY ,Tim=0, Secon=0 });
                    }
                    else
                    {
                        //insY已經是絕對值,要重新判斷正負
                        if (VacCalc[Idx - 1].Spee > VacCalc[Idx].Spee)
                        {
                            insY = VacCalc[Idx - 1].Spee - incrementY;
                        }
                        else
                        {
                            insY = VacCalc[Idx - 1].Spee + incrementY;
                        }
                        insX = midPoint(VacCalc[Idx - 1].Spee, VacCalc[Idx - 1].Pressur, VacCalc[Idx].Spee, VacCalc[Idx].Pressur, insY);
                        VacCalc.Insert(Idx, new PSTs_node() { Pressur = insX, Spee = insY,Tim=0, Secon= 0 });
                    }
                }
                Idx++;
            }
            VacCalc[0].Tim = 0f;
            for (int i = 1; i < VacCalc.Count; i++)
            {
                VacCalc[i].Tim = 2f * 100f / (VacCalc[i-1].Spee + VacCalc[i].Spee) * (float)Math.Log((Math.Pow(10, VacCalc[i-1].Pressur) / Math.Pow(10, VacCalc[i].Pressur)));
            }

            VacCalc[0].Secon = 0f;
            for (int i = 1; i < Curve.Count; i++)
            {
                VacCalc[i].Secon = VacCalc[i].Tim + VacCalc[i - 1].Secon;
            }

            using (SKCanvas newcanvas = new SKCanvas(saveBitmap))
            {
                //newcanvas.DrawLine(0, 0, 600, 600, blackStrokePaint);  //最大值最小值
                float xValueMax= VacCalc[VacCalc.Count - 1].Secon ,
                      xValueMin= VacCalc[0].Secon ,
                      yValueMax= VacCalc[0].Pressur,
                      yValueMin= VacCalc[VacCalc.Count -1].Pressur ,
                      xMultiplier = 1f;

                //刻度結束值,將最大值拆成兩數相乘;尾數*乘數;尾數進位為整數
                while (xValueMax >= 10f ^ xValueMax < 1f)
                {
                    if (xValueMax >= 10)
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

                //刻度起始值結束值,數據是指數,最小值退位為整數,最大值進位為整數
                yValueMax = (float)Math.Ceiling(yValueMax);
                yValueMin = (float)Math.Floor(yValueMin);

                //計算左邊的留多少文字空間
                float tempFloat;
                string tempString;
                tempFloat = yValueMax;
                tempString = tempFloat.ToString();
                paddingL = textPaint.MeasureText(tempString) + textPaint.TextSize / 2f;
                tempFloat = yValueMin;
                tempString = tempFloat.ToString();
                tempFloat = textPaint.MeasureText(tempString) + textPaint.TextSize / 2f;
                if(paddingL < tempFloat)
                {
                    paddingL = tempFloat;
                }

                //計算上標題列高
                paddingU = textPaint.TextSize * 2f;

                //計算右邊的留多少文字空間             
                tempFloat = xValueMax;
                tempString = tempFloat.ToString();
                paddingR = (textPaint.MeasureText(tempString) + textPaint.TextSize) / 4f;

                //計算下刻度列高
                paddingD = textPaint.TextSize * 1.5f;

                float scaleX = (canvasWidth - paddingL - paddingR) / xValueMax ;
                float scaleY = (canvasHeight - paddingU - paddingD) / (yValueMax -yValueMin );

                //畫垂直格線
                for (float i = 0; i < xValueMax ; i=i+xMultiplier )
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = xMultiplier/10; j < xMultiplier ; j = j+xMultiplier/10)
                    {
                        //畫小刻度線
                        newcanvas.DrawLine((i + j) * scaleX + paddingL, paddingU, (i + j) * scaleX + paddingL, canvasHeight + paddingU, redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(i * scaleX + paddingL, paddingU, i * scaleX + paddingL, canvasHeight + paddingU, blackStrokePaint);

                    //刻度值轉文字
                    tempString = tempFloat.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, i * scaleX + paddingL - textWidth / 2, canvasHeight + paddingU + textPaint.TextSize * 1.25f, textPaint);
                }
                newcanvas.DrawLine(xValueMax * scaleX + paddingL, paddingU, xValueMax * scaleX + paddingL, canvasHeight + paddingU, blackStrokePaint);

                //畫水平格線
                for (float i = 0; i < yValueMax-yValueMin ; i ++)
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = 1; j < 10 ;  j++)
                    {
                        newcanvas.DrawLine(paddingL, canvasHeight + paddingU - (i + (float) Math.Log10(j) ) * scaleY, canvasWidth + paddingL, canvasHeight + paddingU - (i + j / 10) * scaleY, redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(paddingL, canvasHeight + paddingU - i * scaleY, canvasWidth + paddingL, canvasHeight + paddingU - i * scaleY, blackStrokePaint);

                    tempString = i.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, paddingL - textWidth - textPaint.TextSize / 4, paddingU + canvasHeight - i * scaleY, textPaint);
                }
                newcanvas.DrawLine(paddingL, canvasHeight + paddingU - yValueMax * scaleY, canvasWidth + paddingL, canvasHeight + paddingU - yValueMax * scaleY, blackStrokePaint);

                //畫標題欄
                tempString = "L/sec VS Torr";
                newcanvas.DrawText(tempString, (canvasWidth - textPaint3X2.MeasureText(tempString)) / 2, textPaint.TextSize * 1.75f, textPaint3X2);

                //畫曲線
                for (int i = 0; i < Curve.Count - 1; i++)
                {
                    newcanvas.DrawLine((VacCalc[i].Pressur - xValueMin) * scaleX + paddingL, canvasHeight + paddingU - VacCalc[i].Spee * scaleY, (VacCalc[i + 1].Pressur - xValueMin) * scaleX + paddingL, canvasHeight + paddingU - VacCalc[i + 1].Spee * scaleY, blueStrokePaint);
                }
            }
            canvasViewPS.InvalidateSurface();
        }

        public float midPoint(float X0, float Y0, float X1, float Y1, float Mid)
        {
            return ((Y1 - Y0) / (X1 - X0) * (Mid - X0) + Y0);
        }

    }
}
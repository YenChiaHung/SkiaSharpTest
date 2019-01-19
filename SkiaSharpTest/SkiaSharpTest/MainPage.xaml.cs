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
        List<PressureSpeed> RowPressureSpeed = new List<PressureSpeed>();
        List<PressureSpeed> RowPressureConductance = new List<PressureSpeed>();
        List<PSTs_node> VacCalc = new List<PSTs_node>();
        SKBitmap savePumpBitmap,saveConductanceBitmap, savePSBitmap,saveTPBitmap ;
        CanvasInfo rowPumpCanvas=new CanvasInfo() ;
        int bitMapNum=1;

        float startPressure, endPressure, chamberVolume, tubeDia, tubeLength;
        int elbowNum;
        int   incrementNum = 100;
        float incrementPress, incrementSpeed;
        float paddingL = 0, paddingR = 0, paddingU = 0, paddingD = 0;
        float canvasWidth, canvasHeight;       

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
                    Maker ="HanBell" , PartNum ="PS902" ,  PSList = new List<PressureSpeed>
                    {
                        new PressureSpeed(){ValueX= 760,     ValueY= 33  },
                        new PressureSpeed(){ValueX= 6.01f,   ValueY= 33  },
                        new PressureSpeed(){ValueX= 6,       ValueY= 257 },
                        new PressureSpeed(){ValueX= 1,       ValueY= 260 },
                        new PressureSpeed(){ValueX= 0.1f,    ValueY= 220 },
                        new PressureSpeed(){ValueX= 0.01f,   ValueY= 164 },
                        new PressureSpeed(){ValueX= 0.001f,  ValueY= 92  },
                        new PressureSpeed(){ValueX= 0.0001f, ValueY= 20  }
                    }
                },
                new PumpInfo ()
                {
                    Maker ="LeyBold" , PartNum ="WSU2001+SV630", PSList = new List<PressureSpeed>
                    {
                        new PressureSpeed(){ValueX= 760,     ValueY= 189.6712f },
                        new PressureSpeed(){ValueX= 608,     ValueY= 189.6712f },
                        new PressureSpeed(){ValueX= 456,     ValueY= 191.5858f },
                        new PressureSpeed(){ValueX= 304,     ValueY= 197.4464f },
                        new PressureSpeed(){ValueX= 152,     ValueY= 218.3074f },
                        new PressureSpeed(){ValueX= 76,      ValueY= 257.656f  },
                        new PressureSpeed(){ValueX= 60.8f,   ValueY= 279.213f  },
                        new PressureSpeed(){ValueX= 45.6f,   ValueY= 310.2672f },
                        new PressureSpeed(){ValueX= 30.4f,   ValueY= 392.8629f },
                        new PressureSpeed(){ValueX= 15.2f,   ValueY= 461.3514f },
                        new PressureSpeed(){ValueX= 7.6f,    ValueY= 480.2635f },
                        new PressureSpeed(){ValueX= 6.08f,   ValueY= 487.5537f },
                        new PressureSpeed(){ValueX= 4.56f,   ValueY= 492.4752f },
                        new PressureSpeed(){ValueX= 3.04f,   ValueY= 499.9508f },
                        new PressureSpeed(){ValueX= 1.52f,   ValueY= 510.0951f },
                        new PressureSpeed(){ValueX= 0.76f,   ValueY= 510.0951f },
                        new PressureSpeed(){ValueX= 0.608f,  ValueY= 510.0951f },
                        new PressureSpeed(){ValueX= 0.456f,  ValueY= 507.5399f },
                        new PressureSpeed(){ValueX= 0.304f,  ValueY= 499.9508f },
                        new PressureSpeed(){ValueX= 0.152f,  ValueY= 482.6814f },
                        new PressureSpeed(){ValueX= 0.076f,  ValueY= 432.1946f },
                        new PressureSpeed(){ValueX= 0.0608f, ValueY= 413.0957f },
                        new PressureSpeed(){ValueX= 0.0456f, ValueY= 383.1213f },
                        new PressureSpeed(){ValueX= 0.0304f, ValueY= 305.6279f }
                    }
                }
            };

            PumpListView.ItemsSource = PumpListData;
        }

      
        public void CanvasView_PressureSpeed(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            rowPumpCanvas.CanvasWidth= canvasWidth = (float)e.Info.Width ;
            rowPumpCanvas.CanvasHeight= canvasHeight = (float)e.Info.Height;

            if (savePumpBitmap == null)
            {
                savePumpBitmap = new SKBitmap(info.Width, info.Height);
            }
            if (savePSBitmap == null)
            {
                savePSBitmap = new SKBitmap(info.Width, info.Height);
            }
            if (saveTPBitmap == null)
            {
                saveTPBitmap = new SKBitmap(info.Width, info.Height);
            }
            if (saveConductanceBitmap == null)
            {
                saveConductanceBitmap = new SKBitmap(info.Width, info.Height);
            }
            canvas.Clear(SKColors.White);

            if (bitMapNum == 1)
            {
                canvas.DrawBitmap(saveTPBitmap, 0, 0);
            }
            if (bitMapNum == 2)
            {
                canvas.DrawBitmap(savePSBitmap, 0, 0);
            }
            if (bitMapNum == 3)
            {
                canvas.DrawBitmap(savePumpBitmap, 0, 0);
            }
            if (bitMapNum == 4)
            {
                canvas.DrawBitmap(saveConductanceBitmap , 0, 0);
            }

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
            var TempList = SelectedPumpInfo.PSList as List<PressureSpeed> ;
            var ListViewID = sender as ViewCell ;

            RowPressureSpeed.Clear();
            RowPressureConductance.Clear();
            VacCalc.Clear();
            for (int i= 0; i < TempList.Count; i++)
            {
                RowPressureSpeed.Add(new PressureSpeed () { ValueX  = TempList[i].ValueX, ValueY = TempList[i].ValueY });
                RowPressureConductance.Add(new PressureSpeed() { ValueX = TempList[i].ValueX, ValueY = 0 });
                VacCalc.Add(new PSTs_node() { Pressur = (float)Math.Log10(TempList[i].ValueX), Spee = TempList[i].ValueY, Tim = 0, Secon = 0 });                 
            }
            savePumpBitmap = DrawXlogYnature(savePumpBitmap, RowPressureSpeed );

            float alpha;
            float viscousBody;
            float molecularBody;
            for (int i = 0; i < RowPressureConductance.Count; i++)
            {
                alpha = 4 * tubeDia / 3 / tubeLength;
                viscousBody = 0.0327f * (float)Math.Pow(tubeDia, 4) * RowPressureConductance[i].ValueX / (tubeLength * 0.0001708f);
                molecularBody = 11.43f * alpha * (float)(Math.Sqrt(293.15 / 28.966f) * Math.Pow(tubeDia / 2f, 2));
                RowPressureConductance[i].ValueY = viscousBody + molecularBody;
            }
            saveConductanceBitmap = DrawXlogYnature(saveConductanceBitmap, RowPressureConductance);

            canvasViewPS.InvalidateSurface();
        }

        public SKBitmap DrawXlogYnature(SKBitmap bitMap, List<PressureSpeed> curveXY)
        {
            //刻度起始值結束值,數據是指數,最小值退位為整數,最大值進位為整數
            rowPumpCanvas.XMax = (float) Math.Log10( curveXY[0].ValueX);
            rowPumpCanvas.XMin = rowPumpCanvas.XMax  ;
            for (int i = 0; i < curveXY.Count; i++ )
            {
                curveXY[i].ValueX=(float) Math.Log10(curveXY[i].ValueX );
                if (rowPumpCanvas.XMax  < curveXY[i].ValueX)
                    rowPumpCanvas.XMax = curveXY[i].ValueX;
                if (rowPumpCanvas.XMin  > curveXY[i].ValueX)
                    rowPumpCanvas.XMin  = curveXY[i].ValueX;
            }
            rowPumpCanvas.XMax = (float)Math.Ceiling(rowPumpCanvas.XMax);
            rowPumpCanvas.XMin  = (float)Math.Floor(rowPumpCanvas.XMin);

            rowPumpCanvas.YMax = curveXY[0].ValueY;
            rowPumpCanvas.YMin = rowPumpCanvas.YMax ;
            for (int i = 0; i < curveXY.Count; i++)
            {
                if (rowPumpCanvas.YMax < curveXY[i].ValueY)
                    rowPumpCanvas.YMax = curveXY[i].ValueY;
                if (rowPumpCanvas.YMin  > curveXY[i].ValueY)
                    rowPumpCanvas.YMin  = curveXY[i].ValueY;
            }
            rowPumpCanvas.YMultiplier = 1f;
            //刻度結束值,將最大值拆成兩數相乘;尾數*乘數;尾數進位為整數
            while (rowPumpCanvas.YMax >= 10f ^ rowPumpCanvas.YMax < 1f)
            {
                if (rowPumpCanvas.YMax >= 10)
                {
                    rowPumpCanvas.YMax = rowPumpCanvas.YMax / 10;
                    rowPumpCanvas.YMultiplier = rowPumpCanvas.YMultiplier * 10;
                }
                if (rowPumpCanvas.YMax < 1)
                {
                    rowPumpCanvas.YMax = rowPumpCanvas.YMax * 10;
                    rowPumpCanvas.YMultiplier = rowPumpCanvas.YMultiplier / 10;
                }
            }
            rowPumpCanvas.YMax = (float)Math.Ceiling(rowPumpCanvas.YMax) * rowPumpCanvas.YMultiplier;

            //開始PS curve
            //
            //計算左邊的留多少文字空間
            float tempFloat = rowPumpCanvas.YMax;
            string tempString = tempFloat.ToString();

            rowPumpCanvas.PaddingL = textPaint.MeasureText(tempString) + textPaint.TextSize / 2f;

            //計算上標題列高
            rowPumpCanvas.PaddingU = textPaint.TextSize * 2f;

            //計算右邊的留多少文字空間             
            tempFloat = (float)Math.Pow(10, rowPumpCanvas.XMax);
            tempString = tempFloat.ToString();
            rowPumpCanvas.PaddingR  = (textPaint.MeasureText(tempString) + textPaint.TextSize) / 4f;

            //計算下刻度列高
            rowPumpCanvas.PaddingD  = textPaint.TextSize * 1.5f;

            rowPumpCanvas.XScale = (rowPumpCanvas.CanvasWidth - rowPumpCanvas.PaddingL - rowPumpCanvas.PaddingR ) / (rowPumpCanvas.XMax - rowPumpCanvas.XMin );
            rowPumpCanvas.YScale = (rowPumpCanvas.CanvasHeight  - rowPumpCanvas.PaddingU - rowPumpCanvas.PaddingD ) / rowPumpCanvas.YMax;

            //用一次性畫布作圖存成saveBitmap
            using (SKCanvas newcanvas = new SKCanvas(bitMap))
            {
                newcanvas.Clear(SKColors.White);
                //畫垂直格線
                for (float i = 0; i < rowPumpCanvas.XMax - rowPumpCanvas.XMin ; i++)
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = 1; j < 10; j++)
                    {
                        //畫小刻度線
                        newcanvas.DrawLine((i + (float)Math.Log10(j)) * rowPumpCanvas.XScale + rowPumpCanvas.PaddingL , rowPumpCanvas.PaddingU , (i + (float)Math.Log10(j)) * rowPumpCanvas.XScale + rowPumpCanvas.PaddingL , rowPumpCanvas.CanvasHeight - rowPumpCanvas.PaddingD , redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(i * rowPumpCanvas.XScale + rowPumpCanvas.PaddingL , rowPumpCanvas.PaddingU , i * rowPumpCanvas.XScale + rowPumpCanvas.PaddingL , rowPumpCanvas.CanvasHeight  - rowPumpCanvas.PaddingD , blackStrokePaint);

                    //刻度值轉文字
                    tempFloat = (float)Math.Pow(10, i + rowPumpCanvas.XMin );
                    tempString = tempFloat.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, i * rowPumpCanvas.XScale + rowPumpCanvas.PaddingL  - textWidth / 2,rowPumpCanvas.CanvasHeight -rowPumpCanvas.PaddingD  + textPaint.TextSize * 1.25f, textPaint);
                }
                newcanvas.DrawLine((rowPumpCanvas.XMax - rowPumpCanvas.XMin ) * rowPumpCanvas.XScale + rowPumpCanvas.PaddingL ,rowPumpCanvas.PaddingU , (rowPumpCanvas.XMax - rowPumpCanvas.XMin ) * rowPumpCanvas.XScale  + rowPumpCanvas.PaddingL , rowPumpCanvas.CanvasHeight  - rowPumpCanvas.PaddingD , blackStrokePaint);

                //畫水平格線
                for (float i = 0; i < rowPumpCanvas.YMax; i = i + rowPumpCanvas.YMultiplier)
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = rowPumpCanvas.YMultiplier / 10; j < rowPumpCanvas.YMultiplier; j = j + rowPumpCanvas.YMultiplier / 10)
                    {
                        newcanvas.DrawLine(rowPumpCanvas.PaddingL , rowPumpCanvas.CanvasHeight - rowPumpCanvas.PaddingD  - (i + j) * rowPumpCanvas.YScale , rowPumpCanvas.CanvasWidth - rowPumpCanvas.PaddingR ,rowPumpCanvas.CanvasHeight - rowPumpCanvas.PaddingD  - (i + j) * rowPumpCanvas.YScale , redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(rowPumpCanvas.PaddingL ,rowPumpCanvas.CanvasHeight -rowPumpCanvas.PaddingD  - i * rowPumpCanvas.YScale , rowPumpCanvas.CanvasWidth  - rowPumpCanvas.PaddingR , rowPumpCanvas.CanvasHeight  - rowPumpCanvas.PaddingD  - i * rowPumpCanvas.YScale , blackStrokePaint);

                    tempString = i.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, rowPumpCanvas.PaddingL  - textWidth - textPaint.TextSize / 4, rowPumpCanvas.CanvasHeight  -rowPumpCanvas.PaddingD  - i * rowPumpCanvas.YScale , textPaint);
                }
                newcanvas.DrawLine(rowPumpCanvas.PaddingL , rowPumpCanvas.CanvasHeight  - rowPumpCanvas.PaddingD  - rowPumpCanvas.YMax * rowPumpCanvas.YScale ,rowPumpCanvas.CanvasWidth - rowPumpCanvas.PaddingR , rowPumpCanvas.CanvasHeight -rowPumpCanvas.PaddingD - rowPumpCanvas.YMax * rowPumpCanvas.YScale , blackStrokePaint);

                //畫標題欄
                tempString = "L/sec VS Torr";
                newcanvas.DrawText(tempString, (rowPumpCanvas.CanvasWidth  - textPaint3X2.MeasureText(tempString)) / 2, textPaint.TextSize * 1.75f, textPaint3X2);

                //畫曲線
                for (int i = 0; i < curveXY .Count - 1; i++)
                {
                    newcanvas.DrawLine((curveXY[i].ValueX - rowPumpCanvas.XMin ) * rowPumpCanvas.XScale + rowPumpCanvas.PaddingL ,rowPumpCanvas.CanvasHeight - rowPumpCanvas.PaddingU  - curveXY[i].ValueY  * rowPumpCanvas.YScale , (curveXY[i + 1].ValueX - rowPumpCanvas.XMin ) * rowPumpCanvas.XScale  + rowPumpCanvas.PaddingL , rowPumpCanvas.CanvasHeight - rowPumpCanvas.PaddingU  - curveXY [i + 1].ValueY * rowPumpCanvas.YScale , blueStrokePaint);
                }
            }
            return bitMap;
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
            VacCalc[0].Spee = MidPoint(VacCalc[0].Pressur, VacCalc[0].Spee, VacCalc[1].Pressur, VacCalc[1].Spee, startPressure);
            VacCalc[0].Pressur = startPressure;
            VacCalc[Idx].Spee = MidPoint(VacCalc[Idx - 1].Pressur, VacCalc[Idx - 1].Spee, VacCalc[Idx].Pressur, VacCalc[Idx].Spee, endPressure);
            VacCalc[Idx].Pressur = endPressure;

            //計算內差的刻度
            incrementPress = (VacCalc[0].Pressur - VacCalc[Idx].Pressur) / incrementNum;
            float speedMax = VacCalc[0].Spee;
            for (int i = 1; i < VacCalc.Count; i++)
            {
                if (speedMax < VacCalc[i].Spee)
                    speedMax = VacCalc[i].Spee;                
            }
            incrementSpeed = speedMax / incrementNum;

            //距離太遠的兩點插入點
            float insX, insY;
            Idx = 1;
            while (Idx < VacCalc.Count)
            {
                //大於1代表兩點距離太寬;
                insX = (VacCalc[Idx - 1].Pressur - VacCalc[Idx].Pressur) / incrementPress;
                insY = Math.Abs(VacCalc[Idx - 1].Spee - VacCalc[Idx].Spee) / incrementSpeed;
                if (insX > 1f ^ insY > 1f)
                {
                    //兩點之間是X比較寬還是Y比較寬
                    if (insX > insY)
                    {
                        insX = VacCalc[Idx - 1].Pressur - incrementPress;
                        insY = MidPoint(VacCalc[Idx - 1].Pressur, VacCalc[Idx - 1].Spee, VacCalc[Idx].Pressur, VacCalc[Idx].Spee, insX);
                        VacCalc.Insert(Idx, new PSTs_node() { Pressur = insX, Spee = insY, Tim = 0, Secon = 0 });
                    }
                    else
                    {
                        //insY已經是絕對值,要重新判斷正負
                        if (VacCalc[Idx - 1].Spee > VacCalc[Idx].Spee)
                        {
                            insY = VacCalc[Idx - 1].Spee - incrementSpeed;
                        }
                        else
                        {
                            insY = VacCalc[Idx - 1].Spee + incrementSpeed;
                        }
                        insX = MidPoint(VacCalc[Idx - 1].Spee, VacCalc[Idx - 1].Pressur, VacCalc[Idx].Spee, VacCalc[Idx].Pressur, insY);
                        VacCalc.Insert(Idx, new PSTs_node() { Pressur = insX, Spee = insY, Tim = 0, Secon = 0 });
                    }
                }
                Idx++;
            }

            //計算區間時間和累計時間
            VacCalc[0].Secon = 0;
            VacCalc[0].Tim = 0;
            for (int i = 1; i < VacCalc.Count; i++)
            {
                VacCalc[i].Secon = chamberVolume * 2 / (VacCalc[i - 1].Spee + VacCalc[i].Spee)*(float) Math.Log(10) *(VacCalc[i-1].Pressur -VacCalc[i].Pressur );
                VacCalc[i].Tim = VacCalc[i - 1].Tim + VacCalc[i].Secon;
            }

            //刻度起始值結束值,數據是指數,最小值退位為整數,最大值進位為整數
            float pressMax = (float)Math.Ceiling(VacCalc[0].Pressur);
            float pressMin = (float)Math.Floor(VacCalc[VacCalc.Count - 1].Pressur);
            
            float speedMultiplier = 1f;
          
            //刻度結束值,將最大值拆成兩數相乘;尾數*乘數;尾數進位為整數
            while (speedMax >= 10f ^ speedMax < 1f)
            {
                if (speedMax >= 10)
                {
                    speedMax = speedMax / 10;
                    speedMultiplier = speedMultiplier * 10;
                }
                if (speedMax < 1)
                {
                    speedMax = speedMax * 10;
                    speedMultiplier = speedMultiplier / 10;
                }
            }
            speedMax = (float)Math.Ceiling(speedMax) * speedMultiplier;

            float timMax = VacCalc[VacCalc.Count - 1].Tim;
            float timMultiplier = 1f;
            while (timMax >= 10f ^ timMax < 1f)
            {
                if (timMax >= 10)
                {
                    timMax = timMax / 10;
                    timMultiplier = timMultiplier * 10;
                }
                if (timMax < 1)
                {
                    timMax = timMax * 10;
                    timMultiplier = timMultiplier / 10;
                }
            }
            timMax = (float)Math.Ceiling(timMax) * timMultiplier;

            //開始PS curve
            //
            //計算左邊的留多少文字空間
            float tempFloat = speedMax;
            string tempString = tempFloat.ToString();

            paddingL = textPaint.MeasureText(tempString) + textPaint.TextSize / 2f;

            //計算上標題列高
            paddingU = textPaint.TextSize * 2f;

            //計算右邊的留多少文字空間             
            tempFloat = (float)Math.Pow(10, pressMax);
            tempString = tempFloat.ToString();
            paddingR = (textPaint.MeasureText(tempString) + textPaint.TextSize) / 4f;

            //計算下刻度列高
            paddingD = textPaint.TextSize * 1.5f;

            float scaleX = (canvasWidth - paddingL - paddingR) / (pressMax - pressMin);
            float scaleY = (canvasHeight - paddingU - paddingD) / speedMax;

            //用一次性畫布作圖存成saveBitmap
            using (SKCanvas newcanvas = new SKCanvas(savePSBitmap))
            {
                newcanvas.Clear(SKColors.White );
                //畫垂直格線
                for (float i = 0; i < pressMax - pressMin; i++)
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
                    tempFloat = (float) Math.Pow(10, i + pressMin); 
                    tempString = tempFloat.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, i * scaleX + paddingL - textWidth / 2, canvasHeight - paddingD + textPaint.TextSize * 1.25f, textPaint);
                }
                newcanvas.DrawLine((pressMax - pressMin) * scaleX + paddingL, paddingU, (pressMax - pressMin) * scaleX + paddingL, canvasHeight - paddingD, blackStrokePaint);

                //畫水平格線
                for (float i = 0; i < speedMax ; i = i + speedMultiplier)
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = speedMultiplier/10; j < speedMultiplier; j = j + speedMultiplier/10)
                    {
                        newcanvas.DrawLine(paddingL, canvasHeight - paddingD - (i + j) * scaleY, canvasWidth - paddingR, canvasHeight - paddingD - (i + j) * scaleY, redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(paddingL, canvasHeight - paddingD - i * scaleY, canvasWidth - paddingR, canvasHeight - paddingD - i * scaleY, blackStrokePaint);

                    tempString = i.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, paddingL - textWidth - textPaint.TextSize / 4, canvasHeight- paddingD - i * scaleY, textPaint);
                }
                newcanvas.DrawLine(paddingL, canvasHeight - paddingD - speedMax * scaleY, canvasWidth - paddingR, canvasHeight - paddingD - speedMax  * scaleY, blackStrokePaint);

                //畫標題欄
                tempString = "L/sec VS Torr";
                newcanvas.DrawText(tempString, (canvasWidth - textPaint3X2.MeasureText(tempString)) / 2, textPaint.TextSize * 1.75f, textPaint3X2);

                //畫曲線
                for (int i = 0; i < VacCalc.Count - 1; i++)
                {
                    newcanvas.DrawLine((VacCalc[i].Pressur - pressMin) * scaleX + paddingL, canvasHeight - paddingU - VacCalc[i].Spee * scaleY, (VacCalc[i + 1].Pressur - pressMin) * scaleX + paddingL, canvasHeight - paddingU - VacCalc[i + 1].Spee * scaleY, blueStrokePaint);
                }                
            }










            //
            //開始TP curve
            //
            //計算左邊的留多少文字空間
            tempFloat = (float)Math.Pow(10, pressMax);
            tempString = tempFloat.ToString();
            paddingL = textPaint.MeasureText(tempString) + textPaint.TextSize / 2f;

            tempFloat = (float)Math.Pow(10, pressMin);
            tempString = tempFloat.ToString();
            tempFloat = textPaint.MeasureText(tempString) + textPaint.TextSize / 2f;
            if (tempFloat > paddingL) paddingL = tempFloat;


            //計算上標題列高
            paddingU = textPaint.TextSize * 2f;

            //計算右邊的留多少文字空間             
            tempFloat = VacCalc[VacCalc.Count-1].Tim ;
            tempString = tempFloat.ToString();
            paddingR = (textPaint.MeasureText(tempString) + textPaint.TextSize) / 4f;

            //計算下刻度列高
            paddingD = textPaint.TextSize * 1.5f;

            scaleX = (canvasWidth - paddingL - paddingR) / timMax ;
            scaleY = (canvasHeight - paddingU - paddingD) / (pressMax - pressMin);

            //用一次性畫布作圖存成saveBitmap
            using (SKCanvas newcanvas = new SKCanvas(saveTPBitmap))
            {
                newcanvas.Clear(SKColors.White);
                //畫垂直格線
                for (float i = 0; i < timMax; i = i + timMultiplier)
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = timMultiplier / 10; j < timMultiplier; j = j + timMultiplier / 10)
                    {
                        newcanvas.DrawLine(paddingL+(i+j)*scaleX ,paddingU , paddingL + (i + j) * scaleX, canvasHeight - paddingD, redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(paddingL + i * scaleX, paddingU, paddingL + i * scaleX, canvasHeight - paddingD, blackStrokePaint);

                    tempString = i.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, i * scaleX + paddingL - textWidth / 2, canvasHeight - paddingD + textPaint.TextSize * 1.25f, textPaint);
                }               
                newcanvas.DrawLine(timMax * scaleX + paddingL, paddingU, timMax  * scaleX + paddingL, canvasHeight - paddingD, blackStrokePaint);

                //畫水平格線
                for (float i = 0; i < pressMax - pressMin; i++)
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = 1; j < 10; j++)
                    {
                        newcanvas.DrawLine(paddingL,canvasHeight- paddingD  - (i + (float)Math.Log10(j)) * scaleY,canvasWidth - paddingR, canvasHeight - paddingD - (i + (float)Math.Log10(j)) * scaleY, redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(paddingL, canvasHeight - paddingD - i * scaleY,canvasWidth - paddingR, canvasHeight - paddingD - i * scaleY, blackStrokePaint);

                    //刻度值轉文字
                    tempFloat = (float)Math.Pow(10, i + pressMin);
                    tempString = tempFloat.ToString();
                    float textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, paddingL - textWidth - textPaint.TextSize / 4, canvasHeight - paddingD - i * scaleY, textPaint);
                }
                newcanvas.DrawLine(paddingL, canvasHeight - paddingD - (pressMax-pressMin)  * scaleY,canvasWidth - paddingR, canvasHeight - paddingD - (pressMax-pressMin)  * scaleY, blackStrokePaint);

                //畫標題欄
                tempString = "Torr VS Second";
                newcanvas.DrawText(tempString, (canvasWidth - textPaint3X2.MeasureText(tempString)) / 2, textPaint.TextSize * 1.75f, textPaint3X2);

                //畫曲線
                for (int i = 0; i < VacCalc.Count - 1; i++)
                {
                    newcanvas.DrawLine(VacCalc[i].Tim  * scaleX + paddingL, canvasHeight - paddingU - (VacCalc[i].Pressur-pressMin )  * scaleY, VacCalc[i + 1].Tim * scaleX + paddingL, canvasHeight - paddingU - (VacCalc[i + 1].Pressur -pressMin )* scaleY, blueStrokePaint);
                }

                canvasViewPS.InvalidateSurface();
            }          
        }

        public void NextPage_ButtenClicked(object sender, EventArgs e)
        {
            double entryDouble = 0;
            int entryInt = 0;

            //chamber info entry
            if (bitMapNum == 1)
            {
                try
                {
                    Double.TryParse(entryPHigh.Text, out entryDouble);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Pressure High", "Cancel");
                }
                startPressure = (float)Math.Log10(entryDouble);

                try
                {
                    Double.TryParse(entryPLow.Text, out entryDouble);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Pressure Low", "Cancel");
                }
                endPressure = (float)Math.Log10(entryDouble);

                try
                {
                    Double.TryParse(entryVolume.Text, out entryDouble);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Chamber Volume", "Cancel");
                }
                chamberVolume = (float)entryDouble;
          
                ChamberInfoGrid1.IsVisible  = false;
                TubeInfoGrid1.IsVisible = true;
                PumpListView.IsVisible = false;
            }

            //Tube info entry
            if (bitMapNum == 2)
            {
                try
                {
                    Double.TryParse(entryDia.Text, out entryDouble);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Chamber Volume", "Cancel");
                }
                tubeDia = (float)entryDouble;

                try
                {
                    Double.TryParse(entryLength.Text, out entryDouble);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Chamber Volume", "Cancel");
                }
                tubeLength = (float)entryDouble;

                try
                {
                    int.TryParse(entryLength.Text, out entryInt);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Chamber Volume", "Cancel");
                }
                elbowNum = entryInt;

                ChamberInfoGrid1.IsVisible = false;
                TubeInfoGrid1.IsVisible = false;
                PumpListView.IsVisible = true;
            }

            //Pump select
            if (bitMapNum == 3)
            {
                ChamberInfoGrid1.IsVisible = true;
                TubeInfoGrid1.IsVisible = false;
                PumpListView.IsVisible = false;
            }
            if (bitMapNum == 4)
            {
               
                ChamberInfoGrid1.IsVisible = true;
                TubeInfoGrid1.IsVisible = false;
                PumpListView.IsVisible = false;
            }

            canvasViewPS.InvalidateSurface();
            bitMapNum++;
            if (bitMapNum > 4 ^ bitMapNum < 1)
                bitMapNum = 1;
        }

        public float MidPoint(float X0, float Y0, float X1, float Y1, float Mid)
        {
            return ((Y1 - Y0) / (X1 - X0) * (Mid - X0) + Y0);
        }

    }
}
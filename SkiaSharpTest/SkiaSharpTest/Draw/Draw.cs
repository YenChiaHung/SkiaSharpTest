using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharpTest.Models;

namespace SkiaSharpTest.Draw
{
    public class DrawGrid
    {
        public void Init()
        {
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
            blackStrokePaint.StrokeWidth = App.ScreenDensity ;
            redStrokePaint.StrokeWidth = App.ScreenDensity ;
            blueStrokePaint.StrokeWidth = App.ScreenDensity*2;
        }
        public CanvasInfo FormXlogYnature(CanvasInfo cI, List<PressureSpeed> curveXY)
        {
            //刻度起始值結束值,數據是指數,最小值退位為整數,最大值進位為整數
            cI.XMax = (float)Math.Log10(curveXY[0].ValueX);
            cI.XMin = cI.XMax;
            for (int i = 0; i < curveXY.Count; i++)
            {
                //curveXY[i].ValueX = (float)Math.Log10(curveXY[i].ValueX);
                if (cI.XMax < (float)Math.Log10(curveXY[i].ValueX))
                    cI.XMax = (float)Math.Log10(curveXY[i].ValueX);
                if (cI.XMin > (float)Math.Log10(curveXY[i].ValueX))
                    cI.XMin = (float)Math.Log10(curveXY[i].ValueX);
            }
            cI.XMax = (float)Math.Ceiling(cI.XMax);
            cI.XMin = (float)Math.Floor(cI.XMin);

            cI.YMax = curveXY[0].ValueY;
            cI.YMin = cI.YMax;
            for (int i = 0; i < curveXY.Count; i++)
            {
                if (cI.YMax < curveXY[i].ValueY)
                    cI.YMax = curveXY[i].ValueY;
                if (cI.YMin > curveXY[i].ValueY)
                    cI.YMin = curveXY[i].ValueY;
            }
            cI.YMultiplier = 1f;
            //刻度結束值,將最大值拆成兩數相乘;尾數*乘數;尾數進位為整數
            while (cI.YMax >= 10f ^ cI.YMax < 1f)
            {
                if (cI.YMax >= 10)
                {
                    cI.YMax = cI.YMax / 10;
                    cI.YMultiplier = cI.YMultiplier * 10;
                }
                if (cI.YMax < 1)
                {
                    cI.YMax = cI.YMax * 10;
                    cI.YMultiplier = cI.YMultiplier / 10;
                }
            }
            cI.YMax = (float)Math.Ceiling(cI.YMax) * cI.YMultiplier;

            //開始PS curve
            //
            //計算左邊的留多少文字空間
            float tempFloat = cI.YMax;
            string tempString = tempFloat.ToString();

            cI.PaddingL = textPaint.MeasureText(tempString) + textPaint.TextSize / 2f;

            //計算上標題列高
            cI.PaddingU = textPaint.TextSize * 2f;

            //計算右邊的留多少文字空間             
            tempFloat = (float)Math.Pow(10, cI.XMax);
            tempString = tempFloat.ToString();
            cI.PaddingR = (textPaint.MeasureText(tempString) + textPaint.TextSize) / 2f;

            //計算下刻度列高
            cI.PaddingD = textPaint.TextSize * 1.5f;

            cI.XScale = (cI.CanvasWidth - cI.PaddingL - cI.PaddingR) / (cI.XMax - cI.XMin);
            cI.YScale = (cI.CanvasHeight - cI.PaddingU - cI.PaddingD) / cI.YMax;
            using (SKCanvas newcanvas = new SKCanvas(cI.SaveBitMap))
            {
                float textWidth;
                newcanvas.Clear(SKColors.White);
                //畫垂直格線

                for (float i = 0; i < cI.XMax - cI.XMin; i++)
                {                    
                    for (float j = 1; j < 10; j++)
                    {
                        //畫小刻度線
                        newcanvas.DrawLine((i + (float)Math.Log10(j)) * cI.XScale + cI.PaddingL, cI.PaddingU, (i + (float)Math.Log10(j)) * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD, redStrokePaint);
                    }

                    newcanvas.DrawLine(i * cI.XScale + cI.PaddingL, cI.PaddingU, i * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD, blackStrokePaint);

                    //刻度值轉文字
                    tempFloat = (float)Math.Pow(10, i + cI.XMin);
                    tempString = tempFloat.ToString();
                    textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, i * cI.XScale + cI.PaddingL - textWidth / 2, cI.CanvasHeight - cI.PaddingD + textPaint.TextSize, textPaint);
                }
                newcanvas.DrawLine(cI.CanvasWidth -cI.PaddingR , cI.PaddingU,cI.CanvasWidth -cI.PaddingR , cI.CanvasHeight - cI.PaddingD, blackStrokePaint);
                tempFloat = (float)Math.Pow(10, cI.XMax);
                tempString = tempFloat.ToString();
                textWidth = textPaint.MeasureText(tempString);
                newcanvas.DrawText(tempString, cI.CanvasWidth - cI.PaddingR - textWidth / 2, cI.CanvasHeight - cI.PaddingD + textPaint.TextSize , textPaint);

                //畫水平格線
                for (float i = 0; i < cI.YMax; i = i + cI.YMultiplier)
                {                   
                    for (float j = cI.YMultiplier / 10; j < cI.YMultiplier; j = j + cI.YMultiplier / 10)
                    {
                        newcanvas.DrawLine(cI.PaddingL, cI.CanvasHeight - cI.PaddingD - (i + j) * cI.YScale, cI.CanvasWidth - cI.PaddingR, cI.CanvasHeight - cI.PaddingD - (i + j) * cI.YScale, redStrokePaint);
                    }                  

                    newcanvas.DrawLine(cI.PaddingL, cI.CanvasHeight - cI.PaddingD - i * cI.YScale, cI.CanvasWidth - cI.PaddingR, cI.CanvasHeight - cI.PaddingD - i * cI.YScale, blackStrokePaint);

                    tempString = i.ToString();
                    textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, cI.PaddingL - textWidth - textPaint.TextSize / 4, cI.CanvasHeight - cI.PaddingD - i * cI.YScale, textPaint);
                }
                newcanvas.DrawLine(cI.PaddingL, cI.PaddingU, cI.CanvasWidth - cI.PaddingR, cI.PaddingU, blackStrokePaint);
                tempFloat = cI.YMax;
                tempString = tempFloat.ToString();
                textWidth = textPaint.MeasureText(tempString);
                newcanvas.DrawText(tempString, cI.PaddingL - textWidth - textPaint.TextSize / 4, cI.PaddingU, textPaint);

                //畫標題欄
                tempString = "L/sec VS Torr";
                newcanvas.DrawText(tempString, (cI.CanvasWidth - textPaint3X2.MeasureText(tempString)) / 2, textPaint.TextSize * 1.75f, textPaint3X2);

            }
            return cI;
        }


        public CanvasInfo FormXlogYlog(CanvasInfo cI, List<PressureSpeed> curveXY)
        {
            cI.XMin = cI.XMax = (float)Math.Log10(curveXY[0].ValueX);
            cI.YMin = cI.YMax = (float)Math.Log10(curveXY[0].ValueY);
            for (int i = 0; i < curveXY.Count; i++)
            {
                if (cI.XMax < (float)Math.Log10(curveXY[i].ValueX))
                    cI.XMax = (float)Math.Log10(curveXY[i].ValueX);
                if (cI.XMin > (float)Math.Log10(curveXY[i].ValueX))
                    cI.XMin = (float)Math.Log10(curveXY[i].ValueX);

                if (cI.YMax < (float)Math.Log10(curveXY[i].ValueY))
                    cI.YMax = (float)Math.Log10(curveXY[i].ValueY);
                if (cI.YMin > (float)Math.Log10(curveXY[i].ValueY))
                    cI.YMin = (float)Math.Log10(curveXY[i].ValueY);
            }
            cI.XMax = (float)Math.Ceiling(cI.XMax);
            cI.XMin = (float)Math.Floor(cI.XMin);
            cI.YMax = (float)Math.Ceiling(cI.YMax);
            cI.YMin = (float)Math.Floor(cI.YMin);
                                  
            //計算左邊的留多少文字空間
            float tempFloat = (float) Math.Pow(10, cI.YMax);
            string tempString = tempFloat.ToString("F0");

            cI.PaddingL = textPaint.MeasureText(tempString) + textPaint.TextSize / 2f;

            //計算上標題列高
            cI.PaddingU = textPaint.TextSize * 2f;

            //計算右邊的留多少文字空間             
            tempFloat = (float)Math.Pow(10, cI.XMax);
            tempString = tempFloat.ToString();
            cI.PaddingR = (textPaint.MeasureText(tempString) + textPaint.TextSize) / 2f;

            //計算下刻度列高
            cI.PaddingD = textPaint.TextSize * 1.5f;

            cI.XScale = (cI.CanvasWidth - cI.PaddingL - cI.PaddingR) / (cI.XMax - cI.XMin);
            cI.YScale = (cI.CanvasHeight - cI.PaddingU - cI.PaddingD) / (cI.YMax - cI.YMin);

            using (SKCanvas newcanvas = new SKCanvas(cI.SaveBitMap))
            {
                float textWidth;
                newcanvas.Clear(SKColors.White);
                //畫垂直格線
                for (float i = 0; i < cI.XMax - cI.XMin; i++)
                {                    
                    for (float j = 1; j < 10; j++)
                    {
                        //畫小刻度線
                        newcanvas.DrawLine((i + (float)Math.Log10(j)) * cI.XScale + cI.PaddingL, cI.PaddingU, (i + (float)Math.Log10(j)) * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD, redStrokePaint);
                    }

                    newcanvas.DrawLine(i * cI.XScale + cI.PaddingL, cI.PaddingU, i * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD, blackStrokePaint);

                    //刻度值轉文字
                    tempFloat = (float)Math.Pow(10, i + cI.XMin);
                    tempString = tempFloat.ToString();
                    textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, i * cI.XScale + cI.PaddingL - textWidth / 2, cI.CanvasHeight - cI.PaddingD + textPaint.TextSize , textPaint);
                }
                newcanvas.DrawLine(cI.CanvasWidth - cI.PaddingR, cI.PaddingU, cI.CanvasWidth - cI.PaddingR, cI.CanvasHeight - cI.PaddingD, blackStrokePaint);
                tempFloat = (float)Math.Pow(10, cI.XMax);
                tempString = tempFloat.ToString();
                textWidth = textPaint.MeasureText(tempString);
                newcanvas.DrawText(tempString, cI.CanvasWidth - cI.PaddingR - textWidth / 2, cI.CanvasHeight - cI.PaddingD + textPaint.TextSize, textPaint);
                
                //畫水平格線
                for (float i = 0; i < cI.YMax - cI.YMin; i++)
                {
                    for (float j = 1; j < 10; j++)
                    {
                        //畫小刻度線
                        newcanvas.DrawLine(cI.PaddingL, cI.CanvasHeight - cI.PaddingD - (i + (float) Math.Log10( j)) * cI.YScale, cI.CanvasWidth - cI.PaddingR, cI.CanvasHeight - cI.PaddingD - (i + (float) Math.Log10( j)) * cI.YScale, redStrokePaint);
                    }

                    newcanvas.DrawLine(cI.PaddingL, cI.CanvasHeight - cI.PaddingD - i * cI.YScale, cI.CanvasWidth - cI.PaddingR, cI.CanvasHeight - cI.PaddingD - i * cI.YScale, blackStrokePaint);

                    //刻度值轉文字
                    tempFloat = (float)Math.Pow(10, i + cI.YMin);
                    tempString = tempFloat.ToString();
                    textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, cI.PaddingL - textWidth - textPaint.TextSize / 4, cI.CanvasHeight - cI.PaddingD - i * cI.YScale, textPaint);
                }
                newcanvas.DrawLine(cI.PaddingL, cI.PaddingU , cI.CanvasWidth - cI.PaddingR,cI.PaddingU , blackStrokePaint);
                tempFloat = (float)Math.Pow(10, cI.YMax);
                tempString = tempFloat.ToString("F0");
                textWidth = textPaint.MeasureText(tempString);
                newcanvas.DrawText(tempString, cI.PaddingL - textWidth - textPaint.TextSize / 4, cI.PaddingU, textPaint);

                //畫標題欄
                tempString = "L/sec VS Torr";
                newcanvas.DrawText(tempString, (cI.CanvasWidth - textPaint3X2.MeasureText(tempString)) / 2, textPaint.TextSize * 1.75f, textPaint3X2);
            }
            return cI;
        }


        public CanvasInfo FormXnatureYlog(CanvasInfo cI, List<PressureSpeed> curveXY)
        {            
            cI.XMax = cI.XMin = curveXY[0].ValueX;;
            for (int i = 0; i < curveXY.Count; i++)
            {
                if (cI.XMax < curveXY[i].ValueX)
                    cI.XMax = curveXY[i].ValueX;
                if (cI.XMin > curveXY[i].ValueX)
                    cI.XMin = curveXY[i].ValueX;
            }
            cI.XMultiplier = 1f;
            //刻度結束值,將最大值拆成兩數相乘;尾數*乘數;尾數進位為整數
            while (cI.XMax >= 10f ^ cI.XMax < 1f)
            {
                if (cI.XMax >= 10)
                {
                    cI.XMax = cI.XMax / 10;
                    cI.XMultiplier = cI.XMultiplier * 10;
                }
                if (cI.XMax < 1)
                {
                    cI.XMax = cI.XMax * 10;
                    cI.XMultiplier = cI.XMultiplier / 10;
                }
            }
            cI.XMax = (float)Math.Ceiling(cI.XMax) * cI.XMultiplier;

            //刻度起始值結束值,數據是指數,最小值退位為整數,最大值進位為整數
            cI.YMax = cI.YMin = (float)Math.Log10(curveXY[0].ValueY);
            for (int i = 0; i < curveXY.Count; i++)
            {
                //curveXY[i].ValueX = (float)Math.Log10(curveXY[i].ValueX);
                if (cI.YMax < (float)Math.Log10(curveXY[i].ValueY))
                    cI.YMax = (float)Math.Log10(curveXY[i].ValueY);
                if (cI.YMin > (float)Math.Log10(curveXY[i].ValueY))
                    cI.YMin = (float)Math.Log10(curveXY[i].ValueY);
            }
            cI.YMax = (float)Math.Ceiling(cI.YMax);
            cI.YMin = (float)Math.Floor(cI.YMin);

            //開始PS curve
            //
            //計算左邊的留多少文字空間
            float tempFloat;
            tempFloat = (float) Math.Pow(10, cI.YMax);
            string tempString = tempFloat.ToString();
            tempFloat = (float)Math.Pow(10, cI.YMin);
            string tempString1 = tempFloat.ToString();
            tempFloat = Math.Max(textPaint.MeasureText(tempString ) , textPaint.MeasureText(tempString1));

            cI.PaddingL = tempFloat + textPaint.TextSize / 2f;

            //計算上標題列高
            cI.PaddingU = textPaint.TextSize * 2f;

            //計算右邊的留多少文字空間             
            tempFloat = cI.XMax;
            tempString = tempFloat.ToString();
            cI.PaddingR = (textPaint.MeasureText(tempString) + textPaint.TextSize) / 2f;

            //計算下刻度列高
            cI.PaddingD = textPaint.TextSize * 1.5f;

            cI.XScale = (cI.CanvasWidth - cI.PaddingL - cI.PaddingR) / cI.XMax;
            cI.YScale = (cI.CanvasHeight - cI.PaddingU - cI.PaddingD) / (cI.YMax -cI.YMin );
            using (SKCanvas newcanvas = new SKCanvas(cI.SaveBitMap))
            {
                float textWidth;
                newcanvas.Clear(SKColors.White);
                //畫垂直格線
                for (float i = 0; i < cI.XMax ; i = i + cI.XMultiplier )
                {
                    for (float j = cI.XMultiplier / 10; j < cI.XMultiplier ; j = j + cI.XMultiplier / 10)
                    {
                        newcanvas.DrawLine(cI.PaddingL + ( i + j ) * cI.XScale ,cI.PaddingU ,cI.PaddingL + (i + j) * cI.XScale ,cI.CanvasHeight - cI.PaddingD , redStrokePaint);
                    }
                    newcanvas.DrawLine(cI.PaddingL + i * cI.XScale, cI.PaddingU, cI.PaddingL + i * cI.XScale, cI.CanvasHeight - cI.PaddingD, blackStrokePaint);

                    tempString = i.ToString();
                    textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, i * cI.XScale + cI.PaddingL - textWidth / 2, cI.CanvasHeight - cI.PaddingD + textPaint.TextSize , textPaint);
                }               
                newcanvas.DrawLine(cI.XMax * cI.XScale + cI.PaddingL, cI.PaddingU, cI.XMax  * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD, blackStrokePaint);
                tempFloat = cI.XMax;
                tempString = tempFloat.ToString();
                textWidth = textPaint.MeasureText(tempString);
                newcanvas.DrawText(tempString, cI.CanvasWidth - cI.PaddingR - textWidth / 2, cI.CanvasHeight - cI.PaddingD + textPaint.TextSize, textPaint);

                //畫水平格線
                for (float i = 0; i < cI.YMax - cI.YMin ; i++)
                {
                    SKColor colorSave = blackStrokePaint.Color;
                    blackStrokePaint.Color = SKColors.Red;
                    for (float j = 1; j < 10; j++)
                    {
                        newcanvas.DrawLine(cI.PaddingL, cI.CanvasHeight- cI.PaddingD - (i + (float)Math.Log10(j)) * cI.YScale, cI.CanvasWidth - cI.PaddingR, cI.CanvasHeight - cI.PaddingD - (i + (float)Math.Log10(j)) * cI.YScale, redStrokePaint);
                    }
                    blackStrokePaint.Color = colorSave;

                    newcanvas.DrawLine(cI.PaddingL, cI.CanvasHeight - cI.PaddingD - i * cI.YScale, cI.CanvasWidth - cI.PaddingR, cI.CanvasHeight - cI.PaddingD - i * cI.YScale, blackStrokePaint);

                    //刻度值轉文字
                    tempFloat = (float)Math.Pow(10, i + cI.YMin);
                    tempString = tempFloat.ToString();
                    textWidth = textPaint.MeasureText(tempString);
                    newcanvas.DrawText(tempString, cI.PaddingL - textWidth - textPaint.TextSize / 4, cI.CanvasHeight - cI.PaddingD - i * cI.YScale, textPaint);
                }
                newcanvas.DrawLine(cI.PaddingL, cI.PaddingU, cI.CanvasWidth - cI.PaddingR,cI.PaddingU, blackStrokePaint);
                tempFloat = (float)Math.Pow(10, cI.YMax);
                tempString = tempFloat.ToString("F0");
                textWidth = textPaint.MeasureText(tempString);
                newcanvas.DrawText(tempString, cI.PaddingL - textWidth - textPaint.TextSize / 4, cI.PaddingU, textPaint);

                //畫標題欄
                tempString = "Torr VS Second";
                newcanvas.DrawText(tempString, (cI.CanvasWidth - textPaint3X2.MeasureText(tempString)) / 2, textPaint.TextSize * 1.75f, textPaint3X2);

            }
            return cI;
        }

        public CanvasInfo CurveXlogYnature(CanvasInfo cI, List<PressureSpeed> curveXY)
        {
            //用一次性畫布作圖存成saveBitmap
            using (SKCanvas newcanvas = new SKCanvas(cI.SaveBitMap))
            {
                //畫曲線
                for (int i = 0; i < curveXY.Count - 1; i++)
                {
                    newcanvas.DrawLine(((float)Math.Log10(curveXY[i].ValueX) - cI.XMin) * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD - curveXY[i].ValueY * cI.YScale, ((float)Math.Log10(curveXY[i + 1].ValueX) - cI.XMin) * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD - curveXY[i + 1].ValueY * cI.YScale, blueStrokePaint);
                }
            }
            return cI;
        }

        public CanvasInfo CurveXlogYlog(CanvasInfo cI, List<PressureSpeed> curveXY)
        {
            //用一次性畫布作圖存成saveBitmap
            using (SKCanvas newcanvas = new SKCanvas(cI.SaveBitMap))
            {
                //畫曲線
                for (int i = 0; i < curveXY.Count - 1; i++)
                {
                    newcanvas.DrawLine(((float)Math.Log10(curveXY[i].ValueX) - cI.XMin) * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD - ((float)Math.Log10(curveXY[i].ValueY) - cI.YMin ) * cI.YScale, ((float)Math.Log10(curveXY[i + 1].ValueX) - cI.XMin) * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD - ((float)Math.Log10(curveXY[i + 1].ValueY) - cI.YMin ) * cI.YScale, blueStrokePaint);
                }
            }
            return cI;
        }

        public CanvasInfo CurveXnatureYlog(CanvasInfo cI, List<PressureSpeed> curveXY)
        {
            //用一次性畫布作圖存成saveBitmap
            using (SKCanvas newcanvas = new SKCanvas(cI.SaveBitMap))
            {
                //畫曲線
                for (int i = 0; i < curveXY.Count - 1; i++)
                {
                    newcanvas.DrawLine(curveXY[i].ValueX * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD - ((float)Math.Log10(curveXY[i].ValueY) - cI.YMin) * cI.YScale, curveXY[i + 1].ValueX * cI.XScale + cI.PaddingL, cI.CanvasHeight - cI.PaddingD - ((float)Math.Log10(curveXY[i + 1].ValueY) - cI.YMin) * cI.YScale, blueStrokePaint);
                }
            }
            return cI;
        }
        SKPaint textPaint3X2 = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 18
        };
        SKPaint textPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 6
        };
        SKPaint blackStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 1f,
            StrokeCap = SKStrokeCap.Butt
            //PathEffect = SKPathEffect.CreateDash(new int[]{1, 2}, 20);
        };
        SKPaint redStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = 1f,
            StrokeCap = SKStrokeCap.Butt
            //PathEffect = SKPathEffect.CreateDash(new int[]{1, 2}, 20);
        };
        SKPaint blueStrokePaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 1f,
            StrokeCap = SKStrokeCap.Butt
        };
    }
}

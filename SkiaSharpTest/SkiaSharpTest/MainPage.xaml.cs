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
using SkiaSharpTest.Draw;

namespace SkiaSharpTest
{
    public partial class MainPage : ContentPage
    {                                      
        CanvasInfo rowPumpCanvas = new CanvasInfo();
        List<PressureSpeed> RowPumpingSpeed = new List<PressureSpeed>();
        List<PressureSpeed> RowDeliverySpeed = new List<PressureSpeed>();

        CanvasInfo rowTubeCanvas = new CanvasInfo();
        List<PressureSpeed> RowConductance = new List<PressureSpeed>();

        CanvasInfo finePumpCanvas = new CanvasInfo();
        List<PressureSpeed> FinePumpingSpeed = new List<PressureSpeed>();
        List<PressureSpeed> FineConductance = new List<PressureSpeed>();
        List<PressureSpeed> FineDeliverySpeed = new List<PressureSpeed>();
        List<PressureSpeed> FinePDTimeNoL = new List<PressureSpeed>();
        List<PressureSpeed> FinePDTimeLoss = new List<PressureSpeed>();
        List<PressureSpeed> FinePDTimeOutG = new List<PressureSpeed>();

        int bitMapNum=1;

        float startPressure=760, endPressure=0.05f, chamberVolume, tubeDia, tubeLength;
        int elbowNum;
        int   incrementNum = 100;
        float incrementPress, incrementSpeed;              

        ObservableCollection<PumpInfo> PumpListData;
        DrawGrid drawGrid = new DrawGrid();

        public MainPage()
        {                    
            InitializeComponent();
            drawGrid.Init();
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

            rowPumpCanvas.CanvasWidth = rowTubeCanvas.CanvasWidth = finePumpCanvas.CanvasWidth = (float)e.Info.Width ;
            
            rowPumpCanvas.CanvasHeight = rowTubeCanvas.CanvasHeight = finePumpCanvas.CanvasHeight = (float)e.Info.Height;

            if (rowPumpCanvas.SaveBitMap == null)
            {
                rowPumpCanvas.SaveBitMap = new SKBitmap(info.Width, info.Height);
            }                       
            if (rowTubeCanvas.SaveBitMap == null)
            {
                rowTubeCanvas.SaveBitMap = new SKBitmap(info.Width, info.Height);
            }
            if (finePumpCanvas.SaveBitMap == null)
            {
                finePumpCanvas.SaveBitMap = new SKBitmap(info.Width, info.Height);
            }


            canvas.Clear(SKColors.White);

            if (bitMapNum == 1)
            {
                canvas.DrawBitmap(rowPumpCanvas.SaveBitMap, 0, 0);
            }
            if (bitMapNum == 2)
            {
                canvas.DrawBitmap(rowPumpCanvas.SaveBitMap, 0, 0);
            }
            if (bitMapNum == 3)
            {
                canvas.DrawBitmap(rowPumpCanvas.SaveBitMap, 0, 0);
            }
            if (bitMapNum == 4)
            {
                canvas.DrawBitmap(rowTubeCanvas.SaveBitMap, 0, 0);
            }
            if (bitMapNum == 5)
            {
                canvas.DrawBitmap(finePumpCanvas.SaveBitMap, 0, 0);
            }
        }

        private void PumpListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var SelectedPumpInfo = PumpListView.SelectedItem  as PumpInfo;
            var TempList = SelectedPumpInfo.PSList as List<PressureSpeed> ;
            var ListViewID = sender as ViewCell ;

            RowPumpingSpeed.Clear();
            RowDeliverySpeed.Clear();
            RowConductance.Clear();
            FinePumpingSpeed.Clear();
            FinePDTimeNoL.Clear();
            FinePDTimeLoss.Clear();
            FinePDTimeOutG.Clear();
            //page3
            //
            //載入Puming curve
            for (int i= 0; i< TempList.Count; i++)
            {
                RowPumpingSpeed.Add(new PressureSpeed () { ValueX = TempList[i].ValueX, ValueY = TempList[i].ValueY });
                RowDeliverySpeed.Add(new PressureSpeed() { ValueX = TempList[i].ValueX, ValueY = 0 });
                RowConductance.Add(new PressureSpeed() { ValueX = TempList[i].ValueX, ValueY = 0 });
                FinePumpingSpeed.Add(new PressureSpeed() { ValueX = TempList[i].ValueX, ValueY = TempList[i].ValueY });
            }
            rowPumpCanvas = drawGrid.FormXlogYnature(rowPumpCanvas, RowPumpingSpeed);
            rowPumpCanvas = drawGrid.CurveXlogYnature(rowPumpCanvas, RowPumpingSpeed);
            //page4
            //
            //計算各壓力流導
            float alpha;
            float viscousBody;
            float molecularBody;
            for(int i= 0; i< RowConductance.Count; i++)
            {
                alpha = 4 * tubeDia / 3 / tubeLength;
                viscousBody = 0.0327f * (float)Math.Pow(tubeDia, 4) * RowConductance[i].ValueX / (tubeLength * 0.0001708f);
                molecularBody = 11.43f * alpha * (float)(Math.Sqrt(293.15 / 28.966f) * Math.Pow(tubeDia / 2f, 2));
                RowConductance[i].ValueY = viscousBody + molecularBody;
            }
            rowTubeCanvas = drawGrid.FormXlogYlog(rowTubeCanvas, RowConductance);
            rowTubeCanvas = drawGrid.CurveXlogYlog(rowTubeCanvas, RowConductance);
            //refine page3
            //
            //calculate delivery speed
            for(int i= 0; i< RowPumpingSpeed.Count; i++)
            {
                RowDeliverySpeed[i].ValueY = RowPumpingSpeed[i].ValueY * RowConductance[i].ValueY / (RowPumpingSpeed[i].ValueY + RowConductance[i].ValueY);
            }
            rowPumpCanvas = drawGrid.CurveXlogYnature (rowPumpCanvas, RowDeliverySpeed );

            //page 5
            //
            //高於startPressure的座標只留最接近那個
            while (FinePumpingSpeed[1].ValueX  >= startPressure)
            {
                FinePumpingSpeed.RemoveAt(0);
            } 
            //低於endPressure的座標只留最接近那個
            while (FinePumpingSpeed[FinePumpingSpeed.Count - 2].ValueX  <= endPressure)
            {
                FinePumpingSpeed.RemoveAt(FinePumpingSpeed.Count - 1);
            }
            //高於startPressure的座標Pressur=startPressure;Spee=線性內插
            //低於endPressure的座標Pressur=endPressure;Spee=線性內插                              
            int Idx = FinePumpingSpeed.Count - 1;
            FinePumpingSpeed[0].ValueY = MidPoint(FinePumpingSpeed[0].ValueX , FinePumpingSpeed[0].ValueY , FinePumpingSpeed[1].ValueX , FinePumpingSpeed[1].ValueY , startPressure);
            FinePumpingSpeed[0].ValueX  = startPressure;
            FinePumpingSpeed[Idx].ValueY  = MidPoint(FinePumpingSpeed[Idx - 1].ValueX , FinePumpingSpeed[Idx - 1].ValueY , FinePumpingSpeed[Idx].ValueX , FinePumpingSpeed[Idx].ValueY , endPressure);
            FinePumpingSpeed[Idx].ValueX  = endPressure;

            for (int i = 0; i < FinePumpingSpeed.Count; i++)
            {
                FinePumpingSpeed[i].ValueX = (float)Math.Log10(FinePumpingSpeed[i].ValueX);
            }
            //計算內差的刻度
            incrementPress = (FinePumpingSpeed[0].ValueX  - FinePumpingSpeed[Idx].ValueX ) / incrementNum;
            float speedMax = FinePumpingSpeed[0].ValueY ;
            for (int i = 1; i < FinePumpingSpeed.Count; i++)
            {
                if (speedMax < FinePumpingSpeed[i].ValueY )
                    speedMax = FinePumpingSpeed[i].ValueY ;
            }
            incrementSpeed = speedMax / incrementNum;


            //距離太遠的兩點插入點
            float insX, insY;
            Idx = 1;
            while (Idx < FinePumpingSpeed.Count)
            {
                //大於1代表兩點距離太寬;
                insX = (FinePumpingSpeed[Idx - 1].ValueX  - FinePumpingSpeed[Idx].ValueX ) / incrementPress;
                insY = Math.Abs(FinePumpingSpeed[Idx - 1].ValueY - FinePumpingSpeed[Idx].ValueY ) / incrementSpeed;
                if (insX > 1f ^ insY > 1f)
                {
                    //兩點之間是X比較寬還是Y比較寬
                    if (insX > insY)
                    {
                        insX = FinePumpingSpeed[Idx - 1].ValueX  - incrementPress;
                        insY = MidPoint(FinePumpingSpeed[Idx - 1].ValueX , FinePumpingSpeed[Idx - 1].ValueY, FinePumpingSpeed[Idx].ValueX, FinePumpingSpeed[Idx].ValueY, insX);
                        FinePumpingSpeed.Insert(Idx, new PressureSpeed() { ValueX = insX, ValueY = insY });
                    }
                    else
                    {
                        //insY已經是絕對值,要重新判斷正負
                        if (FinePumpingSpeed[Idx - 1].ValueY > FinePumpingSpeed[Idx].ValueY )
                        {
                            insY = FinePumpingSpeed[Idx - 1].ValueY  - incrementSpeed;
                        }
                        else
                        {
                            insY = FinePumpingSpeed[Idx - 1].ValueY  + incrementSpeed;
                        }
                        insX = MidPoint(FinePumpingSpeed[Idx - 1].ValueY , FinePumpingSpeed[Idx - 1].ValueX , FinePumpingSpeed[Idx].ValueY , FinePumpingSpeed[Idx].ValueX, insY);
                        FinePumpingSpeed.Insert(Idx, new PressureSpeed() { ValueX = insX,ValueY  = insY });
                    }
                }
                Idx++;
            }

            for (int i = 0; i < FinePumpingSpeed.Count; i++)
            {
                FinePumpingSpeed[i].ValueX = (float)Math.Pow (10,FinePumpingSpeed[i].ValueX);
            }


            FinePDTimeOutG = FinePDTimeLoss = FinePDTimeNoL = FinePumpingSpeed;
            FinePDTimeNoL[0].ValueY = 0;
            for (int i = 1; i < FinePDTimeNoL.Count; i++)
            {
                FinePDTimeNoL[i].ValueY = chamberVolume * 2/(FinePumpingSpeed[i - 1].ValueY + FinePumpingSpeed[i].ValueY) * (float)Math.Log(FinePumpingSpeed[i - 1].ValueX / FinePumpingSpeed[i].ValueX);
            }
            for (int i = 1; i < FinePDTimeNoL.Count; i++)
            {
                FinePDTimeNoL[i].ValueY =FinePDTimeNoL[i - 1].ValueY + FinePDTimeNoL[i].ValueY ;
            }
            finePumpCanvas = drawGrid.FormXlogYnature(finePumpCanvas, FinePDTimeNoL);
            finePumpCanvas = drawGrid.CurveXlogYnature(finePumpCanvas, FinePDTimeNoL);

            canvasViewPS.InvalidateSurface();
        }

        public void NextPage_ButtenClicked(object sender, EventArgs e)
        {
            float entryFloat = 0;
            int entryInt = 0;

            //chamber info entry
            if (bitMapNum == 1)
            {
                try
                {
                    float.TryParse(entryPHigh.Text, out entryFloat);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Pressure High", "Cancel");
                }
                startPressure = entryFloat;

                try
                {
                    float.TryParse(entryPLow.Text, out entryFloat);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Pressure Low", "Cancel");
                }
                endPressure = entryFloat;

                try
                {
                   float.TryParse(entryVolume.Text, out entryFloat);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Chamber Volume", "Cancel");
                }
                chamberVolume = entryFloat;
          
                ChamberInfoGrid1.IsVisible  = false;
                TubeInfoGrid1.IsVisible = true;
                PumpListView.IsVisible = false;
            }

            //Tube info entry
            if (bitMapNum == 2)
            {
                try
                {
                    float.TryParse(entryDia.Text, out entryFloat);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Chamber Volume", "Cancel");
                }
                tubeDia = entryFloat;

                try
                {
                    float.TryParse(entryLength.Text, out entryFloat);
                }
                catch (FormatException ex)
                {
                    DisplayAlert(ex.Message, "Chamber Volume", "Cancel");
                }
                tubeLength = entryFloat;

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
            if (bitMapNum == 5)
            {
                ChamberInfoGrid1.IsVisible = true;
                TubeInfoGrid1.IsVisible = false;
                PumpListView.IsVisible = false;
            }

            canvasViewPS.InvalidateSurface();
            bitMapNum++;
            if (bitMapNum > 5 ^ bitMapNum < 1)
                bitMapNum = 1;
        }

        public float MidPoint(float X0, float Y0, float X1, float Y1, float Mid)
        {
            return ((Y1 - Y0) / (X1 - X0) * (Mid - X0) + Y0);
        }

    }
}
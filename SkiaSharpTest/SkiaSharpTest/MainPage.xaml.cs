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
        List<PSTs_node> VacCalc = new List<PSTs_node>();
        
        SKBitmap savePSBitmap,saveTPBitmap ;

        CanvasInfo rowPumpCanvas = new CanvasInfo();
        List<PressureSpeed> RowPumpingSpeed = new List<PressureSpeed>();
        List<PressureSpeed> RowDeliverySpeed = new List<PressureSpeed>();

        CanvasInfo rowTubeCanvas = new CanvasInfo();
        List<PressureSpeed> RowPressureConductance = new List<PressureSpeed>();

        int bitMapNum=1;

        float startPressure, endPressure, chamberVolume, tubeDia, tubeLength;
        int elbowNum;
        int   incrementNum = 100;
        float incrementPress, incrementSpeed;              

        ObservableCollection<PumpInfo> PumpListData;
        DrawGrid drawLogLog = new DrawGrid();
        public MainPage()
        {                    
            InitializeComponent();

            drawLogLog.Init();
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

            rowPumpCanvas.CanvasWidth= rowTubeCanvas.CanvasWidth = (float)e.Info.Width ;
            
            rowPumpCanvas.CanvasHeight= rowTubeCanvas.CanvasHeight  = (float)e.Info.Height;

            if (rowPumpCanvas.SaveBitMap == null)
            {
                rowPumpCanvas.SaveBitMap = new SKBitmap(info.Width, info.Height);
            }
            if (savePSBitmap == null)
            {
                savePSBitmap = new SKBitmap(info.Width, info.Height);
            }
            if (saveTPBitmap == null)
            {
                saveTPBitmap = new SKBitmap(info.Width, info.Height);
            }
            if ( rowTubeCanvas.SaveBitMap == null)
            {
                rowTubeCanvas.SaveBitMap = new SKBitmap(info.Width, info.Height);
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
                canvas.DrawBitmap(rowPumpCanvas.SaveBitMap, 0, 0);
            }
            if (bitMapNum == 4)
            {
                canvas.DrawBitmap(rowTubeCanvas.SaveBitMap, 0, 0);
            }

        }

        private void PumpListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var SelectedPumpInfo = PumpListView.SelectedItem  as PumpInfo;
            var TempList = SelectedPumpInfo.PSList as List<PressureSpeed> ;
            var ListViewID = sender as ViewCell ;

            RowPumpingSpeed.Clear();
            RowDeliverySpeed.Clear();
            RowPressureConductance.Clear();
            VacCalc.Clear();
            for (int i= 0; i< TempList.Count; i++)
            {
                RowPumpingSpeed.Add(new PressureSpeed () { ValueX  = TempList[i].ValueX, ValueY = TempList[i].ValueY });
                RowDeliverySpeed.Add(new PressureSpeed() { ValueX = TempList[i].ValueX, ValueY = 0 });
                RowPressureConductance.Add(new PressureSpeed() { ValueX = TempList[i].ValueX, ValueY = 0 });
                VacCalc.Add(new PSTs_node() { Pressur = (float)Math.Log10(TempList[i].ValueX), Spee = TempList[i].ValueY, Tim = 0, Secon = 0 });                 
            }
            rowPumpCanvas = drawLogLog.InitXlogYnature(rowPumpCanvas, RowPumpingSpeed);
            rowPumpCanvas = drawLogLog.DrawXlogYnature(rowPumpCanvas, RowPumpingSpeed);

            float alpha;
            float viscousBody;
            float molecularBody;
            for(int i= 0; i< RowPressureConductance.Count; i++)
            {
                alpha = 4 * tubeDia / 3 / tubeLength;
                viscousBody = 0.0327f * (float)Math.Pow(tubeDia, 4) * RowPressureConductance[i].ValueX / (tubeLength * 0.0001708f);
                molecularBody = 11.43f * alpha * (float)(Math.Sqrt(293.15 / 28.966f) * Math.Pow(tubeDia / 2f, 2));
                RowPressureConductance[i].ValueY = viscousBody + molecularBody;
            }
            rowTubeCanvas = drawLogLog.InitXlogYlog(rowTubeCanvas, RowPressureConductance);
            rowTubeCanvas = drawLogLog.DrawXlogYlog(rowTubeCanvas, RowPressureConductance);

            for(int i= 0; i< RowPumpingSpeed.Count; i++)
            {
                RowDeliverySpeed[i].ValueY = RowPumpingSpeed[i].ValueY * RowPressureConductance[i].ValueY / (RowPumpingSpeed[i].ValueY + RowPressureConductance[i].ValueY);
            }
            rowPumpCanvas = drawLogLog.DrawXlogYnature (rowPumpCanvas, RowDeliverySpeed );

            canvasViewPS.InvalidateSurface();
        }

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace mage
{
    public sealed partial class Magehahmo : UserControl
    {

        // Animate magehahmo timer, ajastin milloin "kuvaa" vaihdetaan
        private DispatcherTimer timer;

        // Offset to show, eli mikä kohta Magen kuvasta näytetään suorakulmion sisällä
        private int currentFrame = 0;
        private int direction = 1; // 1 or -1 Vaihtuuko frame "isommaksi" vai "pienemmäksi"
        private int frameheight = 400;

        // Nopeus
        private readonly double MaxSpeed = 10;      // Maxnopeus Magehahmolle
        private readonly double Accelerate = 0.5;   // Kiihtyvyys, tarvitaanko?
        private double speed;   // Nopeus

        // Angle, magehahmon kulma
        private readonly double AngleStep = 5;
        private double Angle = 0;


        // magehahmon sijainti Canvaksella
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public bool Jumping = true;

        public Magehahmo()
        {
            this.InitializeComponent();
            Width = 250; Height = 400;
            // Animate 
            timer = new DispatcherTimer();
            // 125ms,   Aikaväli päivittämiselle (fps)
            timer.Interval = new TimeSpan(0, 0, 0, 0, 125);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            // Current frame 0,1,2,3,4,5,6,7   nykyinen frame Magesta
            if (direction == 1) currentFrame++;
            else currentFrame--;
            if (currentFrame == 0 || currentFrame == 7)
            {
                direction = -1 * direction; // 1 tai -1
            }
            // Set offset
            SpriteSheetOffset.Y = currentFrame * -frameheight;
        }

        // Move -metodi liikkumiseen
        public void MoveLeft()
        {
            // Lisää speediä
            speed += Accelerate;
            if (speed > MaxSpeed) speed = MaxSpeed;
            // Uusi sijainti
            LocationX = LocationX - speed;
            if (LocationX <= -10)   // ESTETÄÄN Magehahmoa poistumasta ruudusta
            {
                LocationX = 0;
            }
            SetLocation();
        }
        public void MoveRight()
        {
            // Lisää speediä
            speed += Accelerate;
            if (speed > MaxSpeed) speed = MaxSpeed;
            // Uusi sijainti
            LocationX = LocationX + speed;
            if (LocationX >= 1150) // ESTETÄÄN Magehahmoa poistumasta ruudusta
            {
                LocationX = 1120;
            }
            SetLocation();
        }
        //public bool Jumping = false;
        public void Jump()
        {
            if (Jumping == false)
            {
                // Mage liikkuu ainoastaan ylöspäin LocationY = LocationY - speed;
                // Magen liikutus sinikäyrällä
                Angle = Angle + 0.1f;
                if (Angle > 50) { Angle = 0; }
                LocationY = 80 + Math.Cos(Angle) * 140;
                SetLocation();
            }
        }

        // KÄÄNTYMINEN MIETI TÄMÄ UUSIKSI, ONKO TÄYSIN TURHA?
        public void Rotate(int direction)
        {
            Angle += direction * AngleStep; // -1 * 5 tai 1 * 5
            MagehahmoRotateAngle.Angle = Angle;
        }

        // Update location, päivitetään hahmon sijainti Canvaksella
        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

    }
}
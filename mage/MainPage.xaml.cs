using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace mage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Luodaan magehahmo
        private Magehahmo magehahmo;
        //Luodaan Skappari
        private Skappari skappari;
        //Luodaan Rynkky
        private Rynkky rynkky;
        // Luodaan tellut
        private List<Tellu> tellut;
        private List<Maata> maat;
      

        // Tutkitaan mitkä näppäimet ovat painettuina tai päästettyinä
        private bool UpPressed;
        private bool LeftPressed;
        private bool RightPressed;

        // Luodaan ajastin timeri
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            magehahmo = new Magehahmo
            {
                LocationX = 1000,     // Määritetään magehahmon aloitussijainti
                LocationY = 220
            };

            maat = new List<Maata>();
            // Luodaan maa1
            Maata maa1 = new Maata();       
            maat.Add(maa1);                 
            maa1.LocationX = 0; maa1.LocationY = 650;
            maa1.SetLocation();
            // maa2
            Maata maa2 = new Maata();
            maat.Add(maa2);
            maa2.LocationX = 100; maa2.LocationY = 625;
            maa2.SetLocation();
            // maa3
            Maata maa3 = new Maata();
            maat.Add(maa3);
            maa3.LocationX = 500; maa3.LocationY = 0;
            maa3.SetLocation();

            // Luodaan skappari
            Skappari skappari = new Skappari();
            Canvas.SetTop(skappari, 400);
            Canvas.SetLeft(skappari, 200);

            // Luodaan Rynkky
            Rynkky rynkky = new Rynkky();
            Canvas.SetTop(rynkky, 400);
            Canvas.SetLeft(rynkky, 1000);

            // alustetaan tellulista
            tellut = new List<Tellu>();
            // Luodaan Tellu1
            Tellu tellu1 = new Tellu();
         // Canvas.SetTop(tellu1, 400);
         // Canvas.SetLeft(tellu1, 300);
            tellut.Add(tellu1);
            tellu1.LocationX = 300; tellu1.LocationY = 500;
            tellu1.SetLocation();

            // Luodaan Tellu2
            Tellu tellu2 = new Tellu();
         // Canvas.SetTop(tellu2, 300);
         // Canvas.SetLeft(tellu2, 700);
            tellut.Add(tellu2);
            tellu2.LocationX = 600; tellu2.LocationY = 500;
            tellu2.SetLocation();

            // Lisätään magehahmo ja muut oliot Canvakselle
            MyCanvas.Children.Add(magehahmo);
            MyCanvas.Children.Add(maa1);
            MyCanvas.Children.Add(maa2);
            MyCanvas.Children.Add(maa3);
            MyCanvas.Children.Add(skappari);
            MyCanvas.Children.Add(rynkky);
            MyCanvas.Children.Add(tellu1);
            MyCanvas.Children.Add(tellu2);


            // Key Listeners,   näppäimien kuuntelua, onko jokin painettuna vai ei?
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;  // Onko näppäimi alhaalla
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp; //Onko näppäimiä "ylhäällä"

            // Start game loop,     PELI LÄHTEE HETI KÄYNTIIN
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // KeyUp haistelu
        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)    // Tutkii tableteilta, puhelimilta, koneilta
            {
                case VirtualKey.Up:
                    UpPressed = false;
                    break;
                case VirtualKey.Left:
                    LeftPressed = false;
                    break;
                case VirtualKey.Right:
                    RightPressed = false;
                    break;
            }
        }
        // KeyDown haistelu
        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)    // Tutkii tableteilta, puhelimilta, koneilta
            {
                case VirtualKey.Up:
                    UpPressed = true;
                    break;
                case VirtualKey.Left:
                    LeftPressed = true;
                    break;
                case VirtualKey.Right:
                    RightPressed = true;
                    break;
            }
        }

        //Peli luuppi
        private void Timer_Tick(object sender, object e)
        {
            // Liikutetaan magehahmoa jos painettu näppäimiä 
            if (LeftPressed) magehahmo.MoveLeft();
            if (RightPressed) magehahmo.MoveRight();
            if (UpPressed) magehahmo.Jump();
            /*
            if (UpPressed && !magehahmo.Jumping) magehahmo.Jump();
            */

            // magehahmon paikka Canvaksella päivitetään
            magehahmo.SetLocation();

            // Collision...
            CheckCollision();
           
        }

        // BUTTON ETUSIVULLE SIIRTYMISEEN
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Etusivu));
        }

        private void CheckCollision()
        {
            // Käydään läpi tellulista
            foreach (Tellu tellu in tellut)
            {
                // Get Rects, katsotaan osuuko mikään tellulistan telluista magehahmoon
                Rect BRect = new Rect(                                               // magehahmon sijainti ja koko
                    magehahmo.LocationX, magehahmo.LocationY, magehahmo.ActualWidth, magehahmo.ActualHeight
                    );
                Rect FRect = new Rect(                                               // Tellun sijainti ja koko
                    tellu.LocationX, tellu.LocationY, tellu.ActualWidth, tellu.ActualHeight
                    );
                // Does objects intersects, törmääkö objektit
                BRect.Intersect(FRect);
                if (!BRect.IsEmpty) // Jos palautettu arvo EI OLE TYHJÄ
                {
                    Debug.WriteLine("HIT");
                    // Collision! Area isn't empty, törmäys - alue ei ole tyhjä
                    // Poistetaan tellu Canvakselta
                    MyCanvas.Children.Remove(tellu);
                    // Poistetaan myös listasta tellu
                    tellut.Remove(tellu);
                    break;
                }

            }
            // Käydään läpi tellulista
            foreach (Maata maa in maat)
            {
                // Get Rects, katsotaan osuuko mikään tellulistan telluista magehahmoon
                Rect BRect = new Rect(                                               // magehahmon sijainti ja koko
                    magehahmo.LocationX, magehahmo.LocationY, magehahmo.ActualWidth, magehahmo.ActualHeight
                    );
                Rect FRect = new Rect(                                               // Tellun sijainti ja koko
                    maa.LocationX, maa.LocationY, maa.ActualWidth, maa.ActualHeight
                    );
                // Does objects intersects, törmääkö objektit
                BRect.Intersect(FRect);
                if (!BRect.IsEmpty) // Jos palautettu arvo EI OLE TYHJÄ
                {
                    // Collision! Area isn't empty, törmäys - alue ei ole tyhjä
                    // Poistetaan tellu Canvakselta
                   // magehahmo.LocationY = maa.LocationY;
                    
                    MyCanvas.Children.Remove(maa);
                    // Poistetaan myös listasta tellu
                    maat.Remove(maa);
                    break;
                }

            }
          

        }

    }
}
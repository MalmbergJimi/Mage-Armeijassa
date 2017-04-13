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
        //Luodaan Rynkky
        private Rynkky rynkky;
        // Luodaan tellut
        private List<Tellu> tellut;
        private List<Maata> maat;
        private List<Maapala> maapalat;
        private List<Skappari> skapparit;


        // Tutkitaan mitkä näppäimet ovat painettuina tai päästettyinä
        private bool UpPressed;
        private bool LeftPressed;
        private bool RightPressed;

        // Luodaan ajastin timeri
        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            // Vaihdetaan oletus StartUp mode
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);
            //disable debugger info
            App.Current.DebugSettings.EnableFrameRateCounter = true;

            magehahmo = new Magehahmo
            {
                LocationX = 0,     // Määritetään magehahmon aloitussijainti
                LocationY = 220,
                Jumping = false
            };
            // MAAT LISTA
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
            

            // Luodaan MAAPALA!
            //Lista
            maapalat = new List<Maapala>();
            //Palat
            Maapala mpala1 = new Maapala();
            maapalat.Add(mpala1);
            mpala1.LocationX = 300; mpala1.LocationY = 500;
            mpala1.SetLocation();
            //Palat
            Maapala mpala2 = new Maapala();
            maapalat.Add(mpala2);
            mpala2.LocationX = 900; mpala2.LocationY = 500;
            mpala2.SetLocation();

            // Luodaan skapparit lista
            skapparit = new List<Skappari>();
            // Luodaan skappari            
            Skappari skappari1 = new Skappari();
            skapparit.Add(skappari1);
            skappari1.LocationX = 100; skappari1.LocationY = 100;
            skappari1.SetLocation();

            // Luodaan Rynkky
            Rynkky rynkky = new Rynkky();
            Canvas.SetTop(rynkky, 400);
            Canvas.SetLeft(rynkky, 1000);

            // alustetaan tellulista
            tellut = new List<Tellu>();
            // Luodaan Tellu1
            Tellu tellu1 = new Tellu();
            tellut.Add(tellu1);
            tellu1.LocationX = 300; tellu1.LocationY = 500;
            tellu1.SetLocation();

            // Luodaan Tellu2
            Tellu tellu2 = new Tellu();
            tellut.Add(tellu2);
            tellu2.LocationX = 600; tellu2.LocationY = 500;
            tellu2.SetLocation();

            // Lisätään magehahmo ja muut oliot Canvakselle
            MyCanvas.Children.Add(magehahmo);
            //MAA
            MyCanvas.Children.Add(maa1);
            MyCanvas.Children.Add(maa2);
            //MAAPALAT
            MyCanvas.Children.Add(mpala1);
            MyCanvas.Children.Add(mpala2);
            //TELLUT
            MyCanvas.Children.Add(tellu1);
            MyCanvas.Children.Add(tellu2);
            //SKAPPARIT
            MyCanvas.Children.Add(skappari1);
            //RYNKKY
            MyCanvas.Children.Add(rynkky);


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
                    magehahmo.Jumping = false;
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
            // Käydään läpi maat lista
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

                    // MyCanvas.Children.Remove(maa);
                    // Poistetaan myös listasta tellu
                    // maat.Remove(maa);
                    break;
                }

            }
            // Käydään läpi maapalat lista
            foreach (Maapala mpala in maapalat)
            {
                // Get Rects, katsotaan osuuko mikään tellulistan telluista magehahmoon
                Rect BRect = new Rect(                                               // magehahmon sijainti ja koko
                    magehahmo.LocationX, magehahmo.LocationY, magehahmo.ActualWidth, magehahmo.ActualHeight
                    );
                Rect FRect = new Rect(                                               // Tellun sijainti ja koko
                    mpala.LocationX, mpala.LocationY, mpala.ActualWidth, mpala.ActualHeight
                    );
                // Ttörmääkö objektit
                
                BRect.Intersect(FRect);
                if (!BRect.IsEmpty) // Jos palautettu arvo EI OLE TYHJÄ
                {
                    // Collision! Area isn't empty, törmäys - alue ei ole tyhjä
                    magehahmo.Jumping = false;
                    Debug.WriteLine(BRect);
                    magehahmo.LocationY = BRect.Y - magehahmo.Height + 20;
                    magehahmo.LocationX = BRect.X - magehahmo.Width + 20;
                    magehahmo.SetLocation();
                    // Poistetaan myös listasta tellu
                    // maapalat.Remove(mpala);
                    break;
                }

            }
            // Käydään läpi skapparit lista
            foreach (Skappari skappari in skapparit)
            {
                // Get Rects, katsotaan osuuko mikään tellulistan telluista magehahmoon
                Rect BRect = new Rect(                                               // magehahmon sijainti ja koko
                    magehahmo.LocationX, magehahmo.LocationY, magehahmo.ActualWidth, magehahmo.ActualHeight
                    );
                Rect FRect = new Rect(                                               // Tellun sijainti ja koko
                    skappari.LocationX, skappari.LocationY, skappari.ActualWidth, skappari.ActualHeight
                    );
                // Does objects intersects, törmääkö objektit
                BRect.Intersect(FRect);
                if (!BRect.IsEmpty) // Jos palautettu arvo EI OLE TYHJÄ
                {
                    // Collision! Area isn't empty, törmäys - alue ei ole tyhjä
                    MyCanvas.Children.Remove(skappari);
                    // Poistetaan myös listasta tellu
                    skapparit.Remove(skappari);
                    break;
                }

            }


        }

    }
}
using System;
using System.Collections.Generic;
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
        private Maata maa;
      

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
            // Luodaan maa
            Maata maa = new Maata();
            Canvas.SetTop(maa, 450);
            Canvas.SetLeft(maa, 00);
           

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
            Canvas.SetTop(tellu1, 500);
            Canvas.SetLeft(tellu1, 300);
            tellut.Add(tellu1);

            // Luodaan Tellu2
            Tellu tellu2 = new Tellu();
            Canvas.SetTop(tellu2, 500);
            Canvas.SetLeft(tellu2, 700);
            tellut.Add(tellu2);

            // Lisätään magehahmo Canvakselle
            MyCanvas.Children.Add(magehahmo);
            MyCanvas.Children.Add(maa);
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
            // Move butterfly if up pressed
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
            // LISÄTÄÄN COLLISION MYÖHEMMIN
        }

        // BUTTON ETUSIVULLE SIIRTYMISEEN
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Etusivu));
        }

        private void CheckCollision()
        {
            // Loop flower list, Käydään läpi kukkalista
            foreach (Tellu tellu in tellut)
            {
                // Get Rects, katsotaan osuuko mikään kukkalistan kukista perhoseen
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
                    // Collision! Area isn't empty, törmäys - alue ei ole tyhjä
                    // Poistetaan tellu Canvakselta
                    MyCanvas.Children.Remove(magehahmo);
                    // Poistetaan myös listasta tellu
                    tellut.Remove(tellu);
                    break;
                }

            }
            
        }

    }
}
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App_RadiationServer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public EmulatedData mydata;
        public MainPage()
        {
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;

            this.mydata = new EmulatedData();
            this.InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.mydata.StartTimer();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.mydata.StopTimer();
        }
    }
    
}

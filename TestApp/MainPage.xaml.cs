using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using TestApp.Nuimo;
using TestApp.Vlc;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            textBox.TextChanged += TextBox_TextChanged;
            InitConnections();
        }

        private async void InitConnections()
        {
            //var nuimos = await GetNuimos();
            //var nuimo = nuimos.FirstOrDefault();

            //if (nuimo == null)
            //{
            //    return;
            //}

            ////nuimo.RunLedTest();
            //UiWriteLine($"FW Version: {nuimo.FirmwareVersion.CurrentValue}");
            //UiWriteLine($"Battery   : {nuimo.BatteryPercentage.CurrentValue}%");

            //nuimo.Rotation.ValueChanged += i => UiWriteLine($"Rotated {i}.");
            //nuimo.BatteryPercentage.ValueChanged += i => UiWriteLine($"Battery Value Changed: {i}");
            //nuimo.Button.ValueChanged += i => UiWriteLine($"Button {(i ? "Pressed" : "Released")}");
            //nuimo.Swype.ValueChanged += i => UiWriteLine($"Swyped {i}");
            //nuimo.Fly.ValueChanged += i => UiWriteLine($"Fly {i.action} {i.hoverDistance}");

            //var n = nuimo as NuimoDevice;

            //n.RunLedText("abf");


            var remote = new Remote();

            await remote.Connect();

            for (var i = 0; i < 100; i++)
            {
                
                await remote.SendCustomCommand("volume 100");
                //UiWriteLine(await remote.ReciveAnswer());
                await Task.Delay(TimeSpan.FromSeconds(1));

                await remote.SendCustomCommand("volume 50");
                //UiWriteLine(await remote.ReciveAnswer());
                await Task.Delay(TimeSpan.FromSeconds(1));

                await remote.SendCustomCommand("volume 0");
                //UiWriteLine(await remote.ReciveAnswer());
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private async Task<IEnumerable<INuimoDevice>> GetNuimos()
        {
            var nuimos = (await NuimoDeviceManager.Instance.GetNuimoDevicesAsync()).ToList();

            if (nuimos.Any())
            {
                foreach (var nuimo in nuimos)
                {
                    UiWriteLine(nuimo.Id);
                }
            }
            else
            {
                UiWriteLine("No Nuimos Found.");
            }

            return nuimos;
        }

        private async void UiWriteLine(string text)
        {
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    textBox.Text += text + Environment.NewLine;
                });
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var grid = (Grid)VisualTreeHelper.GetChild(textBox, 0);
            for (var i = 0; i <= VisualTreeHelper.GetChildrenCount(grid) - 1; i++)
            {
                object obj = VisualTreeHelper.GetChild(grid, i);
                if (!(obj is ScrollViewer)) continue;
                ((ScrollViewer)obj).ChangeView(0.0f, ((ScrollViewer)obj).ExtentHeight, 1.0f);
                break;
            }
        }
    }
}

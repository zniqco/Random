using System.Threading.Tasks;
using System.Windows;

namespace MameIIDXHelper
{
    public partial class MainWindow : Window
    {
        private enum RemapType
        {
            None,
            Classic,
            IIDX,
        }

        private RemapType remapType = RemapType.None;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = MainLoop();
        }

        private async Task MainLoop()
        {
            while (true)
            {
                var className = Native.GetForegroundWindowClass();
                var caption = Native.GetForegroundWindowText();
                var enabled = false;

                if (className == "MAME")
                {
                    if (caption.Contains("beatmania IIDX"))
                        SetRemapType(RemapType.IIDX);
                    else
                        SetRemapType(RemapType.Classic);

                    enabled = true;   
                }

                Native.SetRemapKeycodeEnabled(enabled);

                await Task.Delay(500);
            }
        }

        private void SetRemapType(RemapType remapType)
        {
            if (remapType != this.remapType)
            {
                Native.ClearRemapKeycode();

                switch (remapType)
                {
                    case RemapType.IIDX:
                        Native.AddRemapKeycode(0xA0, 72);
                        Native.AddRemapKeycode(0xA0, 80);
                        Native.AddRemapKeycode(0xA1, 75);
                        Native.AddRemapKeycode(0xA1, 77);
                        Native.AddRemapKeycode(0x15, -1);
                        Native.AddRemapKeycode(0x5B, -1);

                        break;

                    case RemapType.Classic:
                        Native.AddRemapKeycode(0x20, 72);
                        Native.AddRemapKeycode(0x20, 80);
                        Native.AddRemapKeycode(0xA1, 75);
                        Native.AddRemapKeycode(0xA1, 77);
                        Native.AddRemapKeycode(0x15, -1);
                        Native.AddRemapKeycode(0x5B, -1);

                        break;
                }

                this.remapType = remapType;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Discord_win.Pages {
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page {
        private double CanvasServerWidth;
        private double CanvasChannelWidth;
        private double CanvasRoleWidth;
        private double CanvasChatWidth;
        private double CanvasMessageWidth;
        private double CanvasTextBoxMessageWidth;
        private int CanvasMessageTotalPaddingLeftRight;
        public MainPage() {
            InitializeComponent();
            InitializeParameter();
        }
        private void InitializeParameter() {
            this.CanvasServerWidth = this.CanvasServer.Width;
            this.CanvasChannelWidth = this.CanvasChannel.Width;
            this.CanvasRoleWidth = this.CanvasRole.Width;
            this.CanvasChatWidth = this.CanvasChat.Width;
            this.CanvasMessageWidth = this.CanvasMessage.Width;
            this.CanvasTextBoxMessageWidth = this.TextBoxMessage.Width;
            this.CanvasMessageTotalPaddingLeftRight = (int)Application.Current.FindResource("CanvasMessageTotalPaddingLeftRight");
        }
        public void ChangeSize() {
            this.Width = Program.mainWindow.ActualWidth;
            this.Height = Program.mainWindow.ActualHeight;
            this.CanvasChat.Width = this.ActualWidth - (this.CanvasServerWidth + this.CanvasChannelWidth + this.CanvasRoleWidth);
            this.CanvasMessage.Width = this.CanvasChat.ActualWidth - this.CanvasMessageTotalPaddingLeftRight;
            this.TextBoxMessage.Width = this.CanvasMessage.ActualWidth;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e) {
            this.CanvasChat.Width = this.ActualWidth - (this.CanvasServerWidth + this.CanvasChannelWidth + this.CanvasRoleWidth);
            this.CanvasMessage.Width = this.CanvasChat.ActualWidth - this.CanvasMessageTotalPaddingLeftRight;
            this.TextBoxMessage.Width = this.CanvasMessage.ActualWidth;
        }
    }
}

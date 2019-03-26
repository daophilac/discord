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
        public Inventory inventory { get; set; }
        //private double CanvasServerWidth;
        //private double CanvasChannelWidth;
        //private double CanvasRoleWidth;
        //private double CanvasChatWidth;
        //private double CanvasTypeWidth;
        //private double CanvasTextBoxMessageWidth;
        //private int CanvasMessageTotalPaddingLeftRight;
        public MainPage() {
            InitializeComponent();
            InitializeGlobalVariable();
        }
        private void InitializeGlobalVariable() {
            this.inventory = new Inventory();
            //this.CanvasServerWidth = this.CanvasServer.Width;
            //this.CanvasChannelWidth = this.CanvasChannel.Width;
            //this.CanvasRoleWidth = this.CanvasRole.Width;
            //this.CanvasChatWidth = this.CanvasChat.Width;
            //this.CanvasTypeWidth = this.CanvasType.Width;
            //this.CanvasTextBoxMessageWidth = this.TextBoxMessage.Width;
            //this.CanvasMessageTotalPaddingLeftRight = (int)Application.Current.FindResource("CanvasMessageTotalPaddingLeftRight");



            TestPage testPage = new TestPage();
            this.MessageFrame.Navigate(Program.testPage);
        }
        public void ChangeSize() {
            //Program.mainWindow.MainFrame.Width = Program.mainWindow.MainGrid.ActualWidth;
            //Program.mainWindow.MainFrame.Height = Program.mainWindow.MainGrid.ActualHeight;
            //this.Width = Program.mainWindow.MainGrid.ActualWidth;
            //this.Height = Program.mainWindow.MainGrid.ActualHeight;
            //this.CanvasChat.Width = this.ActualWidth - (this.CanvasServerWidth + this.CanvasChannelWidth + this.CanvasRoleWidth);
            //this.CanvasType.Width = this.CanvasChat.Width - this.CanvasMessageTotalPaddingLeftRight;
            //this.TextBoxMessage.Width = this.CanvasType.Width;
        }

        private void Page_Initialized(object sender, EventArgs e) {

        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            //TextBox[] textBoxes = new TextBox[30];
            //for (int i = 0; i < 30; i++) {
            //    textBoxes[i] = new TextBox();
            //    textBoxes[i].Text = i + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab";
            //    textBoxes[i].Width = Program.mainPage.MessageFrame.ActualWidth - 40;
            //    textBoxes[i].Height = 60;
            //    textBoxes[i].VerticalContentAlignment = VerticalAlignment.Center;
            //    textBoxes[i].TextWrapping = TextWrapping.Wrap;
            //    DockPanel.SetDock(textBoxes[i], Dock.Bottom);
            //    Program.testPage.abc.Children.Add(textBoxes[i]);
            //}
        }
    }
}
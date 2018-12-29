using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DCChatMain
{


    public partial class MainWindow : Window
    {
        ParseDC parseDC = new ParseDC();
        string URL;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public MainWindow()
        {

            Selecter selecter = new Selecter();
            selecter.ShowDialog();
            URL = selecter.URL;

            InitializeComponent();
            Topmost = true;

            again:;
            if (URL == "" || URL == null || URL == string.Empty || URL == "http://gall.dcinside.com/mgallery/board/lists?id=" || URL == "http://gall.dcinside.com/board/lists/?id=")
            {
                MessageBox.Show("오류!");
                selecter.ShowDialog();
                URL = selecter.URL;
                goto again;
            }

            TitleBox.Text = URL.Replace("http://gall.dcinside.com/board/lists/?id=", string.Empty);
            TitleBox.Text = TitleBox.Text.Replace("http://gall.dcinside.com/mgallery/board/lists?id=", string.Empty) + " 중계중";


            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            parseDC.ParseNRegex(URL);
            TBox0.Text = parseDC.DCDataList[0];
            TBox1.Text = parseDC.DCDataList[1];
            TBox2.Text = parseDC.DCDataList[2];
            TBox3.Text = parseDC.DCDataList[3];
            TBox4.Text = parseDC.DCDataList[4];
            TBox5.Text = parseDC.DCDataList[5];
            TBox6.Text = parseDC.DCDataList[6];
            TBox7.Text = parseDC.DCDataList[7];
            TBox8.Text = parseDC.DCDataList[8];
            TBox9.Text = parseDC.DCDataList[9];

            this.Topmost = true;
            this.Show();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {



            Window window = (Window)sender;
            window.Topmost = true;


            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;


        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                dispatcherTimer.Stop();
                this.Hide();
                Selecter selecter = new Selecter();
                selecter.ShowDialog();
                URL = selecter.URL;
                again:;
                if (URL == "" || URL == null || URL == string.Empty || URL == "http://gall.dcinside.com/mgallery/board/lists?id=" || URL == "http://gall.dcinside.com/board/lists/?id=")
                {
                    MessageBox.Show("오류!");
                    selecter.ShowDialog();
                    URL = selecter.URL;
                    goto again;
                }

                selecter.Close();




                TitleBox.Text = URL.Replace("http://gall.dcinside.com/board/lists/?id=", string.Empty);
                TitleBox.Text = TitleBox.Text.Replace("http://gall.dcinside.com/mgallery/board/lists?id=", string.Empty) + " 중계중";
                dispatcherTimer.Start();
                this.Show();
            }
        }
    }
    class ParseDC
    {
        public List<string> DCDataList = new List<string>(); //제목, 작성자, 아이피
        public void ParseNRegex(string URL)
        {
            try
            {
                const int MAX = 15;
                DCDataList.Clear();

                WebClient client = new WebClient
                {
                    Encoding = Encoding.UTF8
                };
                string downloadString = client.DownloadString(URL);
                string Pattern = "icon_(txt|pic)\"></em>(.+?)</a>";
                Regex Reg = new Regex(Pattern);
                MatchCollection mc = Reg.Matches(downloadString);
                string tmp = null;
                foreach (Match m in mc)
                {
                    tmp = m.Value;
                    tmp = tmp.Replace("</a>", string.Empty);
                    tmp = tmp.Replace("</em>", string.Empty);
                    tmp = tmp.Replace("icon_pic\">", string.Empty);
                    tmp = tmp.Replace("icon_txt\">", string.Empty);
                    tmp = tmp.Replace("&quot;", string.Empty);
                    if (tmp.Length > MAX)
                    {
                        tmp = tmp.Remove(tmp.Length - (tmp.Length - MAX)) + " ...";
                    }
                    DCDataList.Add(tmp);
                }
            }
            catch (Exception)
            {

            }



        }
    }

    [ContentProperty("Text")]
    public class OutlinedTextBlock : FrameworkElement
    {
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(
            "Fill",
            typeof(Brush),
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Brush),
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness",
            typeof(double),
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(OnFormattedTextInvalidated));

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(
            "TextAlignment",
            typeof(TextAlignment),
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register(
            "TextDecorations",
            typeof(TextDecorationCollection),
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register(
            "TextTrimming",
            typeof(TextTrimming),
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(OnFormattedTextUpdated));

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            "TextWrapping",
            typeof(TextWrapping),
            typeof(OutlinedTextBlock),
            new FrameworkPropertyMetadata(TextWrapping.NoWrap, OnFormattedTextUpdated));

        private FormattedText formattedText;
        private Geometry textGeometry;

        public OutlinedTextBlock()
        {
            this.TextDecorations = new TextDecorationCollection();
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)this.GetValue(TextDecorationsProperty); }
            set { this.SetValue(TextDecorationsProperty, value); }
        }

        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            this.EnsureGeometry();

            drawingContext.DrawGeometry(this.Fill, new Pen(this.Stroke, this.StrokeThickness), this.textGeometry);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.EnsureFormattedText();

            // constrain the formatted text according to the available size
            // the Math.Min call is important - without this constraint (which seems arbitrary, but is the maximum allowable text width), things blow up when availableSize is infinite in both directions
            // the Math.Max call is to ensure we don't hit zero, which will cause MaxTextHeight to throw
            this.formattedText.MaxTextWidth = Math.Min(3579139, availableSize.Width);
            this.formattedText.MaxTextHeight = Math.Max(0.0001d, availableSize.Height);

            // return the desired size
            return new Size(this.formattedText.Width, this.formattedText.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.EnsureFormattedText();

            // update the formatted text with the final size
            this.formattedText.MaxTextWidth = finalSize.Width;
            this.formattedText.MaxTextHeight = finalSize.Height;

            // need to re-generate the geometry now that the dimensions have changed
            this.textGeometry = null;

            return finalSize;
        }

        private static void OnFormattedTextInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
            outlinedTextBlock.formattedText = null;
            outlinedTextBlock.textGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        private static void OnFormattedTextUpdated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var outlinedTextBlock = (OutlinedTextBlock)dependencyObject;
            outlinedTextBlock.UpdateFormattedText();
            outlinedTextBlock.textGeometry = null;

            outlinedTextBlock.InvalidateMeasure();
            outlinedTextBlock.InvalidateVisual();
        }

        private void EnsureFormattedText()
        {
            if (this.formattedText != null || this.Text == null)
            {
                return;
            }

            this.formattedText = new FormattedText(
                this.Text,
                CultureInfo.CurrentUICulture,
                this.FlowDirection,
                new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, FontStretches.Normal),
                this.FontSize,
                Brushes.Black);

            this.UpdateFormattedText();
        }

        private void UpdateFormattedText()
        {
            if (this.formattedText == null)
            {
                return;
            }

            this.formattedText.MaxLineCount = this.TextWrapping == TextWrapping.NoWrap ? 1 : int.MaxValue;
            this.formattedText.TextAlignment = this.TextAlignment;
            this.formattedText.Trimming = this.TextTrimming;

            this.formattedText.SetFontSize(this.FontSize);
            this.formattedText.SetFontStyle(this.FontStyle);
            this.formattedText.SetFontWeight(this.FontWeight);
            this.formattedText.SetFontFamily(this.FontFamily);
            this.formattedText.SetFontStretch(this.FontStretch);
            this.formattedText.SetTextDecorations(this.TextDecorations);
        }

        private void EnsureGeometry()
        {
            if (this.textGeometry != null)
            {
                return;
            }

            this.EnsureFormattedText();
            this.textGeometry = this.formattedText.BuildGeometry(new Point(0, 0));
        }
    }
}

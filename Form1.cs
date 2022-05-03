using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;

namespace ArduinoTest
{
    public partial class Form1 : Form
    {
        SerialPort _serialPort;
        List<Double> list;
        List<Double> list2;
        public Form1()
        {
            InitializeComponent();

            list = new List<Double>();
            list2 = new List<Double>();
            cartesianChart1.Series = new ISeries[]
            {
                new LiveChartsCore.SkiaSharpView.LineSeries<Double>
                {
                     Values = list,
                     DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Start,
                     GeometrySize = 10
                     

                },
                new ColumnSeries<Double>{
                Values =  list2 }
            };
            cartesianChart1.YAxes.ElementAt(0).MinLimit = 10;
            cartesianChart1.YAxes.ElementAt(0).MaxLimit = 100;
            
            list.Add(2);
            if (_serialPort == null)
            {
                _serialPort = new SerialPort("COM3", 9600);
                _serialPort.ReadTimeout = 500;
                try
                {
                    _serialPort.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    _serialPort.Close();
                }
                _serialPort.DataReceived += _serialPort_DataReceived;
            }
            textBox1.Text = String.Empty;
        }

        private void _serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string received = _serialPort.ReadLine();
            this.BeginInvoke(new LineReceivedEvent(LineReceived), received);
        }

        private delegate void LineReceivedEvent(string POT);

        private void LineReceived(string POT)
        {
            textBox3.Text = System.DateTime.Now.TimeOfDay.ToString();
            char[] charsToTrim1 = { '#', 'A'};
            var x1 = POT.Trim(charsToTrim1);
/*            textBox1.Text = x1.ToString();
*/            cartesianChart1.Invalidate();
            if (list.Count > 10)
            {
                list.RemoveAt(0);
                list2.RemoveAt(0);
            }
            if (POT.Contains("#A"))
            {
                char[] charsToTrim = { '#', 'A' };
                var x = POT.TrimStart(charsToTrim);
                textBox1.Text = x.ToString();
                StaticsMethods.SeparatePIN(x, list);
                cartesianChart1.Series.ElementAt(0).Values = list;

            }
            else if (POT.Contains("#B"))
            {
                char[] charsToTrim = { '#', 'B' };
                POT = POT.Trim(charsToTrim);
                textBox2.Text = POT.ToString();
                StaticsMethods.SeparatePIN(POT, list2);
                cartesianChart1.Series.ElementAt(1).Values = list2;
            }
                
        }
        private void button1_Click(object sender, EventArgs e)
        {


            using (var bmp = new Bitmap(cartesianChart1.Width, cartesianChart1.Height))
            {
                cartesianChart1.DrawToBitmap(bmp, new Rectangle(0, 0, cartesianChart1.Width, cartesianChart1.Height));
                bmp.Save("screenshotchart1.png");
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                _serialPort.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }

}


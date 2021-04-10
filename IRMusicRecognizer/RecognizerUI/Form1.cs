using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Globalization;
using System.IO;
using Accord;

namespace RecognizerUI
{
    public partial class Form1 : Form
    {
        // MICROPHONE ANALYSIS SETTINGS
        private int RATE = 44100; // sample rate of the sound card
        private int BUFFERSIZE = (int)Math.Pow(2, 11); // must be a multiple of 2

        public Form1()
        {
            InitializeComponent();
            StartListeningToMicrophone();
            //timer1.Enabled = true;
        }
        public void StartListeningToMicrophone(int audioDeviceNumber = 0)
        {
            WaveIn wi = new WaveIn();
            //wi.DeviceNumber = audioDeviceNumber;
            //wi.WaveFormat = new NAudio.Wave.WaveFormat(RATE, 1);
            //wi.BufferMilliseconds = (int)((double)BUFFERSIZE / (double)RATE * 1000.0);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(AudioDataAvailable);
            try
            {
                wi.StartRecording();
            }
            catch
            {
                string msg = "Could not record from audio device!\n\n";
                msg += "Is your microphone plugged in?\n";
                msg += "Is it set as your default recording device?";
                MessageBox.Show(msg, "ERROR");
            }
        }
        private long prevDateTime = DateTime.Now.Ticks;
        void AudioDataAvailable(object sender, WaveInEventArgs e)
        {
            var nowTicks = DateTime.Now.Ticks;
            var ticksDelta = (nowTicks - prevDateTime) / 10000; // 10k ticks in 1 ms
            prevDateTime = nowTicks;

            //bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
            this.Text = $"length: {e.Buffer.Length}, b recorded: {e.BytesRecorded}, |{ticksDelta}";
            var frequencies = ByteArrayToHz(e.Buffer).Where(el => el != 0).ToArray();
            label1.Text = $"[{ string.Join(", ", frequencies) }]";

            // this reduces flicker and helps keep the program responsive
            Application.DoEvents();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //
        }
        public double[] FFT(double[] data)
        {
            var acceptedLength = (int)Math.Pow(2, Math.Floor(Math.Log2(data.Length)));
            double[] fft = new double[acceptedLength];
            var fftComplex = new System.Numerics.Complex[acceptedLength];
            for (int i = 0; i < acceptedLength; i++)
                fftComplex[i] = new System.Numerics.Complex(data[i], 0.0);
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < acceptedLength; i++)
                fft[i] = fftComplex[i].Magnitude;
            return fft;
        }

        double[] ByteArrayToHz(byte[] bytes)
        {
            // return if there's nothing new to plot
            if (bytes.Length == 0)
                return new double[] { };
            //if (audioBytes[frameSize - 2] == 0)
            //    return;

            // incoming data is 16-bit (2 bytes per audio point)
            int BYTES_PER_POINT = 2;

            // create a (32-bit) int array ready to fill with the 16-bit data
            int graphPointCount = bytes.Length / BYTES_PER_POINT;

            // create double arrays to hold the data we will graph
            double[] pcm = new double[graphPointCount];
            double[] fft = new double[graphPointCount];
            double[] fftReal = new double[graphPointCount / 2];

            // populate Xs and Ys with double data
            for (int i = 0; i < graphPointCount; i++)
            {
                // read the int16 from the two bytes
                Int16 val = BitConverter.ToInt16(bytes, i * 2);

                // store the value in Ys as a percent (+/- 100% = 200%)
                pcm[i] = (double)(val) / Math.Pow(2, 16) * 200.0;
            }

            // calculate the full FFT
            fft = FFT(pcm);

            // pcm - audio bytes, fft - (hope) frequency

            // just keep the real half (the other half imaginary)
            Array.Copy(fft, fftReal, fftReal.Length);

            // Values are: pcm, fftReal
            // scottPlotUC1.PlotSignal(pcm, pcmPointSpacingMs, Color.Blue); scottPlotUC2.PlotSignal(fftReal, fftPointSpacingHz, Color.Blue);

            return fftReal.ToArray();
        }
    }
}

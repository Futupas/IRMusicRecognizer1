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
        //private int BUFFERSIZE = (int)Math.Pow(2, 11); // must be a multiple of 2
        private int BUFFERSIZE = (int)Math.Pow(2, 9); // must be a multiple of 2

        // prepare class objects
        public BufferedWaveProvider bwp;

        public Form1()
        {
            InitializeComponent();
            StartListeningToMicrophone();
            //timer1.Enabled = true;
        }
        public void StartListeningToMicrophone(int audioDeviceNumber = 0)
        {
            WaveIn wi = new WaveIn();
            wi.DeviceNumber = audioDeviceNumber;
            wi.WaveFormat = new NAudio.Wave.WaveFormat(RATE, 1);
            wi.BufferMilliseconds = (int)((double)BUFFERSIZE / (double)RATE * 1000.0);
            wi.DataAvailable += new EventHandler<WaveInEventArgs>(AudioDataAvailable);
            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.BufferLength = BUFFERSIZE * 2;
            bwp.DiscardOnBufferOverflow = true;
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
            label1.Text = $"[{ string.Join(", ", e.Buffer) }";
            //label1.Text = $"[{ string.Join(", ", e.Buffer.Select(el => Math.Round((double)el * 100) / 100)) }";
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
            timer1.Enabled = false;
            PlotLatestData();
            timer1.Enabled = true;
        }
        public void PlotLatestData()
        {
            // check the incoming microphone audio
            int frameSize = BUFFERSIZE;
            var audioBytes = new byte[frameSize];
            bwp.Read(audioBytes, 0, frameSize);

            // return if there's nothing new to plot
            if (audioBytes.Length == 0)
                return;
            //if (audioBytes[frameSize - 2] == 0)
            //    return;

            // incoming data is 16-bit (2 bytes per audio point)
            int BYTES_PER_POINT = 2;

            // create a (32-bit) int array ready to fill with the 16-bit data
            int graphPointCount = audioBytes.Length / BYTES_PER_POINT;

            // create double arrays to hold the data we will graph
            double[] pcm = new double[graphPointCount];
            double[] fft = new double[graphPointCount];
            double[] fftReal = new double[graphPointCount / 2];

            // populate Xs and Ys with double data
            for (int i = 0; i < graphPointCount; i++)
            {
                // read the int16 from the two bytes
                Int16 val = BitConverter.ToInt16(audioBytes, i * 2);

                // store the value in Ys as a percent (+/- 100% = 200%)
                pcm[i] = (double)(val) / Math.Pow(2, 16) * 200.0;
            }

            // calculate the full FFT
            fft = FFT(pcm);

            // determine horizontal axis units for graphs
            double pcmPointSpacingMs = RATE / 1000;
            double fftMaxFreq = RATE / 2;
            double fftPointSpacingHz = fftMaxFreq / graphPointCount;

            // just keep the real half (the other half imaginary)
            Array.Copy(fft, fftReal, fftReal.Length);

            // Values are: pcm, fftReal
            // scottPlotUC1.PlotSignal(pcm, pcmPointSpacingMs, Color.Blue); scottPlotUC2.PlotSignal(fftReal, fftPointSpacingHz, Color.Blue);

            label1.Text = $"[{ string.Join(", ", pcm.Select(el => Math.Round(el * 100) / 100)) }";
            label2.Text = $"[{ string.Join(", ", fftReal.Select(el => Math.Round(el * 100) / 100)) }]";
            //this.Text = $"pcm: {pcm.Length}, fft: {fftReal.Length}";

            // this reduces flicker and helps keep the program responsive
            Application.DoEvents();

        }
        public double[] FFT(double[] data)
        {
            double[] fft = new double[data.Length];
            System.Numerics.Complex[] fftComplex = new System.Numerics.Complex[data.Length];
            for (int i = 0; i < data.Length; i++)
                fftComplex[i] = new System.Numerics.Complex(data[i], 0.0);
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < data.Length; i++)
                fft[i] = fftComplex[i].Magnitude;
            return fft;
        }
    }
}

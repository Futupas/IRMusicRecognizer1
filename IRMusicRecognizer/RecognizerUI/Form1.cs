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

namespace RecognizerUI
{
    public partial class Form1 : Form
    {
        // WaveIn - поток для записи
        WaveIn waveIn;
        //Класс для записи в файл
        WaveFileWriter writer;
        //Имя файла для записи
        string outputFilename = "имя_файла.wav";
        // Indicate whether asynchronous recognition is complete.  
        static bool completed;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //
        }

        //Получение данных из входного буфера 
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<WaveInEventArgs>(waveIn_DataAvailable), sender, e);
            }
            else
            {
                //Записываем данные из буфера в файл
                var buffer = e.Buffer;
                writer.Write(buffer, 0, e.BytesRecorded);
            }
        }
        //Завершаем запись
        void StopRecording()
        {
            MessageBox.Show("StopRecording");
            waveIn.StopRecording();
        }
        //Окончание записи
        private void waveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<StoppedEventArgs>(waveIn_RecordingStopped), sender, e);
            }
            else
            {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
        }
        //Начинаем запись - обработчик нажатия кнопки
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Start Recording");
                waveIn = new WaveIn();
                //Дефолтное устройство для записи (если оно имеется)
                //встроенный микрофон ноутбука имеет номер 0
                waveIn.DeviceNumber = 0;
                //Прикрепляем к событию DataAvailable обработчик, возникающий при наличии записываемых данных
                waveIn.DataAvailable += waveIn_DataAvailable;
                //Прикрепляем обработчик завершения записи
                //waveIn.RecordingStopped += new EventHandler(waveIn_RecordingStopped);
                //waveIn.RecordingStopped += waveIn_RecordingStopped;
                waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(waveIn_RecordingStopped);
                //Формат wav-файла - принимает параметры - частоту дискретизации и количество каналов(здесь mono)
                waveIn.WaveFormat = new WaveFormat(8000, 1);
                //Инициализируем объект WaveFileWriter
                writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                //Начало записи
                waveIn.StartRecording();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        //Прерываем запись - обработчик нажатия второй кнопки
        private void button2_Click(object sender, EventArgs e)
        {
            if (waveIn != null)
            {
                StopRecording();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SpeechRecognitionEngine recognizer = new(new CultureInfo("en-US")))
            {

                // Create and load a grammar.  
                Grammar dictation = new DictationGrammar();
                dictation.Name = "Dictation Grammar";

                recognizer.LoadGrammar(dictation);

                // Configure the input to the recognizer.  
                recognizer.SetInputToAudioStream(
                  File.OpenRead(outputFilename),
                  new SpeechAudioFormatInfo(
                    44100, AudioBitsPerSample.Sixteen, AudioChannel.Mono));

                // Attach event handlers.  
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(
                    SpeechRecognizedHandler);
                recognizer.RecognizeCompleted +=
                  new EventHandler<RecognizeCompletedEventArgs>(
                    RecognizeCompletedHandler);

                // Perform recognition of the whole file.  
                textBox1.AppendText("Starting asynchronous recognition...\r\n");
                completed = false;
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                while (!completed)
                {
                    Thread.Sleep(333);
                }
                textBox1.AppendText("Done.\r\n");
            }
        }


        // Handle the SpeechRecognized event.  
        void SpeechRecognizedHandler(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null && e.Result.Text != null)
            {
                textBox1.AppendText($"Recognized text =  {e.Result.Text}\r\n");
            }
            else
            {
                textBox1.AppendText("Recognized text not available.\r\n");
            }
        }

        // Handle the RecognizeCompleted event.  
        void RecognizeCompletedHandler(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                textBox1.AppendText($"Error encountered, {e.Error.GetType().Name}: {e.Error.Message}\r\n");
            }
            if (e.Cancelled)
            {
                textBox1.AppendText("Operation cancelled.\r\n");
            }
            if (e.InputStreamEnded)
            {
                textBox1.AppendText("End of stream encountered.\r\n");
            }

            completed = true;
        }
    }
}

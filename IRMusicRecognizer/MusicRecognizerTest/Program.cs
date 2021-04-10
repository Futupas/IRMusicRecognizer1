using System;
using System.Globalization;
using System.IO;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Threading;
using NAudio;
using NAudio.Wave;
using System.Threading;

namespace MusicRecognizerTest
{
    // https://docs.microsoft.com/ru-ru/dotnet/api/system.speech.recognition.speechrecognitionengine.setinputtoaudiostream?view=netframework-4.8
    class Program
    {
        // Indicate whether asynchronous recognition is complete.  
        static bool completed;

        static void Main(string[] args)
        {
            new Thread(() => {

                var waveIn = new WaveIn();
                waveIn.StartRecording();
                waveIn.DataAvailable += waveIn_DataAvailable;
                Console.WriteLine(waveIn.DeviceNumber);
            }).Start();

            //using (SpeechRecognitionEngine recognizer = new(new CultureInfo("en-US")))
            //{

            //    // Create and load a grammar.  
            //    Grammar dictation = new DictationGrammar();
            //    dictation.Name = "Dictation Grammar";

            //    recognizer.LoadGrammar(dictation);

            //    // Configure the input to the recognizer.  
            //    recognizer.SetInputToAudioStream(
            //      File.OpenRead(@"c:\temp\audioinput\example.wav"),
            //      new SpeechAudioFormatInfo(
            //        44100, AudioBitsPerSample.Sixteen, AudioChannel.Mono));

            //    // Attach event handlers.  
            //    recognizer.SpeechRecognized +=
            //      new EventHandler<SpeechRecognizedEventArgs>(
            //        SpeechRecognizedHandler);
            //    recognizer.RecognizeCompleted +=
            //      new EventHandler<RecognizeCompletedEventArgs>(
            //        RecognizeCompletedHandler);

            //    // Perform recognition of the whole file.  
            //    Console.WriteLine("Starting asynchronous recognition...");
            //    completed = false;
            //    recognizer.RecognizeAsync(RecognizeMode.Multiple);

            //    while (!completed)
            //    {
            //        Thread.Sleep(333);
            //    }
            //    Console.WriteLine("Done.");
            //}

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        
        //Получение данных из входного буфера 
        static void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            Console.WriteLine(e.BytesRecorded);
            //Console.WriteLine(e.Buffer, 0, e.BytesRecorded);
        }

        // Handle the SpeechRecognized event.  
        static void SpeechRecognizedHandler(
          object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null && e.Result.Text != null)
            {
                Console.WriteLine("  Recognized text =  {0}", e.Result.Text);
            }
            else
            {
                Console.WriteLine("  Recognized text not available.");
            }
        }

        // Handle the RecognizeCompleted event.  
        static void RecognizeCompletedHandler(
          object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.WriteLine("  Error encountered, {0}: {1}",
                  e.Error.GetType().Name, e.Error.Message);
            }
            if (e.Cancelled)
            {
                Console.WriteLine("  Operation cancelled.");
            }
            if (e.InputStreamEnded)
            {
                Console.WriteLine("  End of stream encountered.");
            }

            completed = true;
        }
    }
}

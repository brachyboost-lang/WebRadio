using RestSharp;
using NAudio;
using NAudio.Wave;
using System.Text.Json;


namespace WebRadio
{    class Program
    {
        static void Main(string[] args)
        {
            var client1 = new RestClient("https://de1.api.radio-browser.info/");
            var request = new RestRequest("json/stations/byname/", Method.Get);
            var response = client1.Post(request);
            var result = JsonDocument.Parse(response.Content);

            foreach (var station in result.RootElement.EnumerateArray())
            {
                Console.WriteLine($"Name: {station.GetProperty("name").GetString()} Url: {station.GetProperty("url").GetString()}");
            }

            //var client = new RestClient("http://streamer.psyradio.org:8010/;listen.mp3");
            //var request = new RestRequest("");
            //var response = client.Execute(request);
            //if (response.IsSuccessful)
            //{
            //    // Assuming the response contains a stream of audio data
            //    using (var audioStream = new MemoryStream(response.RawBytes))
            //    {
            //        // Use NAudio to play the audio stream
            //        using (var waveOut = new WaveOutEvent())
            //        using (var waveStream = new WaveFileReader(audioStream))
            //        {
            //            waveOut.Init(waveStream);
            //            waveOut.Play();
            //            // Keep the application running while the audio is playing
            //            while (waveOut.PlaybackState == PlaybackState.Playing)
            //            {
            //                Thread.Sleep(100);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Failed to connect to the radio stream.");
            //}
        }
}
}

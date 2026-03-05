using NAudio;
using NAudio.Wave;
using RestSharp;
using System.Reflection;
using System.Text.Json;


namespace WebRadio
{ class Program
    {
        static void Main(string[] args)
        {
            var sender = new Dictionary<string, string>();


            var client = new RestClient("https://de1.api.radio-browser.info/");
            var request = new RestRequest("json/stations/byname/", Method.Get);
            var response = client.Execute(request);

            var result = JsonDocument.Parse(response.Content);
            var counter = 1;
            foreach (var station in result.RootElement.EnumerateArray())
            {
                sender.Add(counter + ". " + station.GetProperty("name").GetString(), station.GetProperty("url").GetString());
                counter++;
            }
            var radioURL = Search(sender);
            Player(radioURL);
            Console.ReadKey();
            Console.WriteLine("Danke fürs Zuhören!");

        }
        //static void Main(string[] args)
        //{
        //    var stations = new Dictionary<string, string>();

        //    var client1 = new RestClient("https://de1.api.radio-browser.info/");
        //    var request = new RestRequest("json/stations/byname/", Method.Get);
        //    var response = client1.Execute(request);
        //    var result = JsonDocument.Parse(response.Content);

        //    var count = 0;
        //    foreach (var station in result.RootElement.EnumerateArray())
        //    {
        //        count++;
        //        Console.WriteLine($"{count} Name: {station.GetProperty("name").GetString()} Url: {station.GetProperty("url").GetString()}");
        //    }
        //    var radioURL = Search(stations);
        //    Player(radioURL);
        //    Console.ReadKey();
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
        
        //public static string Search(Dictionary<string, string> station)
        //{
        //    Console.WriteLine("Search for a radio station:\n\n");
        //    var search = "";
        //    var url = new List<string>();
        //    while (true)
        //    {
        //        var key = Console.ReadKey(true);
        //        if (key.Key == ConsoleKey.Backspace && search.Length > 0)
        //        {
        //            search = search.Substring(0, search.Length - 1);
        //        }
        //        else if (key.Key == ConsoleKey.Enter)
        //        {
        //            break;
        //        }
        //        else
        //        {
        //            search += key.KeyChar;
        //        }
        //        Console.Clear();

        //        Console.WriteLine("Search for a radio station:\n\n");
        //        Console.WriteLine(search + "\n\n\n Results:");
        //        url.Clear();
        //        station.Where(x => x.Key.ToLower().Contains(search.ToLower())).ToList().ForEach(x =>
        //        {
        //            Console.WriteLine(x.Key); url.Add(x.Value);
        //        });


        //    }
        //    return url.Count == 1 ? url[0] : "";
        //}
        public static string Search(Dictionary<string, string> sender)
        {
            Console.Clear();
            Console.WriteLine("Search for a station: \n");
            var search = "";
            var url = new List<string>();
            while (true)
            {


                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && search.Length > 0) search = search.Substring(0, search.Length - 1);
                else if (key.Key == ConsoleKey.Enter) break;
                else search += key.KeyChar;
                Console.Clear();

                Console.WriteLine("Search for a station: \n");

                Console.WriteLine(search + "\n\n\n Ergebnisse:");
                url.Clear();
                sender.Where(x => x.Key.ToLower().Contains(search.ToLower())).ToList().ForEach(x => {
                    Console.WriteLine(x.Key);
                    url.Add(x.Value);
                });


            }
            return url.Count == 1 ? url[0] : "";
        }
        public static async void Player(string url)
        {
            var reader = new MediaFoundationReader(url);
            var waveOut = new WaveOutEvent();
            waveOut.Init(reader);
            waveOut.Play();

            Console.WriteLine("Press enter to stop playback");
            Console.ReadLine();
            waveOut.Stop();
        }
    }
}

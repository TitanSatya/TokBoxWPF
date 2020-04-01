using JWT.Algorithms;
using JWT.Builder;
using Newtonsoft.Json;
using OpenTok;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TokBoxPOC.Sever;
 
namespace TokBoxPOC.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Session Session;
        private Publisher Publisher;
        private Subscriber subscriber;
        private const string API_KEY = "46617502";
        private string SESSION_ID = "";
        private string TOKEN = "";
        private string GENERATERD_API_KEY = "";
        private readonly string shareURL = "https://opentokclientpoc.herokuapp.com";
        private string newURL = "";
        private bool isAudioPublished = true;
        private bool isVideoPublished = true;
        private bool isCallStarted = false;
        private bool isCallRecordingEnabled = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           

        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {
                if (Session != null)
                {
                    Session.Disconnect();
                    Session = null;
                }
                if (Publisher != null)
                {
                    Publisher = null;
                }

                if (subscriber != null)
                {
                    subscriber = null;
                }
            }



            catch (Exception)
            {


            }

        }

        private void Publisher_AudioLevel(object sender, Publisher.AudioLevelArgs e)
        {
            try
            {

            }
            catch (Exception)
            {


            }
        }

        private void Session_StreamHasAudioChanged(object sender, Session.StreamEventArgs e)
        {

        }


        private void Session_ConnectionCreated(object sender, Session.ConnectionEventArgs e)
        {

        }

        private void GetSessionDetails()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://opentoksatyaserver.herokuapp.com");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("/room/session").Result;
                if (response.IsSuccessStatusCode)
                {
                    OpenTokCredentials credentials = response.Content.ReadAsAsync<OpenTokCredentials>().Result;
                    GENERATERD_API_KEY = credentials.apiKey;
                    SESSION_ID = credentials.sessionId;
                    TOKEN = credentials.token;
                }
            }
        }

        private void Session_Error(object sender, Session.ErrorEventArgs e)
        {

        }

        private void Session_Disconnected(object sender, EventArgs e)
        {

        }

        private void Session_Connected(object sender, EventArgs e)
        {
            Session.Publish(Publisher);
        }

        private void Session_StreamReceived(object sender, Session.StreamEventArgs e)
        {
            subscriber = new Subscriber(Context.Instance, e.Stream, SubscriberVideo);

            Session.Subscribe(subscriber);
        }

        private void Subscriber_AudioLevel(object sender, Subscriber.AudioLevelArgs e)
        {
            try
            {

            }
            catch (Exception)
            {


            }
        }

        private class OpenTokCredentials
        {
            public string apiKey { get; set; }
            public string sessionId { get; set; }
            public string token { get; set; }
        }

        private void CallEndButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isCallStarted)
            {
                var result = MessageBox.Show("Do you want to start recording the session ?", "Recording", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    //Get The Session token
                    GetSessionDetails();
                    StartSession();
                }
                else
                {
                    enterPopup.IsOpen = true;
                }
             
            }
            else
            {
                this.Close();
            }

        }
        private void StartSession()
        {
            Publisher = new Publisher(Context.Instance, renderer: PublisherVideo);
            Session = new Session(Context.Instance, API_KEY, SESSION_ID);
            Session.Connected += Session_Connected;
            Session.Disconnected += Session_Disconnected;
            Session.Error += Session_Error;
            Session.StreamReceived += Session_StreamReceived;
            Session.ConnectionCreated += Session_ConnectionCreated;
            Session.StreamHasAudioChanged += Session_StreamHasAudioChanged;
            Session.Connect(TOKEN);
            CallEndButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E55354"));
            CallEndButton.Content = new MaterialDesignThemes.Wpf.PackIcon
            { Kind = MaterialDesignThemes.Wpf.PackIconKind.CallEnd, Height = 36, Width = 36 };
            isCallStarted = true;

        }
        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            SharePopup.IsOpen = true;
            newURL = string.Concat(shareURL, "?", "sessionid=", SESSION_ID, "&token=", TOKEN);
            ShareTextBox.Text = newURL;
            //using (HttpClient client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://cutt.ly/api/api.php?key=b053ae28cec96a04574e60d577e6efc4f7624&short="+ newURL);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = client.GetStringAsync("").Result;
            //    URLRootObject root = JsonConvert.DeserializeObject<URLRootObject>(response);
            //    ShareTextBox.Text = root.url.shortLink;
            //}

            //using (HttpClient client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://cutt.ly/api/api.php?key=b053ae28cec96a04574e60d577e6efc4f7624&short=" + string.Concat("https://staging.paydc.com/patient/Meeting", "?", "sessionid=", SESSION_ID, "&token=", TOKEN));
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = client.GetStringAsync("").Result;
            //    URLRootObject root = JsonConvert.DeserializeObject<URLRootObject>(response);
            //    ShareStagingTextBox.Text = root.url.shortLink;
            //}
            ShareStagingTextBox.Text = string.Concat("https://staging.paydc.com/patient/Meeting", "?", "sessionid=", SESSION_ID, "&token=", TOKEN);
            ShareSessionIdTextBox.Text = SESSION_ID;
            ShareTokenTextBox.Text = TOKEN;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SharePopup.IsOpen = false;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ShareTextBox.Text);
        }

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            isAudioPublished = !isAudioPublished;
            Publisher.PublishAudio = isAudioPublished;


            if (!isAudioPublished)
            {
                VolumeButton.Content = new MaterialDesignThemes.Wpf.PackIcon
                { Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeMute, Height = 36, Width = 36 };
            }
            else
            {
                VolumeButton.Content = new MaterialDesignThemes.Wpf.PackIcon
                { Kind = MaterialDesignThemes.Wpf.PackIconKind.VolumeHigh, Height = 36, Width = 36 };
            }
        }

        private void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            isVideoPublished = !isVideoPublished;
            Publisher.PublishVideo = isVideoPublished;


            if (!isVideoPublished)
            {
                VideoButton.Content = new MaterialDesignThemes.Wpf.PackIcon
                { Kind = MaterialDesignThemes.Wpf.PackIconKind.VideocamOff, Height = 36, Width = 36 };
            }
            else
            {
                VideoButton.Content = new MaterialDesignThemes.Wpf.PackIcon
                { Kind = MaterialDesignThemes.Wpf.PackIconKind.Videocam, Height = 36, Width = 36 };
            }

        }

        private void btnStagingCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ShareStagingTextBox.Text);
        }



        private void btnShareSessionId_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(SESSION_ID);
        }

        private void btnToken_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TOKEN);
        }

        private async void RwecordingButton_Click(object sender, RoutedEventArgs e)
        {
            //string recordingURL = "https://api.opentok.com/v2/project/46617502/archive";
            //var client = new HttpClient();
            //client.BaseAddress = new Uri(recordingURL);
            //client.DefaultRequestHeaders.Add("X-OPENTOK-AUTH", JWTCreator());
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //CreateRequestRoot();
            //var response = await client.PostAsJsonAsync("", CreateRequestRoot());
            //if (response != null)
            //{
            //    //Do Extract
            //}Session.

            //return opentok.StartArchive((string)Application["sessionId"], "DotNet Archiving Sample").Id.ToString();
            
        }

        private RequestRoot CreateRequestRoot()
        {
            RequestRoot root = new RequestRoot()
            {
                sessionId = SESSION_ID,
                hasAudio = true.ToString(),
                hasVideo = true.ToString(),
                layout = null,
                name = String.Concat("Archive", DateTime.Now.ToShortTimeString()),
                outputMode = "composed",
                resolution = "640x480"
            };
            return root;
        }

        private string JWTCreator()
        {
            var token = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                
                
                .AddClaim("iss", API_KEY)
                .AddClaim("ist", "project")
                .AddClaim("iat", ((DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60) + DateTime.Now.Second).ToString())
                .AddClaim("exp", ((DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60) + DateTime.Now.Second + 180).ToString())
                .AddClaim("jti", Guid.NewGuid().ToString())
                .Encode();
            return token;
        }

        private void btnStartSession_Click(object sender, RoutedEventArgs e)
        {
            SESSION_ID = enterTextbox.Text;
            TOKEN = enterToken.Text;
            if(string.IsNullOrEmpty(SESSION_ID))
            {
                MessageBox.Show("Session Id is empty");
                return;
            }
            if (string.IsNullOrEmpty(TOKEN))
            {
                MessageBox.Show("Token is empty");
                return;
            }
           
            StartSession();
            stackRecording.Visibility = Visibility.Visible;
            enterPopup.IsOpen = false;


        }
        
    }
    public class Layout
    {
        public string type { get; set; }
        public string stylesheet { get; set; }
    }

    public class RequestRoot
    {
        public string sessionId { get; set; }
        public string hasAudio { get; set; }
        public string hasVideo { get; set; }
        public Layout layout { get; set; }
        public string name { get; set; }
        public string outputMode { get; set; }
        public string resolution { get; set; }
    }


    public class Url
    {
        public int status { get; set; }
        public string fullLink { get; set; }
        public string date { get; set; }
        public string shortLink { get; set; }
        public string title { get; set; }
    }

    public class URLRootObject
    {
        public Url url { get; set; }
    }
}

using OpenTok;

using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get The Session token
            GetSessionDetails();

            Publisher = new Publisher(Context.Instance, renderer: PublisherVideo);
            Session = new Session(Context.Instance, API_KEY, SESSION_ID);
            Session.Connected += Session_Connected;
            Session.Disconnected += Session_Disconnected;
            Session.Error += Session_Error;
            Session.StreamReceived += Session_StreamReceived;
            Session.ConnectionCreated += Session_ConnectionCreated;
            Session.StreamHasAudioChanged += Session_StreamHasAudioChanged;
            Session.Connect(TOKEN);
            
            



        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

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
            if (subscriber != null)
            {
                Session.Disconnect();
                Close();
            }
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            SharePopup.IsOpen = true;
            newURL = string.Concat(shareURL, "?", "sessionid=", SESSION_ID, "&token=", TOKEN);
            ShareTextBox.Text = newURL;
            ShareTextBox.SelectAll();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SharePopup.IsOpen = false;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(newURL);
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
    }
}

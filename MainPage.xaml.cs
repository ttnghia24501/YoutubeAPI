using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using YoutubeAPI.Model;
using System.Net.NetworkInformation;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace YoutubeAPI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private YouTubeService youtubeService =
            new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyDQDvH9uUsjE8vfXfMY70kU17j8KOG7JrU",
                ApplicationName = "youtube"
            });
        List<Video> ListVideo = new List<Video>();
        private string TokenNextPage = null, TokenPrivPage = null;
        public MainPage()
        {
            this.InitializeComponent();
            GetVideo();
        }
        private async void GetVideo(string PageToken = null)
        {
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    var Request = youtubeService.Search.List("snippet");
                    Request.ChannelId = "UCWovDkXc7w0821EaKmznmCA";//vera madsen
                    Request.MaxResults = 25;
                    Request.Type = "video";
                    Request.Order = SearchResource.ListRequest.OrderEnum.Date;
                    Request.PageToken = PageToken;
                    var Result = await Request.ExecuteAsync();
                    if (Result.NextPageToken != null)
                        TokenNextPage = Result.NextPageToken;
                    if (Result.PrevPageToken != null)
                        TokenPrivPage = Result.PrevPageToken;
                    foreach (var item in Result.Items)
                    {
                        ListVideo.Add(new Video
                        {
                            Title = item.Snippet.Title,
                            Id = item.Id.VideoId,
                            Img = item.Snippet.Thumbnails.Default__.Url
                        });
                    }
                    lv.ItemsSource = null;
                    lv.ItemsSource = ListVideo;
                }
                else
                {
                    MessageDialog msg = new MessageDialog("Check your internet connection");
                    await msg.ShowAsync();
                }
            }
            catch { }
            }
        private void lv_ItemClick(object sender,ItemClickEventArgs e)
        {
            Video video = e.ClickedItem as Video;
            Frame.Navigate(typeof(VideoPage), video);
        }
        
    }
}

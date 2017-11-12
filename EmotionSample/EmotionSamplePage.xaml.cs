using EmotionSample.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace EmotionSample
{
    public partial class EmotionSamplePage : ContentPage
    {
        MediaFile _file;

        public EmotionSamplePage()
        {
            InitializeComponent();
        }

        async void ComputeImage(object sender, System.EventArgs e)
        {
            if (_file == null)
                return;

            var result = await EmotionService.GetAverageEmotionAsync(_file.GetStream());

            await DisplayAlert("Message", result, "Ok");
        }

        async void CaptureImage(object sender, System.EventArgs e)
        {
            _file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Small,
                Name = "helloEmotions.jpg",
                Directory = "EmotionSample"
            });

            if (_file == null)
                return;

            image.Source = ImageSource.FromStream(() => (_file.GetStream()));
        }
    }
}

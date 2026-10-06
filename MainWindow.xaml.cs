using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace StoryGameApp;

public partial class MainWindow : Window
{
    // Each Slide keeps its text, image and description together.
    private record Slide(string Text, string ImagePath, string Description);
    private readonly Slide[] slides =
    {
        new Slide("Welcome New Members to Greenward. This is the map of the garden and also the different plant types that this garden holds. You're going to want to get used to this and memorize this like the back of your hand!", "Assets/Images/01.png", "A signal card: Introduction to the Garden."),
        new Slide("Here is the list of all the types of plants we have at Greenward, when it comes to our plant portfolio its the best in the land.", "Assets/Images/02.png", "A route card: Plant type list."),
        new Slide("Here is the instructions on how to tend to the plants and upkeep Greenward. Please refer to the instructions list anytime you dont know how to tend to a certain plant.", "Assets/Images/03.png", "A handoff card: PLant tending instructions.")
    };
    private int currentSlide = 0; // Array positions start at zero.

    public MainWindow()
    {
        InitializeComponent(); // Build the named controls from XAML first.
        ShowSlide(currentSlide);
    }

    private void ShowSlide(int index)
    {
        if (index < 0 || index >= slides.Length) return;
        currentSlide = index;
        Slide slide = slides[currentSlide];
        StoryTextBlock.Text = slide.Text;
        ImageDescription.Text = slide.Description;
        SlideCounter.Text = $"Slide {currentSlide + 1} of {slides.Length}";
        BackButton.IsEnabled = currentSlide > 0;
        NextButton.IsEnabled = currentSlide < slides.Length - 1;
        try
        {
            string path = Path.Combine(AppContext.BaseDirectory, slide.ImagePath);
            if (!File.Exists(path)) throw new FileNotFoundException("Image file was not found.", path);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = new Uri(path, UriKind.Absolute);
            image.EndInit();
            StoryImage.Source = image;
            StatusText.Text = "Story ready.";
        }
        catch (Exception ex) when (ex is IOException || ex is NotSupportedException || ex is System.IO.FileFormatException)
        {
            StoryImage.Source = null;
            StatusText.Text = "Image unavailable. Check Assets/Images and the filename. " + ex.Message;
        }
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        ShowSlide(currentSlide + 1);
    }
    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        ShowSlide(currentSlide - 1);
    }
    private void RestartButton_Click(object sender, RoutedEventArgs e)
    {
        ShowSlide(0);
    }
    // DAY 7B: paste CaptureHandlers.cs.txt HERE, inside these class braces.
}

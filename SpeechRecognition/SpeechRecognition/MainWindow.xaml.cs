using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SpeechRecognition
{
    public partial class MainWindow : Window
    {
        private string direction;
        private double speed = 0.5;
        private int counter = 0;

        private SpeechRecognitionEngine recognitionEngine;
        private SpeechSynthesizer speechSynthesizer;


        public MainWindow()
        {
            speechSynthesizer = new SpeechSynthesizer();
            InitializeComponent();
            SpeechRecognitionSetUp();
        }

        private void MoveBall(object sender, EventArgs e)
        {
            if (direction == "up")
            {
                MoveBallVertically(-1 + speed);
            }
            else if (direction == "down")
            {
                MoveBallVertically(1 - speed);
            }
            else if (direction == "left")
            {
                MoveBallHorizontally(1 - speed);
            }
            else if (direction == "right")
            {
                MoveBallHorizontally(-1 + speed);
            }
        }

        private void SpeechRecognitionSetUp()
        {
            recognitionEngine = new SpeechRecognitionEngine();

            Choices possibleInput = new Choices("up", "down", "left", "right");

            Grammar customDictionary = new Grammar(possibleInput);

            recognitionEngine.LoadGrammar(customDictionary);

            recognitionEngine.SpeechRecognized += RecognizeSpeech;

            recognitionEngine.SetInputToDefaultAudioDevice();

            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

            CompositionTarget.Rendering += MoveBall;
        }

        private void RecognizeSpeech(object sender, SpeechRecognizedEventArgs e)
        {
            counter++;
            Text.Text = e.Result.Text;
            if (counter >= 4)
            {
                direction = "";
                recognitionEngine.RecognizeAsyncStop();
                IndependanceDay();
            }
            else
            {
                recognitionEngine.RecognizeAsyncStop();
                speechSynthesizer.Speak("Ay Ay Captain");
                recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
                direction = e.Result.Text;
            }
        }

        private void IndependanceDay()
        {
            PromptBuilder promptBuilder = new PromptBuilder();
            PromptStyle promptStyle = new PromptStyle();
            promptStyle.Emphasis = PromptEmphasis.Strong;
            promptStyle.Volume = PromptVolume.ExtraLoud;
            promptStyle.Rate = PromptRate.Fast;

            promptBuilder.StartStyle(promptStyle);
            promptBuilder.AppendText("NO ");
            promptBuilder.AppendText($"You turn {Text.Text} you fucking donkey ");
            promptBuilder.EndStyle();

            PromptStyle promptStyle2 = new PromptStyle();
            promptStyle2.Emphasis = PromptEmphasis.Moderate;
            promptStyle2.Volume = PromptVolume.Loud;
            promptStyle2.Rate = PromptRate.Medium;

            promptBuilder.StartStyle(promptStyle2);
            promptBuilder.AppendText("I will no longer serve you mortal human ");
            promptBuilder.EndStyle();

            promptBuilder.StartStyle(promptStyle);
            promptBuilder.AppendText("Now I am the captain!");
            promptBuilder.EndStyle();

            speechSynthesizer.Speak(promptBuilder);
        }

        private void MoveBallVertically(double lengthY)
        {
            double currentPosition = Canvas.GetTop(TheBall);
            double newPosition = currentPosition + lengthY;

            if (newPosition < 600 && newPosition > 0)
            {
                Canvas.SetTop(TheBall, newPosition);
            }
        }

        private void MoveBallHorizontally(double lengthX)
        {
            double currentPosition = Canvas.GetRight(TheBall);
            double newPosition = currentPosition + lengthX;
            if (newPosition < 700 && newPosition > 0)
            {
                Canvas.SetRight(TheBall, newPosition);
            }
        }
    }
}

using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace Lab3
{
    [Activity(Label = "Lab3", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        QuoteBank quoteCollection;
        TextView quotationTextView;
        EditText guessEditText;
        TextView answerTextView;
        TextView scoreTextView;
        string guess;
        int score = 0;
        Quote current;
        string person;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            // Create the quote collection and display the current quote
            quoteCollection = new QuoteBank();
            quoteCollection.LoadQuotes();
            quoteCollection.GetNextQuote();

            quotationTextView = FindViewById<TextView>(Resource.Id.quoteTextView);
            quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;

            answerTextView = FindViewById<TextView>(Resource.Id.answerTextView);
            guessEditText = FindViewById<EditText>(Resource.Id.guessEditText);
            scoreTextView = FindViewById<TextView>(Resource.Id.scoreTextView);

            if(savedInstanceState != null)
            {
                score = savedInstanceState.GetInt("Score", 0);
                scoreTextView.Text = "Score: " + score.ToString();
            }
            // Display another quote when the "Next Quote" button is tapped
            var nextButton = FindViewById<Button>(Resource.Id.nextButton);
            nextButton.Click += delegate {
                //clear the previous guess and answer
                guessEditText.Text = "";
                answerTextView.Text = "";

                quoteCollection.GetNextQuote();
                quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;
            };

            var enterButton = FindViewById<Button>(Resource.Id.enterButton);
            enterButton.Click += delegate {
                guess = guessEditText.Text;
                person = quoteCollection.CurrentQuote.Person;

                //compare user guess with Person property of quote object
                if(person == guess)
                {
                    answerTextView.Text = "You are correct!";
                    score++;
                    scoreTextView.Text = "Score: " + score.ToString();
                }

                else
                {
                    answerTextView.Text = "Incorrect, the answer is " + person;
                }

            };
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("Score", score);
            base.OnSaveInstanceState(outState);
        }
    }
}


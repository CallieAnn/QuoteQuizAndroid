using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.IO;
using System.Xml.Serialization;

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
        int right = 0;
        int wrong = 0;
        string person;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            answerTextView = FindViewById<TextView>(Resource.Id.answerTextView);
            guessEditText = FindViewById<EditText>(Resource.Id.guessEditText);
            scoreTextView = FindViewById<TextView>(Resource.Id.scoreTextView);

            if(savedInstanceState != null)
            {
                //Deserialize the QuoteBank object
                string currentQuotes = savedInstanceState.GetString("QuoteCollection");
                XmlSerializer x = new XmlSerializer(typeof(QuoteBank));
                quoteCollection = (QuoteBank)x.Deserialize(new StringReader(currentQuotes));

                //get the score from the bundle and display
                right = savedInstanceState.GetInt("Right", 0);
                wrong = savedInstanceState.GetInt("Wrong", 0);
                SetScore();
            }
            else
            {   // Create the quote collection and display the current quote
                quoteCollection = new QuoteBank();
                quoteCollection.LoadQuotes();
                quoteCollection.GetNextQuote();
            }

            quotationTextView = FindViewById<TextView>(Resource.Id.quoteTextView);
            quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;

            

            
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
                    answerTextView.Text = GetString(Resource.String.CorrectAnswer);
                    right++;
                    SetScore();
                }

                else
                {
                    wrong++;
                    SetScore();
                    answerTextView.Text = GetString(Resource.String.IncorrectAnswer) + person;
                }

            };

            var restartButton = FindViewById<Button>(Resource.Id.restartButton);
            restartButton.Click += delegate
            {
                guessEditText.Text = "";
                answerTextView.Text = "";
                scoreTextView.Text = "";

                right = 0;
                wrong = 0;

                quoteCollection = new QuoteBank();
                quoteCollection.LoadQuotes();
                quoteCollection.GetNextQuote();
                quotationTextView.Text = quoteCollection.CurrentQuote.Quotation;

            };
        }


        private void SetScore()
        {
            scoreTextView.Text = GetString(Resource.String.CorrectScore)
                + right.ToString() + "\n"  + GetString(Resource.String.IncorrectScore) 
                + wrong.ToString();
        }


        protected override void OnSaveInstanceState(Bundle outState)
        {
            //serialize the current quote collection.  
            StringWriter writer = new StringWriter();
            XmlSerializer quoteSerializer = new XmlSerializer(quoteCollection.GetType());
            quoteSerializer.Serialize(writer, quoteCollection);
            string currentQuoteCollection = writer.ToString();
            outState.PutString("QuoteCollection", currentQuoteCollection);

            //save the number of right and wrong answers
            outState.PutInt("Right", right);
            outState.PutInt("Wrong", wrong);
            base.OnSaveInstanceState(outState);
        }
    }
}


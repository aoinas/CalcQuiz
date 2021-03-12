using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace CalculationQuiz
{
    class MainViewModel : ViewModelBase
    {
        private static Decimal FACTOR = 5;
        private static List<Decimal> NUMBERS = new List<Decimal>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        //private static List<Decimal> NUMBERS = new List<Decimal>() { 0, 1, 2, 3 }; // For debugging
        private List<Calculation> _calculations = new List<Calculation>();
        private Random _rand = new Random();
        
        private int _currentCalculationIndex = 0;
        private int _correctAnswersCount = 0;
        private bool _alreadyAnswered = false;

        public bool AnswerIsRight { get => _answerIsRight; private set => Set(ref _answerIsRight, value); }
        public MainViewModel()
        {
            NextCmd = new RelayCommand(NextCmdExecute, NextCmdCanExecute);
            AnswerCmd = new RelayCommand<decimal>(AnswerCmdExecute);

            Restart();
        }

        private void Restart()
        {
            var randos = Randomize(NUMBERS);

            foreach (var no in randos)
                _calculations.Add(new MultiplicationCalculation(FACTOR, no));

            _correctAnswersCount = 0;
            _alreadyAnswered = false;
            _currentCalculationIndex = 0;

            NextCmdExecute();
        }

        private void AnswerCmdExecute(decimal answer)
        {
            AnswerIsRight = answer == CurrentCalculation.CalculatedResult();
            if (!_alreadyAnswered && AnswerIsRight)
                _correctAnswersCount++;
            else
                _alreadyAnswered = true;

        }

        private bool NextCmdCanExecute()
        {
            return true;
        }

        private void NextCmdExecute()
        {
            if (_currentCalculationIndex < _calculations.Count - 1)
            {
                CurrentCalculation = _calculations[++_currentCalculationIndex];
                CreateAnswers(CurrentCalculation.CalculatedResult());
                AnswerIsRight = false;
                _alreadyAnswered = false;
            }
            else
            {
                MessageBox.Show(String.Format("Sait {0} vastausta oikein!", _correctAnswersCount), "Testi päättyi", MessageBoxButton.OK);
                Restart();
            }
        }

        private void CreateAnswers(decimal rightAnswer)
        {
            List<decimal> answers = new List<decimal>();
            answers.Add(rightAnswer);

            // Add two wrong answers
            do
            {
                var rando = GetRandomAnswer();
                if ( rando != rightAnswer )
                    answers.Add(rando);
            }
            while (answers.Count < 3);
            
            answers = Randomize(answers);

            Answer1 = answers[0];
            Answer2 = answers[1];
            Answer3 = answers[2];

        }

        private decimal GetRandomAnswer()
        {
            return _rand.Next((int)Math.Round(FACTOR * 10, 0));
        }

        private Calculation _currentCalculation;

        public Calculation CurrentCalculation
        {
            get { return _currentCalculation; }
            set { Set("CurrentCalculation", ref _currentCalculation, value); }
        }
        public RelayCommand NextCmd { get; }
        public RelayCommand<decimal> AnswerCmd { get; }

        public static List<T> Randomize<T>(List<T> list)
        {
            List<T> randomizedList = new List<T>();
            Random rnd = new Random();
            while (list.Count > 0)
            {
                int index = rnd.Next(0, list.Count); //pick a random item from the master list
                randomizedList.Add(list[index]); //place it at the end of the randomized list
                list.RemoveAt(index);
            }
            return randomizedList;
        }

        private decimal _answer1 = 0.0M;
        public decimal Answer1
        {
            get { return _answer1; }
            set { Set("Answer1", ref _answer1, value); }
        }

        private decimal _answer2 = 0.0M;
        public decimal Answer2
        {
            get { return _answer2; }
            set { Set("Answer2", ref _answer2, value); }
        }

        private decimal _answer3 = 0.0M;
        private bool _answerIsRight = false;

        public decimal Answer3
        {
            get { return _answer3; }
            set { Set("Answer3", ref _answer3, value); }
        }

    }
}

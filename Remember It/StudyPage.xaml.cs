using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Animation;
using Remember_It.ViewModels;

namespace Remember_It
{
    public partial class StudyPage : PhoneApplicationPage
    {
        private ObservableCollection<Tables.CardItem> _cardItems;
        public ObservableCollection<Tables.CardItem> CardItems
        {
            get
            {
                return _cardItems;
            }
            set
            {
                if (_cardItems != value)
                {
                    _cardItems = value;
                    NotifyPropertyChanged("CardItems");
                }
            }
        }
        private ObservableCollection<Tables.CardItem> _cardItems1;
        public ObservableCollection<Tables.CardItem> CardItems1
        {
            get
            {
                return _cardItems1;
            }
            set
            {
                if (_cardItems1 != value)
                {
                    _cardItems1 = value;
                    NotifyPropertyChanged("CardItems1");
                }
            }
        }
        private ObservableCollection<Tables.CardItem> _cardItems2;
        public ObservableCollection<Tables.CardItem> CardItems2
        {
            get
            {
                return _cardItems2;
            }
            set
            {
                if (_cardItems2 != value)
                {
                    _cardItems2 = value;
                    NotifyPropertyChanged("CardItems2");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the app that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public StudyPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int DeckIndex = Convert.ToInt32(NavigationContext.QueryString["DeckIndex"].ToString());
            CardItems = new ObservableCollection<Tables.CardItem>(App.ViewModel.DeckItems[DeckIndex].CardItems);
            CardItems1 = new ObservableCollection<Tables.CardItem>();
            CardItems2 = new ObservableCollection<Tables.CardItem>();
            for (int i = 0; i < CardItems.Count; i++ )
            {
                if ((i+1)%2 == 0)
                {
                    CardItems2.Add(CardItems[i]);
                }
                else
                {
                    CardItems1.Add(CardItems[i]);
                }
            }
            cardsList1.ItemsSource = CardItems1;
            cardsList2.ItemsSource = CardItems2;
        }

        private void cardBack_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var temp = sender as Grid;
            Grid all = (Grid)temp.Parent;
            Storyboard sb = all.Resources["flipTo"] as Storyboard;
            sb.Begin();
        }

        private void cardFront_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var temp = sender as Grid;
            Grid all = (Grid)temp.Parent;
            Storyboard sb = all.Resources["flipFrom"] as Storyboard;
            sb.Begin();
        }

        private void cardsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cardsList1.SelectedIndex = -1;
            cardsList2.SelectedIndex = -1;
        }
    }
}
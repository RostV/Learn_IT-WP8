using Remember_It.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Remember_It.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Tables.CardItem _curCard;
        public Tables.CardItem curCard
        {
            get
            {
                return _curCard;
            }
            set
            {
                if (_curCard != value)
                {
                    _curCard = value;
                    NotifyPropertyChanged("curCard");
                }
            }
        }

        private Tables.DeckItem _studyDeck;
        public Tables.DeckItem studyDeck
        {
            get
            {
                return _studyDeck;
            }
            set
            {
                if (_studyDeck != value)
                {
                    _studyDeck = value;
                    NotifyPropertyChanged("studyDeck");
                }
            }
        }

        private Remember_It.ViewModels.Tables.DecksAndCardsDataContext RemItDB;
        public MainViewModel()
        {
            RemItDB = new Remember_It.ViewModels.Tables.DecksAndCardsDataContext(Remember_It.ViewModels.Tables.DecksAndCardsDataContext.DBConnectionString);
            this.DeckItems = new ObservableCollection<Tables.DeckItem>();
            this.CardItems = new ObservableCollection<Tables.CardItem>();
            
        }

        private ObservableCollection<Tables.DeckItem> _deckItems;
        public ObservableCollection<Tables.DeckItem> DeckItems
        {
            get
            {
                return _deckItems;
            }
            set
            {
                if (_deckItems != value)
                {
                    _deckItems = value;
                    NotifyPropertyChanged("DeckItems");
                }
            }
        }
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

        private string _sampleProperty = "Sample Runtime Property Value";
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            // Пример данных; замените реальными данными
            var DecksInDB = from Tables.DeckItem deck in RemItDB.DeckItems
                            select deck;
            var CardsInDB = from Tables.CardItem card in RemItDB.CardItems
                            select card;

            DeckItems = new ObservableCollection<Tables.DeckItem>(DecksInDB);
            CardItems = new ObservableCollection<Tables.CardItem>(CardsInDB);
            this.IsDataLoaded = true;
        }

        public void SaveData()
        {
            RemItDB.SubmitChanges();
        }

        public void CardsForDeck(Tables.DeckItem workDeck)
        {
            var CardsForDeckInDB = from Tables.CardItem card in RemItDB.CardItems
                                   where card.Deck == workDeck
                                   select card;
            CardItems = new ObservableCollection<Tables.CardItem>(CardsForDeckInDB);
        }

        public List<int> fillQueue()
        {
            List<int> queue = new List<int>(App.ViewModel.studyDeck.CardItems.Count);
            for (int i = 0; i < App.ViewModel.studyDeck.CardItems.Count; i++)
            {
                queue.Add(i);
            }
            return queue;
        }

        public void createDeck(Tables.DeckItem newDeck)
        {
            RemItDB.DeckItems.InsertOnSubmit(newDeck);
            RemItDB.SubmitChanges();
            DeckItems.Add(newDeck);
        }

        public void createCard(Tables.CardItem newCard)
        {
            RemItDB.CardItems.InsertOnSubmit(newCard);
            RemItDB.SubmitChanges();
            CardItems.Add(newCard);  
        }

        public void deleteCard(Tables.CardItem cardForDelete)
        {
            CardItems.Remove(cardForDelete);
            cardForDelete.Deck.cardsCount -= 1;
            RemItDB.CardItems.DeleteOnSubmit(cardForDelete);
            RemItDB.SubmitChanges();
        }

        public void deleteDeck(Tables.DeckItem deckForDelete)
        {
            var CardsForDelete =
                from Tables.CardItem card in RemItDB.CardItems
                where card.Deck == deckForDelete
                select card;
            RemItDB.CardItems.DeleteAllOnSubmit(CardsForDelete);
            RemItDB.DeckItems.DeleteOnSubmit(deckForDelete);
            DeckItems.Remove(deckForDelete);
            foreach (Tables.CardItem card in CardsForDelete)
                CardItems.Remove(card);
            RemItDB.SubmitChanges();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

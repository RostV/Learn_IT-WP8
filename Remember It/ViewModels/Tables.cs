using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Windows;

namespace Remember_It.ViewModels
{
    public class Tables
    {
        [Table]
        public class DeckItem : INotifyPropertyChanged, INotifyPropertyChanging
        {
            private int _Id;
            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int Id
            {
                get
                {
                    return _Id;
                }
                set
                {
                    if (_Id != value)
                    {
                        NotifyPropertyChanging("Id");
                        _Id = value;
                        NotifyPropertyChanged("Id");
                    }
                }
            }

            private string _DeckName;
            [Column]
            public string DeckName
            {
                get
                {
                    return _DeckName;
                }
                set
                {
                    if (_DeckName != value)
                    {
                        NotifyPropertyChanging("DeckName");
                        _DeckName = value;
                        NotifyPropertyChanged("DeckName");
                    }
                }
            }

            private string _DeckDescription;
            [Column]
            public string DeckDescription
            {
                get
                {
                    return _DeckDescription;
                }
                set
                {
                    if (_DeckDescription != value)
                    {
                        NotifyPropertyChanging("DeckDescription");
                        _DeckDescription = value;
                        NotifyPropertyChanged("DeckDescription");
                    }
                }
            }

            private int _cardsCount;
            [Column]
            public int cardsCount
            {
                get
                {
                    return _cardsCount;
                }
                set
                {
                    if (_cardsCount != value)
                    {
                        NotifyPropertyChanging("cardsCount");
                        _cardsCount = value;
                        NotifyPropertyChanged("cardsCount");
                    }
                }
            }

            private EntitySet<CardItem> _cardItems;
            [Association(Storage = "_cardItems", OtherKey = "_deckId", ThisKey = "Id")]
            public EntitySet<CardItem> CardItems
            {
                get
                {
                    return this._cardItems;
                }
                set
                {
                    this._cardItems.Assign(value);
                }
            }

            // Version column aids update performance.
            [Column(IsVersion = true)]
            private Binary _version;

            public DeckItem()
            {
                this._cardItems = new EntitySet<CardItem>(
                  delegate(CardItem entity)
                  {
                      this.NotifyPropertyChanging("CardItems");
                      entity.Deck = this;
                  },
                  delegate(CardItem entity)
                  {
                      this.NotifyPropertyChanging("CardItems");
                      entity.Deck = null;
                  });
            }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            // Used to notify the page that a data context property changed
            private void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion

            #region INotifyPropertyChanging Members

            public event PropertyChangingEventHandler PropertyChanging;

            // Used to notify the data context that a data context property is about to change
            private void NotifyPropertyChanging(string propertyName)
            {
                if (PropertyChanging != null)
                {
                    PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
                }
            }

            #endregion
        }

        [Table]
        public class CardItem : INotifyPropertyChanged, INotifyPropertyChanging
        {

            private int _cardId;
            [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
            public int CardId
            {
                get
                {
                    return _cardId;
                }
                set
                {
                    if (_cardId != value)
                    {
                        NotifyPropertyChanging("CardId");
                        _cardId = value;
                        NotifyPropertyChanged("CardId");
                    }
                }
            }

            private string _CardFrontSide;
            [Column]
            public string CardFrontSide
            {
                get
                {
                    return _CardFrontSide;
                }
                set
                {
                    if (_CardFrontSide != value)
                    {
                        NotifyPropertyChanging("CardFrontSide");
                        _CardFrontSide = value;
                        NotifyPropertyChanged("CardFrontSide");
                    }
                }
            }

            private string _CardBackSide;
            [Column]
            public string CardBackSide
            {
                get
                {
                    return _CardBackSide;
                }
                set
                {
                    if (_CardBackSide != value)
                    {
                        NotifyPropertyChanging("CardBackSide");
                        _CardBackSide = value;
                        NotifyPropertyChanged("CardBackSide");
                    }
                }
            }

            private Visibility _visualFrontSide;
            [Column]
            public Visibility visualFrontSide
            {
                get
                {
                    return _visualFrontSide;
                }
                set
                {
                    if (_visualFrontSide != value)
                    {
                        NotifyPropertyChanging("visualFrontSide");
                        _visualFrontSide = value;
                        NotifyPropertyChanged("visualFrontSide");
                    }
                }
            }

            private Visibility _visualBackSide;
            [Column]
            public Visibility visualBackSide
            {
                get
                {
                    return _visualBackSide;
                }
                set
                {
                    if (_visualBackSide != value)
                    {
                        NotifyPropertyChanging("visualBackSide");
                        _visualBackSide = value;
                        NotifyPropertyChanged("visualBackSide");
                    }
                }
            }

            [Column]
            private int _deckId;

            private EntityRef<DeckItem> _deck;
            [Association(Storage = "_deck", ThisKey = "_deckId", OtherKey = "Id", IsForeignKey = true)]
            public DeckItem Deck
            {
                get
                {
                    return _deck.Entity;
                }
                set
                {
                    NotifyPropertyChanging("Deck");
                    _deck.Entity = value;
                    if (value != null)
                    {
                        _deckId = value.Id;
                    }
                    NotifyPropertyChanging("Deck");
                }
            }

            // Version column aids update performance.
            [Column(IsVersion = true)]
            private Binary _version;

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            // Used to notify the page that a data context property changed
            private void NotifyPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion INotifyPropertyChanged Members

            #region INotifyPropertyChanging Members

            public event PropertyChangingEventHandler PropertyChanging;

            // Used to notify the data context that a data context property is about to change
            private void NotifyPropertyChanging(string propertyName)
            {
                if (PropertyChanging != null)
                {
                    PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
                }
            }

            #endregion INotifyPropertyChanging Members
        }

        public class DecksAndCardsDataContext : DataContext
        {
            // Specify the connection string as a static, used in main page and app.xaml.
            public static string DBConnectionString = "Data Source=isostore:/DecksAndCards.sdf";

            // Pass the connection string to the base class.
            public DecksAndCardsDataContext(string connectionString)
                : base(connectionString)
            { }

            // Specify a single table for the to-do items.
            public Table<DeckItem> DeckItems;
            public Table<CardItem> CardItems;
        }
        private DecksAndCardsDataContext RemItDB = new DecksAndCardsDataContext(DecksAndCardsDataContext.DBConnectionString);

    }
}
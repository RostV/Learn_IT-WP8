using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Remember_It.ViewModels;
using System.Windows.Markup;
using Remember_It.Resources;
using System.Windows.Data;
using System.Collections.Generic;

namespace Remember_It
{
    public partial class MainPage : PhoneApplicationPage
    {
        int answerCount = 0;
        Random rand = new Random();
        public List<int> queue;
        int cardIndex;
        bool firsttime = true, endTest = true;
        Tables.DeckItem workDeck;      
        // Конструктор
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;
        }
    
        // Загрузка данных для элементов ViewModel
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            if (App.ViewModel.DeckItems.Count > 0)
            {
                if (resultBorder.Visibility == Visibility.Collapsed)
                {
                    studingField.Visibility = Visibility.Visible;
                    ifNoDeckField.Visibility = Visibility.Collapsed;
                }         
                workDeck = App.ViewModel.DeckItems[0];  //потом перенести в viewmodel и сделать рандомно
                deckShow.Text = workDeck.DeckName;
                App.ViewModel.studyDeck = workDeck;
                queue = App.ViewModel.fillQueue();
                if (App.ViewModel.studyDeck.CardItems.Count > 0)
                {
                    radBtnFrontSide.IsChecked = true;
                    radBtnRandom.IsChecked = true;
                }
            }
            else
            {
                ifNoDeckField.Visibility = Visibility.Visible;
                studingField.Visibility = Visibility.Collapsed;
                resultBorder.Visibility = Visibility.Collapsed;
                ApplicationBar.IsVisible = false;
            }
            
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Call the base method.
            base.OnNavigatedFrom(e);

            // Save changes to the database.
            App.ViewModel.SaveData();
        }

        public void createpanelSwitch()
        {
            if (createpanel.Visibility == Visibility.Visible)
            {
                deckDescriptionTB.Text = AppResources.TBDeckDescription;
                deckNameTB.Text = AppResources.TBDeckName;
                MainPivot.IsEnabled = true;
                ApplicationBar.IsVisible = true;
                createpanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainPivot.IsEnabled = false;
                ApplicationBar.IsVisible = false;
                createpanel.Visibility = Visibility.Visible;
            }
        }

        private void NewDeck_Click(object sender, EventArgs e)
        {    

            createpanelSwitch();
            if (firsttime)
            {
                ImageBrush imTb = new ImageBrush();
                imTb.ImageSource = new BitmapImage(new Uri(@"Assets\tbUnFocus.png", UriKind.Relative));
                deckNameTB.Background = imTb;
                imTb.ImageSource = new BitmapImage(new Uri(@"Assets\tbUnFocus2.png", UriKind.Relative));
                deckDescriptionTB.Background = imTb;
                ImageBrush imBtn = new ImageBrush();
                imBtn.ImageSource = new BitmapImage(new Uri(@"Assets\btn.png", UriKind.Relative));
                createBtn.Background = imBtn;
                imBtn.ImageSource = new BitmapImage(new Uri(@"Assets\btn2.png", UriKind.Relative));
                cancelBtn.Background = imBtn;
                firsttime = false;
            }
        }

        #region GotLostFocusTB

        private void deckDescriptionTB_GotFocus(object sender, RoutedEventArgs e)
        {
            deckDescriptionTB.Text = "";
            ImageBrush imTb = new ImageBrush();
            imTb.ImageSource = new BitmapImage(new Uri(@"Assets\tbFocus2.png", UriKind.Relative));
            (sender as TextBox).Background = imTb;
        }

        private void deckNameTB_GotFocus(object sender, RoutedEventArgs e)
        {
            deckNameTB.Text = "";
            ImageBrush imTb = new ImageBrush();
            imTb.ImageSource = new BitmapImage(new Uri(@"Assets\tbFocus.png", UriKind.Relative));
            (sender as TextBox).Background = imTb;
        }

        private void deckNameTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (deckNameTB.Text == "")
                deckNameTB.Text = AppResources.TBDeckName;
            ImageBrush imTb = new ImageBrush();
            imTb.ImageSource = new BitmapImage(new Uri(@"Assets\tbUnFocus.png", UriKind.Relative));
            (sender as TextBox).Background = imTb;
        }

        private void deckDescriptionTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (deckDescriptionTB.Text == "")
                deckDescriptionTB.Text = AppResources.TBDeckDescription;
            ImageBrush imTb = new ImageBrush();
            imTb.ImageSource = new BitmapImage(new Uri(@"Assets\tbUnFocus2.png", UriKind.Relative));
            (sender as TextBox).Background = imTb;
        }

        #endregion GotLostFocusTB

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationBarIconButton btn = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    {
                        if (endTest)
                            if (App.ViewModel.DeckItems.Count > 0)
                                if (App.ViewModel.studyDeck == null)
                                {
                                    MessageBoxResult result =
                                    MessageBox.Show(AppResources.MessageBoxNostudyDeck);

                                    if (result == MessageBoxResult.OK)
                                    {
                                        MainPivot.SelectedIndex = 1;
                                    }
                                }
                                else
                                    if (App.ViewModel.studyDeck.CardItems.Count == 0)
                                    {
                                        MessageBoxResult result =
                                            MessageBox.Show(AppResources.MessageBox_Text,
                                            AppResources.MessageBox_caption, MessageBoxButton.OKCancel);

                                        if (result == MessageBoxResult.OK)
                                        {
                                            workDeck = App.ViewModel.studyDeck;
                                            MainPivot.SelectedIndex = 2;
                                            deckShow.Text = App.ViewModel.studyDeck.DeckName;
                                        }
                                    }
                                    else
                                    {
                                        answerCount = 0;
                                        cardIndex = rand.Next(App.ViewModel.studyDeck.CardItems.Count - 1);
                                        App.ViewModel.curCard = App.ViewModel.studyDeck.CardItems[cardIndex];
                                        AnswerPB.Maximum = App.ViewModel.studyDeck.CardItems.Count;
                                        AnswerPB.Value = 0;
                                        countAnsCards.Text = "0/" + App.ViewModel.studyDeck.CardItems.Count.ToString();
                                    }
                            btn.IsEnabled = endTest;
                            btn.Click -= AddNewCard_Click;
                            btn.Click -= NewDeck_Click;
                            btn.Click += setTest_Click;
                            btn.IconUri = new Uri(@"Assets\AppBarButtons\feature.settings.png", UriKind.Relative);
                            btn.Text = AppResources.AppBarButtonSettings;
                            ApplicationBar.Mode = ApplicationBarMode.Minimized;
                            if (App.ViewModel.DeckItems.Count == 0)
                            {
                                if (resultBorder.Visibility == Visibility.Collapsed)
                                {
                                    ifNoDeckField.Visibility = Visibility.Visible;
                                    studingField.Visibility = Visibility.Collapsed;
                                }
                            
                            }
                            else
                            {
                                if (resultBorder.Visibility == Visibility.Collapsed)
                                {
                                    ifNoDeckField.Visibility = Visibility.Collapsed;
                                    studingField.Visibility = Visibility.Visible;
                                }
                            }
                            if (ifNoDeckField.Visibility == Visibility.Visible)
                            {
                                btn.IsEnabled = false;
                            }
                        }                  
                    break;
                case 1:
                    {
                        btn.IsEnabled = true;
                        ApplicationBar.Mode = ApplicationBarMode.Default;
                        btn.Click -= AddNewCard_Click;
                        btn.Click -= setTest_Click;
                        btn.Click += NewDeck_Click;
                        btn.IconUri = new Uri(@"Assets\AppBarButtons\add.png", UriKind.Relative);
                        btn.Text = AppResources.AppBarButtonDeck;
                    }                   
                    break;
                case 2:
                    {
                        btn.IsEnabled = true;
                        ApplicationBar.Mode = ApplicationBarMode.Default;
                        if (workDeck != null)
                        {                           
                            App.ViewModel.CardsForDeck(workDeck);
                            ApplicationBar.IsVisible = true;
                            ApplicationBar.Mode = ApplicationBarMode.Default;
                            btn.Click -= NewDeck_Click;
                            btn.Click -= setTest_Click;
                            btn.Click += AddNewCard_Click;
                            btn.IconUri = new Uri(@"Assets\AppBarButtons\add.png", UriKind.Relative);
                            btn.Text = AppResources.AppBarButtonCard;
                        }
                    }                
                    break;
                default: break;
            }         
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            Tables.DeckItem newDeck = new Tables.DeckItem { DeckDescription = deckDescriptionTB.Text, DeckName = deckNameTB.Text };
            App.ViewModel.createDeck(newDeck);
            createpanelSwitch();
            this.Focus();
            MessageBoxResult result =
                MessageBox.Show(AppResources.MessageBox_Text,
                AppResources.MessageBox_caption, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                workDeck = newDeck;
                MainPivot.SelectedIndex = 2;
                deckShow.Text = newDeck.DeckName;
            }
            
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (App.ViewModel.DeckItems.Count == 0)
            {
                ifNoDeckField.Visibility = Visibility.Collapsed;
                studingField.Visibility = Visibility.Visible;
            }
            createpanelSwitch();
            this.Focus();
        }

        private void FlipCard_Click(object sender, RoutedEventArgs e)
        {
            var temp = sender as Button;
            StackPanel stp = (StackPanel)temp.Parent;
            StackPanel stp1 = (StackPanel)stp.Parent;
            Grid all = (Grid)stp1.Children[0];
            Grid cardBack = (Grid)all.Children[0];
            Grid cardFront = (Grid)all.Children[1];
            if (cardBack.Visibility == Visibility.Visible)
            {
                Storyboard sb = all.Resources["flipTo"] as Storyboard;
                sb.Begin();
            }
            else
            {
                Storyboard sb = all.Resources["flipFrom"] as Storyboard;
                sb.Begin();
            }
            
        }

        private void AddNewCard_Click(object sender, EventArgs e)
        {
            Tables.CardItem newCard = new Tables.CardItem { Deck = workDeck, visualBackSide = Visibility.Visible, visualFrontSide = Visibility.Collapsed };
            App.ViewModel.createCard(newCard);          
            newCard.Deck.CardItems.Add(newCard);
            newCard.Deck.cardsCount = newCard.Deck.CardItems.Count;
            cardsList.ScrollIntoView(newCard);
            this.Focus();
        }

        private void DeleteCard_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Tables.CardItem cardItemforDelete = button.DataContext as Tables.CardItem;
            App.ViewModel.deleteCard(cardItemforDelete);
            this.Focus();
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            //var CardsForDeckInDB = from Tables.CardItem card in RemItDB.CardItems
            //                       where card.Deck == workDeck
            //                       select card;
            //CardItems = new ObservableCollection<Tables.CardItem>(CardsForDeckInDB);
        }

        private void deckList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tables.DeckItem Deck = (Tables.DeckItem)deckList.SelectedItem;
            deckShow.Text = Deck.DeckName;
            workDeck = Deck;
            MainPivot.SelectedIndex = 2;

            this.Focus();
        }

        private void cardsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Tables.CardItem a = (Tables.CardItem)cardsList.SelectedItem;
            //int b = a.Deck.Id;
            //MessageBox.Show(b.ToString());
        }

        #region gotlostFocusCardSide
        private void GFcardSide(object sender, RoutedEventArgs e)
        {
            ApplicationBar.IsVisible = false;
        }

        private void LFcardSide(object sender, RoutedEventArgs e)
        {
            ApplicationBar.IsVisible = true;
        }
        #endregion

        private void deleteDeck_Click(object sender, RoutedEventArgs e)
        {
            if (deckList.ItemContainerGenerator == null) return;
            var selectedListBoxItem = deckList.ItemContainerGenerator.ContainerFromItem(((MenuItem)sender).DataContext) as ListBoxItem;
            if (selectedListBoxItem == null) return;
            var selectedIndex = deckList.ItemContainerGenerator.IndexFromContainer(selectedListBoxItem);
            Tables.DeckItem deckForDelete = selectedListBoxItem.Content as Tables.DeckItem;
            App.ViewModel.deleteDeck(deckForDelete);

            if (selectedIndex + 1 <= App.ViewModel.DeckItems.Count)
            {
                workDeck = App.ViewModel.DeckItems[selectedIndex];
                deckShow.Text = workDeck.DeckName;
            }
            else
            {
                if (App.ViewModel.DeckItems.Count > 0)
                {
                    workDeck = App.ViewModel.DeckItems[selectedIndex - 1];
                    deckShow.Text = workDeck.DeckName;
                }
                else
                {
                    workDeck = null;
                    deckShow.Text = "";
                    App.ViewModel.studyDeck = null;
                }

            }
            
        }

        private void goTest_Click(object sender, RoutedEventArgs e)
        {
            endTest = true;
            if (deckList.ItemContainerGenerator == null) return;
            var selectedListBoxItem = deckList.ItemContainerGenerator.ContainerFromItem(((MenuItem)sender).DataContext) as ListBoxItem;
            App.ViewModel.studyDeck = selectedListBoxItem.Content as Tables.DeckItem;
            queue = App.ViewModel.fillQueue();
            if (App.ViewModel.studyDeck.CardItems.Count > 0)
            {

                MainPivot.SelectedIndex = 0;

            }
            else
            {
                MessageBoxResult result =
                MessageBox.Show(AppResources.MessageBox_Text,
                AppResources.MessageBox_caption, MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    workDeck = App.ViewModel.studyDeck;
                    MainPivot.SelectedIndex = 2;
                    deckShow.Text = App.ViewModel.studyDeck.DeckName;
                }
            }
            
        }

        private void ifNoDeckFieldBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPivot.SelectedIndex = 1;
            createpanelSwitch();
        }

        private void AnswerBtn_Click(object sender, RoutedEventArgs e)   //answer click on 1tab
        {
            if (App.ViewModel.studyDeck.CardItems.Count <= 0)
            {
                MessageBoxResult result =
                MessageBox.Show(AppResources.MessageBox_Text,
                AppResources.MessageBox_caption, MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    workDeck = App.ViewModel.studyDeck;
                    MainPivot.SelectedIndex = 2;
                    deckShow.Text = App.ViewModel.studyDeck.DeckName;
                }
            }
            else
            {
                answerCount++;
                endTest = false;
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
                countAnsCards.Text = answerCount + "/" + App.ViewModel.studyDeck.CardItems.Count.ToString();
                if (QuestionTB.GetBindingExpression(TextBlock.TextProperty).ParentBinding.Path.Path == "curCard.CardFrontSide")
                {
                    if (App.ViewModel.curCard.CardBackSide == AnswerTB.Text)
                    {
                        AnswerPB.Value += 1;
                        appearanceFalse.Stop();
                        appearanceTrue.Begin();
                    }
                    else
                    {
                        appearanceTrue.Stop();
                        appearanceFalse.Begin();
                    }
                }
                else
                {
                    if (App.ViewModel.curCard.CardFrontSide == AnswerTB.Text)
                    {
                        AnswerPB.Value += 1;
                        appearanceFalse.Stop();
                        appearanceTrue.Begin();
                    }
                    else
                    {
                        appearanceTrue.Stop();
                        appearanceFalse.Begin();
                    }
                }
                AnswerTB.Text = "";
                if (answerCount < App.ViewModel.studyDeck.CardItems.Count)
                {
                    if ((bool)radBtnRandom.IsChecked)
                    {
                        queue.RemoveAt(cardIndex);
                        cardIndex = rand.Next(App.ViewModel.studyDeck.CardItems.Count - answerCount - 1);
                        App.ViewModel.curCard = App.ViewModel.studyDeck.CardItems[queue[cardIndex]];

                    }
                        
                }
                    
                else
                {
                    studingField.Visibility = Visibility.Collapsed;
                    resultBorder.Visibility = Visibility.Visible;   
                    resultBorder_answTrue.Text = AnswerPB.Value.ToString() + "/" + App.ViewModel.studyDeck.CardItems.Count.ToString();
                    resultBorder_resultPercent.Text = ((int)((AnswerPB.Value / App.ViewModel.studyDeck.CardItems.Count) * 100)).ToString() + "%";
                }
            }

            }

        private void goStudy_Click(object sender, RoutedEventArgs e)
        {
            if (deckList.ItemContainerGenerator == null) return;
            var selectedListBoxItem = deckList.ItemContainerGenerator.ContainerFromItem(((MenuItem)sender).DataContext) as ListBoxItem;
            if (selectedListBoxItem == null) return;
            var selectedIndex = deckList.ItemContainerGenerator.IndexFromContainer(selectedListBoxItem);
            NavigationService.Navigate(new Uri(string.Format("/StudyPage.xaml?DeckIndex={0}",selectedIndex), UriKind.Relative));
        }

        private void ansTbBorder_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            AnswerTB.Focus();
        }

        private void setTest_Click(object sender, EventArgs e)
        {
            studingField.Visibility = Visibility.Collapsed;
            settingsField.Visibility = Visibility.Visible;
        }

        private void Random_checked(object sender, RoutedEventArgs e)
        {
        }

        private void frontSide_checked(object sender, RoutedEventArgs e)
        {
            Binding myBinding = new Binding();
            myBinding.Source = App.ViewModel;
            myBinding.Path = new PropertyPath("curCard.CardFrontSide");
            myBinding.Mode = BindingMode.OneWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.Default;
            QuestionTB.SetBinding(TextBlock.TextProperty, myBinding);
            qBack.Visibility = Visibility.Visible;
            qFront.Visibility = Visibility.Collapsed;
        }

        private void backSide_checked(object sender, RoutedEventArgs e)
        {
            Binding myBinding = new Binding();
            myBinding.Source = App.ViewModel;
            myBinding.Path = new PropertyPath("curCard.CardBackSide");
            myBinding.Mode = BindingMode.OneWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.Default;
            QuestionTB.SetBinding(TextBlock.TextProperty, myBinding);
            qBack.Visibility = Visibility.Collapsed;
            qFront.Visibility = Visibility.Visible;
        }

        private void SettingsOk_Click(object sender, RoutedEventArgs e)
        {
            settingsField.Visibility = System.Windows.Visibility.Collapsed;
            studingField.Visibility = System.Windows.Visibility.Visible;
        }

        private void resultBorder_OKBtn_Click(object sender, RoutedEventArgs e)
        {
            endTest = true;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = endTest;
            studingField.Visibility = System.Windows.Visibility.Visible;
            resultBorder.Visibility = System.Windows.Visibility.Collapsed;
            answerCount = 0;
            cardIndex = rand.Next(App.ViewModel.studyDeck.CardItems.Count - 1);
            App.ViewModel.curCard = App.ViewModel.studyDeck.CardItems[cardIndex];
            MainPivot.SelectedIndex = 0;
            AnswerPB.Maximum = App.ViewModel.studyDeck.CardItems.Count;
            AnswerPB.Value = 0;
            countAnsCards.Text = "0/" + App.ViewModel.studyDeck.CardItems.Count.ToString();
            queue = App.ViewModel.fillQueue();
        }

    }
}
using MobileJO.Core.Base;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MobileJO.Core.ViewModels.ResponseEditViewModels;
using Plugin.InputKit.Shared.Controls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileJO.Core.Views.ResponseEditPages
{
    public partial class ResponseViewEditPage : BaseContentPage
    {
        public ResponseViewEditPage()
        {
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = (ResponseViewEditViewModel)DataContext;
            if (vm == null) return;

            Entry entry = (Entry)sender;
            if (entry == null) return;

            StackLayout stackLayout = (StackLayout)entry.Parent;
            if (stackLayout == null) return;

            QuestionModel questionDetails = (QuestionModel)stackLayout.BindingContext;
            if (questionDetails == null) return;

            ObservableCollection<QuestionModel> questionList = vm.Questions;
            if (questionList == null) return;

            foreach (QuestionModel question in questionList)
            {
                if (question.QuestionID == questionDetails.QuestionID)
                {
                    // update answer for question
                    AnswerModel answer = new AnswerModel()
                    {
                        AnswerID = question.Answer == null ? 0 : question.Answer.AnswerID,
                        Value = entry.Text
                    };

                    question.Answer = answer;
                    break;
                }
            }

            vm.Questions = questionList;
        }

        private void CheckBox_CheckChanged(object sender, EventArgs e)
        {
            var vm = (ResponseViewEditViewModel)DataContext;
            if (vm == null) return;

            Plugin.InputKit.Shared.Controls.CheckBox checkbox = (Plugin.InputKit.Shared.Controls.CheckBox)sender;
            ChoiceModel choiceDetails = (ChoiceModel) checkbox.Parent.BindingContext;
            ListView listView = (ListView)checkbox.Parent.Parent.Parent;
            QuestionModel questionDetails = (QuestionModel)listView.BindingContext;
            ObservableCollection<QuestionModel> questionList = vm.Questions;

            foreach (QuestionModel question in questionList)
            {
                if (question.QuestionID != questionDetails.QuestionID)
                {
                    continue;
                }

                List<ChoiceModel> choiceList = question.Choices;

                foreach (ChoiceModel choice in choiceList)
                {
                    if (choice.ChoiceID != choiceDetails.ChoiceID)
                    {
                        continue;
                    }

                    choice.IsSelected = checkbox.IsChecked;
                    break;
                }

                break;
            }
        }

        private void SelectionView_PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = (ResponseViewEditViewModel)DataContext;
            if (vm == null) return;

            SelectionView selectionView = (SelectionView)sender;
            if (selectionView == null) return;

            StackLayout stackLayout = (StackLayout)selectionView.Parent;
            if (stackLayout == null) return;

            QuestionModel questionDetails = (QuestionModel)stackLayout.BindingContext;
            if (questionDetails == null) return;

            ObservableCollection<QuestionModel> questionList = vm.Questions;
            if (questionList == null) return;

            foreach (QuestionModel question in questionList)
            {
                if (question.QuestionID != questionDetails.QuestionID)
                {
                    continue;
                }
 
                List<ChoiceModel> selectedItems = new List<ChoiceModel>();
                // update selected items for question
                if (selectionView.SelectedItem != null)
                {
                    ChoiceModel selectedItem = (ChoiceModel)selectionView.SelectedItem;

                    foreach (ChoiceModel choice in question.Choices)
                    {
                        if (choice.ChoiceID != selectedItem.ChoiceID)
                        {
                            choice.IsSelected = false;
                            continue;
                        }

                        choice.IsSelected = true;
                        question.SelectedItem.ChoiceID = selectedItem.ChoiceID;
                        question.SelectedItem.Label = selectedItem.Label;
                        question.SelectedItem.Value = selectedItem.Value;
                    }
                        
                }

                break;
            }

            vm.Questions = questionList;
        }

        private void Slider_ValueChangedEventHandler(object sender, ValueChangedEventArgs e)
        {
            var vm = (ResponseViewEditViewModel)DataContext;
            if (vm == null) return;

            Slider slider = (Slider)sender;
            if (slider == null) return;

            StackLayout stackLayout = (StackLayout)slider.Parent;
            if (stackLayout == null) return;

            QuestionModel questionDetails = (QuestionModel)stackLayout.BindingContext;
            if (questionDetails == null) return;

            ObservableCollection<QuestionModel> questionList = vm.Questions;
            if (questionList == null) return;

            foreach (QuestionModel question in questionList)
            {
                if (question.QuestionID != questionDetails.QuestionID)
                {
                    continue;
                }

                ChoiceModel choice = vm.GetSliderProps(questionDetails.Choices);
                slider.Maximum = choice.Maximum;
                slider.Minimum = choice.Minimum;
                break;
            }

            IList<View> children = stackLayout.Children;
            double stepValue = 1.0;
            foreach (View child in children)
            {
                if (child.GetType() == typeof(Label))
                {
                    var newStep = Math.Round(e.NewValue / stepValue);
                    Label sliderLabel = (Label)child;
                    sliderLabel.Text = (newStep * stepValue).ToString();
                    double sliderWidth = slider.Width + 25;
                    if (sliderWidth <= 25)
                    {
                        var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                        sliderWidth = mainDisplayInfo.Width;
                        sliderWidth = (sliderWidth / mainDisplayInfo.Density) - 20;
                    }
                    sliderLabel.TranslateTo(((newStep * stepValue) - slider.Minimum) * ((sliderWidth - 50) / (slider.Maximum - slider.Minimum)), 0, 100);
                    break;
                }
            }
        }

        private void Slider_DragCompletedEventHandler(object sender, EventArgs e)
        {
            var vm = (ResponseViewEditViewModel)DataContext;
            if (vm == null) return;

            Slider slider = (Slider)sender;
            if (slider == null) return;

            StackLayout stackLayout = (StackLayout)slider.Parent;
            if (stackLayout == null) return;

            QuestionModel questionDetails = (QuestionModel)stackLayout.BindingContext;
            if (questionDetails == null) return;

            ObservableCollection<QuestionModel> questionList = vm.Questions;
            if (questionList == null) return;

            foreach (QuestionModel question in questionList)
            {
                if (question.QuestionID == questionDetails.QuestionID)
                {
                    // update selected items for question
                    if (question.Answer == null)
                        question.Answer = new AnswerModel();

                    IList<View> children = stackLayout.Children;
                    foreach (View child in children)
                    {
                        if (child.GetType() == typeof(Label))
                        {
                            Label sliderLabel = (Label)child;
                            question.Answer.Value = sliderLabel.Text.ToString();
                            break;
                        }
                    }
                    break;
                }
            }

            vm.Questions = questionList;
        }

        private async void TakeAVideoButton_Clicked(object sender, EventArgs e)
        {
            var vm = (ResponseViewEditViewModel)DataContext;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            string mediaName = ConstructMediaName(Constants.Uploads.VideoPrefix, Constants.Uploads.VideoFormat);

            var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
            {
                SaveMetaData = false,
                SaveToAlbum = true,
                Quality = VideoQuality.Medium,
                CompressionQuality = Constants.Uploads.VideoCompressionQuality,
                DesiredSize = Constants.Uploads.VideoMaxSizeInBytes,
                Name = mediaName
            });

            if (file == null) return;

            if (vm == null) return;

            vm.UploadedMediaFile = file;
            ImageButton button = (ImageButton)sender;
            StackLayout stackLayout = (StackLayout)button.Parent;
            QuestionModel questionDetails = (QuestionModel)stackLayout.BindingContext;
            ObservableCollection<QuestionModel> questionList = vm.Questions;

            if (questionDetails == null) return;

            if (questionList == null) return;

            foreach (QuestionModel question in questionList)
            {
                if (question.QuestionID == questionDetails.QuestionID)
                {
                    question.Answer = new AnswerModel()
                    {
                        AnswerID = question.Answer == null ? 0 : question.Answer.AnswerID,
                        Value = file.Path
                    };
                    break;
                }
            }
            vm.Questions = questionList;

            StackLayout parentStackLayout = (StackLayout)stackLayout.Parent;
            View view = parentStackLayout.Children[0];

            StackLayout stack = (StackLayout)view;
            IList<View> stackChildren = stack.Children;

            foreach (View child in stackChildren)
            {
                if (child.GetType() != typeof(Label)) continue;

                Label videoLabel = (Label)child;
                videoLabel.Text = mediaName;
                break;
            }

            vm.UploadMedia.Execute(questionDetails.QuestionID);
        }

        private async void TakeAPhotoButton_Clicked(object sender, EventArgs e)
        {
            var vm = (ResponseViewEditViewModel)DataContext;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            string mediaName = ConstructMediaName(Constants.Uploads.ImagePrefix, Constants.Uploads.ImageFormat);

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                SaveMetaData = false,
                SaveToAlbum = true,
                Name = mediaName
            });

            if (file == null) return;

            if (vm == null) return;

            vm.UploadedMediaFile = file;
            ImageButton button = (ImageButton)sender;
            StackLayout stackLayout = (StackLayout)button.Parent;
            QuestionModel questionDetails = (QuestionModel)stackLayout.BindingContext;
            ObservableCollection<QuestionModel> questionList = vm.Questions;

            if (questionDetails == null) return;

            if (questionList == null) return;

            foreach (QuestionModel question in questionList)
            {
                if (question.QuestionID == questionDetails.QuestionID)
                {
                    question.Answer = new AnswerModel()
                    {
                        AnswerID = question.Answer == null ? 0 : question.Answer.AnswerID,
                        Value = file.Path
                    };

                    break;
                }
            }
            
            vm.Questions = questionList;

            StackLayout parentStackLayout = (StackLayout)stackLayout.Parent;
            View view = parentStackLayout.Children[0];

            StackLayout stack = (StackLayout)view;
            IList<View> stackChildren = stack.Children;

            foreach (View child in stackChildren)
            {
                if (child.GetType() != typeof(Label)) continue;

                Label imageLabel = (Label)child;
                imageLabel.Text = mediaName;
                break;
            }

            vm.UploadMedia.Execute(questionDetails.QuestionID);
        }

        private string ConstructMediaName(string prefix, string format)
        {
            return prefix + DateTime.Now.ToString(Constants.Common.MediaNameDateFormat, CultureInfo.InvariantCulture) + format;
        }
    }
}
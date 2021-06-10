using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileJO.Core.Selectors
{
    public class QuestionDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextQuestionTemplate { get; set; }
        public DataTemplate CheckboxQuestionTemplate { get; set; }
        public DataTemplate RadioQuestionTemplate { get; set; }
        public DataTemplate VideoQuestionTemplate { get; set; }
        public DataTemplate ImageQuestionTemplate { get; set; }
        public DataTemplate SliderQuestionTemplate { get; set; }

        internal enum QuestionType
        {
            Text = Constants.QuestionType.Text,
            Checkbox = Constants.QuestionType.Checkbox,
            Radio = Constants.QuestionType.Radio,
            Video = Constants.QuestionType.Video,
            Image = Constants.QuestionType.Image,
            Slider = Constants.QuestionType.Slider
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            QuestionModel question = (QuestionModel)item;
            DataTemplate template = TextQuestionTemplate;
            string questionType = Enum.GetName(typeof(QuestionType), question.QuestionTypeID);
            switch (questionType)
            {
                case "Text":
                    template = TextQuestionTemplate;
                    break;
                case "Checkbox":
                    template = CheckboxQuestionTemplate;
                    break;
                case "Radio":
                    template = RadioQuestionTemplate;
                    break;
                case "Video":
                    template = VideoQuestionTemplate;
                    break;
                case "Image":
                    template = ImageQuestionTemplate;
                    break;
                case "Slider":
                    template = SliderQuestionTemplate;
                    break;
            }

            return template;
        }
    }
}

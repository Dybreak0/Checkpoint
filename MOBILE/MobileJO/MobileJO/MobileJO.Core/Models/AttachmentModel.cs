using System;
using System.Collections.Generic;
using System.Text;

namespace MobileJO.Core.Models
{
    public class AttachmentModel
    {
        public AttachmentModel()
        {
           Filename = string.Empty;
        }

        public int ID { get; set; }
        public int JobOrderID { get; set; }
        public string Filename { get; set; }
      
    }
}

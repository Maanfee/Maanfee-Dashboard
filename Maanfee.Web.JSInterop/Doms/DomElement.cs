using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Web.JSInterop
{
    public class DomElement
    {
		public string TagName { get; set; }

		public string Selector { get; set; } 

        public string Id { get; set; }

        public string ClassName { get; set; }

        public string Text { get; set; }

        public string Href { get; set; }

        public string Value { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Dashboard.Domain.ViewModels
{
    public class DocumentTypeViewModel
    {
        public string IdAttachmentType { get; set; }

        public string AttachmentTypeTitle { get; set; }

        public string FileName { get; set; }

        public string AliasName { get; set; }

        public byte[] Content { get; set; }

        public long Size { get; set; }

        public string Path { get; set; }

        public string ContentType { get; set; }
    }
}

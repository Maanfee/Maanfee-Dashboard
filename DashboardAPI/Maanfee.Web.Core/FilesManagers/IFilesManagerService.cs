using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maanfee.Web.Core
{
    public interface IFilesManagerService
    {
        public Task<bool> UploadFileChunk(string Url, FileChunk fileChunkDto);
    }
}

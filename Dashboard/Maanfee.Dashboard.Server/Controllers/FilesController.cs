using Maanfee.Web.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace Maanfee.Dashboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        public FilesController()
        {
        }

        [HttpPost("UploadFileChunk")]
        public bool UploadFileChunk([FromBody] FileChunk FileChunk)
        {
            try
            {
                // get the local filename
                string filePath = @"D:\_UploadedDocuments\";
                string fileName = filePath + FileChunk.FileName;

                // delete the file if necessary
                if (FileChunk.FirstChunk && System.IO.File.Exists(fileName))
                    System.IO.File.Delete(fileName);

                // open for writing
                using (var stream = System.IO.File.OpenWrite(fileName))
                {
                    stream.Seek(FileChunk.Offset, SeekOrigin.Begin);
                    stream.Write(FileChunk.Data, 0, FileChunk.Data.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return false;
            }
        } 


        [HttpGet("GetFiles")]
        public List<string> GetFiles()
        {
            var result = new List<string>();
            var files = Directory.GetFiles(Environment.CurrentDirectory + "\\StaticFiles", "*.*");
            foreach (var file in files)
            {
                var justTheFileName = Path.GetFileName(file);
                result.Add($"files/{justTheFileName}");
            }

            return result;
        }


    }
}

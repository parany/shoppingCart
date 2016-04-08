using ImageResizer;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace ShoppingCart.Models.Models.Entities
{
    public class Image : BaseObject
    {
        public string ImageName { get; set; }

        public string ImageType { get; set; }

        public string MIME { get; set; }

        public Dictionary<string, string> Versions { get; set; }

        public Image()
        {
            Versions = new Dictionary<string, string>();
        }

        public void SaveAs(string path, HttpPostedFileBase upload)
        {
            foreach (var suffix in Versions.Keys)
            {
                upload.InputStream.Seek(0, SeekOrigin.Begin);

                //Let the image builder add the correct extension based on the output file type
                ImageBuilder.Current.Build(
                    new ImageJob(
                        upload.InputStream,
                        path + ImageName + suffix,
                        new Instructions(Versions[suffix]),
                        false,
                        true));
            }
        }

        public string  getImageVersion (string version)
        {
            return ImageName + version + ImageType;
        }

    }
}

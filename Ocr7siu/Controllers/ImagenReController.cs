using IronOcr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Drawing.Imaging;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing;
using System.IO.IsolatedStorage;

namespace Ocr7siu.Controllers
{
  

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImagenReController : ApiController
    {
        public static int conta=1;
        public static int nuevoconta = 1;

        const String Storage = "ponysalvaje";
        const String key = "09fTcaVFwVD0KNBUPMW30x2wprzGCmOUYZfz84+9wY6uU9jHbOaW4jVA/Q6J6Yx207RCBwA4G6cJFt1BVHTCUg==";

        public object ViewBag { get; private set; }

       

        public void contador(int a)
        {
            conta = conta + a;
        }

        public void OCRcontador(int a)
        {
            nuevoconta = nuevoconta + a;
        }



        

        //devuelve un string del ocr7 cuando se le envia una base64
        [HttpPost]
        [Route("api/Serresiete")]
        public String DevolverOCR7()
        {


            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Form["Image"];
            String imagenconvertida = postedFile;

            String convertida = imagenconvertida.Replace("data:image/png;base64,", String.Empty);

            
            String CR7 = Extraer(conta.ToString(), convertida);
            OCRcontador(1);
            String resultado = ElComandateCR7(CR7);




            return resultado;

        }
        //Metodo que guarda el archivo y extrae el path para que se implemente el OCR
        public String Extraer(String filename, String base64Image)
        {
            string destinationImgPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EscaneoFisico" + "\\" + filename + ".png";
            try
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\EscaneoFisico");
            }
            catch (IOException iox)
            {
                Console.WriteLine(iox.Message + " " + iox.Data);
            }
            byte[] bytes = Convert.FromBase64String(base64Image);

            using (var imageFile = new FileStream(destinationImgPath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }





            return destinationImgPath;
        }

        //Metodo OCR7
        public String ElComandateCR7(String Imagen)
        {
            String Resultado ="";
            AutoOcr OCR = new AutoOcr() { ReadBarCodes = false };
            var Results = OCR.Read(Imagen);

            Resultado = Results.Text;

            return Resultado;
        }



       





        // crea el blob y haria el ocr pero nel
        [HttpPost]
        [Route("api/Autovr2")]
        public String version2()
        {
            var Stocuenta = new CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(Storage, key), true);
            var blobcliente = Stocuenta.CreateCloudBlobClient();
            var container = blobcliente.GetContainerReference("micontendor");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            String imageName = null;
            imageName = conta.ToString() + ".png";

            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var image = httpRequest.Files["Image"];
            //Create custom filename
             var pati = String.Format("Got image {0} of type {1} and size {2}",
            image.FileName, image.ContentType, image.ContentLength);

            string uniqueBlobName = string.Format("productimages/image_{0}",imageName
            );
            CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = image.ContentType;
            blob.UploadFromStream(image.InputStream);



            String cosa = "image_"+imageName;

            String nueva = ElComandateCR7("https://ponysalvaje.blob.core.windows.net/micontendor/productimages/"+cosa);





            return nueva;

        }

    }
}

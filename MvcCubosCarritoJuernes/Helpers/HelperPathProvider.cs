namespace MvcCoreUtilidades.Helpers
{
    //Enumeracion con las carpetas que deseemos subir ficheros

    public enum Folders { Cubos }

    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;
        private IHttpContextAccessor contextAccessor;

        public HelperPathProvider(IWebHostEnvironment hostEnvironment, IHttpContextAccessor contextAccessor)
        {
            this.hostEnvironment = hostEnvironment;
            this.contextAccessor = contextAccessor;
        }

        //TENDREMOS UN METODO QUE SE ENCARGARA DE RESOLVER LA RUTA
        //COMO UN STRING CUANDO RECIBAMOS EL FICHERO Y LA CARPETA
        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Cubos)
            {
                carpeta = "cubos";
            }
            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, fileName);
            return path;
        }

        public string MapUrlPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Cubos)
            {
                carpeta = "cubos";
            }
            var request = this.contextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            string urlPath = $"{baseUrl}/{carpeta}/{fileName}";
            return urlPath;
        }

    }
}

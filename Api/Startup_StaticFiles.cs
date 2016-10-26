using System;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace Api
{
    partial class Startup
    {    
        private void ConfigureStaticFiles(IAppBuilder builder)
        {
            string webFilesPath = System.Configuration.ConfigurationManager.AppSettings["WebFilesPath"];
            if(webFilesPath == null) throw new NullReferenceException("Web path is not defined");

            var physicalFileSystem = new PhysicalFileSystem(webFilesPath);

            var options = new FileServerOptions
            {
                EnableDirectoryBrowsing = false,
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem
            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[] { "index.html" };
            
            builder.UseFileServer(options);
        }
    }
}

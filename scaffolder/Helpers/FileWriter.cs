using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace scaffolder.Helpers
{
    internal class FileWriter
    {
        public static void Write(string outputPath, string filename, string contents, bool addExtension = true)
        {
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            var path = Path.Combine(outputPath, filename + (addExtension ? ".cs" : ""));

            using (var file = new StreamWriter(path, false))
            {
                file.Write(contents.ToCharArray());
            }
        }
    }
}

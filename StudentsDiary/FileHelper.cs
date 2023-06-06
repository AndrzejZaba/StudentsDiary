using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public class FileHelper<T> where T : new()
    {
        private string _filePath;

        public FileHelper(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Serialize list of students to external .XML file.
        /// </summary>
        /// <param name="students">list of generic type objects</param>
        /// <returns></returns>
        public async Task SerializeToFile(T students)
        {
            await Task.Run(() =>
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var streamWriter = new StreamWriter(_filePath))
                {
                    serializer.Serialize(streamWriter, students);
                    streamWriter.Close();
                }
            });
        }

        /// <summary>
        /// Deserialize objects saved in external .XML file.
        /// </summary>
        /// <returns></returns>
        public T DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new T();
            }
            var serializer = new XmlSerializer(typeof(T));

            using (var streamReader = new StreamReader(_filePath))
            {
                var students = (T)serializer.Deserialize(streamReader);

                streamReader.Close();
                return students;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace UtilityBelt
{
    public class Serializer
    {
        public static void Serialize(Stream streamObject, object objForSerialization)
        {
            if (objForSerialization == null || streamObject == null)
                return;

            XmlSerializer serializer = new XmlSerializer(objForSerialization.GetType());
            serializer.Serialize(streamObject, objForSerialization);
        }

        public static object Deserialize(Stream streamObject, Type serializedObjectType)
        {
            if (serializedObjectType == null || streamObject == null)
                return null;

            XmlSerializer serializer = new XmlSerializer(serializedObjectType);
            return serializer.Deserialize(streamObject);
        }

        public static void SerializeToXML<T>(string file, T movies)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            TextWriter textWriter = new StreamWriter(file);
            serializer.Serialize(textWriter, movies);
            textWriter.Close();
        }

        public static List<T> DeserializeFromXML<T>(string file)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<T>));
            TextReader textReader = new StreamReader(file);
            List<T> data = (List<T>)deserializer.Deserialize(textReader);
            textReader.Close();

            return data;
        }
    }
}

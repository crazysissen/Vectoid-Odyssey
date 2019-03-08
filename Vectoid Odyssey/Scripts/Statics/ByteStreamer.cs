using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VectoidOdyssey
{
    static class ByteStreamer
    {
        /// <summary>
        /// Converts a serializeable object object to an array of bytes
        /// </summary>
        /// <param name="thisTarget">MUST be serializeable</param>
        public static byte[] ToBytes(this object thisTarget)
        {
            if (thisTarget == null)
            {
                return null;
            }

            BinaryFormatter tempFormatter = new BinaryFormatter();

            using (MemoryStream tempStream = new MemoryStream())
            {
                tempFormatter.Serialize(tempStream, thisTarget);

                return tempStream.ToArray();
            }
        }

        /// <summary>
        /// Converts a formatted byte array back to an object of type T
        /// </summary>
        public static T ToObject<T>(this byte[] theseBytes)
        {
            if (theseBytes == null)
            {
                return default(T);
            }

            BinaryFormatter tempFormatter = new BinaryFormatter();

            using (MemoryStream tempStream = new MemoryStream())
            {
                tempStream.Write(theseBytes, 0, theseBytes.Length);
                tempStream.Seek(0, SeekOrigin.Begin);

                return (T)tempFormatter.Deserialize(tempStream);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boykot.PCL.Helpers
{
    public class JSONParser<T> where T : class, new()
    {
        public T parse(string data)
        {
            T obj = new T();
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(data));

            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
                obj = serializer.ReadObject(ms) as T;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("JSON Parser exception: " + ex.Message);
            }

            return obj;
        }
    }
}

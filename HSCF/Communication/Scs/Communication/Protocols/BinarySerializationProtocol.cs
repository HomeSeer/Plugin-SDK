using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using HSCF.Communication.Scs.Communication.Messages;

namespace HSCF.Communication.Scs.Communication.Protocols
{
    /// <summary>
    /// Default communication protocol between server and clients to send and receive a message.
    /// It uses .NET binary serialization to write and read messages.
    /// </summary>
    internal class BinarySerializationProtocol : IScsWireProtocol
    {
        

        /// <summary>
        /// Writes a message to a stream.
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="stream">Stream that is used to write message</param>
        public void WriteMessage(IScsMessage message, Stream stream)
        {
            
            new BinaryFormatter().Serialize(stream, message);
            
            /*
            //rjh tried to detect a serialization error, does not seem to detect one
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, message);
            byte[] b = ms.ToArray();
            stream.Write(b, 0, b.Length);
            stream.Flush();
            //new BinaryFormatter().Serialize(stream, message);
            */
            
        }

        /// <summary>
        /// Reads a message from a stream.
        /// </summary>
        /// <param name="stream">Stream taht is used to read message</param>
        /// <returns>Message that is read from stream</returns>
#if false
        public IScsMessage ReadMessage(Stream stream)
        {
            var binaryFormatter = new BinaryFormatter
            {
                AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                Binder = new DeserializationAppDomainBinder()
            };
            return (IScsMessage)binaryFormatter.Deserialize(stream);
        }
#endif

#if true
        public IScsMessage ReadMessage(Stream stream)
        {
           
            var binaryFormatter = new BinaryFormatter
            {
                AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                Binder = new DeserializationAppDomainBinder()
            };

            // rjh 10/23/14 ver 1.0.3
            // changed comms to send byte count first then data, this part first gets the byte count, then the data
            // On MONO there appears to be a problem if you deserialize from the stream, it may not get all the data and errors out
            // with the byte count, we first the total # of bytes sent, then to into a look reading from the stream until we get all the bytes
            // On windows, if you do a stream.read and pass a count, you always get the number of bytes you are waiting for
            // On MONO, it may return only some of the bytes and you need to call read again, took 3 days to find this out!
            //try
            //{
                byte[] buf = new byte[2];
                int count = 0;

                count = stream.Read(buf, 0, 2);
                
                if (count == 0)
                {
                    throw new Exception("Client is not connected to the server.");
                }

                if (buf[0] == 0xFD && buf[1] == 0xFE)
                {
                    // looks like our byte count, get the count
                    byte[] bcount = new byte[4];
                    if (!stream.CanRead) return null;
                    stream.Read(bcount, 0, 4);
                    int len = BitConverter.ToInt32(bcount, 0);
                    if (len == 0) 
                        return null;
                    //Console.WriteLine(DateTime.Now.ToString() + ":Got byte count: " + len.ToString());
                    //Console.WriteLine("Need to read: " + len.ToString());
                    buf = new byte[len];

                    int total = 0;
                    while (total < len)
                    {
                        if (!stream.CanRead) return null;
                        int bread = stream.Read(buf, total, len - total);
                        //Console.WriteLine("Read: " + bread.ToString());
                        total += bread;
                    }

                    System.IO.MemoryStream ms = new System.IO.MemoryStream(buf);
                    ms.Position = 0;
                    IScsMessage msg = null;
                    try
                    {
                        msg = (IScsMessage)binaryFormatter.Deserialize(ms);
                    //Console.WriteLine("Message: " + msg.ToString());      // display the hs. function that is being calling in HS
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Exception deserializing message: " + e.Message);
                        return null;
                    }
                    return msg;
                }
                else
                {
                    // some kind of error
                    return null;
                }
            //}
            //catch (Exception e)   // rjh if try/catch is added, it needs to throw the exception so the plugin disconnects
            //{
            //    //Console.WriteLine("Error deserializing: " + e.Message);
            //}

            //IScsMessage msg = (IScsMessage)binaryFormatter.Deserialize(stream);
            return null;
        }
        
#endif

        /// <summary>
        /// This class is used in deserializing to allow deserializing objects that are defined
        /// in assemlies that are load in runtime (like PlugIns).
        /// </summary>
        private sealed class DeserializationAppDomainBinder : SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                var toAssemblyName = assemblyName.Split(',')[0];
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.FullName.Split(',')[0] == toAssemblyName)
                    {
                        return assembly.GetType(typeName);
                    }
                }

                return null;
            }
        }
    }
}

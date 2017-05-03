using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Buffer = Windows.Storage.Streams.Buffer;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace TestApp.Vlc
{
    class Remote
    {
        // This is client socket.
        private readonly StreamSocket socket;

        private DataReader reader;
        private DataWriter writer;

        public Remote()
        {
            socket = new StreamSocket();
        }

        public async Task<bool> Connect()
        {
            await socket.ConnectAsync(new HostName("localhost"), "4444");






            return true;
        }

        public void Disconnect()
        {
            reader?.DetachStream();
            reader?.Dispose();
            reader = null;

            writer?.DetachStream();
            writer?.Dispose();
            writer = null;
        }


        /**
         * Connect to VLC player
         * 
         * @return: true if success.
         * */
        /**
         * Disconnect from VLC player.
         * */
        /**
         * Send raw command to VLC. This function add \n to the end.
         * */
        public async Task<string> ReciveAnswer()
        {
            var stringBuilder = new StringBuilder();

            using (reader = new DataReader(socket.InputStream)
            {
                InputStreamOptions = InputStreamOptions.Partial,
                UnicodeEncoding = UnicodeEncoding.Utf8
            })
            {
                await reader.LoadAsync(256);

                while (reader.UnconsumedBufferLength > 0)
                {
                    stringBuilder.Append(reader.ReadString(reader.UnconsumedBufferLength));
                    await reader.LoadAsync(256);
                }

                reader.DetachStream();
            }




            return stringBuilder.ToString();
        }

        public async Task<bool> SendCustomCommand(string command)
        {
            var message = command + "\n";

            using (writer = new DataWriter(socket.OutputStream)
            {
                UnicodeEncoding = UnicodeEncoding.Utf8
            })
            {
                // Write a string value to the output stream.
                writer.WriteString(message);

                // Send the contents of the writer to the backing stream.
                try
                {
                    await writer.StoreAsync();
                }
                catch (Exception exception)
                {
                    switch (SocketError.GetStatus(exception.HResult))
                    {
                        case SocketErrorStatus.HostNotFound:
                            // Handle HostNotFound Error
                            throw;
                        default:
                            // If this is an unknown status it means that the error is fatal and retry will likely fail.
                            throw;
                    }
                }

                await writer.FlushAsync();
                writer.DetachStream();
            }

            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace benschatting
{
    //client side is almost finished it just needs to take the message recieved from the server and put it in the rich text box
    public partial class messaging_application : Form
    {
        string text = "string did not take in value";
        TcpClient client = new TcpClient();
       
        public messaging_application()
        {
            InitializeComponent();
            ReceiveMessage(client,richTextBox1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
             

        }

        private void sa_Click(object sender, EventArgs e)
        {

            
           

            text = textBox1.Text;

           

            textBox1.Clear();

            SendMessage(text, client);
            richTextBox1.Text =  $"{richTextBox1.Text} \n {text
                }";
           
        }

        private void SendMessage(string str, TcpClient client)
        {

            

            NetworkStream stream = client.GetStream();

            byte[] message = Encoding.UTF8.GetBytes(str);

            stream.Write(message, 0, message.Length);

            Debug.WriteLine($"the debug before reset is {message.ToString()}");

            message = new byte[1024];

            Debug.WriteLine($"the debug after reset is {message.ToString()}");
        }
        private void ReceiveMessage(TcpClient client, RichTextBox textbox)
        {
            

            client.Connect("127.0.0.1", 5000);
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            Thread mythread = new Thread(() => {

                while (true)
                {
                   

                    int bytesread = stream.Read(buffer, 0, buffer.Length);

                    if (bytesread < 0)
                    {
                        Debug.Write("server connection broken");
                        break;
                    }

                    string returnmessage = Encoding.UTF8.GetString(buffer,0,bytesread);

                    if(textbox.InvokeRequired)
                    {
                        textbox.Invoke((MethodInvoker)delegate
                        {
                            textbox.Text = $"{textbox.Text}  \n {returnmessage}";
                        });

                    }
                    else
                    {
                        textbox.Text = $"{textbox.Text}  \n {returnmessage}";
                    }

                   buffer = new byte[1024];
                }








            });
            mythread.Start();


        }

       
    }
}

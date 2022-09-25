using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Func<int, int, int> function;

        bool ArgsValidation(out int x, out int y)
        {
            bool xIsCorrect = Int32.TryParse(textBoxX.Text, out x);
            bool yIsCorrect = Int32.TryParse(textBoxY.Text, out y);
            if (!(xIsCorrect && yIsCorrect))
            {
                MessageBox.Show("Args invalid");
                return true;
            }
            return false;
        }

        public int Add(int x, int y)
        {
            Thread.Sleep(1000);
            return x + y;
        }

        public void CallBack(IAsyncResult asyncResult)
        {
            AsyncResult async = (AsyncResult)asyncResult;
            var delegateAdd = (Func<int, int, int>)async.AsyncDelegate;
            var result = delegateAdd.EndInvoke(asyncResult);

            TextBox textBoxResult = (TextBox)asyncResult.AsyncState;
            textBoxResult.Text = result.ToString();
        }

        private void IsComplete_Click(object sender, EventArgs e)
        {
            int x, y;
            if (ArgsValidation(out x, out y))
            {
                return;
            }

            function = new Func<int, int, int>(Add);
            IAsyncResult func = function.BeginInvoke(x, y, null, null);
            while (!func.IsCompleted)
            {
                Thread.Sleep(100);
            }

            MessageBox.Show("IsCompleted");
            textBoxResult.Text = function.EndInvoke(func).ToString();
        }

        private void End_Click(object sender, EventArgs e)
        {
            int x, y;
            if (ArgsValidation(out x, out y))
            {
                return;
            }

            function = new Func<int, int, int>(Add);
            IAsyncResult func = function.BeginInvoke(x, y, null, null);
            textBoxResult.Text = function.EndInvoke(func).ToString();
            MessageBox.Show("EndInvoke");
        }

        private void Callback_Click(object sender, EventArgs e)
        {
            int x, y;

            if (ArgsValidation(out x, out y))
            {
                return;
            }

            function= new Func<int, int, int> (Add);

            IAsyncResult func =function.BeginInvoke(x,y, CallBack, textBoxResult);
            MessageBox.Show("CallBack");
        }

    }
}

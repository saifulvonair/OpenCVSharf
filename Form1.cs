using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//From: https://github.com/shimat/opencvsharp
// https://shimat.github.io/opencvsharp_docs/search.html?SearchText=bitmap

namespace OpenCVSharf
{
    //public delegate void DelegateObserver(Object param);

    public partial class Form1 : Form
    {
        private bool mStopCapture = true;

        public Form1()
        {
            InitializeComponent();
        }


        private void capture()
        {
            Thread thread = new Thread(doExecute);
            thread.Priority = ThreadPriority.Highest;
            thread.Start(null);
        }

        void doExecute(object param)
        {

            // Capture capture;

            VideoCapture capture = VideoCapture.FromCamera(0, VideoCaptureAPIs.ANY);

            UMat frame = new UMat();

            /*

            var src = new Mat("lenna.png", ImreadModes.Grayscale);
            var dst = new Mat();

            Cv2.Canny(src, dst, 50, 200);
            using (new Window("src image", src))
            using (new Window("dst image", dst))
            {
                Cv2.WaitKey();
            }
            */


            for (; ; )
            {

                if (!mStopCapture)
                    break;

                _ = capture.Read(frame);
                Bitmap bitmap = BitmapConverter.ToBitmap(frame.GetMat(AccessFlag.MASK));

                if (frame.Empty())
                {
                    //  cerr << "Can't capture frame: " << i << std::endl;
                    break;
                }

                // OpenCV Trace macro for NEXT named region in the same C++ scope
                // Previous "read" region will be marked complete on this line.
                // Use this to eliminate unnecessary curly braces.
                // process_frame(frame);

                updateView(bitmap);
            }
        }

        void updateView(Bitmap bitmap)
        {
            if (this.pictureBox1.InvokeRequired)
            {
                this.pictureBox1.BeginInvoke((MethodInvoker)delegate
                {
                    pictureBox1.Image = bitmap;
                    return;
                });
            }
            else
            {
                pictureBox1.Image = bitmap;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            capture();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mStopCapture = false;
        }
    }
}

using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
namespace AgOpenGPS.Forms
{
    public partial class FormToastNotification : Form
    {
        private Timer _timer;

        public FormToastNotification(string message, string status)
        {
            InitializeComponent();
            this.Text = status;
            this.labelMessage.Text = message;

            // Configurar el temporizador para cerrar la notificación
            _timer = new Timer();
            _timer.Interval = 3000; // 3 segundos
            _timer.Tick += (s, e) => this.Close();
            _timer.Start();

            // Posicionar la notificación en la esquina inferior derecha
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Right - this.Width,
                Screen.PrimaryScreen.WorkingArea.Bottom - this.Height
            );
        }

       

        private Label labelMessage;
    }
}
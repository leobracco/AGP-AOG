using System;

using System.IO;
using System.Media;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using AgLibrary.Logging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace AgOpenGPS.Forms.Seeders
{
    public partial class FormMonitorDeSiembra : Form
    {
        private WebView2 webView;
        private string _htmlContent;
        private Form _parentForm;
        //public FormMonitorDeSiembra(Form parentForm, int numberOfTubes)   
        public FormMonitorDeSiembra(Form parentForm)
        {
            InitializeComponent(); // Asegurar que esto es lo PRIMERO en el constructor
            _parentForm = parentForm;
            if (_parentForm != null) // Verificar si parentForm es nulo
            {
                _parentForm.FormClosed += (s, e) =>
                {
                    this.Close(); // Cerrar este formulario cuando el padre se cierre
                };
            }

            InitializeWebView();
        }
        public void PosicionarDesdeAbajo(int distanciaDesdeMargen)
        {
            // 1. Obtener el área de trabajo de la pantalla (excluye la barra de tareas)
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;

            // 2. Calcular la coordenada Y para la parte inferior de la pantalla
            int yInferior = workingArea.Bottom - this.Height - distanciaDesdeMargen;

            // 3. Calcular la coordenada X para centrar horizontalmente
            int xCentrado = workingArea.Left + (workingArea.Width - this.Width) / 2;

            // 4. Establecer la posición del formulario
            this.Location = new Point(xCentrado, yInferior);
        }
        private async void InitializeWebView()
        {
            try
            {
                webView = new WebView2
                {
                    Dock = DockStyle.Fill,
                    Visible = false // Ocultar hasta que esté listo
                };

                this.Controls.Add(webView);
                this.Resize += (s, e) => webView.Size = this.ClientSize;

                var env = await CoreWebView2Environment.CreateAsync();
                await webView.EnsureCoreWebView2Async(env);

                // Evento crítico para detectar carga completa
                webView.CoreWebView2.NavigationCompleted += (s, e) =>
                {
                    webView.Visible = true;
                    this.Refresh();
                    webView.CoreWebView2.WebMessageReceived += WebMessageReceived;
                };

                string htmlPath = Path.Combine(Application.StartupPath, "Resources", "Web", "MonitorDeSiembra.html");

                if (!File.Exists(htmlPath))
                {
                    throw new FileNotFoundException($"Archivo HTML no encontrado: {htmlPath}");
                }

                webView.CoreWebView2.Navigate(htmlPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico: {ex.Message}");
                this.Close();
            }
        }
        private void WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            Log.EventWriter($"WebMessageReceived");
            try
            {
                dynamic data = JsonConvert.DeserializeObject(e.WebMessageAsJson);
                switch ((string)data.action)
                {
                    case "resizeForm":
                        HandleJavascriptResizeRequest((int)data.width, (int)data.height);
                        PosicionarDesdeAbajo(100);
                        break;
                    case "showModal":
                        ShowModal(data);
                        break;
                    case "showAlert":
                        ShowAlert(data.status.ToString(), data.message.ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error processing web message: {ex.Message}");
            }
        }

        private void HandleJavascriptResizeRequest(int width, int height)
        {
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    this.Size = new Size(width, height); // Directly set the size
                    this.CenterToScreen(); // Center the form

                    // Important: Resize the webview to fill the form
                    webView.Size = this.ClientSize;
                }
                catch (Exception ex)
                {
                    Log.EventWriter($"Error resizing form: {ex.Message}");
                }
            });
        }


        private void ShowModal(dynamic data)
        {
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    var modalForm = new Form
                    {
                        Text = $"Detalle del Surco {data.tubeNumber}",
                        StartPosition = FormStartPosition.CenterParent,
                        FormBorderStyle = FormBorderStyle.SizableToolWindow
                    };

                    var modalWebView = new WebView2 { Dock = DockStyle.Fill };
                    modalForm.Controls.Add(modalWebView);

                    modalForm.Shown += async (s, ev) =>
                    {
                        await modalWebView.EnsureCoreWebView2Async();
                        modalWebView.CoreWebView2.NavigateToString(GenerateModalHtml(data.tubeNumber.ToString(), data.status.ToString(), data.dosis.ToString(), data.color.ToString()));
                    };

                    modalForm.Show(this);
                }
                catch (Exception ex)
                {
                    Log.EventWriter($"Error showing modal: {ex.Message}");
                }
            });
        }


        private void ShowAlert(string status, string message)
        {
            // Reproducir sonido según el estado
            switch (status)
            {
                case "falla":
                    SystemSounds.Hand.Play(); // Sonido de error
                    break;
                case "dosis no alcanzada":
                    SystemSounds.Exclamation.Play(); // Sonido de advertencia
                    break;
                case "dosis superada":
                    SystemSounds.Asterisk.Play(); // Sonido de información
                    break;
            }

            // Mostrar la notificación toast
            var toast = new FormToastNotification(message, status);
            toast.Show();
        }

        private string GenerateModalHtml(string tubeNumber, string status, string dosis, string color)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css' rel='stylesheet'>
                    <style>
                        body {{ padding: 20px; background: #f8f9fa; }}
                        .progress {{ height: 25px; }}
                        .modal-title {{ color: {color}; }}
                    </style>
                </head>
                <body>
                    <h3 class='modal-title'>Surco #{tubeNumber}</h3>
                    <div class='mt-4'>
                        <p>Estado: <span class='badge' style='background-color: {color}'>{status}</span></p>
                        <p>Dosis actual: {dosis}%</p>
                        <div class='progress'>
                            <div class='progress-bar' 
                                 role='progressbar' 
                                 style='width: {dosis}%; background-color: {color}'>
                                {dosis}%
                            </div>
                        </div>
                    </div>
                    
                    <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js'></script>
                </body>
                </html>";
        }

        private void ConfigureForm(Form parentForm)
        {
            this.StartPosition = FormStartPosition.CenterScreen; // Start centered
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            // No need for manual positioning or size setting here.
            this.Size = new Size(800, 600); // Set a reasonable default size.
            this.Size = new Size(800, 600); // Tamaño fijo
            this.Location = new Point(100, 100); // Ubicación inicial

            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.ControlBox = true;
            this.WindowState = FormWindowState.Normal;
            this.TopMost = false;
            this.CenterToScreen();

            // Handle parent form resize if needed
           
        }
    }
}
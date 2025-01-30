using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;

namespace AgOpenGPS.Forms.Seeders
{
    public partial class FormConfigSembradora : Form
    {
        private WebView2 webView;

        public FormConfigSembradora()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            try
            {
                // Inicializa el control WebView2
                webView = new WebView2();
                webView.Dock = DockStyle.Fill;
                this.Controls.Add(webView);

                // Configura el entorno WebView2
                await webView.EnsureCoreWebView2Async(null);

                // Carga el archivo HTML
                string htmlPath = Path.Combine(Application.StartupPath, "Resources", "Web", "ConfigSembradora.html");
                webView.CoreWebView2.Navigate(htmlPath);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                string logPath = Path.Combine(Application.StartupPath, "error.log");
                string errorDetails = $"Message: {ex.Message}\nStackTrace: {ex.StackTrace}";
                File.WriteAllText(logPath, errorDetails);
                MessageBox.Show($"Error: {ex.Message}\nVer logs en {logPath}");
                this.Close(); // Cierra el formulario en caso de error
            }
        }
    }
}
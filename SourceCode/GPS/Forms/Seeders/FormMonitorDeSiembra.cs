using System;
using System.Drawing;
using System.Windows.Forms;
using AGOpenGPS.GPS;
using MQTTnet;
using AgLibrary.Logging;
using Newtonsoft.Json;
using System.Media; // Para reproducir sonidos
namespace AgOpenGPS.Forms.Seeders
{
    public partial class FormMonitorDeSiembra : Form
    {
        private MqttClientService mqttService;
        private FlowLayoutPanel tubeContainer;

        public FormMonitorDeSiembra(Form parentForm, int numberOfTubes)
        {
            InitializeComponent();
            InitializeMQTT();
            GenerateTubes(numberOfTubes);

            // Configurar el formulario
            this.StartPosition = FormStartPosition.Manual; // Para controlar manualmente la posición
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow; // Evitar que se redimensione
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            // Centrar el formulario horizontalmente respecto al formulario padre
            // y posicionarlo a una altura manualmente definida
            this.Load += (sender, e) => PositionFormRelativeToParent(parentForm, 500); // 100 es la altura manual

            // Minimizar el formulario cuando el programa se minimice
            this.Owner = parentForm; // Establecer el formulario principal como Owner
            this.Owner.Resize += Owner_Resize;
        }

        private void PositionFormRelativeToParent(Form parentForm, int manualHeight)
        {
            // Centrar horizontalmente respecto al formulario padre
            int parentCenterX = parentForm.Location.X + (parentForm.Width / 2);
            int formX = parentCenterX - (this.Width / 2);

            // Posicionar verticalmente en base a la altura manual
            int formY = parentForm.Location.Y + manualHeight;

            // Establecer la posición del formulario
            this.Location = new Point(formX, formY);
        }

        private void Owner_Resize(object sender, EventArgs e)
        {
            // Verificar si Owner no es null antes de acceder a sus propiedades
            if (this.Owner != null)
            {
                // Minimizar el formulario si el formulario principal se minimiza
                if (this.Owner.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }
        }

        // Inicializar el cliente MQTT
        private async void InitializeMQTT()
        {
            try
            {
                mqttService = new MqttClientService();

                // Suscribirse al tópico AOG/SENSORES/#
                await mqttService.SubscribeAsync("AOG/SENSORES/#", (topic, message) =>
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            // Deserializar el mensaje JSON
                            var tubeMessage = JsonConvert.DeserializeObject<TubeMessage>(message);
                            if (tubeMessage == null)
                            {
                                Log.EventWriter("Error: El mensaje no es un JSON válido.");
                                return;
                            }

                            // Actualizar la UI
                            Log.EventWriter("Antes : UpdateTube.");
                            UpdateTube(tubeMessage);
                            Log.EventWriter("Antes : Despues.");
                        }
                        catch (Exception ex)
                        {
                            Log.EventWriter($"Error procesando mensaje MQTT: {ex.Message}");
                        }
                    });
                });

                Log.EventWriter("Suscripción MQTT establecida para AOG/SENSORES/#");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error inicializando MQTT: {ex.Message}");
            }
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
        // Método para generar los tubos
        private const int TubeWidth = 10; // Ancho fijo de cada tubo
        private const int TubeHeight = 50; // Alto fijo de cada tubo
        private const int TubeMargin = 2; // Margen entre tubos
        private const int FormHeight = 80; // Altura fija del formulario
        private const int FormPadding = 1; // Margen del formulario
        private void GenerateTubes(int numberOfTubes)
        {
            // Calcular el ancho necesario para los tubos
            int tubesWidth = (TubeWidth +  TubeMargin*2) * numberOfTubes- TubeMargin; // Restamos el último margen

            // Calcular el ancho total del formulario, incluyendo márgenes
            int formWidth = tubesWidth + 2 * FormPadding;


            // Ajustar el tamaño del formulario
            this.Width = formWidth ; // Ancho dinámico
            this.Height = FormHeight + 2 * FormPadding; // Alto fijo + márgenes

            // Crear el contenedor de tubos
            tubeContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill, // El contenedor ocupa todo el formulario
                AutoScroll = true, // Habilitar scroll si no caben todos los tubos
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true, // Evitar que los tubos se "envuelvan"
                Padding = new Padding(FormPadding), // Margen igual al del formulario
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // Agregar los tubos
            for (int i = 1; i <= numberOfTubes; i++)
            {
                var tubePanel = CreateTubePanel(i);
                tubeContainer.Controls.Add(tubePanel);
            }

            // Añadir el contenedor al formulario
            this.Controls.Add(tubeContainer);
        }

        // Crear un "tubo" (un panel que representa la dosis)
        private Panel CreateTubePanel(int tubeNumber)
        {
            // Crear el panel principal del tubo
            var tubePanel = new Panel
            {
                Width = TubeWidth, // Ancho fijo
                Height = TubeHeight, // Alto fijo
                Margin = new Padding(TubeMargin),
                BackColor = Color.FromArgb(233, 236, 239), // #e9ecef
                BorderStyle = BorderStyle.None,
                Tag = new TubeState { Target = 3.5f, Actual = 3.5f } // Estado inicial
            };

            // Añadir bordes redondeados al tubo
            tubePanel.Paint += (sender, e) =>
            {
                using (var path = GetRoundedRectangle(tubePanel.ClientRectangle, 15))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(new SolidBrush(tubePanel.BackColor), path);
                    e.Graphics.DrawPath(new Pen(Color.FromArgb(222, 226, 230), 1), path); // #dee2e6
                }
            };

            // Crear el "cuello" del tubo
            var tubeNeck = new Panel
            {
                Width = tubePanel.Width,
                Height = 20,
                BackColor = Color.FromArgb(222, 226, 230), // #dee2e6
                Dock = DockStyle.Top
            };

            // Añadir bordes redondeados al cuello
            tubeNeck.Paint += (sender, e) =>
            {
                using (var path = GetRoundedRectangle(tubeNeck.ClientRectangle, 15))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(new SolidBrush(tubeNeck.BackColor), path);
                }
            };

            // Crear el relleno líquido
            var liquidFill = new Panel
            {
                Width = tubePanel.Width,
                Height = tubePanel.Height - tubeNeck.Height,
                BackColor = Color.Green, // Color inicial (verde)
                Dock = DockStyle.Bottom
            };

            // Añadir un degradado al relleno líquido
            liquidFill.Paint += (sender, e) =>
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new Point(0, liquidFill.Height),
                    new Point(0, 0),
                    Color.FromArgb(0, 0, 0, 10), // rgba(0, 0, 0, 0.1)
                    Color.Transparent))
                {
                    e.Graphics.FillRectangle(brush, liquidFill.ClientRectangle);
                }
            };

            // Añadir el texto de estado (rotado)
            var statusInfo = new Label
            {
                
                AutoSize = false,
                Width = liquidFill.Height,
                Height = liquidFill.Width,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Arial", 5, FontStyle.Bold),
                BackColor = Color.Transparent,
                Location = new Point(0, 0),
                Text = "3.5/3.5 SxMetro"
            };

            // Rotar el texto -90 grados
            statusInfo.Paint += (sender, e) =>
            {
                e.Graphics.TranslateTransform(statusInfo.Width / 2, statusInfo.Height / 2);
                e.Graphics.RotateTransform(-90);
                e.Graphics.DrawString(statusInfo.Text, statusInfo.Font, new SolidBrush(statusInfo.ForeColor), -statusInfo.Width / 2, -statusInfo.Height / 2);
            };

            // Añadir los controles al panel del tubo
            tubePanel.Controls.Add(tubeNeck);
            tubePanel.Controls.Add(liquidFill);
            tubePanel.Controls.Add(statusInfo);

            return tubePanel;
        }

        // Método para obtener el color del tubo según la dosis
        private Color GetTubeColor(float actual, float target)
        {
            if (actual == 0)
                return Color.Black; // Sin sembrar o tapado
            else if (actual < target)
                return InterpolateColor(Color.Green, Color.Yellow, (target - actual) / target); // Hacia amarillo
            else if (actual > target)
                return InterpolateColor(Color.Green, Color.Blue, (actual - target) / target); // Hacia azul
            else
                return Color.Green; // Dosis óptima
        }

        // Interpolar entre dos colores
        private Color InterpolateColor(Color start, Color end, float ratio)
        {
            int r = (int)(start.R + (end.R - start.R) * ratio);
            int g = (int)(start.G + (end.G - start.G) * ratio);
            int b = (int)(start.B + (end.B - start.B) * ratio);
            return Color.FromArgb(r, g, b);
        }

        // Método para crear un rectángulo con bordes redondeados
        private System.Drawing.Drawing2D.GraphicsPath GetRoundedRectangle(Rectangle bounds, int radius)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius, radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        // Método UpdateTube modificado para usar el objeto JSON
        private void UpdateTube(TubeMessage tubeMessage)
        {
            if (tubeMessage == null) return;

            // Buscar el panel del tubo correspondiente
            var tubePanel = tubeContainer.Controls[tubeMessage.TubeNumber - 1] as Panel;
            if (tubePanel == null) return;

            // Obtener el control liquidFill
            var liquidFill = tubePanel.Controls[1] as Panel;
            if (liquidFill == null) return;

            // Obtener el control statusInfo
            var statusInfo = tubePanel.Controls[2] as Label;
            if (statusInfo == null) return;

            // Actualizar el texto del label
            statusInfo.Text = $"{tubeMessage.Actual:F1}/{tubeMessage.Target:F1} SxMetro";

            // Determinar el color basado en el estado
            Color newColor = tubeMessage.Status switch
            {
                "falla" => Color.FromArgb(220, 53, 69), // Rojo (#dc3545)
                "tapado" => Color.FromArgb(52, 58, 64), // Negro (#343a40)
                "dosis no alcanzada" => Color.FromArgb(255, 193, 7), // Amarillo (#ffc107)
                "dosis superada" => Color.FromArgb(0, 123, 255), // Azul (#007bff)
                _ => Color.FromArgb(40, 167, 69) // Verde (#28a745)
            };

            // Actualizar el color del líquido
            liquidFill.BackColor = newColor;
            liquidFill.Invalidate(); // Forzar redibujado
        }
        // Clase para almacenar el estado del tubo
        private class TubeState
        {
            public float Target { get; set; }
            public float Actual { get; set; }
        }

        // Clase para el mensaje MQTT
        public class TubeMessage
        {
            public int TubeNumber { get; set; }
            public float Actual { get; set; }
            public float Target { get; set; }
            public string Status { get; set; } // "ok", "falla", "tapado"
        }
    }
}
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

        public FormMonitorDeSiembra()
        {
            InitializeComponent();
            InitializeMQTT();
            GenerateTubes();
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
        private void GenerateTubes()
        {
            tubeContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(240, 240, 240) // Fondo claro
            };

            // Agregar 10 tubos con dosis iniciales
            for (int i = 1; i <= 20; i++)
            {
                var tubePanel = CreateTubePanel(i);
                tubeContainer.Controls.Add(tubePanel);
            }

            // Añadir los tubos al formulario
            this.Controls.Add(tubeContainer);
        }

        // Crear un "tubo" (un panel que representa la dosis)
        private Panel CreateTubePanel(int tubeNumber)
        {
            var tubePanel = new Panel
            {
                Width = 40, // Más pequeño
                Height = 100, // Más pequeño
                Margin = new Padding(5),
                BackColor = Color.White, // Fondo blanco
                BorderStyle = BorderStyle.None,
                Tag = new TubeState { Target = 3.5f, Actual = 3.5f } // Estado inicial
            };

            // Crear la "liquid-fill" que representará la dosis
            var liquidFill = new Panel
            {
                Width = tubePanel.Width - 10, // Margen interno
                Height = tubePanel.Height,
                BackColor = GetTubeColor(3.5f, 3.5f), // Color inicial (verde)
                Dock = DockStyle.Bottom,
                Padding = new Padding(5)
            };

            // Añadir bordes redondeados al líquido
            liquidFill.Paint += (sender, e) =>
            {
                using (var path = GetRoundedRectangle(liquidFill.ClientRectangle, 10))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(new SolidBrush(liquidFill.BackColor), path);
                }
            };

            // Añadir el líquido al panel del tubo
            tubePanel.Controls.Add(liquidFill);

            // Añadir bordes redondeados al tubo
            tubePanel.Paint += (sender, e) =>
            {
                using (var path = GetRoundedRectangle(tubePanel.ClientRectangle, 15))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(new SolidBrush(tubePanel.BackColor), path);
                    e.Graphics.DrawPath(new Pen(Color.Gray, 1), path);
                }
            };

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
            Log.EventWriter("Dentro tubo:" + tubeMessage.TubeNumber);
            if (tubeMessage == null) return;
            Log.EventWriter("Dentro.");

            // Buscar el panel del tubo correspondiente
            var tubePanel = tubeContainer.Controls[tubeMessage.TubeNumber - 1] as Panel;
            if (tubePanel == null) return;
            Log.EventWriter("Dentro tubo Panel:" + tubePanel);

            // Obtener el control liquidFill
            var liquidFill = tubePanel.Controls[0] as Panel;
            if (liquidFill == null) return;

            // Determinar el color basado en el estado
            Color newColor = tubeMessage.Status switch
            {
                "falla" => Color.Red,
                "dosis no alcanzada" => Color.Yellow,
                "dosis superada" => Color.Blue,
                _ => GetTubeColor(tubeMessage.Actual, tubeMessage.Target)
            };

            // Solo actualizar si el color ha cambiado
            if (liquidFill.BackColor != newColor)
            {
                liquidFill.BackColor = newColor;
                liquidFill.Invalidate(); // Forzar redibujado del liquidFill

                // Mostrar alerta si el estado es crítico
                if (tubeMessage.Status == "falla" || tubeMessage.Status == "dosis no alcanzada" || tubeMessage.Status == "dosis superada")
                {
                    string message = $"Tubo {tubeMessage.TubeNumber}: {tubeMessage.Status}\n" +
                                     $"Actual: {tubeMessage.Actual:F1}, Target: {tubeMessage.Target:F1}";
                    ShowAlert(tubeMessage.Status, message);
                }
            }

            Log.EventWriter("Actualización completada para el tubo " + tubeMessage.TubeNumber);
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
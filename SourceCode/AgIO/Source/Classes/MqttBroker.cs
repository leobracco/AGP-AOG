using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AgLibrary.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace AgIO
{
    public class MqttBroker
    {
        private IHost _host;
        private MqttServer _mqttServer;

        public async Task StartBrokerAsync(string brokerAddress = "localhost", int tcpPort = 1883, int wsPort = 3001)
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(kestrelOptions =>
                    {
                        // Configuración para MQTT sobre TCP
                        kestrelOptions.ListenAnyIP(tcpPort, listenOptions => listenOptions.UseMqtt());

                        // Configuración para WebSocket
                        kestrelOptions.ListenAnyIP(wsPort);
                    });

                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedMqttServer(options =>
                    {
                        options.WithDefaultEndpoint();
                    });

                    services.AddMqttConnectionHandler();
                    services.AddConnections();
                    services.AddSingleton<MqttBrokerService>();
                })
                .Build();

            await _host.StartAsync();
            _mqttServer = _host.Services.GetRequiredService<MqttServer>();

            // Configurar eventos
            _mqttServer.ValidatingConnectionAsync += ValidateConnection;
            _mqttServer.ClientConnectedAsync += ClientConnected;

            Log.EventWriter($"Broker MQTT iniciado:\nTCP: {brokerAddress}:{tcpPort}\nWS: ws://{brokerAddress}:{wsPort}/mqtt");
        }

        private Task ValidateConnection(ValidatingConnectionEventArgs args)
        {
            args.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.Success;
            Log.EventWriter($"Nueva conexión: {args.ClientId}");
            return Task.CompletedTask;
        }

        private Task ClientConnected(ClientConnectedEventArgs args)
        {
            Log.EventWriter($"Cliente conectado: {args.ClientId}");
            return Task.CompletedTask;
        }

        public async Task StopBrokerAsync()
        {
            if (_host == null) return;

            await _host.StopAsync();
            _host.Dispose();

            Log.EventWriter("Broker MQTT detenido");
        }
    }

    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapConnectionHandler<MqttConnectionHandler>(
                    "/mqtt",
                    options => options.WebSockets.SubProtocolSelector =
                        protocolList => protocolList.FirstOrDefault() ?? string.Empty);
            });
        }
    }

    public class MqttBrokerService
    {
        // Lógica adicional del broker aquí
    }
}
const mqtt = require("mqtt");

// Configuración del broker MQTT
const brokerUrl = "mqtt://localhost";
const topic = "AOG/SENSORES";

// Crear cliente MQTT
const client = mqtt.connect(brokerUrl);

client.on("connect", () => {
  console.log("Conectado al broker MQTT");

  const enviarMensajes = () => {
    const mensajes = []; // Array para almacenar los mensajes a enviar

    // Generar mensajes para múltiples tubos (ejemplo: 5 tubos aleatorios)
    const numMensajes = 5; // Cambia este número para controlar cuántos mensajes se envían
    for (let i = 0; i < numMensajes; i++) {
      const tubeNumber = Math.floor(Math.random() * 24) + 1;
      var actual = parseFloat((Math.random() * (4.6 - 3.4) + 3.4).toFixed(2));
      if (tubeNumber == Math.floor(Math.random() * 24) + 1) actual = 0;

      const mensaje = {
        TubeNumber: tubeNumber,
        Actual: actual,
      };
      mensajes.push(mensaje);
    }

    // Publicar todos los mensajes en formato JSON
    mensajes.forEach((mensaje) => {
      client.publish(topic, JSON.stringify(mensaje), (err) => {
        if (err) {
          console.error("Error al publicar mensaje:", err);
        }
      });
    });
  };

  setInterval(enviarMensajes, 400); // Intervalo de 400ms
});

client.on("error", (err) => {
  console.error("Error de conexión MQTT:", err);
});

client.on("disconnect", () => {
  console.log("Desconectado del broker MQTT");
});

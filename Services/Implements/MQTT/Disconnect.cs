using MAUI_IOT.Services.Interfaces.MQTT;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Implements.MQTT
{
    public class Disconnect : IDisconnect
    {
        public async Task<IMqttClient> IDisconnect(IMqttClient mqttClient)
        {
            await mqttClient.DisconnectAsync(MqttClientDisconnectOptionsReason.ImplementationSpecificError);
            Debug.WriteLine("Disconnection to the Broker successful");
            return mqttClient;
        }
    }
}

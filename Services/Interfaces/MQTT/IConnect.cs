using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

namespace MAUI_IOT.Services.Interfaces.MQTT
{
    public interface IConnect
    {
        Task<IMqttClient> IConnect(MqttFactory mqttFactory, string tcpServer, int port, string username, string password);
        Task<IMqttClient> IConnect(MqttFactory mqttFactory, string tcpServer, int port);
    }
}

using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Interfaces.MQTT
{
    public interface IDisconnect
    {
        Task<IMqttClient> IDisconnect(IMqttClient mqttClient);
    }
}

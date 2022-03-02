using HomeSeer.PluginSdk;
using HomeSeer.PluginSdk.Logging;
using HomeSeer.PluginSdk.Speech;
using HSCF.Communication.Scs.Communication.EndPoints.Tcp;
using HSCF.Communication.ScsServices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HomeSeer.PluginSdk.Constants;

namespace HSPI_HomeSeerSamplePlugin
{
    class SpeakerClient : IFromSpeaker
    {
        private ISpeechAPI _speakHost;
        private IScsServiceClient<ISpeechAPI> _client;
        private const int SPEAKER_INTERFACE_VERSION = 10; // 10=first version for HS3 with new WCF like API
        private string _clientName = "";
        public static object objlock = new object();

        public SpeakerClient(string name)
        {
            _clientName = name;
        }

        public bool Connect(string username, string password)
        {
            lock (objlock)
            {
                string ipAddress = "127.0.0.1";
                
                Logger.LogInfo("Connecting speaker client {0} to HomeSeer IP {1}", _clientName, ipAddress);

                try
                {
                    _client = ScsServiceClientBuilder.CreateClient<ISpeechAPI>(new ScsTcpEndPoint(ipAddress, 10401), this);
                    _client.Disconnected += new EventHandler(ClientDisconnected);
                    _client.Connected += new EventHandler(ClientConnected);
                }
                catch (Exception e)
                {
                    Logger.LogError(e.ToString());
                    return false;
                }

                try
                {
                    _client.Connect();
                    _speakHost = _client.ServiceProxy;
                    //make sure the interface is supported
                    int v;
                    try
                    {
                        v = _speakHost.version();
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Error attempting to connect to server, please check your connection options: {0}", e.Message);
                        return false;
                    }

                    if (v != SPEAKER_INTERFACE_VERSION)
                    {
                        Logger.LogError("Speaker API version mismatch");
                        return false;
                    }

                    //string localIp = Util.GetLocalIp();
                    string rval = _speakHost.Connect("Sample Plugin", _clientName, "127.0.0.1", username, password);

                    if (!string.IsNullOrEmpty(rval))
                    {
                        Logger.LogError("Error, Unable to connect speaker client interface: {0}", rval);
                        DisconnectNow();
                        return false;
                    }


                }
                catch (Exception e)
                {
                    Logger.LogError("Error while connecting speaker client: {0}", e.ToString());
                    return false;
                }
            }

            return true;
        }

        private void ClientDisconnected(object sender, EventArgs e)
        {
            Logger.LogInfo("Speaker client {0} has disconnected", _clientName);
        }

        private void ClientConnected(object sender, EventArgs e)
        {
            Logger.LogInfo("Speaker client {0} has connected", _clientName);
        }

        private void DisconnectNow()
        {
            lock (objlock)
            {
                try
                {
                    if (_client != null)
                    {
                        try
                        {
                            if (_speakHost != null)
                            {
                                _speakHost = null;
                            }
                            _client.Disconnect();
                            _client.Dispose();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public bool Disconnect()
        {
            DisconnectNow();
            return true;
        }

        public void SpeakText(string speaktxt, bool wait)
        {
            Logger.LogInfo("Speaking from Sample Speaker Client: " + speaktxt);
        }


        public void PlayWaveFile(string fname, int fsize)
        {
            Logger.LogInfo("Playing wave file from Sample Speaker Client, file: " + fname);
        }

        public bool IsBusy()
        {
            //throw new NotImplementedException();
            return false;
        }

        public void VRChanged()
        {
            //throw new NotImplementedException();
        }

        public void StartListen()
        {
            //throw new NotImplementedException();
        }

        public void StopListen()
        {
            //throw new NotImplementedException();
        }

        public short SetVoice(string voice)
        {
            //throw new NotImplementedException();
            return 0;
        }

        public void SpeakToFile(string txt, string fname, string voice = "")
        {
            //throw new NotImplementedException();
        }

        public int SendMessage(string message, bool showballoon)
        {
            //throw new NotImplementedException();
            return 0;
        }

        public void PauseAudio()
        {
            //throw new NotImplementedException();
        }

        public void UnPauseAudio()
        {
            //throw new NotImplementedException();
        }

        public void MuteAudio()
        {
            //throw new NotImplementedException();
        }

        public void UnMuteAudio()
        {
            //throw new NotImplementedException();
        }

        public bool GetMuteStatus()
        {
            //throw new NotImplementedException();
            return false;
        }

        public bool GetListenStatus()
        {
            //throw new NotImplementedException();
            return false;
        }

        public short GetPauseStatus()
        {
            //throw new NotImplementedException();
            return 0;
        }

        public string GetVoiceName()
        {
            //throw new NotImplementedException();
            return string.Empty;
        }

        public int MEDIAPlay(string filename, int fsize)
        {
            //throw new NotImplementedException();
            return 0;
        }

        public string MEDIAFunction(EMediaOperation operation, string p)
        {
            //throw new NotImplementedException();
            return string.Empty;
        }

        public int GetVolume()
        {
            //throw new NotImplementedException();
            return 0;
        }

        public void SetVolume(int vol)
        {
        }

        public void StopSpeaking()
        {
        }

        public int SetListenMode(int mode)
        {
            return 0;
        }

        public int GetListenMode()
        {
            return 0;
        }

        public void SetVRState(string state)
        {
        }

        public void SetSpeaker(string speaker_name)
        {
        }

        public int SetSpeakingSpeed(int speed)
        {
            return 0;
        }

    }

    internal static class Logger
    {
        public static void Log(string line, ELogType level)
        {
            try
            {
                Program._plugin.WriteLog(level, line);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static void LogDebug(string line)
        {
            Log(line, ELogType.Debug);
        }
        public static void LogDebug(string format, params object[] args)
        {
            LogDebug(string.Format(format, args));
        }
        public static void LogInfo(string line)
        {
            Log(line, ELogType.Info);
        }
        public static void LogInfo(string format, params object[] args)
        {
            LogInfo(string.Format(format, args));
        }
        public static void LogWarning(string line)
        {
            Log(line, ELogType.Warning);
        }
        public static void LogWarning(string format, params object[] args)
        {
            LogWarning(string.Format(format, args));
        }
        public static void LogError(string line)
        {
            Log(line, ELogType.Error);
        }
        public static void LogError(string format, params object[] args)
        {
            LogError(string.Format(format, args));
        }
    }
}
